using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu(nameof(Rigidbody2D))]
public abstract class BaseBullet : MonoBehaviour
{
    public float _speed;
    private Rigidbody2D _rigidbody2D;
    public Sprite _bulletSprite;
    private Define.WorldObject _worldObject;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public virtual void Init(Define.WorldObject worldType, float speed, string spriteName = null) 
    {
        _speed = speed;

        if (!string.IsNullOrEmpty(spriteName))
            _bulletSprite = Managers.Resource.Load<Sprite>($"{Define.Path.Sprite}/{spriteName}");
    }

    public virtual void Fire() 
    {
        _rigidbody2D.velocity = transform.forward * _speed;

    }

    public virtual void Destroy() { }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(_worldObject.ToString()))
            return;

        BaseController other = collision.GetComponent<BaseController>();
        if (other == null) return;

        //todo : 데미지 입히기, 오브젝트 없애는 로직 만들기
    }
}
