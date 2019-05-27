using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TastePlugin : MonoBehaviour
{
    // Start is called before the first frame update


        public float nearRange = 0f, nearVal = 0f, farRamge = 0f, farVal = 0f;
    void Start()
    {
        baseActivated = 0;
        SixensePlugin.sixenseGetFilterParams(ref nearRange, ref nearVal, ref farRamge, ref farVal);

    }
    public int baseActivated;
    // Update is called once per frame
    void Update()
    {
       // Debug.Log("BASES>>>>>  " + SixensePlugin.sixenseGetMaxBases());
        Debug.Log("BASE  0>>>>>  " + SixensePlugin.sixenseIsBaseConnected(0));
        Debug.Log("BASE  1>>>>>  " + SixensePlugin.sixenseIsBaseConnected(1));
        
        Debug.Log("Controllersss   -> " + SixensePlugin.sixenseGetNumActiveControllers());
        SixensePlugin.sixenseControllerData cd = new SixensePlugin.sixenseControllerData();
        for (int i = 0; i < 4; i++)
        {
            Debug.Log("Controller   -> " + i + "-> " + SixensePlugin.sixenseIsControllerEnabled(i));
            if (SixensePlugin.sixenseIsControllerEnabled(i) == 1)
                {
                    SixensePlugin.sixenseGetNewestData(i, ref cd);
                Debug.Log("DATA INF FREQUENCY --- NUM>" + " // FREQ>" + cd.magnetic_frequency);
                }
                
          }


            if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("ACTIVATED>>>>>  " + baseActivated);

            SixensePlugin.sixenseSetActiveBase(baseActivated);
            SixensePlugin.sixenseSetFilterEnabled(baseActivated);
            if(baseActivated == 0)
                SixensePlugin.sixenseSetBaseColor(0, 1, 0);
            else
                SixensePlugin.sixenseSetBaseColor(0, 0, 1);


            SixensePlugin.sixenseGetFilterParams(ref nearRange, ref nearVal, ref farRamge, ref farVal);

            // SixensePlugin.sixenseSetFilterParams( nearRange,  nearVal,  farRamge,  farVal);

            Debug.Log("FILTER >>>>>  NearRange : " + nearRange + " <> NearVal : " + nearVal + " <> farRamge : " + farRamge + " <> farVal : " + farVal);

        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("APAGO >>>>" + SixensePlugin.sixenseExit());


            
        }



        }
}
