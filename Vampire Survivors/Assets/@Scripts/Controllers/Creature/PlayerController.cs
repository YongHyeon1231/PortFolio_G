using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : CreatureController
{
    Vector2 _moveDir = Vector2.zero;
    float _speed = 5.0f;

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
    }

    void MovePlayer()
    {
        // _moveDir = Managers.Game.MoveDir;

        Vector3 dir = _moveDir * _speed * Time.deltaTime;
        transform.position += dir;
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
