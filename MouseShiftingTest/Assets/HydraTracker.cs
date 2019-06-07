using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraTracker : MonoBehaviour
{
    public CapsuleHand capsuleHand;

    public bool reatach;

    private PropController positionReference;
    public GameObject realWorldReference;

    private Vector3 deltaPosition;

    Vector3 m_baseOffset;
    Quaternion m_baseOffset_Rot;
    public float m_sensitivity = 0.00017f; // Sixense units are in mm
    bool m_bInitialized;
    bool atached;
    public int orientation;

    public Vector3 trackerOffset;

    public float m_sensitivityX;
    public float m_sensitivityY;
    public float m_sensitivityZ;



    Vector3 initialPosition;
    Quaternion initialRotation;
    Quaternion firsTrackedRotation;
    private Vector3 firstTrackedPosition;

    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public Quaternion InitialRotation { get => initialRotation; set => initialRotation = value; }

    public Quaternion FirstTrackedRotation { get => firsTrackedRotation; set => firsTrackedRotation = value; }
    public SixenseInput.Controller Controller { get => controller; set => controller = value; }
    public Vector3 FirstTrackedPosition { get => firstTrackedPosition; set => firstTrackedPosition = value; }
    public PropController PositionReference { get => positionReference; set => positionReference = value; }

    public SixenseHands SideController;

    private SixenseInput.Controller controller;
    private SixenseHands sideController;


    private GameObject master;
    private MasterController masterController;
    private TargetedController targetedController;
    private Logic logic;
    // Start is called before the first frame update
    void Start()
    {
        atached = false;
        master = GameObject.Find("Master");
        targetedController = master.GetComponent<TargetedController>();
        masterController = master.GetComponent<MasterController>();
        trackerOffset = -transform.GetChild(1).localPosition;//new Vector3(0.0f, 0.0f, 0.0f);
        //deltaPosition = transform.GetChild(1).position - transform.position;
        reatach = false;
        
 
    }
    private void Awake()
    {
        if (Controller == null)
        {
           // Debug.Log("Initializaing controller");
            Controller = SixenseInput.GetController(SideController);
           // Debug.Log("Initializaing controller done: "  + (Controller == null));
        }
    }    // Update is called once per frame
    void Update()
    {
        if (Controller == null)
        {
 
            Controller = SixenseInput.GetController(SideController);
            //Debug.Log("Initializaing controller done: " + (Controller == null));
        }
        if (m_bInitialized && IsControllerActive())
        {
            if(PositionReference != null && atached)
            {
                /*PositionReference.transform.position = (controller.Position - firstTrackedPosition) * m_sensitivity + InitialPosition;
                PositionReference.transform.rotation = controller.Rotation * initialRotation * Quaternion.Inverse(FirstTrackedRotation);*/
                Vector3 nPos = orientation * Controller.Position;
                //transform.position = (nPos - firstTrackedPosition) * m_sensitivity + InitialPosition ;

                
                Vector3 realPos = Vector3.zero;

                //realPos = (nPos - firstTrackedPosition) * m_sensitivity + InitialPosition + trackerOffset;
                realPos = (nPos - firstTrackedPosition);
                realPos = new Vector3(realPos.x * m_sensitivityX, realPos.y*m_sensitivityY, realPos.z*m_sensitivityZ) + InitialPosition + trackerOffset;
                transform.position = targetedController.giveRetargetedPosition(realPos);
                
                
                
                transform.rotation = Controller.Rotation * initialRotation * Quaternion.Inverse(FirstTrackedRotation);
                GameObject refer = transform.GetChild(1).gameObject;
                PositionReference.RealPosition = realPos;
                PositionReference.transform.position = refer.transform.position;
                PositionReference.transform.rotation = refer.transform.rotation;
            }
        }
        if(reatach)
        {
            reatach = false;
            attach();
        }
        
    }


    public void detach()
    {
        //positionReference.hydraTracker = null;
        PositionReference = null;
        m_bInitialized = false;
        InitialPosition = Vector3.zero;
        m_baseOffset = Vector3.zero;
        firstTrackedPosition = Vector3.zero;
        //InitialRotation = controller.Rotation;
        atached = false;
    }

    public void attach ()//(PropController nPositionReference)
    {
        //PositionReference = nPositionReference;
        bool att = false;
        if(PositionReference != null)
        {
            positionReference.hydraTracker = this;                
            if (masterController.currentStage == MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING || masterController.currentStage == MasterController.EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING)
            {
                InitialPosition = realWorldReference.transform.position; //+ trackerOffset;
                InitialRotation = Quaternion.identity;
                att = true;
            }
            if(!att)
            {
                InitialPosition = PositionReference.positionReference.transform.position; //+ trackerOffset;
                InitialRotation = PositionReference.positionReference.transform.rotation  ;
            }
            
            Vector3 nPos = orientation * Controller.Position;
            FirstTrackedRotation =  controller.Rotation;
            firstTrackedPosition = nPos;
            atached = true;
            m_bInitialized = true;
        }
    }

    /** returns true if a controller is enabled and not docked */
    bool IsControllerActive()
    {
        //Debug.Log("Controller > Isnull: " + controller != null + " ---> Enabled:" + controller.Enabled + "----> Docked: " + controller.Docked);
        return (controller != null && controller.Enabled && !controller.Docked);
    }
}
