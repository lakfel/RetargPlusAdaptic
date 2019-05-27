using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGoal : MonoBehaviour
{
    private GameObject master;
    private MasterController masterController;

    // Start is called before the first frame update
    void Start()
    {
        master = GameObject.Find("Master");
        masterController = master.GetComponent<MasterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Renderer rend = gameObject.GetComponent<Renderer>();
        if (Physics.OverlapBox(transform.position, new Vector3(0.1f, 0.1f, 0.1f)).Length == 1)
        {
            rend.material.color = Color.white;
            masterController.handOnInitialPosition = false;
        }
        else
        {
            rend.material.color = Color.green   ;
            masterController.handOnInitialPosition = true;
        }
    }

   
}
