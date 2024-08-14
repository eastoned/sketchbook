using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCreationController : MonoBehaviour
{
    [SerializeField] private GameObject[] parts;
    [SerializeField] private PartController bg;

    public List<GameObject> spawnObjs;
    public List<Character> spawnedChars;
    private int layerCount = 100;

    private void Start()
    {
        //Randomize();
    }

    [ContextMenu("Random")]
    public void Randomize()
    {
        ClearPrevFace(spawnObjs);
        bg.RandomizeData();
        
        foreach(GameObject part in parts)
        {
            for(int i = 0; i < Random.Range(1, 2); i++)
            {
                spawnObjs.Add(Instantiate(part, Random.insideUnitCircle, Quaternion.Euler(0, 0, Random.Range(0, 360))));
            }
        }
    }

    private void ClearPrevFace(List<GameObject> gos)
    {
        if(gos.Count > 0)
        {
            foreach(GameObject go in gos)
            {
                Destroy(go);
            }
            gos.Clear();
        }
    }

    public void DeleteCharacterHead(GameObject head)
    {
        foreach(Character cha in spawnedChars)
        {
            if(head == cha.head)
            {
                DeleteCharacter(cha);
            }
        }  
    }

    public void DeleteCharacter(Character cha)
    {
        foreach(GameObject go in cha.charObjs)
        {
            Destroy(go);
        }
        spawnedChars.Remove(cha);
    }

    public void FaceRandom()
    {
        //ClearPrevFace(spawnObjs);
        spawnObjs.Clear();
        bg.RandomizeData();
        GameObject alien = new GameObject("Alien: " + layerCount);
        
        Vector3 headPos = new Vector3(0f, Random.Range(-1.5f, 1.5f), 0.1f + layerCount);

        spawnObjs.Add(Instantiate(parts[0], headPos, Quaternion.Euler(0, 0, Random.Range(0, 0)), alien.transform));
        spawnObjs[0].GetComponent<BodyPartController>().cachePosition = headPos;
        spawnObjs[0].GetComponent<SpringJoint2D>().connectedAnchor = headPos;
        Vector3 neckPos = new Vector3(headPos.x, -2f, 0.2f + layerCount);
        spawnObjs.Add(Instantiate(parts[1], neckPos, Quaternion.Euler(0, 0, 0), alien.transform));
        spawnObjs[1].GetComponent<BodyPartController>().cachePosition = neckPos;
        spawnObjs[1].transform.localScale = new Vector3(1, headPos.y+2f, 1);
        Vector3 eyePos = new Vector3(headPos.x + Random.Range(-.5f, .5f), headPos.y + Random.Range(-.5f, .5f), -0.05f + layerCount);
        Vector3 eyePos2 = new Vector3(headPos.x - (eyePos.x - headPos.x), eyePos.y, -0.05f + layerCount);
        float eyePosY = Random.Range(-.3f, .3f);
        spawnObjs.Add(Instantiate(parts[2], eyePos, Quaternion.Euler(0, 0, 0), alien.transform));
        spawnObjs.Add(Instantiate(parts[2], eyePos2, Quaternion.Euler(0, 0, 0), alien.transform));
        spawnObjs[2].GetComponent<BodyPartController>().cachePosition = eyePos;
        spawnObjs[3].GetComponent<BodyPartController>().cachePosition = eyePos2;
        spawnObjs[2].GetComponent<SpringJoint2D>().connectedBody = spawnObjs[0].GetComponent<Rigidbody2D>();
        spawnObjs[3].GetComponent<SpringJoint2D>().connectedBody = spawnObjs[0].GetComponent<Rigidbody2D>();
        spawnObjs[2].GetComponent<SpringJoint2D>().connectedAnchor = new Vector2(-.5f, eyePosY);
        spawnObjs[3].GetComponent<SpringJoint2D>().connectedAnchor = new Vector2(.5f, eyePosY);
        spawnObjs[2].transform.localScale = new Vector3(.5f, .5f, 1);
        spawnObjs[3].transform.localScale = new Vector3(.5f, .5f, 1);
        /*
        spawnObjs.Add(Instantiate(parts[3], new Vector3(headPos.x, headPos.y - .05f, -0.1f + layerCount), Quaternion.Euler(0, 0, 0)));
        spawnObjs.Add(Instantiate(parts[4], new Vector3(headPos.x, headPos.y - .1f, 0f + layerCount), Quaternion.Euler(0, 0, 0)));
        spawnObjs[4].GetComponent<SpringJoint2D>().connectedBody = spawnObjs[0].GetComponent<Rigidbody2D>();
        spawnObjs[5].GetComponent<SpringJoint2D>().connectedBody = spawnObjs[0].GetComponent<Rigidbody2D>();
        spawnObjs[4].GetComponent<SpringJoint2D>().connectedAnchor = new Vector2(0, -.1f);
        spawnObjs[5].GetComponent<SpringJoint2D>().connectedAnchor = new Vector2(0, -.2f);
        */
        layerCount--;
        spawnedChars.Add(new Character(spawnObjs, spawnObjs[0]));
    }
}

[System.Serializable]
public class Character
{
    public List<GameObject> charObjs;
    public GameObject head;
    public Character(List<GameObject> spawnObjs, GameObject head)
    {
        charObjs = new List<GameObject>();
        charObjs.AddRange(spawnObjs);
        this.head = head;
    }
}