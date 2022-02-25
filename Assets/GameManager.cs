using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //states: clicking to grab pickup, holding, dropping, watching

    public enum GameState
    {
        PickingUp,
        Holding,
        Dropping,
        Watching
    }
    public GameState state = GameState.Watching;
    public GameObject heldItem = null;

    private void Update()
    {
        switch (state)
        {
            case GameState.PickingUp:
                PickingUp();
                break;
            case GameState.Holding:
                Holding();
                if (Input.GetMouseButtonUp(0))
                {
                    state = GameState.Dropping;
                }
                break;
            case GameState.Dropping:
                Dropping(Camera.main.ScreenToWorldPoint(Input.mousePosition));
                break;
            case GameState.Watching:

                if (Input.GetMouseButtonDown(0))
                {
                    state = GameState.PickingUp;
                }
                break;
        }
        
    }

    void PickingUp()
    {
        //check for colliders to grab
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D col = Physics2D.OverlapPoint(mousePos);
        if (!col.isTrigger)
        {
            heldItem = col.transform.gameObject;
            state = GameState.Holding;
        }
        else
        {
            state = GameState.Watching;
        }
        
    }

    void Holding()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(target.x, target.y, 0);
        heldItem.transform.position = Vector3.MoveTowards(heldItem.transform.position, target, 1f*Time.deltaTime);
       
    }

    void Dropping(Vector3 pos)
    {
        heldItem.transform.position = pos;
        heldItem.transform.position = new Vector3(heldItem.transform.position.x, heldItem.transform.position.y, 0);
        heldItem = null;
        state = GameState.Watching;
    }

}
