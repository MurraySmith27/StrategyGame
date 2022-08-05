using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float zoomRate = 1f;

    Vector3 initialPosition;

    Camera mainCamera;
    private void Awake()
    {
        mainCamera = gameObject.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = gameObject.transform.localPosition;
        mainCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0) {
            gameObject.transform.localPosition =  gameObject.transform.localPosition + (gameObject.transform.rotation * (new Vector3(0,0, Input.mouseScrollDelta.y * zoomRate)));
        }


        
    }
}
