using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropController : MonoBehaviour
{

    private GameObject master;
    private TargetedController targetedController;
    private Logic logic;
    private PersistanceManager persistanceManager;
    private TrackerMannager trackerMannager;
    private MasterController masterController;

    // Describes the range of possibly rotations
    private readonly Quaternion[] flatRots = {
                                        new Quaternion(0f, 0.0f, 0.7f, 0.7f),
                                        new Quaternion(-0.3f, -0.3f, 0.7f, 0.7f),
                                        new Quaternion(-0.5f, -0.5f, 0.5f, 0.5f),
                                        new Quaternion(-0.7f, -0.7f, 0.3f, 0.3f),
                                        new Quaternion(-0.7f, -0.7f, 0.0f, 0.0f)
                                    };

    private readonly Quaternion[] cyllRots = {
                                        new Quaternion(0.0f, -0.4f, 0.0f, 0.9f),
                                        new Quaternion(0.0f, -0.7f, 0f, 0.7f),
                                        new Quaternion(0.0f, -0.9f, 0.0f, 0.4f),
                                        new Quaternion(0.0f, -1f, 0f, 0.3f),
                                        new Quaternion(0.0f, -1f, 0f, 0.3f)
                                    };

    private Quaternion[] rangeRotation;
    public Quaternion[] RangeRotation { get => rangeRotation; set => rangeRotation = value; }


    /**
     * Position where should be in Virtual world
     */
    public GameObject positionReference;

    /**
     * The distance from the table so we consider the object is still in the participant hand
     */
    private float deltaY;

    /**
     * Guide object 
     */
    public GameObject dockProp;

    /**
     * Reference to PropManager, in charge to control te adaptic 
     */
    public PropMannager propManager;
    public int trackerObject;

    /**
     *  Reference to the Hydra tracker, if it has it 
     */
    public HydraTracker hydraTracker;

        /**
     * Original position, no ret
     */
    Vector3 realPosition;

    Vector3 initialPosition;
    Quaternion initialRotation;
    Quaternion initialControllerRotation;
    private Vector3 firstTrackedPosition;

    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public Quaternion InitialRotation { get => initialRotation; set => initialRotation = value; }

    public Quaternion InitialControllerRotation { get => initialControllerRotation; set => initialControllerRotation = value; }
    public SixenseInput.Controller Controller { get => controller; set => controller = value; }
    public Vector3 FirstTrackedPosition { get => firstTrackedPosition; set => firstTrackedPosition = value; }
    public Vector3 RealPosition { get => realPosition; set => realPosition = value; }
    public bool IsMoving { get => isMoving; set => isMoving = value; }

    public SixenseHands SideController;

    private SixenseInput.Controller controller;
    private SixenseHands sideController;

    private Collider handCollider;
    private bool isMoving;


    private Quaternion inicialRotation;

    // Start is called before the first frame update
    void Start()
    {
        inicialRotation = dockProp.transform.rotation;
        gameObject.transform.position = positionReference.transform.position;
        IsMoving = false;
        deltaY = 0.5f;

        master = gameObject;
        masterController = master.GetComponent<MasterController>();
        targetedController = master.GetComponent<TargetedController>();
        persistanceManager = master.GetComponent<PersistanceManager>();
        trackerMannager = master.GetComponent<TrackerMannager>();
        logic = master.GetComponent<Logic>();

    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("PROPCONTROLLER IS NULL ----- " + logic == null);
        if(logic.stage == 0)
        { 
            handCollider = other;
            IsMoving = true;
            CapsuleHand capsuleHand = handCollider.gameObject.GetComponent<CapsuleHand>();
            if (capsuleHand != null)
            {
                capsuleHand.canDraw = false;
                objectGreen(true);
                masterController.handOnObject = true;
            }
            Debug.Log("Collision detected --- ");
        }
    }
  
  


    public void objectGreen(bool isGreen)
    {
        Renderer rend = transform.GetChild(0).GetChild(0).gameObject.GetComponent<Renderer>();
        if(isGreen)
            rend.material.color = Color.green;
        else
            rend.material.color = Color.white;
    }

    private void OnTriggerExit(Collider other)
    {
        if (logic.stage == 0 || logic.stage == 4)
        {
            handCollider = other;
            IsMoving = false;
            CapsuleHand capsuleHand = handCollider.gameObject.GetComponent<CapsuleHand>();
            if (capsuleHand != null)
            {
                capsuleHand.canDraw = true;
                objectGreen(false);
                masterController.handOnObject = false;
            }
            Debug.Log("Collision cancelled --- ");
        }

    }

    // Update is called once per frame
    void Update()
    {
        // dockProp.transform.rotation =  tryRota;
        //Debug.Log("ROTATION ---> " + dockProp.transform.rotation);
        if (logic == null)
            logic = master.GetComponent<Logic>();

        if (IsMoving)
        {
            bool overlapped = true;
            CapsuleHand capsuleHand = handCollider.gameObject.GetComponent<CapsuleHand>();
            if (capsuleHand != null && capsuleHand.canDraw == false)
            {
                Collider[] ovelaped = Physics.OverlapBox(handCollider.transform.position, new Vector3(0.05f, 0.05f, 0.05f));
                if (Physics.OverlapBox(handCollider.transform.position, new Vector3(0.1f, 0.1f, 0.1f)).Length == 1)
                {
                    overlapped = false;
                }
            }

        

            if (handCollider.gameObject.activeSelf )
            {
                if (logic.stage == 0 || logic.stage == 4)
                {
                    if (!overlapped)
                    {
                        IsMoving = false;
                        objectGreen(false);
                        if (capsuleHand != null)
                            capsuleHand.canDraw = true;
                        masterController.handOnObject = false;
                    }
                    else
                    {
                        objectGreen(true);
                        masterController.handOnObject = true;
                    }
                }
                else
                {
                    if (capsuleHand != null)
                    {
                        capsuleHand.canDraw = false;
                    }

                    objectGreen(true);
                }
            }
        }

        //Debug.Log("PROP   POS///// x:" + gameObject.transform.position.x + "///  y:" + gameObject.transform.position.y + "///  z:" + gameObject.transform.position.z);
        //Debug.Log("PROP   ROT///// W:" + gameObject.transform.rotation.w + "///// x:" + gameObject.transform.rotation.x + "///  y:" + gameObject.transform.rotation.y + "///  z:" + gameObject.transform.rotation.z);
/*
        GameObject master = GameObject.Find("Master");
        TargetedController positionController = master.GetComponent<TargetedController>();
        if (positionController != null)
        {
            transform.position = positionController.giveRetargetedPosition(transform.position);
        }
  */      
    }

    public string preSetShape()
    {
        string presetResult = "";
        if(gameObject.transform.childCount > 0)
        {
            Debug.Log("PropController ---- Found children");
            GameObject prop = gameObject.transform.GetChild(0).gameObject;
            if (propManager != null)
            {
                Debug.Log("PropController ---- Asking propmanager.");
                PropSpecs specs = prop.GetComponent<PropSpecs>();
                propManager.adapticCommand(specs.type);
                //presetResult = propManager.readData();
            }
        }
        return presetResult;
    }

    public void movePropDock(bool toPointZ)
    {
        if (toPointZ)
        {
            GameObject pointZ = GameObject.Find("HandStartPoint");
            if(pointZ != null)
            {
                dockProp.transform.parent = pointZ.transform;
                dockProp.transform.localPosition = new Vector3(0.0f, 1.5f, 0.0f);
                randomizeRotation(false);
            }
        }
        else
        {
            dockProp.transform.parent = positionReference.transform;
            dockProp.transform.localPosition = Vector3.zero;
            randomizeRotation(true);
        }
    }

    public void relocatePropDock()
    {
        dockProp.transform.parent = transform.GetChild(0).transform;
        dockProp.transform.localPosition = Vector3.zero;
    }

    public void randomizeRotation (bool neutral)
    {
        
        GameObject child = gameObject.transform.GetChild(0).gameObject;
        PropSpecs propSpecs = child.GetComponent<PropSpecs>();
        if(propSpecs.type == PropMannager.PRESET_TYPE.CYLINDER)
            dockProp.transform.rotation = cyllRots[neutral ? 0: Random.Range(0, 5)];
        else
            dockProp.transform.rotation = flatRots[neutral ? 0 : Random.Range(0, 5)];

    }
    
    public void activeChildren(bool activate)
    {
        gameObject.transform.GetChild(0).gameObject.SetActive(activate);
    }

}
