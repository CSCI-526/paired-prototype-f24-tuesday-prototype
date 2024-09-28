using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControlSystem : MonoBehaviour
{
    public GameObject map;
    public float moveSpeedMin = 1.0f, moveSpeedMax = 5.0f;
    public float zoomSpeed = 1.0f, zoomMin = 0.5f, zoomMax = 5.0f;

    private Vector3 mapBoundMin, mapBoundMax;
    private Vector3 mousePos;
    private float mouseScroll;

    private void Start()
    {
        SpriteRenderer spriteRenderer = map.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            mapBoundMin = spriteRenderer.bounds.min;
            mapBoundMax = spriteRenderer.bounds.max;
        }
        else
        {
            mapBoundMin = Vector3.zero;
            mapBoundMax = Vector3.zero;
        }
    }

    void Update()
    {
        mousePos = Input.mousePosition;

        Vector3 movement = Vector3.zero;
        float moveSpeed = Mathf.Lerp(moveSpeedMin, moveSpeedMax,
                                    (transform.GetComponent<Camera>().orthographicSize - zoomMin) / (zoomMax - zoomMin));
        if (mousePos.x <= 0)
            movement.x = -moveSpeed * Time.deltaTime;
        else if (mousePos.x >= Screen.width - 1)
            movement.x = moveSpeed * Time.deltaTime;
        if (mousePos.y <= 0)
            movement.y = -moveSpeed * Time.deltaTime;
        else if (mousePos.y >= Screen.height - 1)
            movement.y = moveSpeed * Time.deltaTime;

        Vector3 newCamPos = transform.position;
        newCamPos += movement;
        newCamPos.x = Mathf.Clamp(newCamPos.x, mapBoundMin.x, mapBoundMax.x);
        newCamPos.y = Mathf.Clamp(newCamPos.y, mapBoundMin.y, mapBoundMax.y);
        transform.position = newCamPos;

        float mouseScroll = Input.GetAxis("Mouse ScrollWheel");
        if (mouseScroll != 0.0f)
        {
            float newCamZoom = transform.GetComponent<Camera>().orthographicSize;
            newCamZoom -= mouseScroll * zoomSpeed;
            transform.GetComponent<Camera>().orthographicSize = Mathf.Clamp(newCamZoom, zoomMin, zoomMax);
        }
    }
}
