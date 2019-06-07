using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraSettings : MonoBehaviour
{

    public bool refresh;
    public bool update;
    public bool autoEnable;
    // Start is called before the first frame update
    public int controller;
    public int stateHemisphereTrackingMode;
    public int enableHemisphereTracking;
    public int highPriotiyuHemisphere;
    public int filterActivated;
    public float filterNearRange;
    public float filterNearVal;
    public float filterFarRang;
    public float filterFarVal;
    public byte[] color;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(refresh)
        {
            refresh = false;
            SixensePlugin.sixenseGetHemisphereTrackingMode(controller, ref stateHemisphereTrackingMode);
            SixensePlugin.sixenseGetHighPriorityBindingEnabled(ref highPriotiyuHemisphere);
            SixensePlugin.sixenseGetFilterEnabled(ref filterActivated);
            SixensePlugin.sixenseGetFilterParams(ref filterNearRange, ref filterNearVal, ref filterFarRang, ref filterFarVal);
            SixensePlugin.sixenseGetBaseColor(ref color[0], ref color[1], ref color[2]);

        }
        if(update)
        {
            update = false;
            SixensePlugin.sixenseSetHemisphereTrackingMode(controller,  stateHemisphereTrackingMode);
            SixensePlugin.sixenseSetHighPriorityBindingEnabled( highPriotiyuHemisphere);
            SixensePlugin.sixenseSetFilterEnabled( filterActivated);
            SixensePlugin.sixenseSetFilterParams( filterNearRange,  filterNearVal,  filterFarRang,  filterFarVal);
            SixensePlugin.sixenseSetBaseColor( color[0],  color[1],  color[2]);
        }
        if (autoEnable)
        {
            autoEnable = false;
            SixensePlugin.sixenseAutoEnableHemisphereTracking(controller);
        }
    }
}
