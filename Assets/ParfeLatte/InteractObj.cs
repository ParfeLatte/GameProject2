using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObj : MonoBehaviour
{
    public GameObject InteractKeyUI;
    public bool isInteract;
    public bool isMoveObj;//리프트인지 엘베인지 확인

    public LiftTest Lift;
    public Elevator Elev;

    private void Awake()
    {
        CheckLift();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInteract) return;
        if (isMoveObj) return;
        if (Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    public void ShowInteractUI()
    {
        InteractKeyUI.SetActive(true);
    }

    public void HideInteractUI()
    {
        InteractKeyUI.SetActive(false);
    }

    public void Interact()
    {
        InteractKeyUI.SetActive(false);
        Invoke("ShowInteractUI", 0.5f);
    }

    public void CheckLift()
    {
        if(Elev != null || Lift != null)
        {
            isMoveObj = true;
        }
        else if(Elev == null && Lift == null)
        {
            isMoveObj = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            InteractKeyUI.SetActive(true);
            isInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag == "Player")
        {
            InteractKeyUI.SetActive(false);
            isInteract = false;
        }
    }
}