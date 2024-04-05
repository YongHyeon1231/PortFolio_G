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

    Define.StageType _stageType;
    public Define.StageType StageType
    {
        get { return _stageType; }
        set
        {
            _stageType = value;

            if (_spawningPool != null)
            {
                switch (value)
                {
                    case Define.StageType.Normal:
                        _spawningPool.Stopped = false;
                        break;
                    case Define.StageType.Boss:
                        _spawningPool.Stopped = true;
                        break;
                }
            }
        }
    }

    void StartLoaded()
    {
        Managers.Data.Init();

        Managers.UI.ShowSceneUI<UI_GameScene>();

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

        Managers.Game.OnKillCountChanged -= HandleOnKillCountChanged;
        Managers.Game.OnKillCountChanged += HandleOnKillCountChanged;
        Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
        Managers.Game.OnGemCountChanged += HandleOnGemCountChanged;
    }

    #region Action Handler
    int _collectedGemCount = 0;
    int _remainingTotalGemCount = 10;
    public void HandleOnGemCountChanged(int gemCount)
    {
        _collectedGemCount++;

        // 스킬 레벨업
        if (_collectedGemCount == _remainingTotalGemCount)
        {
            Managers.UI.ShowPopup<UI_SkillSelectPopup>();
            _collectedGemCount = 0;
            _remainingTotalGemCount *= 2;
        }

        Managers.UI.GetSceneUI<UI_GameScene>().SetGemGountRatio((float)_collectedGemCount / _remainingTotalGemCount);
    }

    public void HandleOnKillCountChanged(int killCount)
    {
        Managers.UI.GetSceneUI<UI_GameScene>().SetSkillCount(killCount);

        // 보스가 언제 스폰될지 고민
        if(killCount == 5)
        {
            // BOSS
            StageType = Define.StageType.Boss;

            Managers.Object.DespawnAllMonsters();

            Vector2 spawnPos = Utils.GenerateMonsterSpawnPosition(Managers.Game.Player.transform.position, 5, 10);

            Managers.Object.Spawn<MonsterController>(spawnPos, Define.BOSS_ID);
        }
    }

    private void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.OnGemCountChanged -= HandleOnGemCountChanged;
    }
    #endregion
}
