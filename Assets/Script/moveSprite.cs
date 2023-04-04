using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveSprite : MonoBehaviour
{
    public Renderer renderer;
    public AudioSource sound;
    public List<AudioClip> sounds;
    public bool holding;
    public float dragX, dragY;
    public Vector2 dragPoint;
    public Vector3 startPosition;
    public bool done;
    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        startPosition = transform.position;
        transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-25, 25), 0);
        //sound.clip = sounds[Random.Range(0, sounds.Count)];
        holding = false;
        done = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("HIT");
    }

    void OnMouseDrag()
    {
        
        transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 5f);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        dragX += Input.GetAxis("Mouse X");
        dragY += Input.GetAxis("Mouse Y");
        holding = true;
    }

    void OnMouseUp()
    {
        sound.clip = sounds[Random.Range(0, sounds.Count)];
        sound.pitch = 1/(Mathf.Abs(dragX) + Mathf.Abs(dragY));
        sound.Play();

        holding = false;

    }

    private void Update()
    {
        float shakeAmount = 5;
        dragX = Mathf.Lerp(dragX, 0, Time.deltaTime * 3);
        dragY = Mathf.Lerp(dragY, 0, Time.deltaTime * 3);
        float distance = Vector3.Distance(startPosition, transform.position)/100;
        if (distance * 100 <= 0.5f)
        {
            distance = 0;
            done = true;
        }else{
            done = false;
        }
       /* if(!holding){
            shakeAmount = 40;
        }else{
            shakeAmount = 0;
        }*/

        renderer.material.SetFloat("_HorAmount", dragX);
        renderer.material.SetFloat("_VertAmount", dragY);
        renderer.material.SetFloat("_RandomAmount", distance);
        renderer.material.SetVector("_DragPoint", dragPoint);
    }

     public void Move()
    {
        startPosition = transform.position;
        transform.position = new Vector3(Random.Range(-10, 10), Random.Range(-25, 25), 0);
    }
}
