using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectManager
{
    public PlayerController Player { get; private set; }
    public HashSet<MonsterController> Monsters { get; } = new HashSet<MonsterController>();
    public HashSet<ProjectileController> Projectiles { get; } = new HashSet<ProjectileController>();
    public HashSet<GemController> Gems { get; } = new HashSet<GemController>();

    public T Spawn<T>(Vector3 position, int templateID = 0, string prefabNale = "") where T : BaseController
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
            string name = "";

            switch (templateID)
            {
                case Define.GOBLIN_ID:
                    name = "Goblin_01";
                    break;
                case Define.SNAKE_ID:
                    name = "Snake_01";
                    break;
                case Define.BOSS_ID:
                    name = "Boss_01";
                    break;
            };

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

            // TEMP
            GameObject.Find("@Grid").GetComponent<GridController>().Add(go);

            return gc as T;
        }
        else if (type == typeof(ProjectileController))
        {
            GameObject go = Managers.Resource.Instantiate(Define.FIRE_PROJECTILE_PREFAB, pooling: true);
            go.transform.position = position;

            ProjectileController pc = go.GetOrAddComponent<ProjectileController>();
            Projectiles.Add(pc);
            pc.Init();

            return pc as T;
        }
        else if (typeof(T).IsSubclassOf(typeof(SkillBase)))
        {
            if (Managers.Data.SkillDic.TryGetValue(templateID, out Data.SkillData skillData) == false)
            {
                Debug.LogError($"ObjectManager Spawn Skill Failed {templateID}");
                return null;
            }

            GameObject go = Managers.Resource.Instantiate(skillData.prefab, pooling: true);
            go.transform.position = position;

            T t = go.GetOrAddComponent<T>();
            t.Init();

            return t;
        }

        return null;
    }

    public void Despawn<T> (T obj) where T : BaseController
    {
        if (obj.IsValid() == false)
        {
            return;
        }

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

            //TEMP
            GameObject.Find("@Grid").GetComponent<GridController>().Remove(obj.gameObject);
        }
        else if (type == typeof(ProjectileController))
        {
            Projectiles.Remove(obj as ProjectileController);
            Managers.Resource.Destroy(obj.gameObject);
        }
    }

    public void DespawnAllMonsters()
    {
        var monsters = Monsters.ToList();

        foreach (var monster in monsters)
        {
            Despawn<MonsterController>(monster);
        }
    }
}
