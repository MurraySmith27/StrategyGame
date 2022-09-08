using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public float zoomRate = 1f;
    public float nearLimit = 10f;
    public float farLimit= 30f;

    public float xMinBound = -10f;
    public float xMaxBound = 90f;
    public float zMinBound = -10f;
    public float zMaxBound = 90f;

    public float scrollSpeed = 0.4f;

    private bool isDragging = false;
    private Vector3 lastFrameMousePos;


    Camera mainCamera;
    private void Awake()
    {
        mainCamera = gameObject.GetComponent<Camera>();
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera.depthTextureMode = DepthTextureMode.Depth;
    }

    // Update is called once per frame
    void Update()
    {
        float positionInFacingDirection = (gameObject.transform.rotation * gameObject.transform.position).y;
        if ((Input.mouseScrollDelta.y > 0 && nearLimit < positionInFacingDirection) ||
            (Input.mouseScrollDelta.y < 0 && positionInFacingDirection < farLimit)){
            gameObject.transform.localPosition =  gameObject.transform.localPosition + (gameObject.transform.rotation * (new Vector3(0,0, Input.mouseScrollDelta.y * zoomRate)));
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 currentPosition = gameObject.transform.rotation * Input.mousePosition;
            if (isDragging)
            {
                Vector3 diff = lastFrameMousePos - currentPosition;
                Vector3 amountToMove = scrollSpeed * new Vector3(diff.x, 0, diff.z);
                Vector3 newPosition = gameObject.transform.position + amountToMove;

                newPosition.x = Mathf.Min(Mathf.Max(newPosition.x, xMinBound), xMaxBound);
                newPosition.z = Mathf.Min(Mathf.Max(newPosition.z, zMinBound), zMaxBound);

                gameObject.transform.position = newPosition;
            }
            isDragging = true;
            lastFrameMousePos = currentPosition;
        }
        else
        {
            isDragging = false;
        }
    }
}
