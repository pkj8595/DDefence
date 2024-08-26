using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIBase : MonoBehaviour
{
    public Canvas canvas;

    public string UIName { get; set; }

    public virtual void SetUIBaseData()
    {
        if (canvas == null)
            canvas = GetComponent<Canvas>();
        UIName = this.GetType().Name;
    }

    public void SetSortingOrder(int sortingOrder)
    {
        canvas.sortingOrder = sortingOrder;
    }

    public virtual void Init(UIData uiData)
    {
        gameObject.SetActive(true);
    }

    public virtual void UpdateUI()
    {

    }

    public virtual void SetActive(bool isActive)
    {
        gameObject.SetActive(isActive);
    }
    public virtual void Close()
    {
        gameObject.SetActive(false);
    }
}
