using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class tileGenerator : MonoBehaviour
{
    public GameObject tile;
    public List<Sprite> tileImg;
    public List<GameObject> tiles = new List<GameObject>();
    public List<AudioClip> chimes;
    public bool newGrid;
    public float newGridTimer;
    public BoxCollider2D handleCol;
    public AudioMixerGroup mixer;


    void OnMouseDown(BoxCollider2D collider2D)
    {
        Debug.Log("hepl");
        if(collider2D == handleCol){
            newGrid = true;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        newGrid = false;
        newGridTimer = 11;
        tileGen();
    }

    // Update is called once per frame
    void Update()
    {   if(newGrid){
            for (int i = 0; i < tiles.Count; i++)
            {
                tiles[i].transform.position -= new Vector3(0, tiles[i].GetComponent<breakTile>().speed * Time.deltaTime, 0);
            }
            newGridTimer -= Time.deltaTime;
        }

        if(newGridTimer <= 0){
            newGrid = false;
            newGridTimer = 11;
            for (int i = 0; i < tiles.Count; i++)
            {
                Destroy(tiles[i]);

            }
            tiles.Clear();
            tileGen();
        }
    }

    void tileGen(){
        for (int i = 0; i < 11; i++)
        {
            for (int j = 0; j < 16; j++)
            {
                GameObject tileClone = (GameObject)Instantiate(tile, new Vector3(-3.05f + (0.675f * i), -5f + (0.675f * j), 0), Quaternion.Euler(Quaternion.ToEulerAngles(Quaternion.identity) + new Vector3(0, 0, 90 * (int)Random.Range(0, 4))));
                tiles.Add(tileClone);
                tileClone.GetComponent<SpriteRenderer>().sprite = tileImg[Random.Range(0, 8)];
                tileClone.AddComponent<BoxCollider2D>();
                tileClone.AddComponent<AudioSource>();
                tileClone.GetComponent<AudioSource>().clip = chimes[Random.Range(0, chimes.Count)];
                tileClone.GetComponent<AudioSource>().outputAudioMixerGroup = mixer;
                tileClone.GetComponent<SpriteRenderer>().sortingOrder = Random.Range(-3, 0);
            }
        }
    }
        
    
}
