using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotificationsMannager : MonoBehaviour
{

    public TextMesh[] steps;

    public TextMesh personalNotificationBar;
    public TextMesh personalStageBar;

    public int counterGoals;
    public bool masterDecide;
    // Start is called before the first frame update
    void Start()
    {
        counterGoals = 0;
        masterDecide = false;
    }
    private bool triggerPressed;
    public void registerGoal()
    {
        counterGoals++;
        if(counterGoals == 2)
        {
            showGoalDone(true);
            counterGoals = 0;
            masterDecide = true;
        }

        


    }
    void showGoalDone(bool show)
    {
        personalStageBar.gameObject.SetActive(show);
    }
    // Update is called once per frame
    void Update()
    {/*
        if (!triggerPressed)
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) > 0.7f)
            {
                triggerPressed = true;

            }
        } 
        else
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, OVRInput.Controller.RTouch) < 0.4f)
            {
                showGoalDone(false);
                masterDecide = false;
                triggerPressed = false;
            }
        }*/
        if (Input.GetKeyDown(KeyCode.N))
        { 
            showGoalDone(false);
            masterDecide = false;
        }
    }

    public void lightStepNotification(int step)
    {
        for(int i = 0; i < steps.Length; i++)
        {
            steps[i].color = Color.white;
            steps[i].fontSize = 4;
        }
        steps[step-1].color = Color.green;
        steps[step - 1].fontSize = 45;
    }

    public void messageToUser(string message)
    {
        StartCoroutine(showMessage(message));
    }

    IEnumerator showMessage(string message)
    {
        personalNotificationBar.gameObject.SetActive(true);
        personalNotificationBar.text = message;
        yield return new WaitForSeconds(2);
        personalNotificationBar.gameObject.SetActive(false);
    }

    public void normalSettings()
    {
        counterGoals = 0;
        showGoalDone(false);
        for (int i = 0; i < steps.Length; i++)
        {
            steps[i].color = Color.white;
            steps[i].fontSize = 35;
        }
    }
}
