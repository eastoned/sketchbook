using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PeelPull : MonoBehaviour
{

    private float dragX, dragY;

    public float dragXMultiplier, dragYMultiplier;

    private float horizontalDisplacement, verticalDisplacement, depthDisplacement;

    public float horizontalClamp, verticalClamp;

    public float verticalThreshold;
    public float inputThreshold;
    private bool breakThreshold = false;
    private bool broken = false;
    public bool canInput = false;

    private Material front, back;

    public string textLine;

    public float fallSpeed, curlSpeed;

    public AudioClip[] clips;
    private AudioSource audio;
    public float audioThreshold;

    public Vector3 anchorPoint;
    public Texture frontImage, backImage;



    // Start is called before the first frame update
    void Start()
    {
    
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.GetSiblingIndex());

        audio = GetComponent<AudioSource>();

        front = GetComponent<MeshRenderer>().materials[1];
        back = GetComponent<MeshRenderer>().materials[0];

        front.SetVector("_AnchorPoint", anchorPoint);
        back.SetVector("_AnchorPoint", anchorPoint);

        front.SetTexture("_MainTex", frontImage);
        back.SetTexture("_MainTex", backImage);

        int order = transform.GetSiblingIndex();
        //int order = (int)transform.position.z;
        back.renderQueue = 2000 + order;
        front.renderQueue = 2000 - order;


}

    // Update is called once per frame
    void Update()
    {

        if (Input.GetMouseButton(0) && canInput)
        {

            //calculate displacement based on mouse input

            horizontalDisplacement += dragX * dragXMultiplier;
            verticalDisplacement += dragY * dragYMultiplier;
            depthDisplacement = Mathf.Abs(horizontalDisplacement) + Mathf.Abs(verticalDisplacement);

            if (((Mathf.Abs(dragX) + Mathf.Abs(dragY)) >= inputThreshold) && (verticalDisplacement < verticalThreshold))
            {
                breakThreshold = true;
                broken = true;
            }

            //play sounds when certain peel threshold
            if ((Mathf.Abs(dragX) + Mathf.Abs(dragY)) >= audioThreshold)
            {
                if (!audio.isPlaying)
                {
                    audio.clip = clips[Random.Range(0, clips.Length)];
                    audio.pitch = Random.Range(0.9f, 1.1f);
                    audio.Play();
                }
            }

            if (!broken)
            {
                //store mouse input

                dragX = Input.GetAxis("Mouse X");
                dragY = Input.GetAxis("Mouse Y");
                float textMultiplier = textLine.Length * 8;
                float textNum = textMultiplier * depthDisplacement;
                textNum = (int)textNum;
                textNum = Mathf.Clamp(textNum, 0, textLine.Length + 1);

                for (int i = 0; i < textNum; i++)
                {
                    TextDisplay.textDisplay = textLine.Substring(0, i);
                }
            }else
            {
                transform.gameObject.layer = 9;

                dragX = 0;
                dragY = 0;
                TextDisplay.textDisplay = textLine;
            }

            //vertical max -1 to 1
            //horizontal max -1 to 1
            //depth max -0.5 to 1
            horizontalDisplacement = Mathf.Clamp(horizontalDisplacement, -horizontalClamp, horizontalClamp);
            verticalDisplacement = Mathf.Clamp(verticalDisplacement, -verticalClamp, verticalClamp);
            depthDisplacement = Mathf.Clamp(depthDisplacement, 0, 1);

            //send values to shader
            front.SetFloat("_HorAmount", horizontalDisplacement);
            front.SetFloat("_VertAmount", verticalDisplacement);
            front.SetFloat("_DepthAmount", depthDisplacement);

            back.SetFloat("_HorAmount", horizontalDisplacement);
            back.SetFloat("_VertAmount", verticalDisplacement);
            back.SetFloat("_DepthAmount", depthDisplacement);

        }

        if(canInput && broken)
        {
            if(peelingOrder.currentPeel < peelingOrder.orderLength)
            { peelingOrder.currentPeel += 1; }
            else
            {
                //END GAME
            }

        }


        if (breakThreshold)
        {
            Fall();
        }

//originally would return to beginning position
        //horizontalDisplacement = Mathf.Lerp(horizontalDisplacement, 0, Time.deltaTime/10f);
        //verticalDisplacement = Mathf.Lerp(verticalDisplacement, 0, Time.deltaTime/10f);
    }

    void Fall()
    { 
        front.SetVector("_AnchorPoint", front.GetVector("_AnchorPoint") - new Vector4(0, Time.deltaTime * curlSpeed, 0, 0));
        back.SetVector("_AnchorPoint", front.GetVector("_AnchorPoint") - new Vector4(0, Time.deltaTime * curlSpeed, 0, 0));

            front.SetFloat("_VertAmount", front.GetFloat("_VertAmount") - Time.deltaTime * fallSpeed);
            back.SetFloat("_VertAmount", back.GetFloat("_VertAmount") - Time.deltaTime * fallSpeed);

        
    }

}
