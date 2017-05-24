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
    [Header("Camera Movement Settings")]
    public float cameraSpeed = 8.5f;
    public bool useMouseToMove = false;
    public int pixelsBeforeBorder = 4;
    public int pixelsBeyondBorder = 100;
    private float speedScale;

    [Header("Camera Zoom Settings")]
    public List<float> cameraZooms = new List<float>();
    public float cameraZoomSpeed = 0.5f;
    private int currentCameraZoomIndex = 0;
    private int cameraZoomDirection; //0: Still | 1: Zoom-in | -1: Zoom-out
    private float currentCameraZoom = 0;
    private Vector3 cameraDirection = Vector3.zero;
    private float cameraHalfWidth;
    private float cameraHalfHeight;

    [Header("Camera Border Settings")]
    [Tooltip("Extra space (in squares) left around the map.\nClockwise, Starting from Top.")]
    public Vector4 extraBorder = new Vector4(2, 5, 2, 3);
    private float mapBottomBorder;
    private float mapTopBorder;
    private float mapLeftBorder;
    private float mapRightBorder;

    void Start()
    {
        ResetCameraBorders(10, 10);
        cameraZooms.Sort();
        currentCameraZoom = cameraZooms[0];
        UpdateCameraZoom();

    }

    void Update()
    {
        MoveCamera();        
        Zoom();
    }

    void MoveCamera()
    {
        if (useMouseToMove && !(Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.UpArrow)))
        {
            MoveCameraWithMouse();
        }
        MoveCameraWithArrows();
    }

    void MoveCameraWithMouse()
    {
        Vector3 mousePos = Input.mousePosition;
        if (mousePos.x < pixelsBeforeBorder && mousePos.x > -pixelsBeyondBorder)
        {
            if (transform.position.x - cameraHalfWidth > mapLeftBorder)
                cameraDirection.x = -1;
            else
                cameraDirection.x = 0;
        }
        else if (mousePos.x > Screen.width - pixelsBeforeBorder && mousePos.x < Screen.width + pixelsBeyondBorder)
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
        if (mousePos.y < pixelsBeforeBorder && mousePos.y > -pixelsBeyondBorder)
        {
            if (transform.position.y - cameraHalfHeight > mapBottomBorder)
                cameraDirection.y = -1;
            else
                cameraDirection.y = 0;
        }
        else if (mousePos.y > Screen.height - pixelsBeforeBorder && mousePos.y < Screen.height + pixelsBeyondBorder)
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
        speedScale = currentCameraZoom / cameraZooms[0];
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
        speedScale = currentCameraZoom / cameraZooms[0];
        transform.Translate(cameraDirection * cameraSpeed * speedScale * Time.deltaTime);
    }

    public void ResetCameraBorders(int mapSizeX, int mapSizeY)
    {
        transform.position = new Vector3(0, 0, -10);
        mapRightBorder = mapSizeX / 2 + extraBorder.y;
        mapLeftBorder = -(mapSizeX / 2 + extraBorder.w);
        mapTopBorder = mapSizeY / 2 + extraBorder.x;
        mapBottomBorder = -(mapSizeY / 2 + extraBorder.z);
    }

    void UpdateCameraZoom()
    {
        Camera.main.orthographicSize = currentCameraZoom;
        cameraHalfHeight = Camera.main.orthographicSize;
        cameraHalfWidth = cameraHalfHeight * Screen.width / Screen.height;
    }

    void GetZoomDirectionFromScrollWheel()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && currentCameraZoomIndex > 0 && cameraZoomDirection == 0)            
        {
            cameraZoomDirection = +1;
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && currentCameraZoomIndex < cameraZooms.Count && cameraZoomDirection == 0)
        {
            cameraZoomDirection = -1;
        }
    }

    void Zoom()
    {
        GetZoomDirectionFromScrollWheel();
        float cameraMaxZoom = cameraZooms[currentCameraZoomIndex];
        float cameraMinZoom = cameraZooms[currentCameraZoomIndex];
        if (currentCameraZoomIndex > 0)
        {
            cameraMinZoom = cameraZooms[currentCameraZoomIndex - 1];
        }
        if (currentCameraZoomIndex < cameraZooms.Count - 1)
        {
            cameraMaxZoom = cameraZooms[currentCameraZoomIndex + 1];
        }

        if (cameraZoomDirection > 0 && currentCameraZoom > cameraMinZoom) //zoom in
        {
            currentCameraZoom -= cameraZoomSpeed * Time.deltaTime;
            currentCameraZoom = Mathf.Clamp(currentCameraZoom, cameraMinZoom, cameraMaxZoom);
            UpdateCameraZoom();
        }        
        else if (cameraZoomDirection < 0 && currentCameraZoom < cameraMaxZoom) //zoom out
        {
            currentCameraZoom += cameraZoomSpeed * Time.deltaTime;
            currentCameraZoom = Mathf.Clamp(currentCameraZoom, cameraMinZoom, cameraMaxZoom);
            UpdateCameraZoom();
        }
        else
        {
            if(cameraZoomDirection > 0 && currentCameraZoomIndex > 0)
            {
                currentCameraZoomIndex--;
            }
            if(cameraZoomDirection < 0 && currentCameraZoomIndex < cameraZooms.Count - 1)
            {
                currentCameraZoomIndex++;
            }
            cameraZoomDirection = 0;
        }
    }
 
}

