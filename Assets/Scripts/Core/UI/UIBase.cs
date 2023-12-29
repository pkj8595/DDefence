using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    Canvas canvas;
    private string uiName;

    public string UIName { get => uiName; set => uiName = value; }

    public void SetUIBaseData(int sortingOrder)
    {
        uiName = this.GetType().Name;
    }

    public virtual void Init(UIData uiData)
    {
        gameObject.SetActive(true);
    }

    public virtual void UpdateUI()
    {

    }

    public virtual void Hide()
    {
        gameObject.SetActive(false);
    }
    public virtual void Close()
    {

    }
}
