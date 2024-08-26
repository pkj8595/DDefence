using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] int _monsterCount = 0;

    [SerializeField] List<GameObject> _gateList = new();
    [SerializeField] readonly Queue<int> _enemyQueue = new Queue<int>();

    public void Init()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    public void AddMonsterCount(int value) { _monsterCount += value; }

    public void StartWaveEnemySpawn(Data.WaveData data)
    {
        //wave 데이터의 캐릭터 코드를 모두 큐에 삽입
        for (int i = 0; i < data.arr_characterNum.Length; i++)
        {
            for (int j = 0; j < data.arr_charAmount[i]; j++)
            {
                _enemyQueue.Enqueue(data.arr_characterNum[i]);
            }
        }

        if (0 < _gateList.Count)
            RunSpawnWave().Forget();
        else
            Debug.LogError("not find gate");
    }

    async UniTaskVoid RunSpawnWave()
    {
        await UniTask.NextFrame();

        while(_enemyQueue.Count > 0)
        {
            _monsterCount++;
            PawnBase obj = Managers.Game.SpawnPawn(_enemyQueue.Dequeue(), Define.ETeam.Enemy);
            int gateIndex = Utils.Round(Random.Range(0, _gateList.Count - 1));
            obj.transform.position = _gateList[gateIndex].transform.position;

            await UniTask.Delay(500);
        }

    }



}
