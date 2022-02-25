using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer player;

    private void Start()
    {
        player = GetComponent<VideoPlayer>();
        player.url = System.IO.Path.Combine(Application.streamingAssetsPath, "Untitled_Artwork.mp4");
        player.Play();
        
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && player.time < player.length-1)
        {
            player.Play();
            player.time += 1;
            
            Debug.Log(player.time);
            Debug.Log(player.length);
            //Application.OpenURL("http://unity3d.com/");
        }
        else {player.Pause(); }
        

    }
}
