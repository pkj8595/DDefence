using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : ManagerBase
{
    public Action KeyAction = null;
    public Action<Define.MouseEvent> MouseAction = null;
    bool _pressed = false;
    float _pressedTime = 0;

    
    public void Init(GameObject managerObj)
    {
        base.Init();
        // todo : 나중에 풀기
        //if (!managerObj.TryGetComponent(out EventSystem eventSystem))
        //{
        //    managerObj.AddComponent<EventSystem>();
        //    managerObj.AddComponent<StandaloneInputModule>();
        //}
    }

    public override void OnUpdate()
    {
        //ui가 클릭된 상태라면
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.anyKey && KeyAction != null)
        {
            KeyAction.Invoke();
        }

        if(MouseAction != null)
        {
            if (Input.GetMouseButton(0))
            {
                if (!_pressed)
                {
                    MouseAction.Invoke(Define.MouseEvent.PointerDown);
                    _pressedTime = Time.time;
                }
                MouseAction.Invoke(Define.MouseEvent.Press);
                _pressed = true;
            }
            else
            {
                if (_pressed)
                {
                    if(Time.time < _pressedTime + 0.2f)
                        MouseAction.Invoke(Define.MouseEvent.Click);
                    MouseAction.Invoke(Define.MouseEvent.PointerUp);
                }
                _pressed = false;
                _pressedTime = 0;
            }
        }
    }

    public override void Clear()
    {
        KeyAction = null;
        MouseAction = null;
        _pressed = false;
    }
}
