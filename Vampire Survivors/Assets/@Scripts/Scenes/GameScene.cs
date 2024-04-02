using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers.Resource.LoadAllAsync<GameObject>("Prefabs", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            if (count == totalCount)
            {
                Managers.Resource.LoadAllAsync<TextAsset>("Data", (key3, count3, totalCount3) =>
                {
                    Debug.Log($"{key3} {count3} / {totalCount3}");
                    if (count3 == totalCount3)
                    {
                        StartLoaded();
                    }
                });
            }
        });
    }

    SpawningPool _spawningPool;

    void StartLoaded()
    {
        _spawningPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers.Object.Spawn<PlayerController>();

        for (int i = 0; i < 100; i++)
        {
            MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0,2));
            mc.transform.position = new Vector2(Random.Range(-50, 50), Random.Range(-50, 50));
        }

        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("Map.prefab");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().Target = player.gameObject;

        // Data Test
        Managers.Data.Init();

        foreach (var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Level : {playerData.level}, Hp : {playerData.maxHp}");
        }
    }

    void Update()
    {
        
    }
}
