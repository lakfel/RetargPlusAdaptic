using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerMannager : MonoBehaviour
{
    public enum TRCKER_SIDE { LEFT, RIGHT};

    public HydraTracker leftTracker;
    public HydraTracker rightTracker;

    public PropController leftProp;
    public PropController rightProp;

    private MasterController masterController;
    // Start is called before the first frame update
    void Start()
    {
        masterController = gameObject.GetComponent<MasterController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setTrackers()
    {
        masterController = gameObject.GetComponent<MasterController>();
        if (masterController.currentStage == MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING || masterController.currentStage == MasterController.EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING)
        {
            leftProp.hydraTracker = leftTracker;
            rightProp.hydraTracker = leftTracker;
        }
        else
        {
            leftProp.hydraTracker = leftTracker;
            rightProp.hydraTracker = rightTracker;
        }
    }
}
