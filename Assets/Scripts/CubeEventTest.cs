using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventTest : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Managers.Effect.PlayAniEffect("EletricA", this.transform);
        }
    }
}
