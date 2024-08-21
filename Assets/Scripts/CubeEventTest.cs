using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeEventTest : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
          
        }
    }

    private float GetCalculateDamege(float damage, float protection)
    {
        if (0 <= protection)
        {
            return (float)(damage / (1f + (protection * 0.01f)));
        }
        else
        {
            return damage;
        }
    }
}
