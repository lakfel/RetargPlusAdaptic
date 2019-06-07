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

    public void starShifting(Vector3 newEndPosition, Vector3 newStartPosition)
    {
        //startPosition = gameObject.transform.position;
        startPosition = newStartPosition;
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

       // Debug.Log("Compensation factor X :" + compensationFactorX);
       // Debug.Log("Compensation factor Z :" + compensationFactorZ);
       // Debug.Log("StarZ :" + startPosition.z + " --- EndZ : " + endPosition.z +  " ---  CenterZ  " + retPosition.z);

    }


    public Vector3 giveRetargetedPosition(Vector3 realPosition)
    {
        Vector3 rePosition = new Vector3();
        rePosition = realPosition;

        MasterController masController = gameObject.GetComponent<MasterController>();
        if (shifting && masController != null)
        {
            if (masController.currentStage == MasterController.EXP_STAGE.PROP_MATCHING_PLUS_RETARGETING || masController.currentStage == MasterController.EXP_STAGE.PROP_NOT_MATCHING_PLUS_RETARGETING)
            {
                float xAnchor = (realPosition.x - startPosition.x) * movementFactor;
                float yAnchor = (realPosition.y - startPosition.y) * movementFactor;
                float zAnchor = (realPosition.z - startPosition.z) * movementFactor;

                rePosition = new Vector3(xAnchor + zAnchor * compensationFactorX + startPosition.x, yAnchor + startPosition.y, zAnchor * compensationFactorZ + startPosition.z);
                //Debug.Log("RETARGETTING ");
            }
        }

        
        return rePosition;
    }


}
