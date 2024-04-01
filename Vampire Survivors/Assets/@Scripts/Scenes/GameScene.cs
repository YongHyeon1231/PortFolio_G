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
                StartLoaded();
            }
        });
    }

    void StartLoaded()
    {
        var player = Managers.Object.Spawn<PlayerController>();

        for (int i = 0; i < 100; i++)
        {
            MonsterController mc = Managers.Object.Spawn<MonsterController>(Random.Range(0,2));
            mc.transform.position = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));
        }

        var joystick = Managers.Resource.Instantiate("UI_Joystick.prefab");
        joystick.name = "@UI_Joystick";

        var map = Managers.Resource.Instantiate("Map.prefab");
        map.name = "@Map";
        Camera.main.GetComponent<CameraController>().Target = player.gameObject;
    }

    void Update()
    {
        
    }
}
