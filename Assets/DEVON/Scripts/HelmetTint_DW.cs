using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetTint_DW : MonoBehaviour
{
	public GameObject helmetObject;   // parent object holding all meshes
	public GameObject tintObject;
	public string playerRootName = "Player_Joan"; // name of the player root folder to search for

	[Header("Worn offsets (local to FollowHead)")]
	public Vector3 wornLocalPosition = Vector3.zero;
	public Vector3 wornLocalEuler = Vector3.zero;

	private MeshRenderer[] helmetRenderers;

	// Stored so we can restore the original parent/transform on exit
	private Transform originalParent;
	private Vector3 originalLocalPosition;
	private Quaternion originalLocalRotation;

	// runtime attachment state
	private Transform followHead;
	private GameObject wearAnchor; // runtime anchor parented to followHead
	private Rigidbody[] helmetRigidbodies;
	private Collider[] helmetColliders;
	private bool[] rbWasKinematic;
	private bool[] rbUsedGravity;
	private bool[] colliderWasTrigger;

	void Start()
	{
		// Get ALL mesh renderers in the helmet (including children)
		if (helmetObject != null)
			helmetRenderers = helmetObject.GetComponentsInChildren<MeshRenderer>(true);

		// cache physics components so we can toggle them while "worn"
		if (helmetObject != null)
		{
			helmetRigidbodies = helmetObject.GetComponentsInChildren<Rigidbody>(true);
			helmetColliders = helmetObject.GetComponentsInChildren<Collider>(true);

			rbWasKinematic = new bool[helmetRigidbodies.Length];
			rbUsedGravity = new bool[helmetRigidbodies.Length];
			colliderWasTrigger = new bool[helmetColliders.Length];
		}
	}

	void SetHelmetVisible(bool visible)
	{
		if (helmetRenderers == null) return;

		foreach (MeshRenderer r in helmetRenderers)
		{
			r.enabled = visible;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if (!other.CompareTag("PlayerHelmet") || helmetObject == null)
			return;

		Debug.Log("Helmet ON");

		// visual tint behavior
		SetHelmetVisible(false);
		if (tintObject != null)
			tintObject.SetActive(true);

		// Save original parent/transform the first time we attach
		if (originalParent == null)
		{
			originalParent = helmetObject.transform.parent;
			originalLocalPosition = helmetObject.transform.localPosition;
			originalLocalRotation = helmetObject.transform.localRotation;
		}

		Transform fh = FindFollowHead(other.transform);
		if (fh == null)
		{
			Debug.LogWarning("FollowHead not found under " + playerRootName);
			return;
		}

		// Store follow reference
		followHead = fh;

		// Make physics quiet before changing hierarchy:
		// - make all rigidbodies kinematic and disable gravity
		// - mark colliders as triggers so they won't push the head
		for (int i = 0; i < helmetRigidbodies.Length; i++)
		{
			var rb = helmetRigidbodies[i];
			rbWasKinematic[i] = rb.isKinematic;
			rbUsedGravity[i] = rb.useGravity;
			rb.isKinematic = true;
			rb.useGravity = false;
		}

		for (int i = 0; i < helmetColliders.Length; i++)
		{
			var col = helmetColliders[i];
			colliderWasTrigger[i] = col.isTrigger;
			col.isTrigger = true; // keep triggers so SteamVR hand hovering still works
		}

		// Create an anchor under FollowHead to parent the helmet to.
		// Parenting to a collider-free anchor avoids physics resolution impulses.
		CreateWearAnchor();

		// Parent helmet to anchor without changing local transform (snap to anchor)
		helmetObject.transform.SetParent(wearAnchor.transform, worldPositionStays: false);
		helmetObject.transform.localPosition = Vector3.zero;
		helmetObject.transform.localRotation = Quaternion.Euler(wornLocalEuler);
	}

	void OnTriggerExit(Collider other)
	{
		if (!other.CompareTag("PlayerHelmet") || helmetObject == null)
			return;

		Debug.Log("Helmet OFF");

		SetHelmetVisible(true);

		if (tintObject != null)
			tintObject.SetActive(false);

		// stop following and restore physics & parents
		// If the player grabbed the helmet, SteamVR may have changed parent; we still restore original states
		// but we avoid forcibly overriding the parent if the helmet is currently attached to a Hand (Interactable).
		bool isAttachedToHand = false;
		var interactable = helmetObject.GetComponentInChildren<Valve.VR.InteractionSystem.Interactable>(true);
		if (interactable != null && interactable.attachedToHand != null)
			isAttachedToHand = true;

		// restore rigidbody state
		for (int i = 0; i < helmetRigidbodies.Length; i++)
		{
			var rb = helmetRigidbodies[i];
			rb.isKinematic = rbWasKinematic[i];
			rb.useGravity = rbUsedGravity[i];
		}

		// restore collider state
		for (int i = 0; i < helmetColliders.Length; i++)
		{
			var col = helmetColliders[i];
			col.isTrigger = colliderWasTrigger[i];
		}

		// Only restore parent/transform if the helmet is not currently held by the player.
		if (!isAttachedToHand)
		{
			helmetObject.transform.SetParent(originalParent, worldPositionStays: true);
			helmetObject.transform.localPosition = originalLocalPosition;
			helmetObject.transform.localRotation = originalLocalRotation;
		}
		else
		{
			// If held, leave parenting to SteamVR (it will restore to the saved original parent when detached
			// if the helmet's Throwable.restoreOriginalParent is true). We still clear our runtime anchor.
		}

		DestroyWearAnchor();
		followHead = null;
	}

	void CreateWearAnchor()
	{
		if (followHead == null) return;

		if (wearAnchor != null) return;

		wearAnchor = new GameObject("HelmetWearAnchor");
		// parent to followHead so it follows HMD automatically
		wearAnchor.transform.SetParent(followHead, worldPositionStays: false);
		wearAnchor.transform.localPosition = wornLocalPosition;
		wearAnchor.transform.localRotation = Quaternion.Euler(wornLocalEuler);
	}

	void DestroyWearAnchor()
	{
		if (wearAnchor != null)
		{
			// If helmet is still parented to wearAnchor and not held, reparent it before destroying
			if (helmetObject != null && helmetObject.transform.parent == wearAnchor.transform)
			{
				helmetObject.transform.SetParent(originalParent, worldPositionStays: true);
			}

			Destroy(wearAnchor);
			wearAnchor = null;
		}
	}

	// Finds the FollowHead transform starting from the collider's transform:
	// 1) walk up the ancestor chain to find a GameObject named playerRootName
	// 2) if found, look for a child named "FollowHead" (searches children)
	// 3) fallback: search the scene for playerRootName then its FollowHead
	private Transform FindFollowHead(Transform start)
	{
		Transform t = start;
		while (t != null && t.name != playerRootName)
			t = t.parent;

		if (t == null)
		{
			GameObject foundRoot = GameObject.Find(playerRootName);
			if (foundRoot != null)
				t = foundRoot.transform;
		}

		if (t != null)
		{
			// Direct child lookup first
			Transform fh = t.Find("FollowHead");
			if (fh != null)
				return fh;

			// If not direct child, search all descendants
			Transform[] allChildren = t.GetComponentsInChildren<Transform>(true);
			foreach (Transform child in allChildren)
			{
				if (child.name == "FollowHead")
					return child;
			}
		}

		return null;
	}
}