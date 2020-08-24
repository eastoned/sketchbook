using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scrollControl : MonoBehaviour
{
    public List<GameObject> texts;
    public float sensitivity;
    public List<float> momentum = new List<float>();
    public List<Vector3> originalPos = new List<Vector3>();
    public float scrollMax, scrollMin, scrollAmount;
    //public AudioSource swipe;
    // Start is called before the first frame update
    void Start()
    {
        scrollAmount = texts[0].GetComponent<RectTransform>().anchoredPosition3D.y;
        for (int i = 0; i < texts.Count; i++)
        {
            momentum.Add(0);
            originalPos.Add(texts[i].GetComponent<RectTransform>().anchoredPosition3D);
            //momentum[i] = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        scrollAmount = texts[0].GetComponent<RectTransform>().anchoredPosition3D.y;

        if(Input.GetMouseButton(0)){
            
            if( scrollAmount <= scrollMax && scrollAmount >= scrollMin){
                for (int i = 0; i < texts.Count; i++)
                {
                    momentum[i] += Input.GetAxis("Mouse Y");
                    originalPos[i] = texts[i].GetComponent<RectTransform>().anchoredPosition3D;
                }

            }else{
                for (int i = 0; i < texts.Count; i++)
                {
                    momentum[i] = 0;
                    if(scrollAmount > scrollMax){
                        texts[i].GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(texts[i].GetComponent<RectTransform>().anchoredPosition3D, originalPos[i] - new Vector3(0, 5, 0), Time.deltaTime);
                    }else if (scrollAmount < scrollMin){
                        texts[i].GetComponent<RectTransform>().anchoredPosition3D = Vector3.Lerp(texts[i].GetComponent<RectTransform>().anchoredPosition3D, originalPos[i] + new Vector3(0,5,0), Time.deltaTime);
                    }

                }
            }

            //momentum += Input.GetAxis("Mouse Y") * texts[i].transform.position.y;
        }else{
            for (int i = 0; i < texts.Count; i++)
            {
                momentum[i] = Mathf.Lerp(momentum[i], 0, 4f * Time.deltaTime);
            }
            //momentum = Mathf.Lerp(momentum, 0, 3f*Time.deltaTime);
        }

        for (int i = 0; i < texts.Count; i++)
        {
               
            texts[i].GetComponent<RectTransform>().anchoredPosition3D += Vector3.up * momentum[i] * Time.deltaTime * sensitivity;
            //texts[i].transform.position = Vector3.Lerp(texts[i].transform.position, texts[i].transform.position + (Vector3.up * Input.GetAxis("Mouse Y")), i*Time.deltaTime);
        }


    }
}
