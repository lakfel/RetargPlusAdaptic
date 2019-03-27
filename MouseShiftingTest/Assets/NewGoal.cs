using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGoal : MonoBehaviour
{
    public Logic logic;

    // Start is called before the first frame update
    void Start()
    {
        if(logic == null)
        {
            GameObject objectLogic = GameObject.Find("Handle");
            if(objectLogic != null)
            {
                logic = objectLogic.GetComponent<Logic>();
            }
            else
            {
                Debug.Log("CLASE : New Goal -------------- Imposible to fin Handle object and logic component. Not gonna work :|");            

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if(logic != null)
        {
            Debug.Log("CLASE : New Goal -------------- Reagoaling|");
            logic.reGoal();
        }
    }
}
