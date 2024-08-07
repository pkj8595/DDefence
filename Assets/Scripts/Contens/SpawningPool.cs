using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpawningPool : MonoBehaviour
{
    [SerializeField] int _monsterCount = 0;
    [SerializeField] int _reserveCount = 0;
    [SerializeField] int _keepMonsterCount = 0;
    [SerializeField] Vector3 _spawnPos;
    [SerializeField] float _spawnRadius = 15.0f;
    [SerializeField] float _spawnTime = 5.0f;

    void Start()
    {
        Managers.Game.OnSpawnEvent -= AddMonsterCount;
        Managers.Game.OnSpawnEvent += AddMonsterCount;
    }

    void Update()
    {
        
    }

    public void AddMonsterCount(int value) { _monsterCount += value; }
    public void SetKeepMonsterCount(int count) { _keepMonsterCount = count; }

    IEnumerator ReserveSpawn()
    {
        _reserveCount++;
        yield return new WaitForSeconds(Random.Range(0, _spawnTime));
        GameObject obj = Managers.Game.Spawn(Define.WorldObject.NonePlayable, "Knight");
        NavMeshAgent nma = obj.GetOrAddComponent<NavMeshAgent>();

        Vector3 randPos;

        while (true)
        {
            //좌표를 기준으로 반지름안에 점을 반환
            Vector3 randDir = Random.insideUnitCircle * Random.Range(0, _spawnRadius);
            randDir.y = 0;
            randPos = _spawnPos + randDir;

            NavMeshPath path = new NavMeshPath();
            if (nma.CalculatePath(randPos, path))
                break;
        }
        obj.transform.position = randPos;
        _reserveCount--;
    }


}
