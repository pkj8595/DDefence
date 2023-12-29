using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestColision : MonoBehaviour
{
    /*
     1. 나 혹은 상대에게 regidBody가 있어야한다. (is kinematic : off)
     2. 나에게 collider가 있어야한다.(is kinematic : off)
     3. 상대 collider가 있어야한다.(is kinematic : off)
     
     collider 충돌 검사
     regidBody 물리
     is kinematic를 사용하면 유니티 물리엔진의 영향을 받지 않는다.
     */
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter");
    }

    //collider에 is Trigger가 켜져있으면 물리연산을 진행하지 않고 trigger 역할만 한다.
    // 1) 둘다 collider가 있어야 한다.
    // 2) 둘 중 하나는 IsTrigger : on
    // 3) 둘 중 하나는 RigidBody가 있어야한다.
    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("OnCollisionEnter");
    }
    void Start()
    {

    }

    void Update()
    {
        //로컬을 월드로 변환
        //Vector3 look = transform.TransformDirection(Vector3.forward);
        //Debug.DrawRay(transform.position + Vector3.up, look * 10, Color.red);
        //if (Physics.Raycast(transform.position + Vector3.up, look, out RaycastHit hit,10))
        //{
        //    Debug.Log($"raycast {hit.collider.gameObject.name}");
        //}

        //Debug.Log(Input.mousePosition); // 스크린 좌표

        //Debug.Log(Camera.main.ScreenToViewportPoint(Input.mousePosition)); //화면 비율 표기 viewport

        //if (Input.GetMouseButtonDown(0))
        //{
        //    //스크린상 위치한 마우스의 월드 레이를 가져온다.
        //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //    Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        //    //int mask = (1 << 8) | (1 << 9); // 768 << 비트 플레그를 이용한 레이어 마스크 식별 8,9번째 레이어 마스크 가져오기
        //    LayerMask mask = LayerMask.GetMask("Monster") | LayerMask.GetMask("Wall");// 레이어마스크 struct 사용하서 마스크 가져오기

        //    if (Physics.Raycast(ray, out RaycastHit hit, 100.0f, mask))
        //    {
                
        //        Debug.Log($"Raycast Camera {hit.collider.gameObject.name} >> {hit.collider.gameObject.tag}");
        //    }

        //}

        //수동으로 
        //if (Input.GetMouseButtonDown(0))
        //{
              //카메라 near를 y 축으로 월드에 나타는 마우스의 월드 position를 가져온다.
        //    Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //    Vector3 dir = mousePos - Camera.main.transform.position;
        //    dir = dir.normalized;

        //    Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);

        //    if (Physics.Raycast(Camera.main.transform.position, dir, out RaycastHit hit, 100.0f))
        //    {
        //        Debug.Log($"Raycast Camera {hit.collider.gameObject.name}");
        //    }

        //}


    }
}
