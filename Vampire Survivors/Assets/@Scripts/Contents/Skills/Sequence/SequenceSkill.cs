using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class SequenceSkill : SkillBase
{
    public SequenceSkill() : base(Define.SkillType.Sequence)
    {
    }

    public abstract void DoSkill(Action callBack);
}
