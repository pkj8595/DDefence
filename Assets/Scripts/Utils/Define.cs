using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    public const int Pawn_Rune_Limit_Count = 3;
    public const int Rune_Count = 3;
    public const int Trait_Count = 10;
    public const int Affect_Count = 3;
    /// <summary>
    /// worldObject 상태
    /// </summary>
    public enum WorldObject
    {
        Unknown,
        Pawn,
        Building,
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
        Slash,
        Shot,
        Hit,
        Heal,
        Cool
    }
  

    public enum TrapState
    {
        Idle,
        Attack,
    }

    public enum EParentObj
    {
        Pawn,
        Projectile,
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

    public enum ETeam
    {
        Playable,
        Enumy,
    }

    public enum EDamageType
    {
        Melee,
        Ranged,
        Magic,
    }

    public enum EAffectType
    {
        Damage,
        Heal,
        Debuff,
        Buff,
    }

    public enum ESkillType
    {
        one,
        Area,
    }

    public enum ESkillDistanceType
    {
        LessMin,
        Excuteable,
        MoreMax,
    }

    public static class Path
    {
        public const string Sprite = "Sprites/";

        public const string Prefab_Trap = "Prefabs/Tiles/Traps/";
        public const string Sprite_Trap = "Sprites/Tiles/Traps/";

        public const string Prefab_Bullet = "Projectiles/";
        public const string Sprite_Bullet = "Sprites/Projectiles/";

        public const string UI = "UI/UIBase/";

    }

}
