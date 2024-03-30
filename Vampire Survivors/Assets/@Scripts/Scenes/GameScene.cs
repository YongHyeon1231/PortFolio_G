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
        GameObject prefab = Managers.Resource.Load<GameObject>("Slime_01.prefab");

        GameObject go = new GameObject() { name = "@Monsters" };

        Camera.main.GetComponent<CameraController>().Target = prefab;
    }

    void Update()
    {
        
    }
}
