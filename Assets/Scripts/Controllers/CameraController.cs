using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;


public class CameraController : MonoBehaviour
{
    [SerializeField] private float _cameraSpeed;
    
  
    public void Update()
    {
    }


    
    private void MoveCameraPosition()
    {
        //마우스 포지션 확인
        Vector3 moveDirection = Vector3.zero;
        if (!(0 < Input.mousePosition.x && Input.mousePosition.x < Screen.width))
            moveDirection.x = (0 < Input.mousePosition.x ? 1f : -1f);
        if (!(0 < Input.mousePosition.y && Input.mousePosition.y < Screen.height))
            moveDirection.y = (0 < Input.mousePosition.y ? 1f : -1f);

        transform.Translate(moveDirection.normalized * _cameraSpeed * Time.deltaTime);
    }


    public void MoveWASD()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        }

    }

}
