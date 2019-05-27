using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO.Ports;
using System.Text;
using System;

public class PropMannager : MonoBehaviour
{

    public enum PRESET_TYPE
    {
        NONE,
        FLAT,
        CYLINDER,
        BOOK
    };

    public static string serialName = @"\\.\COM16";
    public SerialPort mySPort = new SerialPort(serialName, 115200);

    // Start is called before the first frame update
    void Start()
    {
        try
        {
            mySPort.Open();
            mySPort.ReadTimeout = 10;
        }
        catch(Exception e)
        {
            mySPort = null;
            Debug.Log("ERROR OPENNING PORT " + e.Message);
        }
    }

    // Update is called once per frame
    void Update()
    {/*
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
        }*/
        
        if (string.Equals(readData(), "OK"))
            {
                GameObject tracker1 = GameObject.Find("Tracker1");
                if (tracker1 != null)
                {
                    HydraTracker hTracker = tracker1.GetComponent<HydraTracker>();
                    if (hTracker != null)
                    {
                        hTracker.attach();
                        Debug.Log("ATTACHING PROP");
                        // hTracker.PositionReference = propContr;
                    }
                }
            }
       
    }

    public void adapticCommand(PRESET_TYPE type)
    {
        
        Debug.Log("PropManager ---- Adaptic  " + type.ToString());
        if (type == PRESET_TYPE.FLAT)
        {
            mySPort.Write("<1>");
        }
        else if (type == PRESET_TYPE.CYLINDER)
        {
            mySPort.Write("<2>");
        }
        else if (type == PRESET_TYPE.BOOK)
        {
            mySPort.Write("<3>");
        }
        mySPort.DiscardOutBuffer();
    }

    public string readData()
    {
        string data = "";
        //for(int i =0; i<10000 && !string.Equals(data,"OK"); i++)
            try
            {
                data = mySPort.ReadLine();
            }
            catch(Exception e)
            {

            }

        return data;
    }
}
