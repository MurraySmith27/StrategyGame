using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraRay : MonoBehaviour
{

    public Camera cam;

    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        ray = new Ray();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)){ // if left button pressed...
            ray = cam.ScreenPointToRay(Input.mousePosition);
            
        }
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
    }
}
