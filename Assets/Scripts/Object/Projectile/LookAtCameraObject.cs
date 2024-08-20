using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCameraObject : MonoBehaviour
{
    private Transform _parentTransform;
    void Start()
    {
        _parentTransform = transform.parent;
    }

    private void LateUpdate()
    {
        Vector3 characterForward = _parentTransform.forward;
        // 캐릭터의 forward 벡터를 카메라 좌표계로 변환
        transform.localRotation = Quaternion.Euler(Camera.main.transform.InverseTransformDirection(characterForward));
    }
}
