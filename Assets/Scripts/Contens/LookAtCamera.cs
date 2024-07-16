using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _render;
    private Vector3 _preVec = Vector3.zero;

    private void LateUpdate()
    {
        //transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
        //                    Camera.main.transform.rotation * Vector3.up);

        //카메라 기준으로 캐릭터의 방향벡터를 계산하여 캐릭터가 움직이는 방향을 자연스럽게 한다.
        Vector3 thisVector = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 dirVector = thisVector - _preVec;
        _render.flipX = dirVector.normalized.x < 0;
        _preVec = thisVector;

        //캐릭터의 Y축이 카메라와 동일하게 하여 카메라를 바라보게 한다.
        Vector3 cameraAngle = Camera.main.transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(new Vector3(0f, cameraAngle.y, 0f));

        //카메라 위치에 따라 스케일을 조정한다.
        transform.localScale = new Vector3(transform.localScale.x,
                                            transform.localScale.x + GetScalePercent(cameraAngle),
                                            transform.localScale.z);
    }

    /// <summary>
    /// 20~60도 까지 비율에 따라 스케일을 조절한다.
    /// </summary>
    /// <param name="cameraAngle"></param>
    /// <returns></returns>
    private static float GetScalePercent(Vector3 cameraAngle)
    {
        return (float)((Mathf.Clamp(cameraAngle.x, 30f, 60f) - 30f) * 0.015);
    }
}
