/// <summary>
/// P5Fingers
/// Brief example of using a P5 Glove with Unity
/// By Justin Scaniglia / WiredEarp
/// </summary>

using UnityEngine;
using System;
using System.Collections;
using System.Runtime.InteropServices;

public struct FingerData
{
    public short Index;
    public short Middle;
    public short Ring;
    public short Pinky;
    public short Thumb;
};

public class P5Fingers : MonoBehaviour
{

    [DllImport("P5GloveDLL")]
    //private static extern int PrintANumber(); 
    private static extern void P5Glove_Reset();
    [DllImport("P5GloveDLL")]
    private static extern void P5Glove_Start();
    [DllImport("P5GloveDLL")]
    private static extern void P5Glove_Stop();
    [DllImport("P5GloveDLL")]
    private static extern char P5Glove_GetIndexFinger();
    [DllImport("P5GloveDLL")]
    private static extern FingerData P5Glove_GetFingers();

    Transform Index, Middle, Ring, Pinky, Thumb;


    public float RemapRange(float value, float from1, float to1, float from2, float to2)
    {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }

    // Use this for initialization
    void Start()
    {
        Debug.Log("Start");
        P5Glove_Start();

        Index = transform.Find("JNT_Root/JNT_Wrist/JNT_Palm/JNT_Pointer01");
        Middle = transform.Find("JNT_Root/JNT_Wrist/JNT_Palm/JNT_Middle01");
        Ring = transform.Find("JNT_Root/JNT_Wrist/JNT_Palm/JNT_Ring01");
        Pinky = transform.Find("JNT_Root/JNT_Wrist/JNT_Palm/JNT_Pinky01");
        Thumb = transform.Find("JNT_Root/JNT_Wrist/JNT_Palm/JNT_Thumb01");
    }


    // Update is called once per frame
    public void Update()
    {
        FingerData fingerdata = P5Glove_GetFingers();

        //calculate the finger position
        int FingerPos = (int)RemapRange(fingerdata.Index, 0f, 63f, 0f, 90f);

        //loop through the subjoints of the fingers and set them to the correct angle
        Transform[] fingerJoints = Index.GetComponentsInChildren<Transform>();
        foreach (Transform joint in fingerJoints)
        {
            joint.localEulerAngles = new Vector3(joint.localEulerAngles.x, joint.localEulerAngles.y, FingerPos);
        }
        FingerPos = (int)RemapRange(fingerdata.Middle, 0f, 63f, 0f, 90f);
        fingerJoints = Middle.GetComponentsInChildren<Transform>();
        foreach (Transform joint in fingerJoints)
        {
            joint.localEulerAngles = new Vector3(joint.localEulerAngles.x, joint.localEulerAngles.y, FingerPos);
        }
        FingerPos = (int)RemapRange(fingerdata.Ring, 0f, 63f, 0f, 90f);
        fingerJoints = Ring.GetComponentsInChildren<Transform>();
        foreach (Transform joint in fingerJoints)
        {
            joint.localEulerAngles = new Vector3(joint.localEulerAngles.x, joint.localEulerAngles.y, FingerPos);
        }
        FingerPos = (int)RemapRange(fingerdata.Pinky, 0f, 63f, 0f, 90f);
        fingerJoints = Pinky.GetComponentsInChildren<Transform>();
        foreach (Transform joint in fingerJoints)
        {
            joint.localEulerAngles = new Vector3(joint.localEulerAngles.x, joint.localEulerAngles.y, FingerPos);
        }
        FingerPos = (int)RemapRange(fingerdata.Thumb, 0f, 63f, 0f, 90f);
        fingerJoints = Thumb.GetComponentsInChildren<Transform>();
        foreach (Transform joint in fingerJoints)
        {
            joint.localEulerAngles = new Vector3(joint.localEulerAngles.x, joint.localEulerAngles.y, FingerPos);
        }

        //   Debug.Log ("FingerTest: " + fingerdata.Index + fingerdata.Middle + fingerdata.Ring + fingerdata.Pinky + fingerdata.Thumb);

    }

    void OnDestroy()
    {
        Debug.Log("stop");
    }





}
