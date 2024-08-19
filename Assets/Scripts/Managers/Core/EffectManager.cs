using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IEffectComponent
{
    public void PlayEffect(Vector3 position);
    public void PlayEffect(Vector3 position, Vector3 normal, Transform parent = null, Define.EEffectType effectType = Define.EEffectType.Common);

    public void SetActive(bool isActive);
}


public class EffectManager : ManagerBase
{
    public override void Init()
    {
        base.Init();

    }
    public override void Clear() { }


    /// <summary>
    /// ParticleSystem 플레이 만약 지속 데미지라면 parent 넣어주기
    /// </summary>
    /// <param name="name"></param>
    /// <param name="pos"></param>
    /// <param name="normal"></param>
    /// <param name="parent"></param>
    /// <param name="effectType"></param>
    public ParticleSystem PlayEffect(string name, Vector3 pos, Vector3 normal, Transform parent = null)
    {
        var effect = LoadEffect(name,pos,normal,parent);
        effect.Play();
        return effect;
    }

    public ParticleSystem LoadEffect(string name, Vector3 pos, Vector3 normal, Transform parent)
    {
        string path = $"Prefebs/Effects/{name}";
        GameObject prefab = Resources.Load<GameObject>(path);
        GameObject gameObj;
        if (parent == null)
            gameObj = GameObject.Instantiate(prefab, pos, Quaternion.LookRotation(normal));
        else
            gameObj = GameObject.Instantiate(prefab, pos, Quaternion.LookRotation(normal), parent);

        gameObj.name = prefab.name;
        return gameObj.GetComponent<ParticleSystem>();
    }


}
