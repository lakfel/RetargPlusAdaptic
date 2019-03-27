using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using System.Text;


public class PropMannager : MonoBehaviour
{


    public static string serialName = @"\\.\COM16";
    public SerialPort mySPort = new SerialPort(serialName, 115200);

    // Start is called before the first frame update
    void Start()
    {
        mySPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            Debug.Log("1 key was pressed");
            mySPort.Write("<1>");
        }
        else if (Input.GetKeyDown("2"))
        {
            Debug.Log("2 key was pressed");
            mySPort.Write("<2>");
        }
        else if (Input.GetKeyDown("3"))
        {
            Debug.Log("3 key was pressed");
            mySPort.Write("<3>");
        }
    }
}
