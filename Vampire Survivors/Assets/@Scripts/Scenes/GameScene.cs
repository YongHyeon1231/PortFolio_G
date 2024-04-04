using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : MonoBehaviour
{
    void Start()
    {
        Managers.Resource.LoadAllAsync<Object>("PreLoad", (key, count, totalCount) =>
        {
            Debug.Log($"{key} {count} / {totalCount}");

            if (count == totalCount)
            {
                StartLoaded();
            }
        });
    }

    SpawningPool _spawningPool;

    void StartLoaded()
    {
        Managers.Data.Init();

        _spawningPool = gameObject.AddComponent<SpawningPool>();

        var player = Managers.Object.Spawn<PlayerController>(Vector3.zero);

        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("@Map.prefab");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().Target = player.gameObject;

        // Data Test

        foreach (var playerData in Managers.Data.PlayerDic.Values)
        {
            Debug.Log($"Level : {playerData.level}, Hp : {playerData.maxHp}");
        }
    }

    void Update()
    {
        
    }
}
