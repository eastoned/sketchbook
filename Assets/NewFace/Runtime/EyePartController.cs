using System.Collections;
using UnityEngine;

public class EyePartController : BodyPartController
{

    public Vector2 eyeLookAtPos;

    [Range(0f, 1f)]
    public float blinkOverride;

    private Coroutine eyeAnimation;

    public enum EyeTarget
    {
        MOUSE,
        PART,
        BLANK
    }

    public EyeTarget eyeTarget;
    public Transform attentionTarget;
    public AnimationCurve blinkAnimation;

    public override void OnMouseDown()
    {
        base.OnMouseDown();
        
        if(eyeAnimation != null)
        {
            StopCoroutine(eyeAnimation);
        }

        eyeAnimation = StartCoroutine(BlinkAnimation());
    }

    private IEnumerator BlinkAnimation()
    {
        float journey = 0;
        float eyelidTopOpen = GetSingleShaderFloat("_EyelidTopOpen");
        float eyelidBottomOpen = GetSingleShaderFloat("_EyelidBottomOpen");

        while(journey < 0.35f)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey/0.35f);
            float blinkPercent = blinkAnimation.Evaluate(percent);
            UpdateSingleShaderFloatUnsafe("_EyelidTopOpen", blinkPercent * eyelidTopOpen);
            UpdateSingleShaderFloatUnsafe("_EyelidBottomOpen", blinkPercent * eyelidBottomOpen);
            UpdateRenderPropBlock();
            yield return null;
        }

        UpdateSingleShaderFloatUnsafe("_EyelidTopOpen", eyelidTopOpen);
        UpdateSingleShaderFloatUnsafe("_EyelidBottomOpen", eyelidBottomOpen);
        UpdateRenderPropBlock();
        yield return null;
    }

    void Update()
    {
        
        float eyeTargetPosX = 0f;
        float eyeTargetPosY = 0f;

        switch(eyeTarget)
        {
            case EyeTarget.MOUSE:
                eyeLookAtPos = new Vector2(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            break;
            case EyeTarget.PART:
                if(attentionTarget != null)
                {
                    eyeLookAtPos = new Vector2(attentionTarget.position.x, attentionTarget.position.y);
                }
                else
                {
                    eyeLookAtPos = Vector2.zero;
                }
            break;
            case EyeTarget.BLANK:
                eyeTargetPosX = 0.5f;
                eyeTargetPosY = 0.5f;
            break;
        }

        eyeTargetPosX = transform.position.x - eyeLookAtPos.x;
        eyeTargetPosY = transform.position.y - eyeLookAtPos.y;

        eyeTargetPosX = Mathf.Clamp(eyeTargetPosX/5f, -.5f, .5f);
        eyeTargetPosY = Mathf.Clamp(eyeTargetPosY/5f, -.25f, .25f);

        float rotatedTargetPosX;
        float rotatedTargetPosY;

        if(flippedXAxis)
        {
            rotatedTargetPosX = (eyeTargetPosX*Mathf.Cos(-transform.localEulerAngles.z + 180f * Mathf.Deg2Rad)) + (eyeTargetPosY*Mathf.Sin(-transform.localEulerAngles.z + 180f * Mathf.Deg2Rad));
            rotatedTargetPosY = (eyeTargetPosX*Mathf.Sin(-transform.localEulerAngles.z + 180f * Mathf.Deg2Rad)) - (eyeTargetPosY*Mathf.Cos(-transform.localEulerAngles.z + 180f * Mathf.Deg2Rad));
        }
        else{
            rotatedTargetPosX = (eyeTargetPosX*Mathf.Cos(-transform.localEulerAngles.z * Mathf.Deg2Rad)) - (eyeTargetPosY*Mathf.Sin(-transform.localEulerAngles.z * Mathf.Deg2Rad));
            rotatedTargetPosY = (eyeTargetPosX*Mathf.Sin(-transform.localEulerAngles.z * Mathf.Deg2Rad)) + (eyeTargetPosY*Mathf.Cos(-transform.localEulerAngles.z * Mathf.Deg2Rad));
        }

        
        UpdateSingleShaderFloatUnsafe("_PupilOffsetX", rotatedTargetPosX);
        UpdateSingleShaderFloatUnsafe("_PupilOffsetY", rotatedTargetPosY);
        UpdateRenderPropBlock();
    }
}
