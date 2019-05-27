using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterController : MonoBehaviour
{


    private PersistanceManager persistanceManager;
    private TrackerMannager trackerMannager;
    

    public bool handOnObject;
    public bool handOnInitialPosition;
    public bool objectCloseToDock;

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
   

    private int stageCounter;

    public EXP_STAGE currentStage;

    // Start is called before the first frame update
    void Start()
    {
        handOnObject = false;
        persistanceManager = gameObject.GetComponent<PersistanceManager>();
        trackerMannager = gameObject.GetComponent<TrackerMannager>();

        started = false;
        if (notificacionTextObject != null)
            notificacionTextObject.SetActive(true);

        stageCounter = 0;
        stagesDone = new bool[5];
        for(int i = 0; i<5; i ++)
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

        Debug.Log("-----------Waiting---------");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!started)
            {
                started = true;

                currentStage = EXP_STAGE.TUTORIAL;
                stagesDone[0] = true;

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
    }

    public void nextStage()
    {
        if(stageCounter <= 4)
        {
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
    }

    public bool conditionsToProceed(int stage   )
    {
        bool answer = false;
        if(stage == 0)
        {
            answer = true;
        }
        else if (stage == 1)
        {
            answer = handOnObject;
        }
        else if (stage == 2)
        {
            answer = objectCloseToDock;
        }
        else if (stage == 3)
        {
            answer = objectCloseToDock;
        }
        else if (stage == 4)
        {

        }

        return answer;
    }


}
