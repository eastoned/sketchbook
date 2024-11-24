using TMPro;
using Unity.Mathematics;
using UnityEngine;

public class LegController : MonoBehaviour
{
    public ConfigurableJoint footJointLeft;
    public ConfigurableJoint legJointLeft;

     public ConfigurableJoint footJointRight;
    public ConfigurableJoint legJointRight;
    public HingeJoint leftSpring, rightSpring;

    public TextMeshProUGUI textDebug;

    public bool touchSupported = false;

    public SkinnedMeshRenderer smr;
    public Rigidbody body;

    public Transform leftArm, leftHand, rightArm, rightHand;

    public void Update()
    {

        if(body.velocity.magnitude > 5)
        {
            //smr.SetBlendShapeWeight(0, 100);
            //Debug.Log(body.velocity); 
        }
        else
        {
            //smr.SetBlendShapeWeight(0, 0);
        }

        if (Input.touchCount > 0)
        {
            touchSupported = true;
        }

        if(!touchSupported)
        {

            footJointLeft.targetRotation = Quaternion.Euler(Input.GetMouseButton(0) ? 0 : 270, 0, 0);
            legJointLeft.targetRotation = Quaternion.Euler(Input.GetMouseButton(0) ? 90 : 0, 0, 0);

            footJointRight.targetRotation = Quaternion.Euler(Input.GetMouseButton(1) ? 0 : 90, 0, 0);
            legJointRight.targetRotation = Quaternion.Euler(Input.GetMouseButton(1) ? 270 : 0, 0, 0);
        
            float handPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.mousePosition).y);
            leftSpring.anchor = new Vector3(-Mathf.Cos(handPos * Mathf.PI)*1.5f, Mathf.Sin(handPos * Mathf.PI)*1.5f, -.7f);
            rightSpring.anchor = new Vector3(-Mathf.Cos(handPos * Mathf.PI)*1.5f, Mathf.Sin(handPos * Mathf.PI)*1.5f, .7f);
            body.centerOfMass = new Vector3(Mathf.Cos(handPos * Mathf.PI)*2,0 , 0);

            leftArm.localRotation = Quaternion.Lerp(Quaternion.Euler(-15, -10, 70), Quaternion.Euler(0, 0, -60), handPos);
            leftHand.localRotation = Quaternion.Lerp(Quaternion.Euler(31, -41, -160), Quaternion.Euler(25, 15, -40), handPos);

            rightArm.localRotation = Quaternion.Lerp( Quaternion.Euler(-15, -10, -70),Quaternion.Euler(0, 0, 60), handPos);
            rightHand.localRotation = Quaternion.Lerp(Quaternion.Euler(31, 41, 160), Quaternion.Euler(25, 15, 40), handPos);

            textDebug.text = "Debug for touch screen";

        }
        else
        {
            
            float leftHandPos = 0.5f;
            float rightHandPos = 0.5f;

            if(Input.touchCount >= 2)
            {
                if(Input.touches[0].position.x > Input.touches[1].position.x)
                {
                    leftHandPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.touches[1].position).y);
                    rightHandPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.touches[0].position).y);
                }
                else
                {
                    leftHandPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.touches[0].position).y);
                    rightHandPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.touches[1].position).y);
                }
            }
            else if (Input.touchCount == 1)
            {
                float handXPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.touches[0].position).x);
               
                if(handXPos > 0.5f)
                {
                    rightHandPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.touches[0].position).y);
                }
                else
                {
                    leftHandPos = Mathf.Clamp01(Camera.main.ScreenToViewportPoint(Input.touches[0].position).y);
                }
            }

            footJointLeft.targetRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(270, 0, 0), leftHandPos);
            legJointLeft.targetRotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0), Quaternion.Euler(0, 0, 0), leftHandPos);

            footJointRight.targetRotation = Quaternion.Lerp(Quaternion.Euler(0, 0, 0), Quaternion.Euler(90, 0, 0), rightHandPos);
            legJointRight.targetRotation = Quaternion.Lerp(Quaternion.Euler(270, 0, 0), Quaternion.Euler(0, 0, 0), rightHandPos);

            leftSpring.anchor = new Vector3(leftHandPos*2-1, 2, 1);
            rightSpring.anchor = new Vector3(rightHandPos*2-1, 2, 1);

            leftArm.localRotation = Quaternion.Lerp(Quaternion.Euler(-15, -10, 70), Quaternion.Euler(0, 0, -60), leftHandPos);
            leftHand.localRotation = Quaternion.Lerp(Quaternion.Euler(31, -41, -160), Quaternion.Euler(25, 15, -40), leftHandPos);

            rightArm.localRotation = Quaternion.Lerp(Quaternion.Euler(-15, -10, 70), Quaternion.Euler(0, 0, -60), leftHandPos);
            rightHand.localRotation = Quaternion.Lerp(Quaternion.Euler(31, -41, -160), Quaternion.Euler(25, 15, -40), leftHandPos);

            textDebug.text = "touch1 Pos: " + leftHandPos + "\n"
                            + "touch2 Pos: " + rightHandPos;
        }
    }
}
