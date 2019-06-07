using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class PersistanceManager : MonoBehaviour
{

    public bool storeInForms;
    public bool storeLocal;

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

    public string PATH_LOCAL = @"C:\Users\johannavila\Dropbox\Uniandes\Results\";
    public const string NAME_GENERAL = "general.csv";
    public const string NAME_DETAIL_DOCK = "detail_dock.csv";
    public const string NAME_RUN_TIME = "runtime.csv";
    public const string NAME_HEAD_POSE = "headpose.csv";
    public const string NAME_DIF_SURVEY = "difsurvey.csv";


    public const string URL_GENERAL = "https://docs.google.com/forms/d/e/1FAIpQLSfMmM6OOVKMTr-TNzcqTkuIKumgu0mkqW9PzL7WI32A5KXRmQ/formResponse";
    public const string URL_DETAIL_DOCK = "https://docs.google.com/forms/d/e/1FAIpQLSdQJULfmvnaVGUCQdgcrnlrMrQoNdhziewxmILRUHpDGg7_gA/formResponse";
    public const string URL_RUN_TIME = "https://docs.google.com/forms/d/e/1FAIpQLSc-4fe004Qn46P4JEh1KbL0y0UemxLVWmS9PSiFq8wj_VURUA/formResponse";
    public const string URL_HEAD_POSE = "https://docs.google.com/forms/d/e/1FAIpQLSdCz8XIg1Te2mTkTMoCznQ5lRuNqFuXtRxAnPx-ltefgTbcAw/formResponse";
    public const string URL_DIF_SURVEY = "https://docs.google.com/forms/d/e/1FAIpQLSeHHBdiIb6yrp6Z6KDKrLqwvIfOQ4D1D_lEU_FrPK4ZpXhoIA/formResponse";

    // General Fields 
    public const string GEN_ID = "entry.2052369808";
    public const string GEN_TIME_STAMP = "entry.1655917028";
    public const string GEN_ORDER_STAGES = "entry.252353393"; // We have to set this order.
    public const string HEADINGS_GEN_ID = "ID_USER";
    public const string HEADINGS_GEN_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_GEN_ORDER_STAGES = "ORDER_STAGES";


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

    public const string HEADINGS_DET_ID_TRIAL = "ID_TRIAL ";
    public const string HEADINGS_DET_ID_MOVEMENT = "ID_MOVEMENT ";
    public const string HEADINGS_DET_STAGE = "STAGE ";
    public const string HEADINGS_DET_TIME_STAMP = "TIME_STAMP ";
    public const string HEADINGS_DET_TIME = "TIME ";
    public const string HEADINGS_DET_TYPE_OF_MOVEMENT = "TYPE_OF_MOVEMENT ";
    public const string HEADINGS_DET_GOAL_POS_X = "GOAL_POS_X ";
    public const string HEADINGS_DET_GOAL_POS_Y = "GOAL_POS_Y ";
    public const string HEADINGS_DET_FINAL_POS_X = "FINAL_POS_X ";
    public const string HEADINGS_DET_FINAL_POS_Y = "FINAL_POS_Y ";
    public const string HEADINGS_DET_GOAL_POS_Z = "GOAL_POS_Z ";
    public const string HEADINGS_DET_FINAL_POS_Z = "FINAL_POS_Z ";
    public const string HEADINGS_DET_GOAL_ROT_QX = "GOAL_ROT_QX ";
    public const string HEADINGS_DET_GOAL_ROT_QY = "GOAL_ROT_QY ";
    public const string HEADINGS_DET_GOAL_ROT_QZ = "GOAL_ROT_QZ ";
    public const string HEADINGS_DET_GOAL_ROT_QW = "GOAL_ROT_QW ";
    public const string HEADINGS_DET_FINAL_ROT_QX = "FINAL_ROT_QX ";
    public const string HEADINGS_DET_FINAL_ROT_QY = "FINAL_ROT_QY ";
    public const string HEADINGS_DET_FINAL_ROT_QZ = "FINAL_ROT_QZ ";
    public const string HEADINGS_DET_FINAL_ROT_QW = "FINAL_ROT_QW ";


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

    public const string HEADINGS_RT_ID_TRIAL = "ID_TRIAL";
    public const string HEADINGS_RT_ID_MOVEMENT = "ID_MOVEMENT";
    public const string HEADINGS_RT_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_RT_REAL_POS_X = "REAL_POS_X";
    public const string HEADINGS_RT_REAL_POS_Y = "REAL_POS_Y";
    public const string HEADINGS_RT_REAL_POS_Z = "REAL_POS_Z";
    public const string HEADINGS_RT_RET_POS_X = "RET_POS_X";
    public const string HEADINGS_RT_RET_POS_Y = "RET_POS_Y";
    public const string HEADINGS_RT_RET_POS_Z = "RET_POS_Z";
    public const string HEADINGS_RT_TRACK_REAL_POS_X = "TRACK_REAL_POS_X";
    public const string HEADINGS_RT_TRACK_REAL_POS_Y = "TRACK_REAL_POS_Y";
    public const string HEADINGS_RT_TRACK_REAL_POS_Z = "TRACK_REAL_POS_Z";
    public const string HEADINGS_RT_TRACK_RET_POS_X = "TRACK_RET_POS_X";
    public const string HEADINGS_RT_TRACK_RET_POS_Y = "TRACK_RET_POS_Y";
    public const string HEADINGS_RT_TRACK_RET_POS_Z = "TRACK_RET_POS_Z";
    public const string HEADINGS_RT_TRACK_ROT_QUA_X = "TRACK_ROT_QUA_X";
    public const string HEADINGS_RT_TRACK_ROT_QUA_Y = "TRACK_ROT_QUA_Y";
    public const string HEADINGS_RT_TRACK_ROT_QUA_Z = "TRACK_ROT_QUA_Z";
    public const string HEADINGS_RT_TRACK_ROT_QUA_W = "TRACK_ROT_QUA_W";
    public const string HEADINGS_RT_GRASPING = "GRASPING";
    public const string HEADINGS_RT_HAND_DETECTED = "HAND_DETECTED";
    public const string HEADINGS_RT_CURR_STAGE = "CURR_STAGE";






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
    public const string HEADINGS_HP_ID_TRIAL = "ID_TRIAL";
    public const string HEADINGS_HP_ID_MOVEMENT = "ID_MOVEMENT";
    public const string HEADINGS_HP_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_HP_POS_X = "POS_X";
    public const string HEADINGS_HP_POS_Y = "POS_Y";
    public const string HEADINGS_HP_POS_Z = "POS_Z";
    public const string HEADINGS_HP_ROT_QUA_X = "ROT_QUA_X";
    public const string HEADINGS_HP_ROT_QUA_Y = "ROT_QUA_Y";
    public const string HEADINGS_HP_ROT_QUA_Z = "ROT_QUA_Z";
    public const string HEADINGS_HP_ROT_QUA_W = "ROT_QUA_W";
    public const string HEADINGS_HP_CURR_STAGE = "CURR_STAGE";



    //Dificulty survey
    public const string DS_ID_TRIAL = "entry.1068923774";
    public const string DS_TIME_STAMP = "entry.1388950401";
    public const string DS_PREV_STAGE = "entry.2080720688";
    public const string DS_CURR_STAGE = "entry.1019444180";
    public const string DS_QUESTION = "entry.921290749";
    public const string DS_ANSWER = "entry.379185453";

    public const string HEADINGS_DS_ID_TRIAL = "ID_TRIAL";
    public const string HEADINGS_DS_TIME_STAMP = "TIME_STAMP";
    public const string HEADINGS_DS_PREV_STAGE = "PREV_STAGE";
    public const string HEADINGS_DS_CURR_STAGE = "CURR_STAGE";
    public const string HEADINGS_DS_QUESTION = "QUESTION";
    public const string HEADINGS_DS_ANSWER = "ANSWER";
    public bool recording;

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
            UnityEngine.Debug.Log("WWW Request Response > " + www.responseHeaders.ToArrayString().ToString());
            UnityEngine.Debug.Log("WWW Request Response > " + www.text);
            UnityEngine.Debug.Log("WWW Request Response > " + www.error);
            //if (unityWebRequest.isNetworkError || unityWebRequest.isNetworkError)
            //  UnityEngine.Debug.Log("ERROR IN FORM --- " + unityWebRequest.error);
        }
    }

    private IEnumerator saveLocal(string path, string[] entries, string[] values)
    {
        StreamWriter sw = null;
        if (!File.Exists(path))
        {
            sw = new StreamWriter(path, true);
            sw.WriteLine(string.Join(";", entries));
        }
        sw = sw ?? new StreamWriter(path, true);

        sw.WriteLine(string.Join(";", values));
        sw.Close();
        yield return null;
    }
    private IEnumerator saveRecords()
    {
        while(true)
        {
            if (masterController.started && recording)
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
        recording = false;
        ovrCamera = GameObject.Find("CenterEyeAnchor");
        counterMovements = 1;
        userId  =  System.DateTime.Now.ToString("yyMMddHHmmss");
        masterController = gameObject.GetComponent<MasterController>();
        StartCoroutine(saveRecords());
        //TODO 
    }

    public void saveGeneral( )
    {
        //TODO How preset the order of stages
        //UnityEngine.Debug.Log("-----------General record ---------");
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");
        string url = URL_GENERAL;
        
        string order = string.Format("{0}-{1}-{2}-{3}", masterController.ordersStages[masterController.subjectOrder,0], masterController.ordersStages[masterController.subjectOrder, 1], masterController.ordersStages[masterController.subjectOrder, 2], masterController.ordersStages[masterController.subjectOrder, 3]);


        string[] entries = { GEN_ID, GEN_TIME_STAMP, GEN_ORDER_STAGES };
        string[] values = { userId, timeStamp, order };
        string pathLocal = PATH_LOCAL + NAME_GENERAL;
        string[] entries2 = { HEADINGS_GEN_ID, HEADINGS_GEN_TIME_STAMP, HEADINGS_GEN_ORDER_STAGES };

        if(storeInForms)
            StartCoroutine(Post(url, entries, values));
        if(storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
        //UnityEngine.Debug.Log("-----------General record Done---------");
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


        string pathLocal = PATH_LOCAL + NAME_DETAIL_DOCK;
        string[] entries2 = { HEADINGS_DET_ID_TRIAL, HEADINGS_DET_ID_MOVEMENT, HEADINGS_DET_STAGE, HEADINGS_DET_TIME_STAMP, HEADINGS_DET_TIME, HEADINGS_DET_TYPE_OF_MOVEMENT,
                            HEADINGS_DET_GOAL_POS_X, HEADINGS_DET_GOAL_POS_Y, HEADINGS_DET_GOAL_POS_Z, HEADINGS_DET_FINAL_POS_X, HEADINGS_DET_FINAL_POS_Y, HEADINGS_DET_FINAL_POS_Z,
                            HEADINGS_DET_GOAL_ROT_QX,DET_GOAL_ROT_QY, HEADINGS_DET_GOAL_ROT_QZ,DET_GOAL_ROT_QW, HEADINGS_DET_FINAL_ROT_QX, HEADINGS_DET_FINAL_ROT_QY, HEADINGS_DET_FINAL_ROT_QZ, HEADINGS_DET_FINAL_ROT_QW};

        if (storeInForms)
            StartCoroutine(Post(url, entries, values));
        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));

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
        }


        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");

        string[] entries = { RT_ID_TRIAL, RT_ID_MOVEMENT, RT_TIME_STAMP,
                             RT_REAL_POS_X, RT_REAL_POS_Y, RT_REAL_POS_Z, RT_RET_POS_X, RT_RET_POS_Y, RT_RET_POS_Z
                            ,RT_TRACK_REAL_POS_X ,RT_TRACK_REAL_POS_Y,RT_TRACK_REAL_POS_Z, RT_TRACK_RET_POS_X, RT_TRACK_RET_POS_Y, RT_TRACK_RET_POS_Z
                            ,RT_TRACK_ROT_QUA_X,RT_TRACK_ROT_QUA_Y,RT_TRACK_ROT_QUA_Z,RT_TRACK_ROT_QUA_W, RT_GRASPING, RT_HAND_DETECTED};

        string[] values = { userId, movementId, timeStamp,
                            handPos.x.ToString(),handPos.y.ToString(),handPos.z.ToString(), handRetPos.x.ToString(),handRetPos.y.ToString(),handRetPos.z.ToString(),
                            trackPos.x.ToString(),trackPos.y.ToString(),trackPos.z.ToString(), trackPosRet.x.ToString(), trackPosRet.y.ToString(), trackPosRet.z.ToString(),
                            trackRot.x.ToString(),trackRot.y.ToString(),trackRot.z.ToString(),trackRot.w.ToString(), grasping.ToString(), handDetected.ToString(), currentStage
                            };
        string url = URL_RUN_TIME;

        string pathLocal = PATH_LOCAL + NAME_RUN_TIME;
        string[] entries2 = { HEADINGS_RT_ID_TRIAL, HEADINGS_RT_ID_MOVEMENT, HEADINGS_RT_TIME_STAMP,
                             HEADINGS_RT_REAL_POS_X, HEADINGS_RT_REAL_POS_Y, HEADINGS_RT_REAL_POS_Z, HEADINGS_RT_RET_POS_X, HEADINGS_RT_RET_POS_Y, HEADINGS_RT_RET_POS_Z
                            , HEADINGS_RT_TRACK_REAL_POS_X , HEADINGS_RT_TRACK_REAL_POS_Y, HEADINGS_RT_TRACK_REAL_POS_Z, HEADINGS_RT_TRACK_RET_POS_X, HEADINGS_RT_TRACK_RET_POS_Y, HEADINGS_RT_TRACK_RET_POS_Z
                            , HEADINGS_RT_TRACK_ROT_QUA_X, HEADINGS_RT_TRACK_ROT_QUA_Y, HEADINGS_RT_TRACK_ROT_QUA_Z, HEADINGS_RT_TRACK_ROT_QUA_W, HEADINGS_RT_GRASPING, HEADINGS_RT_HAND_DETECTED, HEADINGS_RT_CURR_STAGE};

        if (storeInForms)
            StartCoroutine(Post(url, entries, values));
        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
    }

    public void saveHeadPose()
    {
        
        Vector3 hpPos = ovrCamera.transform.position;
        Quaternion hpRot = ovrCamera.transform.rotation;
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");

        string[] entries = { HP_ID_TRIAL, HP_ID_MOVEMENT, HP_TIME_STAMP,
                             HP_POS_X, HP_POS_Y, HP_POS_Z, HP_ROT_QUA_X, HP_ROT_QUA_Y, HP_ROT_QUA_Z, HP_ROT_QUA_W};

        string[] values = { userId, movementId, timeStamp,
                            hpPos.x.ToString(),hpPos.y.ToString(),hpPos.z.ToString(),hpRot.x.ToString(),hpRot.y.ToString(),hpRot.z.ToString(),hpRot.w.ToString(), currentStage };

        string url = URL_HEAD_POSE;


        string pathLocal = PATH_LOCAL + NAME_HEAD_POSE;
        string[] entries2 = { HEADINGS_HP_ID_TRIAL, HEADINGS_HP_ID_MOVEMENT, HEADINGS_HP_TIME_STAMP,
                             HEADINGS_HP_POS_X, HEADINGS_HP_POS_Y, HEADINGS_HP_POS_Z, HEADINGS_HP_ROT_QUA_X, HEADINGS_HP_ROT_QUA_Y, HEADINGS_HP_ROT_QUA_Z, HEADINGS_HP_ROT_QUA_W, HEADINGS_HP_CURR_STAGE};

        if (storeInForms)
            StartCoroutine(Post(url, entries, values));
        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
    }

    public void saveSurveyResponse(string prevStage, string currStage, string question, string answer)
    {

        
        string timeStamp = System.DateTime.Now.ToString("HH:mm:ss dd/MM/yy");

        string[] entries = { DS_ID_TRIAL, DS_TIME_STAMP, DS_PREV_STAGE,
                             DS_CURR_STAGE, DS_QUESTION, DS_ANSWER};

        string[] values = { userId, timeStamp,
                            prevStage, currStage, question , answer };

        string url = URL_DIF_SURVEY;

        string pathLocal = PATH_LOCAL + NAME_DIF_SURVEY;
        string[] entries2 = { HEADINGS_DS_ID_TRIAL, HEADINGS_DS_TIME_STAMP, HEADINGS_DS_PREV_STAGE,
                             HEADINGS_DS_CURR_STAGE, HEADINGS_DS_QUESTION, HEADINGS_DS_ANSWER};

        if (storeInForms)
            StartCoroutine(Post(url, entries, values));
        if (storeLocal)
            StartCoroutine(saveLocal(pathLocal, entries2, values));
    }


}
