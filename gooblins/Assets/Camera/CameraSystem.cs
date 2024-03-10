using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;

public class CameraSystem : MonoBehaviour
{

    [SerializeField] private float orthographicSizeMax = 50;
    [SerializeField] private float orthographicSizeMin = 1;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    private float targetOrthographicSize = 5;

    public float moveSpeed = 5f;

    private void FixedUpdate() {
        HandleCameraMovement();
        HandleCameraZoom_OrthographicSize();

        //Debug.Log(transform.position); 
    }
        
    private void HandleCameraMovement()
    {
        Vector2 moveDir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.y = +1f;
        if (Input.GetKey(KeyCode.S)) moveDir.y = -1f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;

        // int.edgeScrollSize = 20;

        // if (Input.mousePosition.x < edgeScrollSize) {
        //     inputDir.x = -1f;
        // }
        // if (Input.mousePosition.y < edgeScrollSize) {
        //     inputDir.y = -1f;
        // }
        // if (Input.mousePosition.x > Screen.width - edgeScrollSize) {
        //     inputDir.x = -1f;
        // }
        // if (Input.mousePosition.y > Screen.height - edgeScrollSize) {
        //     inputDir.y = -1f;
        // }

        moveDir.x += moveDir.x * moveSpeed * Time.deltaTime;
        moveDir.y += moveDir.y * moveSpeed * Time.deltaTime;
        //Debug.Log(moveDir.x +","+moveDir.y);
        transform.position = new Vector2(transform.position.x + moveDir.x, transform.position.y + moveDir.y);
    }

    private void HandleCameraZoom_OrthographicSize()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetOrthographicSize -= 5;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetOrthographicSize += 5;
        }

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, orthographicSizeMin, orthographicSizeMax);

        float zoomSpeed = 10f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }
}
