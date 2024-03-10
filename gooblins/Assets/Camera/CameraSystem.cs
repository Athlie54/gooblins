using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Runtime.CompilerServices;
using System.Xml.Schema;

public class CameraSystem : MonoBehaviour
{

    [SerializeField] private float orthographicSizeMax = 30;
    [SerializeField] private float orthographicSizeMin = 1;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float xMax = 29f;
    [SerializeField] private float xMin = -1f;
    [SerializeField] private float yMax = 29f;
    [SerializeField] private float yMin = -1f;

    private float targetOrthographicSize = 5;

    public float moveSpeed = 2f;

    private void FixedUpdate() {
        HandleCameraMovement();
        HandleCameraZoom_OrthographicSize();

        //Debug.Log(transform.position); 
    }
        
    private void HandleCameraMovement()
    {
        Vector2 moveDir = new Vector2(0, 0);

        if (Input.GetKey(KeyCode.W)) moveDir.y = +0.3f;
        if (Input.GetKey(KeyCode.S)) moveDir.y = -0.3f;
        if (Input.GetKey(KeyCode.A)) moveDir.x = -0.3f;
        if (Input.GetKey(KeyCode.D)) moveDir.x = +0.3f;

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
        Debug.Log($"camera x = {Mathf.Clamp(transform.position.x + moveDir.x, xMax, xMin)}");
        Debug.Log($"camera y = {Mathf.Clamp(transform.position.y + moveDir.y, yMax, yMin)}");

        transform.position = new Vector2(transform.position.x + moveDir.x, transform.position.y + moveDir.y);
        //transform.position.x = Mathf.Clamp(transform.position.x + moveDir.x, xMax, xMin);
        //transform.position.y = Mathf.Clamp(transform.position.y + moveDir.y, yMax, yMin);


    }
    
    private void HandleCameraZoom_OrthographicSize()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            targetOrthographicSize -= 1;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            targetOrthographicSize += 1;
        }

        targetOrthographicSize = Mathf.Clamp(targetOrthographicSize, orthographicSizeMin, orthographicSizeMax);

        float zoomSpeed = 4f;
        cinemachineVirtualCamera.m_Lens.OrthographicSize = Mathf.Lerp(cinemachineVirtualCamera.m_Lens.OrthographicSize, targetOrthographicSize, Time.deltaTime * zoomSpeed);
    }
    
}
