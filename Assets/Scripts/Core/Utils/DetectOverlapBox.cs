using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectOverlapBox : MonoBehaviour, IDetectComponent
{
    [Header("OverlapBox Settings")]
    public Vector3 boxSize = new Vector3(1.0f, 1.0f, 1.0f); // Box의 크기
    public Vector3 boxCenterOffset;                         // Box의 중심 위치 오프셋
    public LayerMask collisionLayers;                       // 충돌 레이어 설정
    public Quaternion boxOrientation = Quaternion.identity; // Box의 회전 설정

    public Collider[] DetectCollision()
    {
        Vector3 boxCenter = transform.position + boxCenterOffset;
        Collider[] hitColliders = Physics.OverlapBox(boxCenter, boxSize / 2, boxOrientation, collisionLayers);
        return hitColliders;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + boxCenterOffset, boxSize);
    }
}
