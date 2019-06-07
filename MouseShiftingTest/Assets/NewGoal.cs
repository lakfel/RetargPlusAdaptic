using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGoal : MonoBehaviour
{
    private GameObject master;
    private MasterController masterController;
    private Logic logic;

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("Master");
        logic = master.GetComponent<Logic>();
    }
    /*
    private void OnTriggerEnter(Collider other)
    {

        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = Color.green;
        masterController.handOnInitialPosition = true;
    }

    private void OnTriggerExit(Collider other)
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = Color.white;
        masterController.handOnInitialPosition = false;
    }

    private void OnBecameInvisible()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = Color.white;
        masterController.handOnInitialPosition = false;
    }

    private void OnBecameVisible()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        rend.material.color = Color.white;
        masterController.handOnInitialPosition = false;
    }*/
    // Update is called once per frame
    void Update()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        Collider spehre = gameObject.GetComponent<SphereCollider>();
        if (Physics.OverlapBox(spehre.transform.position, new Vector3(0.05f, 0.05f, 0.05f)).Length == 1)
        {
            rend.material.color = Color.white;
            logic.handOnInitialPosition = false;
        }
        else
        {
            rend.material.color = Color.green   ;
            logic.handOnInitialPosition = true;
        }
    }

   
}
