using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Leap.Unity;

public class DefaultCapsuleHand : MonoBehaviour
{

    private bool isGrabbing;
    public bool IsGrabbing { get => isGrabbing; set => isGrabbing = value; }

    private Collider collider;


    public CapsuleHand capsuleHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (capsuleHand.gameObject.activeSelf && capsuleHand.canDraw)
            transform.position = capsuleHand.PositionPalmUpdatedRet;
        else if (collider != null)
            transform.position = collider.transform.position;
    }
    private void OnTriggerExit(Collider other)
    {

       /* if (collider == other)
        {
            collider = null;
            capsuleHand.canDraw = true;
        }*/
    }
    private void OnTriggerEnter(Collider other)
    {/*
        if (collider == null)
        {
            collider = other;
            IsGrabbing = true;
            capsuleHand.canDraw = false;
        }*/
    }
   
    
}
