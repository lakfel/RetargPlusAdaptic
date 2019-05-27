using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Networking;

public class PersistanceManager : MonoBehaviour
{

    //Master
    private MasterController masterController;

    // Right hand of leapmotion. need to know position to log
    public CapsuleHand trackedHand;
    
    // Reference to the actual object to pick
    public PropController trackedObject;


    //Camera Reference
    private GameObject ovrCamera;

    // Id of the current trial
    public string userId;

    // Id of movement
    public string movementId;

    // Stage. ith or without proxy's match, with or without retargetting or tutorial
    public string currentStage;

    //  0 hand point z to object, 1 object original point to point z, 2 object from point z to original point, 3 hand from original point to point z
    public int typeMovement;


    public string stage;
    public string condition;

    // Counter of movements in this experiment
    public int counterMovements;
    
    // Stopwatch to measure times
    private Stopwatch sw;

    
    public const string URL_GENERAL = "https://docs.google.com/forms/d/e/1FAIpQLSfMmM6OOVKMTr-TNzcqTkuIKumgu0mkqW9PzL7WI32A5KXRmQ/formResponse";
    public const string URL_DETAIL_DOCK = "https://docs.google.com/forms/d/e/1FAIpQLSdQJULfmvnaVGUCQdgcrnlrMrQoNdhziewxmILRUHpDGg7_gA/formResponse";
    public const string URL_RUN_TIME = "https://docs.google.com/forms/d/e/1FAIpQLSc-4fe004Qn46P4JEh1KbL0y0UemxLVWmS9PSiFq8wj_VURUA/formResponse";
    public const string URL_HEAD_POSE = "https://docs.google.com/forms/d/e/1FAIpQLSdCz8XIg1Te2mTkTMoCznQ5lRuNqFuXtRxAnPx-ltefgTbcAw/formResponse";

    // General Fields 
    public const string GEN_ID = "entry.2052369808";
    public const string GEN_TIME_STAMP = "entry.1655917028";
    public const string GEN_ORDER_STAGES = "entry.252353393"; // We have to set this order.

    // Detail Dock Fields
    public const string DET_ID_TRIAL = "entry.92399919";
    public const string DET_ID_MOVEMENT = "entry.697696657";
    public const string DET_STAGE = "entry.569595674";
    public const string DET_TIME_STAMP = "entry.1393716351";
    public const string DET_TIME = "entry.1045933254";
    public const string DET_TYPE_OF_MOVEMENT = "entry.679613488";
    public const string DET_GOAL_POS_X = "entry.1987913033";
    public const string DET_GOAL_POS_Y = "entry.1300502039";
    public const string DET_FINAL_POS_X = "entry.1711231828";
    public const string DET_FINAL_POS_Y = "entry.752608953";
    public const string DET_GOAL_POS_Z = "entry.89087735";
    public const string DET_FINAL_POS_Z = "entry.1004315462";
    public const string DET_GOAL_ROT_QX = "entry.1252628235";
    public const string DET_GOAL_ROT_QY = "entry.1660182423";
    public const string DET_GOAL_ROT_QZ = "entry.1060958952";
    public const string DET_GOAL_ROT_QW = "entry.727938622";
    public const string DET_FINAL_ROT_QX = "entry.1450331604";
    public const string DET_FINAL_ROT_QY = "entry.1099651534";
    public const string DET_FINAL_ROT_QZ = "entry.1774605920";
    public const string DET_FINAL_ROT_QW = "entry.2144835198";

    // Run time fields. Hand and object positions
    public const string RT_ID_TRIAL = "entry.727967645";
    public const string RT_ID_MOVEMENT = "entry.811823153";
    public const string RT_TIME_STAMP = "entry.1319825401";
    public const string RT_REAL_POS_X = "entry.2065265783";
    public const string RT_REAL_POS_Y = "entry.99196264";
    public const string RT_REAL_POS_Z = "entry.86141721";
    public const string RT_RET_POS_X = "entry.733490593";
    public const string RT_RET_POS_Y = "entry.1100701781";
    public const string RT_RET_POS_Z = "entry.262216847";
    public const string RT_TRACK_REAL_POS_X = "entry.2088637180";
    public const string RT_TRACK_REAL_POS_Y = "entry.1909432453";
    public const string RT_TRACK_REAL_POS_Z = "entry.619119828";
    public const string RT_TRACK_RET_POS_X = "entry.1883869650";
    public const string RT_TRACK_RET_POS_Y = "entry.344764315";
    public const string RT_TRACK_RET_POS_Z = "entry.1221800307";
    public const string RT_TRACK_ROT_QUA_X = "entry.49311802";
    public const string RT_TRACK_ROT_QUA_Y = "entry.655326122";
    public const string RT_TRACK_ROT_QUA_Z = "entry.23905528";
    public const string RT_TRACK_ROT_QUA_W = "entry.543299155";
    public const string RT_GRASPING = "entry.1210016417";
    public const string RT_HAND_DETECTED = "entry.790516061";


