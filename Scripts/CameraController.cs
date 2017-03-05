using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Classe responsabile del movimento della telecamera in giro per la mappa.
 * Gestisce il movimento con le frecce direzionali, oppure se abilitato nell'editor di Unity
 * gestisce anche il movimento col mouse sui bordi
 * Inoltre gestisce il livello di zoom, controllabile con la rotellina del mouse
 */

public class CameraController : MonoBehaviour
{
    public bool useMouseToMove = false;
    public float cameraSpeed = 8.5f;
    public float cameraMaxZoom = 10;
    public float cameraMinZoom = 5;
    public float cameraZoomSpeed = 0.5f;
    float cameraCurrentZoom = 10;
    public int pixelsBeforeBorder = 4;
    public int pixelsBeyongBorder = 100;

    Vector3 cameraDirection = Vector3.zero;

    public Vector2 extraBorder = new Vector2(3, 2);
    float mapBottomBorder;
    float mapTopBorder;
    float mapLeftBorder;
    float mapRightBorder;

    float cameraHalfWidth;
    float cameraHalfHeight;

    public float speedScale;


    // Use this for initialization
    void Start()
    {
        ResetCameraBorders(10, 10);
        UpdateCameraZoom();
    }

    // Update is called once per frame
    void Update()
    {
        if (!(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) ||
             Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow)) && useMouseToMove)
            MoveCameraWithMouse();
        MoveCameraWithArrows();

        ResizeCameraWithScrollWheel();
    }

    void MoveCameraWithMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < pixelsBeforeBorder && mousePos.x > -pixelsBeyongBorder)
        {
            if (transform.position.x - cameraHalfWidth > mapLeftBorder)
                cameraDirection.x = -1;
            else
                cameraDirection.x = 0;
        }
        else if (mousePos.x > Screen.width - pixelsBeforeBorder && mousePos.x < Screen.width + pixelsBeyongBorder)
        {
            if (transform.position.x + cameraHalfWidth < mapRightBorder)
                cameraDirection.x = 1;
            else
                cameraDirection.x = 0;
        }
        else
        {
            cameraDirection.x = 0;
        }
        if (mousePos.y < pixelsBeforeBorder && mousePos.y > -pixelsBeyongBorder)
        {
            if (transform.position.y - cameraHalfHeight > mapBottomBorder)
                cameraDirection.y = -1;
            else
                cameraDirection.y = 0;
        }
        else if (mousePos.y > Screen.height - pixelsBeforeBorder && mousePos.y < Screen.height + pixelsBeyongBorder)
        {
            if (transform.position.y + cameraHalfHeight < mapTopBorder)
                cameraDirection.y = 1;
            else
                cameraDirection.y = 0;
        }
        else
        {
            cameraDirection.y = 0;
        }
        //cameraDirection.Normalize();
        speedScale = cameraCurrentZoom / cameraMinZoom;
        transform.Translate(cameraDirection * cameraSpeed * speedScale * Time.deltaTime);
    }

    void MoveCameraWithArrows()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x - cameraHalfWidth > mapLeftBorder)
                cameraDirection.x = -1;
            else
                cameraDirection.x = 0;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x + cameraHalfWidth < mapRightBorder)
                cameraDirection.x = 1;
            else
                cameraDirection.x = 0;
        }
        else
        {
            cameraDirection.x = 0;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.y - cameraHalfHeight > mapBottomBorder)
                cameraDirection.y = -1;
            else
                cameraDirection.y = 0;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.y + cameraHalfHeight < mapTopBorder)
                cameraDirection.y = 1;
            else
                cameraDirection.y = 0;
        }
        else
        {
            cameraDirection.y = 0;
        }
        //cameraDirection.Normalize();
        speedScale = cameraCurrentZoom / cameraMinZoom;
        transform.Translate(cameraDirection * cameraSpeed * speedScale * Time.deltaTime);
    }

    public void ResetCameraBorders(int mapSizeX, int mapSizeY)
    {
        transform.position = new Vector3(0, 0, -10);
        mapRightBorder = mapSizeX / 2 + extraBorder.x;
        mapLeftBorder = -mapRightBorder;
        mapTopBorder = mapSizeY / 2 + extraBorder.y;
        mapBottomBorder = -mapTopBorder;
    }

    void UpdateCameraZoom()
    {
        Camera.main.orthographicSize = cameraCurrentZoom;
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Screen.width / Screen.height;
    }

    void ResizeCameraWithScrollWheel()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && cameraCurrentZoom > cameraMinZoom)
        {
            cameraCurrentZoom -= cameraZoomSpeed * Time.deltaTime;
            cameraCurrentZoom = Mathf.Clamp(cameraCurrentZoom, cameraMinZoom, cameraMaxZoom);
            UpdateCameraZoom();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && cameraCurrentZoom < cameraMaxZoom)
        {
            cameraCurrentZoom += cameraZoomSpeed * Time.deltaTime;
            cameraCurrentZoom = Mathf.Clamp(cameraCurrentZoom, cameraMinZoom, cameraMaxZoom);
            UpdateCameraZoom();
        }
    }
}
