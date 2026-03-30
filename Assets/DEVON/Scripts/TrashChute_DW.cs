using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashChute_DW : MonoBehaviour
{

    public Animator TrashChuteAnimator;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("TrashBattery"))
		{
			if (TrashChuteAnimator != null)
				TrashChuteAnimator.SetTrigger("ChuteClose");

			Debug.Log("Trash Chute door closing");
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("TrashBattery"))
		{
			if (TrashChuteAnimator != null)
				TrashChuteAnimator.SetTrigger("ChuteOpen");

			Debug.Log("Trash Chute door opening");
		}
	}



}
