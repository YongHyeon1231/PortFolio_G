using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();
    public HashSet<GemController> Gems { get; } = new HashSet<GemController>();

    public T Spawn<T>(Vector3 position, int templateID = 0) where T : BaseController
    {
        System.Type type = typeof(T);

        if (type == typeof(PlayerController))
        {
            // TODO : Data
            // 나중에 데이터 시트를 뒤져가지고 이 해당하는 템플릿 아이디에
            // 해당하는 프리팹이 몇번인지를 뒤져서 체크를 해줍니다.
            GameObject go = Managers.Resource.Instantiate("Slime_01.prefab", pooling: true);
            go.name = "Player";
            go.transform.position = position;

            PlayerController pc = Utils.GetOrAddComponent<PlayerController>(go);
            Player = pc;
            // Pool에서 다시 꺼냈을때 처리를 하기 위해서 넣어줘야 함
            // Init()을 다시 실행해 줘야 하기 때문이다.
            pc.Init();

            return pc as T;
        }
        else if (type == typeof(MonsterController))
        {
            string name = (templateID == 0 ? "Goblin_01" : "Snake_01");
            GameObject go = Managers.Resource.Instantiate(name + ".prefab", pooling: true);
            go.transform.position = position;

            MonsterController mc = Utils.GetOrAddComponent<MonsterController>(go);
            Monsters.Add(mc);
            // Pool에서 다시 꺼냈을때 처리를 하기 위해서 넣어줘야 함
            // Init()을 다시 실행해 줘야 하기 때문이다.
            mc.Init();

            return mc as T;
        }
        else if (type == typeof(GemController))
        {
            GameObject go = Managers.Resource.Instantiate(Define.EXP_GEM_PREFAB, pooling: true);
            go.transform.position = position;

            GemController gc = Utils.GetOrAddComponent<GemController>(go);
            Gems.Add(gc);
            // Pool에서 다시 꺼냈을때 처리를 하기 위해서 넣어줘야 함
            // Init()을 다시 실행해 줘야 하기 때문이다.
            gc.Init();

            string key = UnityEngine.Random.Range(0, 2) == 0 ? "EXPGem_01.sprite" : "EXPGem_02.sprite";
            Sprite sprite = Managers.Resource.Load<Sprite>(key);
            go.GetComponent<SpriteRenderer>().sprite = sprite;

            return gc as T;
        }

        return null;
    }

    public void Despawn<T> (T obj) where T : BaseController
    {
        System.Type type = typeof(T);
        
        if (type == typeof(PlayerController))
        {
            // ?
        }
        else if (type == typeof(MonsterController))
        {
            Monsters.Remove(obj as MonsterController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(GemController))
        {
            Gems.Remove(obj as GemController);
            Managers.Resource.Destroy(obj.gameObject);
        }
        else if (type == typeof(ProjectileController))
        {
            Projectiles.Remove(obj as ProjectileController);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }
}
