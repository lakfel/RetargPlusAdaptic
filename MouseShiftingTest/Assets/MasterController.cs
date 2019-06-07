using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{


    private PersistanceManager persistanceManager;
    private TrackerMannager trackerMannager;
    private NotificationsMannager notificationsMannager;
    private SurveyMannager surveyMannager;
    private Logic logic;
    public bool surveyActivated;

    

    public bool started;

    public int subjectOrder;

    public readonly int[,] ordersStages = new int[,] { { 1, 2, 4, 3 } ,  {2, 3, 1, 4 } , { 3, 4, 2, 1 } , { 4, 1, 3, 2 } }; //balanced Latin square

    public enum EXP_STAGE
    {
        TUTORIAL,
        PROP_MATCHING_PLUS_RETARGETING,
        PROP_MATCHING_NO_RETARGETING,
        PROP_NOT_MATCHING_PLUS_RETARGETING,
        PROP_NOT_MATCHING_NO_RETARGETING
    }



    public GameObject notificacionTextObject;

    private bool[] stagesDone;
    private EXP_STAGE[] stages;
   

    public int stageCounter;

    public EXP_STAGE currentStage;

    // Start is called before the first frame update
    void Start()
    {
        
        persistanceManager = gameObject.GetComponent<PersistanceManager>();
        trackerMannager = gameObject.GetComponent<TrackerMannager>();
        notificationsMannager = gameObject.GetComponent<NotificationsMannager>();
        surveyMannager = gameObject.GetComponent<SurveyMannager>();
        logic = gameObject.GetComponent<Logic>();

        surveyActivated = false;
        setNew();

        

        //Debug.Log("-----------Waiting---------");
    }

    public void setNew()
    {
        started = false;
        if (notificacionTextObject != null)
            notificacionTextObject.SetActive(true);

        stageCounter = 0;
        stagesDone = new bool[5];
        for (int i = 0; i < 5; i++)
        {
            stagesDone[i] = false;
        }

        stages = new EXP_STAGE[]
        {
            EXP_STAGE.TUTORIAL,
            EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING,
            EXP_STAGE.PROP_MATCHING_NO_RETARGETING,
            EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING,
            EXP_STAGE.PROP_NOT_MATCHING_NO_RETARGETING
        };
        persistanceManager.recording = false;
        persistanceManager.userId = System.DateTime.Now.ToString("yyMMddHHmmss");
        TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
        text.text = "Welcome";
    }

    // Update is called once per frame
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        //if(OVRInput.GetDown(OVRInput.Button.One))
        {


            if (!started)
            {
                started = true;

                currentStage = EXP_STAGE.TUTORIAL;
                stagesDone[0] = true;

                notificationsMannager.lightStepNotification(1);

                if (notificacionTextObject != null)
                {
                    TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
                    text.text = "TUTORIAL";
                }

                if (persistanceManager != null)
                    persistanceManager.saveGeneral();
                else
                    Debug.LogError("PersistanceMannager missing!!! No results reported");
                //Call persistance to update
            }
            else
                nextStage();

            trackerMannager.setTrackers();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            setNew();
            notificationsMannager.normalSettings();
            persistanceManager.recording = false;

            GameObject leftProp = GameObject.Find("LeftProp");
            if (leftProp != null)
                leftProp.GetComponent<PropController>().angleNumber = 0;

            GameObject rightProp = GameObject.Find("RightProp");
            if (rightProp != null)
                rightProp.GetComponent<PropController>().angleNumber = 0;

        }
    }

    public void nextStage()
    {
        persistanceManager.recording = true;
        if(stageCounter < 4)
        {
            logic.stage = -1;
            stageCounter++;
            int newStage = ordersStages[subjectOrder, stageCounter-1];
            stagesDone[newStage] = true; 
            currentStage = stages[newStage];
            if (notificacionTextObject != null)
            { 
                TextMesh text = notificacionTextObject.GetComponent<TextMesh>();
                text.text = "Stage number : " + stageCounter;  
            }

        }
        if (stageCounter >= 2 && stageCounter <= 5)
        {
            surveyMannager.startSurvey(stageCounter, currentStage.ToString("G"));
        }
    }

    


}