    // Head pose pos and rot
    public const string HP_ID_TRIAL = "entry.210934768";
    public const string HP_ID_MOVEMENT = "entry.295098166";
    public const string HP_TIME_STAMP = "entry.1042133585";
    public const string HP_POS_X = "entry.1781128447";
    public const string HP_POS_Y = "entry.1050296081";
    public const string HP_POS_Z = "entry.193860193";
    public const string HP_ROT_QUA_X = "entry.494952291";
    public const string HP_ROT_QUA_Y = "entry.281682147";
    public const string HP_ROT_QUA_Z = "entry.2057307761";
    public const string HP_ROT_QUA_W = "entry.910120948";

    public Dictionary<string, string[]> urlFields;
        
        //public const string URL_SUMMARY;
 


    IEnumerator Post(string urlForm, string[] entries, string[] values)
    {
        WWWForm form = new WWWForm();
        //UnityEngine.Debug.Log("-----------SENDING .... ---------" + urlForm);
        if (entries.Length != values.Length)
        {
           // UnityEngine.Debug.LogError("Error> Saving logs of data for url " + urlForm);
        }
        else
        {
            for(int  i = 0; i< entries.Length; i++)
            {
                form.AddField(entries[i], values[i]);
               // UnityEngine.Debug.Log("---Field >" + entries[i] + " >>><<< Values> " + values[i]);
            }
           // form.AddField("entry.1064445353", email);

            byte[] rawData = form.data;

            string url = urlForm;
            // Post a request to an URL with our custom headers
            //UnityWebRequest 
           // UnityWebRequest unityWebRequest = UnityWebRequest.Get(url, form);
            //unityWebRequest.
            WWW www = new WWW(url, rawData);
            yield return www;
            //UnityEngine.Debug.Log("WWW Request Response > " + www.responseHeaders.ToArrayString().ToString());
            //if (unityWebRequest.isNetworkError || unityWebRequest.isNetworkError)
            //  UnityEngine.Debug.Log("ERROR IN FORM --- " + unityWebRequest.error);
        }
    }

