using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HydraTester : MonoBehaviour
{

    private PropController positionReference;
    public GameObject realWorldReference;

    Vector3 m_baseOffset;
    Quaternion m_baseOffset_Rot;
    public float m_sensitivity = 0.00075f; // Sixense units are in mm
    bool m_bInitialized;
    bool atached;

    public Vector3 controlValue;
    public Quaternion rotationValue;
    public bool isRotating;
    public Vector3 sensibiliy;

    Vector3 initialPosition;
    Quaternion initialRotation;
    Quaternion firsTrackedRotation;
    public Vector3 firstTrackedPosition;

    public Vector3 InitialPosition { get => initialPosition; set => initialPosition = value; }
    public Quaternion InitialRotation { get => initialRotation; set => initialRotation = value; }

    public Quaternion FirstTrackedRotation { get => firsTrackedRotation; set => firsTrackedRotation = value; }
    public SixenseInput.Controller Controller { get => controller; set => controller = value; }
    public Vector3 FirstTrackedPosition { get => firstTrackedPosition; set => firstTrackedPosition = value; }
    public PropController PositionReference { get => positionReference; set => positionReference = value; }

    public SixenseHands SideController;

    private SixenseInput.Controller controller;
    private SixenseHands sideController;

    // Start is called before the first frame update
    void Start()
    {
        atached = false;
        InitialPosition = transform.position;
        initialRotation = transform.rotation;
        sensibiliy = new Vector3(300, 1100, 1100);
        isRotating = false; 
    }
    private void Awake()
    {
        if (Controller == null)
        {
            Debug.Log("Initializaing controller");
            Controller = SixenseInput.GetController(SideController);
            Debug.Log("Initializaing controller done: " + (Controller == null));
            attach();
        }
    }    // Update is called once per frame
    void Update()
    {
        if (Controller == null)
        {
            Debug.Log("Initializaing controller");
            Controller = SixenseInput.GetController(SideController);
            Debug.Log("Initializaing controller done: " + (Controller == null));
            
        }

        if(Input.GetKeyDown(KeyCode.K))
            attach();
        if (Controller != null && atached)
        {
            
                /*PositionReference.transform.position = (controller.Position - firstTrackedPosition) * m_sensitivity + InitialPosition;
                PositionReference.transform.rotation = controller.Rotation * initialRotation * Quaternion.Inverse(FirstTrackedRotation);*/
                Vector3 nPos = Controller.Position;

            controlValue = (nPos - firstTrackedPosition);
            nPos = new Vector3(controlValue.x/ sensibiliy.x, controlValue.y/ sensibiliy.y, controlValue.z/ sensibiliy.z);
                transform.position = (nPos)  + InitialPosition;
            int resp = 0;
            SixensePlugin.sixenseGetHemisphereTrackingMode(0, ref resp);
            Debug.Log("Controller status C1 -<<" + resp);
            SixensePlugin.sixenseGetHemisphereTrackingMode(1, ref resp);
            Debug.Log("Controller status C2 -<<" + resp);
            SixensePlugin.sixenseGetHemisphereTrackingMode(2, ref resp);
            Debug.Log("Controller status C2 -<<" + resp);
            if(isRotating)
                 transform.rotation = Controller.Rotation * initialRotation * Quaternion.Inverse(FirstTrackedRotation);

            rotationValue = Controller.Rotation * initialRotation * Quaternion.Inverse(FirstTrackedRotation);
        }

    }


    public void detach()
    {
        positionReference.hydraTracker = null;
        PositionReference = null;
        m_bInitialized = false;
        InitialPosition = Vector3.zero;
        m_baseOffset = Vector3.zero;
        firstTrackedPosition = Vector3.zero;
        InitialRotation = controller.Rotation;
        atached = false;
    }

    public void attach()//(PropController nPositionReference)
    {
        //PositionReference = nPositionReference;
        if(controller != null)
        { 
            Vector3 nPos = Controller.Position;
            FirstTrackedRotation = controller.Rotation;
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
