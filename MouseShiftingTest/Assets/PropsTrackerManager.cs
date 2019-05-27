using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropsTrackerManager : MonoBehaviour
{
    public PropController[] trackers;

        Vector3 m_baseOffset;
        Quaternion m_baseOffset_Rot;
        float m_sensitivity = 0.001f; // Sixense units are in mm
        bool m_bInitialized;


    // Use this for initialization
    void Start()
    {
        // m_hands = GetComponentsInChildren<SixenseHand>();
    }


    // Update is called once per frame
    void Update()
    {
        bool bResetHandPosition = false;
        
        foreach (PropController hand in trackers)
        {
            // if (IsControllerActive(hand.controller) && hand.controller.GetButtonDown(SixenseButtons.START))
            if (IsControllerActive(hand.Controller) && hand.Controller.GetButtonDown(SixenseButtons.ONE))
            {
                bResetHandPosition = true;
            }

            Debug.Log("Controller active ? -///----- ");
            Debug.Log(IsControllerActive(hand.Controller));
            if (m_bInitialized)
            {
                UpdateGameObject(hand);
            }
        }

        if (bResetHandPosition)
        {
            m_bInitialized = true;

            m_baseOffset = Vector3.zero;
            m_baseOffset_Rot = Quaternion.identity;

            // Get the base offset assuming forward facing down the z axis of the base
            foreach (PropController hand in trackers)
            {
                m_baseOffset += hand.Controller.Position;
                m_baseOffset_Rot = hand.Controller.Rotation;
            }

           // m_baseOffset /= 2;
        }
    }


    /** Updates hand position and rotation */
    void UpdateGameObject(PropController prop)
    {
        bool bControllerActive = IsControllerActive(prop.Controller);
        if (bControllerActive)
        {
            prop.transform.position = (prop.Controller.Position - m_baseOffset ) * m_sensitivity + prop.InitialPosition;
            prop.transform.localRotation = prop.Controller.Rotation * prop.InitialRotation* Quaternion.Inverse(m_baseOffset_Rot);
            if (prop.FirstTrackedPosition == Vector3.zero)
                prop.FirstTrackedPosition = prop.transform.position;
        }

        else
        {
            // use the inital position and orientation because the controller is not active
            prop.transform.localPosition = prop.InitialPosition;
            prop.transform.localRotation = prop.InitialRotation;
        }
    }


    void OnGUI()
    {
        if (!m_bInitialized)
        {
            //GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height - 40, 100, 30), "Press Start");
        }
    }


    /** returns true if a controller is enabled and not docked */
    bool IsControllerActive(SixenseInput.Controller controller)
    {
        
        return (controller != null && controller.Enabled && !controller.Docked);
    }
}