    private IEnumerator saveRecords()
    {
        while(true)
        {
            if (masterController.started)
            {
                saveRunTime();
                saveHeadPose();
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    private void Update()
    {
       
        
    }

    private void Start()
    {
        ovrCamera = GameObject.Find("CenterEyeAnchor");
        counterMovements = 1;
        userId  =  System.DateTime.Now.ToString("yyMMddHHmmss");
        masterController = gameObject.GetComponent<MasterController>();
       // StartCoroutine(saveRecords());
        //TODO 
    }

    public void saveGeneral( )
    {
        //TODO How preset the order of stages
        UnityEngine.Debug.Log("-----------General record ---------");
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
        string url = URL_GENERAL;
        string[] entries = { GEN_ID, GEN_TIME_STAMP };
        string[] values = { userId, timeStamp };
        StartCoroutine(Post(url, entries, values));
        UnityEngine.Debug.Log("-----------General record Done---------");
    }

    public void startDocking(int partMovement)
    {
        sw = Stopwatch.StartNew();
        counterMovements++;
        movementId = System.DateTime.Now.ToString("HHmmss") +"N" + counterMovements;
        currentStage = masterController.currentStage.ToString("G");
    }

    public void saveDocking()
    {
        // textId
        Vector3 goalPosition = Vector3.zero;
        Quaternion goalRotation = Quaternion.identity;
        Vector3 finalPosition = Vector3.zero;
        Quaternion finalRotation = Quaternion.identity;

        if (trackedObject != null)
        {
            finalPosition = trackedObject.gameObject.transform.position;
            finalRotation = trackedObject.gameObject.transform.rotation;
            GameObject goalObj = trackedObject.dockProp;
            if(goalObj != null && (typeMovement == 1 || typeMovement == 2))
            {
                goalPosition = goalObj.transform.position;
                goalRotation = goalObj.transform.rotation;
            }
        }

        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
        string[] entries = { DET_ID_TRIAL, DET_ID_MOVEMENT, DET_STAGE, DET_TIME_STAMP, DET_TIME, DET_TYPE_OF_MOVEMENT,
                            DET_GOAL_POS_X, DET_GOAL_POS_Y, DET_GOAL_POS_Z, DET_FINAL_POS_X, DET_FINAL_POS_Y, DET_FINAL_POS_Z,
                            DET_GOAL_ROT_QX,DET_GOAL_ROT_QY,DET_GOAL_ROT_QZ,DET_GOAL_ROT_QW, DET_FINAL_ROT_QX, DET_FINAL_ROT_QY, DET_FINAL_ROT_QZ, DET_FINAL_ROT_QW};
        string[] values = { userId, movementId, currentStage, timeStamp, sw.ElapsedMilliseconds.ToString(), typeMovement.ToString(),
                            goalPosition.x.ToString(), goalPosition.y.ToString() , goalPosition.z.ToString() , finalPosition.x.ToString(), finalPosition.y.ToString() , finalPosition.z.ToString(),
                            goalRotation.x.ToString(), goalRotation.y.ToString() , goalRotation.z.ToString() , goalRotation.w.ToString(), finalRotation.x.ToString(), finalRotation.y.ToString() , finalRotation.z.ToString() , finalRotation.w.ToString() };
        sw.Stop();
        string url = URL_DETAIL_DOCK;
        StartCoroutine(Post(url, entries, values));

    }

    public void saveRunTime()
    {

        Vector3 handPos = Vector3.zero;
        Vector3 handRetPos = Vector3.zero;
        Vector3 trackPos = Vector3.zero;
        Vector3 trackPosRet = Vector3.zero;
        Quaternion trackRot = Quaternion.identity;
        bool grasping = false;
        bool handDetected = false;

        if(trackedHand != null)
        {
            if(trackedHand.gameObject.activeSelf)
            {
                handDetected = true;
                handPos = trackedHand.getHandPosition();
                handRetPos = trackedHand.PositionPalmUpdatedRet;
            }
        }
        if(trackedObject != null)
        {
            trackPos = trackedObject.RealPosition;
            trackPosRet = trackedObject.transform.position;
            trackRot = trackedObject.transform.rotation;
            handDetected = trackedObject.IsMoving;
        }


        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");

        string[] entries = { RT_ID_TRIAL, RT_ID_MOVEMENT, RT_TIME_STAMP,
                             RT_REAL_POS_X, RT_REAL_POS_Y, RT_REAL_POS_Z, RT_RET_POS_X, RT_RET_POS_Y, RT_RET_POS_Z
                            ,RT_TRACK_REAL_POS_X ,RT_TRACK_REAL_POS_Y,RT_TRACK_REAL_POS_Z, RT_TRACK_RET_POS_X, RT_TRACK_RET_POS_Y, RT_TRACK_RET_POS_Z
                            ,RT_TRACK_ROT_QUA_X,RT_TRACK_ROT_QUA_Y,RT_TRACK_ROT_QUA_Z,RT_TRACK_ROT_QUA_W, RT_GRASPING, RT_HAND_DETECTED};

        string[] values = { userId, movementId, timeStamp,
                            handPos.x.ToString(),handPos.y.ToString(),handPos.z.ToString(), handRetPos.x.ToString(),handRetPos.y.ToString(),handRetPos.z.ToString(),
                            trackPos.x.ToString(),trackPos.y.ToString(),trackPos.z.ToString(), trackPosRet.x.ToString(), trackPosRet.y.ToString(), trackPosRet.z.ToString(),
                            trackRot.x.ToString(),trackRot.y.ToString(),trackRot.z.ToString(),trackRot.w.ToString(), grasping.ToString(), handDetected.ToString()
                            };
        string url = URL_RUN_TIME;
        StartCoroutine(Post(url, entries, values));
    }

    public void saveHeadPose()
    {
        
        Vector3 hpPos = ovrCamera.transform.position;
        Quaternion hpRot = ovrCamera.transform.rotation;
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");

        string[] entries = { HP_ID_TRIAL, HP_ID_MOVEMENT, HP_TIME_STAMP,
                             HP_POS_X, HP_POS_Y, HP_POS_Z, HP_ROT_QUA_X, HP_ROT_QUA_Y, HP_ROT_QUA_Z, HP_ROT_QUA_W};

        string[] values = { userId, movementId, timeStamp,
                            hpPos.x.ToString(),hpPos.y.ToString(),hpPos.z.ToString(),hpRot.x.ToString(),hpRot.y.ToString(),hpRot.z.ToString(),hpRot.w.ToString() };

        string url = URL_HEAD_POSE;
        StartCoroutine(Post(url, entries, values));
    }


}
