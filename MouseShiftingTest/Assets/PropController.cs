using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{
    public GameObject positionReference;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = positionReference.transform.position;   
    }

    private void OnTriggerEnter(Collider other)
    {
        // Try to find the hand controller object first and inform the goual has been achieved

        GameObject.Find("InitialStatus").SetActive(true);
        gameObject.SetActive(false);
        Debug.Log("Trigger CubeTest");
        Logic uss = other.gameObject.GetComponent<Logic>();
        uss.pickedDone();
    }
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Collision CubeTest");
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
