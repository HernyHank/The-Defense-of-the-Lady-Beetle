using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRotationToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;

    public bool isRotating = false;
    public TextMeshProUGUI bText;

    //private bool debugB = false;

    // Update is called once per frame
    void Start()
    {
       // bText.gameObject.SetActive(debugB);
        animator.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")) // && isRotating == false && animator != null
        {
            animator.SetBool("isInsideShip", !animator.GetBool("isInsideShip"));


            isRotating = true;
            //debugB = !debugB;
            bText.SetText(animator.GetBool("isInsideShip").ToString());
            
        }
    }

    public void FinishRotation()
    {
        isRotating = false;
    }

    private void Update()
    {
        if(isRotating)
        {
            animator.enabled = true;
        } 
        else
        {
            animator.enabled = false;
        }
    }
}
