using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerRotationToggle : MonoBehaviour
{
    // Start is called before the first frame update
    public Animator animator;

    private int insideShipToggle = 1;
    public bool isRotating = false;
    public TextMeshProUGUI bText;

    private bool debugB = false;

    // Update is called once per frame
    void Start()
    {
        bText.gameObject.SetActive(debugB);
        animator.enabled = false;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && isRotating == false && animator != null)
        {
            insideShipToggle *= -1;
            animator.SetBool("isInsideShip", false);


            isRotating = true;
            debugB = !debugB;
            bText.gameObject.SetActive(debugB);
            


        }
    }

    public void FinishRotation()
    {
        isRotating = false;
    }
}
