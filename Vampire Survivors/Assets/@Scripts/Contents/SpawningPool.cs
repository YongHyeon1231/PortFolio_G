using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPool : MonoBehaviour
{
    // ������ �ֱ��?
    // ���� �ִ� ������?
    // ����?
    float _spawnInterval = 1.0f;
    int _maxMonsterCount = 100;
    Coroutine _coUpdateSpawningPool;

    void Start()
    {
        _coUpdateSpawningPool = StartCoroutine(CoUpdateSpawningPool());
    }

    IEnumerator CoUpdateSpawningPool()
    {
        while(true)
        {
            TrySpawn();
            yield return new WaitForSeconds(_spawnInterval);
        }
    }

    void TrySpawn()
    {
        int monsterCount = Managers.Object.Monsters.Count;
        if (monsterCount >= _maxMonsterCount)
            return;

        //TEMP : DataID ?
        MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0, 2));
        mc.transform.position = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
    }
}
