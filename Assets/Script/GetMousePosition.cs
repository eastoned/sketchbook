using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMousePosition : MonoBehaviour
{
    private void Update()
    {
        Shader.SetGlobalVector("_MousePos", Input.mousePosition);
    }
}
