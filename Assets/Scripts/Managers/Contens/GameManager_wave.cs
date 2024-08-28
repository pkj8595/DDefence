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
        Battle,
        Count
    }

    public Stack<Data.WaveData> WaveStack { get; set; } = new();
    public PriorityQueue<Data.StoryData> StoryStack { get; set; } = new();

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

        //딕셔너리를 처음부터 순회해서 카테고리가 0인 데이터만 넣는다.
        var storyEnumer = Managers.Data.StoryDict.GetEnumerator();
        while (storyEnumer.MoveNext())
        {
            if (Utils.CalculateCategory(storyEnumer.Current.Value.tableNum) == 0) 
            {
                StoryStack.Enqueue(storyEnumer.Current.Value, storyEnumer.Current.Value.priority);
            }
            else
                break;
        }

        var waveEnumer = Managers.Data.WaveDict.GetEnumerator();
        while (waveEnumer.MoveNext())
        {
            if (Utils.CalculateCategory(waveEnumer.Current.Value.tableNum) == 0)
            {
                WaveStack.Push(waveEnumer.Current.Value);
            }
        }

        // _pool에서 적이 모두 디스폰 됐다면 EndPahse 실행
        _pool.SetEndWaveAction(_battlePhase.EndPhase);
        _pool.SetEndWaveAction(this.RunEndWave);
    }

    public void RunBattlePhase()
    {
        if (WaveStack.TryPop(out Data.WaveData data))
        {
            _pool.StartWaveEnemySpawn(data);
        }
        else
        {
            Debug.Log("WaveStack에 남은 웨이브가 없습니다.");
            NextPhase();
        }
    }

    public void StartGame()
    {
        SetPhase(GamePhase);
    }

    public void NextPhase()
    {
        GamePhase++;
        if(EGamePhase.Count <= GamePhase)
        {
            GamePhase = EGamePhase.Stroy;
            _waveCount++;
        }
        SetPhase(GamePhase);
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
        if (!_productionList.Contains(productionable))
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