using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    /// <summary>
    /// player의 소환수 및 NonePlayable 몬스터 간의 관계
    /// summon && friendly == playable
    /// summon && enemy == 적대 -> 제어는 가능하지만 플레이어를 때림(제어 x)
    /// NonePlayable && friendly == 친밀관계 -> 제어할순없지만 적을 때림
    /// NonePlayable && enemy == 적대 -> 제어할 수 없고 플레이어를 때림
    /// </summary>
    public enum ERelationShip
    {
        Friendly,
        //Neutrality,
        Enemy
    }

    /// <summary>
    /// worldObject 상태
    /// </summary>
    public enum WorldObject
    {
        Unknown,
        Playable,
        NonePlayable,
    }

    public enum Layer
    {
        Water       = 1 << 4,
        UI          = 1 << 5,
        Ground      = 1 << 6,
        Wall        = 1 << 7,
        Building    = 1 << 8,
        Pawn        = 1 << 9,
        PawnGroup   = 1 << 10,
    }

    public enum EPawnAniState
    {
        Idle,
        Ready,
        Running,
        Dead,
        Casting
    }

    public enum EPawnAniTriger
    {
        Attack,
        Skill,
        Hit,
        Heal,
        Cool
    }

    public enum DamageType
    {
        MeleeAttack,
        RangeAttack
    }

    public enum TrapState
    {
        Idle,
        Attack,
    }

    public enum Scene
    {
        Unknown,
        Login,
        Game,
    }

    public enum Sound
    {
        Bgm,
        Effect,
        Count,
    }

    public enum MouseEvent
    {
        Press,
        PointerDown,
        PointerUp,
        Click,
    }

    public enum Tags
    {
        Pawn
    }
    

    public static class Path
    {
        public const string Sprite = "Assets/Resources/Sprites/";

        public const string Prefab_Trap = "Assets/Resources/Prefabs/Tiles/Traps/";
        public const string Sprite_Trap = "Assets/Resources/Sprites/Tiles/Traps/";

        public const string Prefab_Bullet = "Assets/Resources/Prefabs/Projectile/";
        public const string Sprite_Bullet = "Assets/Resources/Sprites/Projectile/";

        public const string UI = "Assets/Resources/UI/";

    }

}
