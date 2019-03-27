using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
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
