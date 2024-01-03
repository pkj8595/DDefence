using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Define
{
    /// <summary>
    /// player의 소환수 및 NonePlayable 몬스터 간의 관계
    /// </summary>
    public enum ERelationShip
    {
        Friendly,
        Neutrality,
        Enemy
    }

    public enum WorldObject
    {
        Unknown,
        Player,
        Summon,
        NonePlayable,
        trap,
    }

    public enum Layer
    {
        Ground = 6,
        Wall = 7,
        Building = 8,
        Unit = 9,
    }

    public enum State
    {
        Idle,
        Moving,
        Attack,
        Skill,
        Take_Damage,
        Die,
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

    public static class Path
    {
        public const string Sprite = "Assets/Resources/Sprites/";

        public const string Prefab_Trap = "Assets/Resources/Prefabs/Tiles/Traps/";
        public const string Sprite_Trap = "Assets/Resources/Sprites/Tiles/Traps/";

        public const string Prefab_Bullet = "Assets/Resources/Prefabs/Bullets/";
        public const string Sprite_Bullet = "Assets/Resources/Sprites/Bullets/";
    }

}
