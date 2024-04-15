using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SkillCardItem : UI_Base
{
    // 어떤 스킬?
    // 몇 레벨?
    // 데이터시트?
    int _templateID;
    Data.SkillData _skillData;

    public void SetInfo(int templateID)
    {
        _templateID = templateID;

        Managers.Data.SkillDic.TryGetValue(templateID, out _skillData);
        // 나중에 스킬 데이터를 다 파주면 검증하는 코드가 들어가야 합니다.
    }

    public void OnClickItem()
    {
        //  스킬 레벨 업그레이드
        Debug.Log("OnClickItem");
        Managers.UI.ClosePopup();
    }
}
