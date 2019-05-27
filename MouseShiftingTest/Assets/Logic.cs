using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Leap.Unity;
using System.Threading;

public class Logic : MonoBehaviour
{
    // Reference to goals. 3 props in front of the user.
    // 0. Left 1. Middle 2 Right
    public GameObject [] props;

    // Numbre of the current goal. TODO// Change the int for the object?
    public int goal; //   0 , 1,2 

    // Tells if a goal prop is currently being showed
    public bool showing;

    // Tells in wich stage we are
    // 0 None object is showind, 1 Object is showing and going to point Z, 2 object is showing and going to normal position.
    public int stage;


    public CapsuleHand capsuleHand;
    // Start is called before the first frame update

    // Initial point for the hand
    public GameObject initialPoint;


    // Comunication with the master object
    private GameObject master;
    private TargetedController targetedController;
    private Logic logic;
    private PersistanceManager persistanceManager;
    private TrackerMannager trackerMannager;
    private MasterController masterController;

    // Current Propcontroller
    private PropController propContr;

    void Start()
    {

        master = GameObject.Find("Master");
        masterController = master.GetComponent<MasterController>();
        targetedController = master.GetComponent<TargetedController>();
        persistanceManager = master.GetComponent<PersistanceManager>();
        trackerMannager = master.GetComponent<TrackerMannager>();

        goal = 0;
        for (int i = 0; i < 2; i++)
            props[i].SetActive(false);
        showing = false;
        stage = -1;

        propContr = null;
        triggerPressed = false;

    }

    // Handle the press of the trigger.
    private bool triggerPressed;
    // Update is called once per frame
    void Update()
    {
        
        if(!triggerPressed)
        {
            if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.7f)
            {
                triggerPressed = true;
                if(masterController.started)
                    reGoal();
            }
        }
        else
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) < 0.7f)
            {
                triggerPressed = false;
            }
        }
        

    }


    private IEnumerator pairTracker(int seconds, PropController controller)
    {

        controller.gameObject.SetActive(true);
        controller.activeChildren(false);
        controller.hydraTracker.PositionReference = controller;
        controller.hydraTracker.attach();
        yield return new WaitForSeconds(seconds);
        controller.activeChildren(true);
        controller.movePropDock(true);
        controller.objectGreen(false);
        controller.dockProp.SetActive(false);
        //yield return null;
    }

    // It controls ALL the movements. The trigger is actionated by the Master for each pressed trigger done by the user
    // To every task completly, it has to start in point Z (InitialStatus Object). The process as follow
    // Stage starts at 0.
    // If hand in initialstatus and trigger. Initialstatus object dissapears, the object to pick shows up. Stage = 0
    // When object picked, Second trigger shadow for next movement appears. Stage = 1
    // when object realeased, then trigger, shadows dissapear for pioint zero and appears in starts position. Stage = 2
    // When object in initial position and trigger objects dissapears and user move hand back to point zero. Stage = 3
    // When hand in initial position and trigger, movements ends. Stage = -1
    public void reGoal()
    {
      
        if(stage == -1)
        {
            initialPoint.SetActive(true);
            stage = 0;
        }
        else if (stage == 0) // Hand in point Zero. Pre. No object to pick. Pos Object showed up initial position.
        {
            masterController.handOnObject = false;
            int nGoal;
            while ((nGoal = Random.Range(0, 2)) == goal) ; 
            
            goal = nGoal;
            //goal = 0;

            this.propContr = props[goal].GetComponent<PropController>();
            StartCoroutine(pairTracker(1, this.propContr));

            persistanceManager.trackedObject = this.propContr;
            if(masterController.currentStage == MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING)
            { 
                string result = this.propContr.preSetShape(); // TODO The way to attach the object is still bad.     
            }

            
            //TODO trackers. How mannage 3 trackers. This part should dissapear since all trackers should be in the scene so we should not need to look for the tracker everytime
           /* GameObject tracker1 = GameObject.Find("Tracker1");
            if(tracker1 != null)
            {
                HydraTracker hTracker = tracker1.GetComponent<HydraTracker>();
                if(hTracker != null)
                {
                    //hTracker.attach(propContr);
                    hTracker.PositionReference = this.propContr;
                    if (masterController.currentStage != MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING)
                    {
                        hTracker.attach();
                    }

                }
            }*/

            GameObject PositionReference = propContr.positionReference;
            targetedController.starShifting(PositionReference.transform.position, capsuleHand.GetLeapHand().PalmPosition.ToVector3()); //Capsulehand has a simplified methos for giving the hand. does not work here?
            if (masterController.currentStage == MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING || masterController.currentStage == MasterController.EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING)
            {
                props[goal].transform.position = targetedController.retargetedPosition.transform.position;
            }
            else
            {
                props[goal].transform.position = PositionReference.transform.position;
            }
            //props[goal].SetActive(true);
            persistanceManager.startDocking(stage);
            initialPoint.SetActive(false);
            stage = 1;

        }
        else if(stage ==1) // Hand in object Maybe should check the coliders are overlapped. Pre No shadow in scene. Pos shadow in point Z
        {
            persistanceManager.saveDocking();
            propContr.dockProp.SetActive(true);
            persistanceManager.startDocking(stage);
            stage = 2;
        }
        else if (stage == 2) // Object moved in desired position i point Z. Pre Shadow in point Z. Pos shadow in initial position
        {
            persistanceManager.saveDocking();
            propContr.movePropDock(false);
            persistanceManager.startDocking(stage);
            stage = 3;
        }
        else if (stage == 3) // Object moved to initial position in desired orientation. Pre. Shadow on, initial status off. Pos Shadow off, initial position on
        {
            persistanceManager.saveDocking();
            propContr.dockProp.SetActive(false);
            initialPoint.SetActive(true);
            persistanceManager.startDocking(stage);
            /*
            GameObject tracker1 = GameObject.Find("Tracker1");
            if (tracker1 != null)
            {
                HydraTracker hTracker = tracker1.GetComponent<HydraTracker>();
                if (hTracker != null)
                {
                    //hTracker.attach(propContr);
                    hTracker.PositionReference = this.propContr;
                    if (masterController.currentStage != MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING)
                    {
                        hTracker.detach();
                    }

                }
            }
            */
            propContr.hydraTracker.detach();
            propContr.gameObject.SetActive(false);

            stage = 4;
        }
        else if (stage == 4) // Hand moves to point z
        {
            persistanceManager.saveDocking();
            persistanceManager.startDocking(stage);
            initialPoint.SetActive(false);
            stage = -1;
        }
    }


    //Detach the tracker to the object. This should actually never be done here. It depends on the stage TODO BIG
    public void pickedDone()
    {
        GameObject tracker1 = GameObject.Find("Tracker1");
        if (tracker1 != null)
        {
            HydraTracker hTracker = tracker1.GetComponent<HydraTracker>();
            if (hTracker != null)
            {
                hTracker.detach();
            }
        }
        showing = false;
    }

    void delay(int secs)
    {
        Thread.Sleep(2000);
    }

}
