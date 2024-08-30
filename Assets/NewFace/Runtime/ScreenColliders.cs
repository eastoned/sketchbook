using UnityEngine;

public class ScreenColliders : MonoBehaviour
{

    public Transform leftWall, rightWall, ceiling, floor;

    // Start is called before the first frame update
    void Start()
    {
        UpdateScreenColliders(new Vector2(Screen.width, Screen.height));
    }

    void OnEnable()
    {
        ScreenDetector.OnScreenSizeChanged += UpdateScreenColliders;
    }

    void OnDisable()
    {
        ScreenDetector.OnScreenSizeChanged -= UpdateScreenColliders;
    }

    void UpdateScreenColliders(Vector2 screenSize)
    {
        if(transform != null)
        {
            transform.localScale = new Vector3((float)screenSize.x/screenSize.y * 4, 4, 1);
            ceiling.localScale = transform.localScale;
            floor.localScale = transform.localScale;
            leftWall.position = new Vector3(transform.localScale.x/2f + 6f, 0, 101);
            rightWall.position = new Vector3(-leftWall.position.x, 0, 101);
        }
    }
}