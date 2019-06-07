using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurveyMannager : MonoBehaviour
{
    public bool isSurveyActive;
    private int questionNumber;
    public GameObject[] options;
    public GameObject normalMenu;
    public GameObject surveyMenu;
    public TextMesh questionMesh;
    private int actualStage;
    private string answer;

    private string strPrevStage;
    private string strCurrStage;

    // Handle the press of the trigger.
    private bool triggerPressed;
    LineRenderer lineRenderer = new LineRenderer();

    private NotificationsMannager notificationsMannager;
    private PersistanceManager persistanceManager;
    // Start is called before the first frame update
    void Start()
    {
        isSurveyActive = false;
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        notificationsMannager = gameObject.GetComponent<NotificationsMannager>();
        persistanceManager  = gameObject.GetComponent<PersistanceManager>();
        lineRenderer.startWidth = 0;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.startColor = Color.green;
        lineRenderer.endColor = Color.white;
        actualStage = 0;
        strPrevStage = "";
        strCurrStage = "";
        lineRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isSurveyActive)
        {


            int layerMask = 1 << 9;
            RaycastHit hit;
            Vector3 controllerPoristion = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) + GameObject.Find("OVRCameraRig").transform.position;
            Vector3 controllerRotation = OVRInput.GetLocalControllerRotation(OVRInput.Controller.LTouch) * Vector3.forward;
            Ray raydirection = new Ray(controllerPoristion, controllerRotation);

            lineRenderer.SetPosition(0, controllerPoristion);
            lineRenderer.SetPosition(1, controllerPoristion + raydirection.direction*5f);
            for (int i = 0; i < options.Length; i++)
            {
                Renderer rend = options[i].GetComponent<Renderer>();
                rend.material.color = Color.white;
            }
            if (Physics.Raycast(raydirection, out hit, Mathf.Infinity, layerMask))
            {
                Renderer rend = hit.collider.gameObject.GetComponent<Renderer>();
                rend.material.color = Color.green;
                TextMesh textAnswer = hit.collider.gameObject.GetComponentInChildren<TextMesh>();
                answer = textAnswer.text;
                Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                Debug.Log("Did Hit");
            }
            else
            {
                answer = "";
                //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                //Debug.Log("Did not Hit");
            }

            if (!triggerPressed)
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) > 0.7f)
                {
                    triggerPressed = true;
                    if (answer != "")
                        nextQuestion();
                    else
                        notificationsMannager.messageToUser("Select one of the options in the wall");
                }
            }
            else
            {
                if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, OVRInput.Controller.LTouch) < 0.5f)
                {
                    triggerPressed = false;
                }
            }
        }
    }

    public void startSurvey(int stage, string strageStr)
    {
        strPrevStage = strCurrStage;
        strCurrStage = strageStr;
        actualStage = stage;
        lineRenderer.enabled = true;
        isSurveyActive = true;
        normalMenu.SetActive(false);
        surveyMenu.SetActive(true);
        questionMesh.text = "From 1 to 7, how easy was to accomplish the task? \n(1 Very hard - 7 Very easy)";
        questionNumber = 1;
    }

    private void nextQuestion()
    {
        string question = questionMesh.text.Replace("\n","");
        persistanceManager.saveSurveyResponse(strPrevStage, strCurrStage, question, answer);

        if (actualStage > 2)
        {
            if(questionNumber == 1)
            { 
                questionMesh.text = "From 1 to 7, Compared to the previous stage \n how easier was to accomplish the task? \n (1 Much harder - 7 Much easier)";
                questionNumber++;
            }
            else
            {
                isSurveyActive = false;
                normalMenu.SetActive(true);
                surveyMenu.SetActive(false);
                lineRenderer.enabled = false;
            }
        }
        else
        {
            isSurveyActive = false;
            normalMenu.SetActive(true);
            surveyMenu.SetActive(false);
            lineRenderer.enabled = false;
        }
    }

}
