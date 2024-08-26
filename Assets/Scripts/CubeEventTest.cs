using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventTest : MonoBehaviour
{
    
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Test();
        }
    }

    public void Test()
    {
        Managers.UI.ShowToastMessage("테스트 메세지");
    }
    
}
