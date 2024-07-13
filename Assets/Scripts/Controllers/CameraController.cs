using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.U2D;


public class CameraController : MonoBehaviour
{
    public GameObject _player;
    [SerializeField] private float _cameraSpeed;
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private PixelPerfectCamera _pixelCamera;
    [SerializeField] private float _pixelCameraLevel;

    private void Start()
    {
        _pixelCamera = GetComponent<PixelPerfectCamera>();
    }

    public void Update()
    {
    }

    private void LateUpdate()
    {
        if(_player != null)
        {
            OnMouseScroll_UpdateGameScale();
        }
        else
        {
            MoveCameraPosition();
            ClampCameraToTilemap();
        }
    }

    private void OnMouseScroll_UpdateGameScale()
    {
        transform.position = new Vector3(_player.transform.position.x, _player.transform.position.y, -10);

        float scrollAmount = Input.GetAxis("Mouse ScrollWheel");
        if (scrollAmount != 0)
        {
            scrollAmount = scrollAmount < 0 ? -1 : 1;
            _pixelCamera.refResolutionX -= (int)scrollAmount * 16;
            _pixelCamera.refResolutionY -= (int)scrollAmount * 9;
        }
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

    private void ClampCameraToTilemap()
    {
        BoundsInt tilemapBounds = _tilemap.cellBounds;

        //var camera = GetComponent<UnityEngine.U2D.PixelPerfectCamera>();
        //int pixelCameraX = camera.refResolutionX;
        //int pixelCameraY = camera.refResolutionY;

        Vector3 cameraPosition = transform.position;
        Vector3 tilemapMin = _tilemap.GetCellCenterWorld(tilemapBounds.min);
        Vector3 tilemapMax = _tilemap.GetCellCenterWorld(tilemapBounds.max);

        cameraPosition.x = Mathf.Clamp(cameraPosition.x, tilemapMin.x, tilemapMax.x);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, tilemapMin.y, tilemapMax.y);

        transform.position = cameraPosition;
    }
}
