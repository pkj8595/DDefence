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
        var data = Managers.Data.StatDict.GetEnumerator();
        while (data.MoveNext())
        {
            Debug.Log(data.Current.Value.name);
        }

    }



}
