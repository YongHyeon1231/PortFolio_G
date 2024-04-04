using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : SkillController
{
    CreatureController _owner;
    Vector3 _moveDir;
    float _speed = 10.0f;
    float _lifeTime = 10.0f;

    public override bool Init()
    {
        base.Init();

        StartDestroy(_lifeTime);

        return true;
    }

    public void SetInfo(int templateID, CreatureController owner, Vector3 moveDir)
    {
        if (Managers.Data.SkillDic.TryGetValue(templateID, out Data.SkillData data) == false)
        {
            Debug.LogError("ProjectileController SetInfo Failed");
            return;
        }

        _owner = owner;
        _moveDir = moveDir;
        SkillData = data;
        // TODO : Data Parsing
    }

    public override void UpdateController()
    {
        base.UpdateController();

        transform.position += _moveDir * _speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        MonsterController mc = collision.gameObject.GetComponent<MonsterController>();
        // �츮��Ŀ����� Ǯ���� üũ�ؾ��մϴ�.
        if (mc.IsValid() == false)
            return;
        // ���� Ǯ�� �Ǿ��ٸ� return
        if (this.IsValid() == false)
            return;

        mc.OnDamaged(_owner, SkillData.damage);

        StopDestroy();

        Managers.Object.Despawn(this);
    }
}
