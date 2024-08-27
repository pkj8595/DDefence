using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPhase
{
    public void EnterPhase();
    public void EndPhase();
}

public partial class GameManager
{
    public enum EGamePhase
    {
        Stroy,
        BattleReady,
        Battle
    }

    public Stack<Data.WaveData> WaveStack { get; set; } = new();
    public Stack<Data.StoryData> StoryStack { get; set; } = new();

    public Data.StoryData _currentStoryData;
    public Data.WaveData _currentWaveData;

    private EGamePhase GamePhase { get; set; } = EGamePhase.Stroy;
    public int _waveCount = 0;
    private readonly SpawningPool _pool = new SpawningPool();

    IPhase _phase;
    readonly StoryPhase _storyPhase = new();
    readonly BattleReadyPhase _battleReadyPhase = new();
    readonly BattlePhase _battlePhase = new();

    public void InitWave()
    {
        _pool.Init();
        GamePhase = EGamePhase.Stroy;
        _waveCount = 0;
        
    }

    public void NextPhase()
    {
        GamePhase++;

    }

    public void SetPhase(EGamePhase gamePhase)
    {
        _phase?.EndPhase();
        switch (gamePhase)
        {
            case EGamePhase.Stroy:
                _phase = _storyPhase;
                break;
            case EGamePhase.BattleReady:
                _phase = _battleReadyPhase;
                break;
            case EGamePhase.Battle:
                _phase = _battlePhase;
                break;
        }
        GamePhase = gamePhase;
        _phase.EnterPhase();
    }




    #region production
    public HashSet<IProductionable> _productionList = new HashSet<IProductionable>();

    public void RegisterProduction(IProductionable productionable)
    {
        _productionList.Add(productionable);
    }

    public void RemoveProduction(IProductionable productionable)
    {
        _productionList.Remove(productionable);
    }

    public void RunEndWave()
    {
        foreach(var production in _productionList)
        {
            production.EndWave();
        }
    }
    #endregion
}