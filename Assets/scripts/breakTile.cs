using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakTile : MonoBehaviour
{
    public Renderer renderer;
    public bool holding, grabbed, died;
    public float dragX, dragY;
    public float speed;
    public Vector2 dragPoint;
    public AudioSource sound;

    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        holding = false;
        grabbed = false;
        speed = Random.Range(1f, 5f);
        died = false;
        renderer.material.SetFloat("_RandomAmount", Random.Range(-.01f, .01f));
    }

    void OnMouseDrag()
    {
        
        dragX += Input.GetAxis("Mouse X")/10;
        dragY += Input.GetAxis("Mouse Y")/10;
        holding = true;

        if(Mathf.Abs(dragX) >= 0.5f || Mathf.Abs(dragY) >= 0.5f){
            grabbed = true;
            if (!sound.isPlaying)
            {
                sound.pitch = Random.Range(0.5f, 1.5f);
                sound.Play();
            }

        }
    }
    void OnMouseOver()
    {
        
    }


    void OnMouseUp()
    {
        grabbed = false;
    }

    void Update()
    {
        
        dragX = Mathf.Lerp(dragX, 0, 2*Time.deltaTime);
        dragY = Mathf.Lerp(dragY, 0, 2*Time.deltaTime);

        renderer.material.SetFloat("_HorAmount", dragX/2);
        renderer.material.SetFloat("_VertAmount", dragY/2);

        if(grabbed){
            transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 7f);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        if(transform.position.y <= -5.1f){
            if (!sound.isPlaying && !died)
            {
                sound.pitch = Random.Range(0.5f, 1.5f);
                sound.Play();
                died = true;
            }
        }

    }
}
