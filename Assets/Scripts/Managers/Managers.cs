using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Managers : MonoBehaviour
{
    static Managers s_Instance;
    static Managers Instance { get { InitManagers(); return s_Instance; } }

    #region Contens
    GameManager _game = new GameManager();
    
    public static GameManager Game { get { return Instance._game; } }
    #endregion
    
    #region Core
    DataManager _data = new DataManager();
    InputManager _input = new InputManager();
    PoolManager _pool = new PoolManager();
    ResourceManager _resource = new ResourceManager();
    SceneManagerEx _scene = new SceneManagerEx();
    SoundManager _sound = new SoundManager();
    UIManager _ui = new UIManager();

    public static DataManager Data { get { return Instance._data; } }
    public static InputManager Input { get { return Instance._input; } }
    public static PoolManager Pool { get { return Instance._pool; } }
    public static ResourceManager Resource { get { return Instance._resource; } }
    public static SceneManagerEx Scene { get { return Instance._scene; } }
    public static SoundManager Sound { get { return Instance._sound; } }
    public static UIManager UI { get { return Instance._ui; } }
    #endregion

    void Start()
    {
        Application.targetFrameRate = 30;
        InitManagers(); 
    }

    void Update()
    {
        _input.OnUpdate();
    }

    public static void InitManagers()
    {
        if(s_Instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if (go == null)
            {
                go = new GameObject { name = "@Managers" };
                go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);
            s_Instance = go.GetComponent<Managers>();

            s_Instance._scene.Init();
            s_Instance._data.Init();
            s_Instance._resource.Init();
            s_Instance._pool.Init();
            s_Instance._sound.Init();
            s_Instance._input.Init(go);
            s_Instance._ui.Init(go);

            DOTween.Init(true, true, LogBehaviour.Default).SetCapacity(200,10);

        }
    }

    public static void Clear()
    {
        Input.Clear();
        Sound.Clear();
        Scene.Clear();
        UI.Clear();
        Pool.Clear();
    }

    
}
