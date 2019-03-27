using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetedController : MonoBehaviour
{

    public float compensation;
    public int newGoal;

    /**
     * This factor helps to adjust the movement of the hand in the scene.
    */
    private float movementFactor = 1f;

    /**
     *  Initial position of the hand at the moment to star the retargettin
     */
    private Vector3 startPosition;

    /**
     *  Goal position. 
     */
    private Vector3 endPosition;

    /**
     * Retargetered position
     */
    public GameObject retargetedPosition;

    

    /**
     *  Compensation factors
     */
    public float compensationFactorX;
    public float compensationFactorZ;


    public bool shifting;

    int metodo;
    public GameObject anchor;

    private void OnMouseDown()
    {
        

    }
    public void starShifting(Vector3 newEndPosition, Vector3 newStartPosition)
    {
        startPosition = gameObject.transform.position;
        endPosition = newEndPosition;

        Vector3 retPosition = retargetedPosition.transform.position;

        float xCenter = retPosition.x - startPosition.x;
        float zCenter = retPosition.z - startPosition.z;

        float xGoal = endPosition.x - startPosition.x;
        float zGoal = endPosition.z - startPosition.z;

        compensationFactorX = (xGoal - xCenter) / zCenter;
        // compensationFactorZ = (zGoal) / zCenter; It is actually this but that must be one cuz they are at the same position in z
        // For some reason, when the goal is the right prop, the z value is higher, it is actually the parent object z value.
        compensationFactorZ = 1;

        shifting = true;

        Debug.Log("Compensation factor X :" + compensationFactorX);
        Debug.Log("Compensation factor Z :" + compensationFactorZ);
        Debug.Log("StarZ :" + startPosition.z + " --- EndZ : " + endPosition.z +  " ---  CenterZ  " + retPosition.z);

        // First Apprach. Replicate of the first code
        /*
        Debug.Log("Shifting starting");
        Debug.Log("Shifting starting");
        Debug.Log("Shifting starting");
        Debug.Log("Shifting starting");

        float mX = (Input.mousePosition.x - mouseX) / 65;
        float mY = (Input.mousePosition.y - mousey) / 65;
        goalX = 0;
        if (NewGoal == 0)
            goalX = -2;
        if (NewGoal == 2)
            goalX = 2;
        goalY = 4;
        startX = mX;
        startY = mY;
        difX = Mathf.Abs(startX - goalX);
        difY = Mathf.Abs(startY - goalY)*0.8f;
        step = Mathf.Abs(startY - goalY) * 0.2f;
        Debug.Log("StartXY : " + startX + " -- " + startY);
        Debug.Log("GoalXY : " + goalX + " -- " + goalY);
        Debug.Log("diflXY : " + difX + " -- " + difY);
        step = difY / precisionl;
        redirection = difX / precisionl;

        if (startX > goalX)
            redirection = -redirection;
        shifting = true;
        thAway = startY;
        thBack = thAway - step;

        Debug.Log("Redirection : " + redirection );
        Debug.Log("thAwayBack : " + thAway + "  ---  " + thBack);
        Debug.Log("Step : " + step );
        acum = 0f;
        */

    }

    // Start is called before the first frame update

    void Start()
    {
       /* startPosition = gameObject.transform.position;
        Debug.Log("Checking Start");
        shifting = false;
        metodo = 2;*/
    }

    //Compensation
    void method1()
    {
        float ver = (Input.mousePosition.x - mouseX) / 65;
        float hor = (Input.mousePosition.y - mousey) / 140;
        int facto = 1;

        if (shifting || true)
        {
            if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
                Debug.Log("X : " + ver + "  ---  Y : " + hor + "        Away: " + thAway + " //*//*//  Back " + thBack);

            if (thAway < hor && thAway < goalY - offset)
            {
                thAway = thAway + step;
                thBack = thBack + step;
                Debug.Log("CASE 1:  Away: " + thAway + " //*//*//  Back " + thBack);
                acum += redirection;
            }

            else if (thBack > hor && thBack > startY)
            {
                thAway = thAway - step;
                thBack = thBack - step;
                Debug.Log("CASE 2:  Away: " + thAway + " //*//*//  Back " + thBack);
                acum -= redirection;
            }

        }
        /*if (NewGoal == 0)
        {

            ver = ver - (Mathf.Abs(2-ver)*hor/4);
            Transform nt = this.gameObject.transform;
            //Debug.Log(Input.mousePosition.x + "-" + Input.mousePosition.y);
            //Debug.Log("----" + ver+ "-" + hor);
        }
        else if (NewGoal == 2)
        {
            ver = ver + (Mathf.Abs(2 - ver) * hor / 4);
            Transform nt = this.gameObject.transform;
            //Debug.Log(Input.mousePosition.x + "-" + Input.mousePosition.y);
            //Debug.Log("----" + ver + "-" + hor);
        }*/

    transform.position = new Vector3(ver, hor, -3f);

        if (shifting)
            transform.Translate(new Vector3(acum, 0f, 0f));
    }

    /**
     * This is the first approach in our study for retargetting.
     * It takes the original position and the final position in order to create parameter for a 
     * linear transformation of position in the plane X-Z
     * TODO. Think about better compensations. Dinamcally? 
     */
    void method2()
    {
        /*
        float ver = anchor.transform.position.x*7f;
        float hor = anchor.transform.position.y *7f ;
        if (NewGoal == 1 || hor > goalY + 0.3f || hor <  -0.3f)
            transform.position = new Vector3(ver, hor, -3f);
        else if (NewGoal == 2)
        transform.position = new Vector3(ver + 0.4f* Mathf.Max(hor,0)/1.2f, hor, -3f);
        else if (NewGoal == 0)
            transform.position = new Vector3(ver - 0.4f * Mathf.Max(hor, 0) / 1.2f , hor, -3f);
       */
        float xAnchor = (anchor.transform.position.x - startPosition.x) * movementFactor;
        float yAnchor = (anchor.transform.position.y - startPosition.y) * movementFactor;
        float zAnchor = (anchor.transform.position.z - startPosition.z) * movementFactor;



        Vector3 nPosition = new Vector3(xAnchor + zAnchor * compensationFactorX +startPosition.x ,yAnchor + startPosition.y, zAnchor * compensationFactorZ + startPosition.z);
        if (shifting)
            transform.position = nPosition;
        else
        {
            xAnchor = (anchor.transform.position.x  - startPosition.x) * movementFactor + startPosition.x;
            yAnchor = (anchor.transform .position.y - startPosition.y) * movementFactor + startPosition.y;
            zAnchor = (anchor.transform.position.z - startPosition.z) * movementFactor + startPosition.z;
            //transform.position = new Vector3(xAnchor , yAnchor , zAnchor );
            transform.position = anchor.transform.position;
        }   


    }

    public Vector3 giveRetargetedPosition(Vector3 realPosition)
    {
        Vector3 rePosition = new Vector3();
        float xAnchor = (realPosition.x - startPosition.x) * movementFactor;
        float yAnchor = (realPosition.y - startPosition.y) * movementFactor;
        float zAnchor = (realPosition.z - startPosition.z) * movementFactor;

         rePosition = new Vector3(xAnchor + zAnchor * compensationFactorX + startPosition.x, yAnchor + startPosition.y, zAnchor * compensationFactorZ + startPosition.z);
        if (!shifting)
        {
            /*xAnchor = (anchor.transform.position.x - startPosition.x) * movementFactor + startPosition.x;
            yAnchor = (anchor.transform.position.y - startPosition.y) * movementFactor + startPosition.y;
            zAnchor = (anchor.transform.position.z - startPosition.z) * movementFactor + startPosition.z;*/
            //transform.position = new Vector3(xAnchor , yAnchor , zAnchor );
            rePosition = realPosition;
        }
        return rePosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            mouseX = Input.mousePosition.x;
            mousey = Input.mousePosition.y;
            Debug.Log("Adjunsting");
        }
        if (metodo == 1)
            method1();
        else if (metodo == 2)
            method2();

    }


    //    Using in the first example. Replication of first retargetting test. 
    float thAway;
    float thBack;
    public float precisionl;
    float step;
    float redirection;
    float startX;
    float startY;
    float goalX;
    float goalY;
    float difX;
    float difY;
    float offset;
    float acum;

    float mouseX;
    float mousey;

}
