using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : CreatureController
{
    Vector2 _moveDir = Vector2.zero;
    float _speed = 5.0f;

    float EnvCollectDist { get; set; } = 1f;

    public Vector2 MoveDir
    {
        get { return _moveDir; }
        set { _moveDir = value.normalized; }
    }

    void Start()
    {
        Managers.Game.onMoveDirChanged += HandleOnMoveDirChanged;
    }

    void OnDestroy()
    {
        if (Managers.Game != null)
            Managers.Game.onMoveDirChanged -= HandleOnMoveDirChanged;
    }

    void HandleOnMoveDirChanged(Vector2 dir)
    {
        _moveDir = dir;
    }

    void Update()
    {
        MovePlayer();
        CollectEnv();
    }

    void MovePlayer()
    {
        // _moveDir = Managers.Game.MoveDir;

        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;
    }

    void CollectEnv()
    {
        float sqrCollectDist = EnvCollectDist * EnvCollectDist;

        List<GemController> gems = Managers.Object.Gems.ToList();
        foreach (GemController gem in gems)
        {
            Vector3 dir = gem.transform.position - transform.position;
            if (dir.sqrMagnitude <= sqrCollectDist)
            {
                Managers.Game.Gem += 1;
                Managers.Object.Despawn(gem);
            }
        }

        var findGems = GameObject.Find("@Grid").GetComponent<GridController>().GatherObjects(transform.position, EnvCollectDist + 0.5f);

        Debug.Log($"SearchGems({findGems.Count}) TotalGems({gems.Count}");
    }

    private void OnCollisionEnter2D(Collision2D collsion)
    {
        MonsterController target = collsion.gameObject.GetComponent<MonsterController>();
        if (target == null)
            return;
        if (target.isActiveAndEnabled == false)
            return;
    }

    public override void OnDamaged(BaseController attacker, int damage)
    {
        base.OnDamaged(attacker, damage);

        Debug.Log($"OnDamaged ! {Hp}");

        // TEMP 풀링을 위해 간단하게 넣어놨습니다.
        CreatureController cc = attacker as CreatureController;
        cc?.OnDamaged(this, 10000);
    }
}
