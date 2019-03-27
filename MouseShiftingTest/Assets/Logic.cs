using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
public class Logic : MonoBehaviour
{
    // Reference to goals. 3 props in front of the user.
    // 0. Left 1. Middle 2 Right
    public GameObject [] props;

    // Numbre of the current goal. TODO// Change the int for the object?
    public int goal; //   0 , 1,2 

    // Tells if a goal prop is currently being showed
    public bool showing;

    // Start is called before the first frame update


    void Start()
    {
        //props = new []{GameObject.Find("CubeTestL"),
        //               GameObject.Find("CubeTestC"),
        //             GameObject.Find("CubeTestR")};
        goal = 0;
        for (int i = 0; i < 3; i++)
            props[i].SetActive(false);
        showing = false;
        //reGoal();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void reGoal()
    {
        if(!showing)
        { 
            Debug.Log("GOAL" + goal);
            goal = Random.Range(0, 3);
            props[goal].SetActive(true);
            showing = true;
            CapsuleHand hand = gameObject.GetComponent<CapsuleHand>();
            if(hand != null)
                gameObject.GetComponent<TargetedController>().starShifting(props[goal].transform.position, hand.GetLeapHand().PalmPosition.ToVector3());
        }
    }
    public void pickedDone()
    {
        showing = false;
    }



}
