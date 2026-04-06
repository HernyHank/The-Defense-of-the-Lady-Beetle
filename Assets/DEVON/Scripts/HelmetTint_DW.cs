using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelmetTint_DW : MonoBehaviour
{
	public GameObject helmetObject;   // parent object holding all meshes
	public GameObject tintObject;

	private MeshRenderer[] helmetRenderers;

	void Start()
	{
		// Get ALL mesh renderers in the helmet (including children)
		if (helmetObject != null)
			helmetRenderers = helmetObject.GetComponentsInChildren<MeshRenderer>();
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
		if (other.CompareTag("PlayerHelmet"))
		{
			Debug.Log("Helmet ON");

			SetHelmetVisible(false);

			if (tintObject != null)
				tintObject.SetActive(true);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("PlayerHelmet"))
		{
			Debug.Log("Helmet OFF");

			SetHelmetVisible(true);

			if (tintObject != null)
				tintObject.SetActive(false);
		}
	}
}