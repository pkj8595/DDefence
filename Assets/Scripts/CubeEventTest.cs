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
        Destroy(this);

    }

    private void OnDisable()
    {
        Debug.Log("disable");
    }

    private void OnDestroy()
    {
        Debug.Log("destroy");
    }

}
