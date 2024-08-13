using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int PawnRuneLimitCount = 3;
    public const int runeCount = 3;
    public const int traitCount = 10;
    /// <summary>
    /// worldObject 상태
    /// </summary>
    public enum WorldObject
    {
        Unknown,
        PlayerPawn,
        Tower,
        EnumyPawn,
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
    
    public enum EBaseStat
    {
        Vitality,       //체력
        Strength,       //힘
        Agility,        //민첩
        Intelligence,   //지력
        Willpower,      //정신력
        Accuracy,       //정확성
        Count
    }

    public enum ETargetType
    {
        Self,
        Enemy,
        Ally,
    }
    public enum EDamageType
    {
        Melee,
        Range,
        Magic,
    }

    public enum EAffectType
    {
        Damage,
        Heal,
        Debuff,
        Buff,
    }

    public static class Path
    {
        public const string Sprite = "Assets/Resources/Sprites/";

        public const string Prefab_Trap = "Assets/Resources/Prefabs/Tiles/Traps/";
        public const string Sprite_Trap = "Assets/Resources/Sprites/Tiles/Traps/";

        public const string Prefab_Bullet = "Projectile/";
        public const string Sprite_Bullet = "Assets/Resources/Sprites/Projectile/";

        public const string UI = "Assets/Resources/UI/";

    }

}
