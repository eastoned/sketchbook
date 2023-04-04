using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class touchFace : MonoBehaviour
{
    public Renderer renderer;
    public AudioSource sound;
    public List<AudioClip> sounds;
    public bool holding;
    public float dragX, dragY;
    public Vector2 dragPoint;

    public void Start()
    {
        renderer = GetComponent<SpriteRenderer>();
        sound = GetComponent<AudioSource>();
        //sound.clip = sounds[Random.Range(0, sounds.Count)];
        holding = false;
    }

    private void OnMouseDown()
    {
        Debug.Log("HIT");
    }

    void OnMouseOver()
    {
        //transform.position = Vector3.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), Time.deltaTime * 4f);
        //transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        dragX += Input.GetAxis("Mouse X")/50f;
        dragY += Input.GetAxis("Mouse Y")/50f;
        holding = true;
        /*if(!sound.isPlaying){
            sound.pitch = 1/(Mathf.Abs(dragX) + Mathf.Abs(dragY)) + 1;
            Debug.Log(sound.pitch);
            sound.clip = sounds[Random.Range(0, sounds.Count)];
            sound.Play();
        }*/

    }

    void OnMouseUp()
    {
        /*sound.clip = sounds[Random.Range(0, sounds.Count)];
        sound.pitch = 1 / (Mathf.Abs(dragX) + Mathf.Abs(dragY));
        sound.Play();

        holding = false;*/

    }

    private void Update()
    {
        float shakeAmount = 0;
        dragX = Mathf.Lerp(dragX, 0, Time.deltaTime);
        dragY = Mathf.Lerp(dragY, 0, Time.deltaTime);

        if (!holding)
        {
            shakeAmount = 40;
        }
        else
        {
            shakeAmount = 10;
        }

        renderer.material.SetFloat("_HorAmount", dragX);
        renderer.material.SetFloat("_VertAmount", dragY);
        renderer.material.SetVector("_DragPoint", dragPoint);
    }
}
