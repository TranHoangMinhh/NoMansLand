using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraReplaceShader : MonoBehaviour
{
    [SerializeField] private Camera targetCamera;

    // Start is called before the first frame update
    void Start()
    {
        targetCamera.SetReplacementShader(Shader.Find("Unlit/Color"), "RenderType");
    }
}
