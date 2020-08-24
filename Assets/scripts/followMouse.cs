using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followMouse : MonoBehaviour
{

    public Vector2 allowedDistance;
    public Vector3 originalPlace;
    // Start is called before the first frame update
    private void Start()
    {
        originalPlace = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        target = new Vector3(Mathf.Clamp(target.x, -1, 1), Mathf.Clamp(target.y, originalPlace.y - 0.3f, originalPlace.y + 0.3f), 0);
        //if(Mathf.Abs(originalPlace.x - transform.position.x) < allowedDistance.x && Mathf.Abs(originalPlace.y - transform.position.y) < allowedDistance.y){
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * 0.25f);
        //}else{
        //transform.position = Vector3.Lerp(transform.position, originalPlace, Time.deltaTime * 1f);
        //}
        Debug.Log(target);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);





    }
}
