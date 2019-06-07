using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR;

public class OVRCompensate : MonoBehaviour
{

    public Vector3 LeftTracker;
    public Vector3 RightTracker;

    public bool Configured;

    public Transform VRCamHolder; //parent of the cam
    public float WaitBeforeCompensation = 0.5f;
    Vector3 deltaPos;
    Vector3 tmp;

    // Use this for initialization
    void Start()
    {
        Configured = false;
        // StartCoroutine(Compensate());
        
    }
    private void Update()
    {
        if(!Configured )
        {
            for (int i = 0; i <= OVRManager.tracker.count ; i++)
            {
                if (OVRManager.tracker.GetPoseValid(i) )//)&& !Configured)
                {
                    Configured = true;
                    if(OVRManager.tracker.GetPose(i).position.x < 0)
                        transform.position = LeftTracker - OVRManager.tracker.GetPose(i).position + GameObject.Find("MiddlePositionConfiguration").transform.position;
                    else
                        transform.position = RightTracker -  OVRManager.tracker.GetPose(i).position  + GameObject.Find("MiddlePositionConfiguration").transform.position;
                }
                OVRPose tp = OVRManager.tracker.GetPose(i);
                
                //Debug.Log("TRACKER " + i + " -> " + OVRManager.tracker.GetPoseValid(i) + "->X:" + tp.position.x + " **  Y:" + tp.position.y + " **  Z:" + tp.position.z);
            }
           
        }
       
    }
    //    IEnumerator Compensate()
    //   {
    /*   yield return new WaitForSeconds(WaitBeforeCompensation);
       deltaPos = InputTracking.GetLocalPosition(VRNode.Head);
       tmp = Vector3.zero;
       tmp.x = deltaPos.x;
       tmp.y = deltaPos.y * -1f;
       tmp.z = deltaPos.z;
       VRCamHolder.position += tmp;*/
    // }
}
