using System;
using UnityEngine;

[Serializable]
public class QuestTagChecker : TagChecker
{
    protected override bool GetTagFlag()
    {
        return FlagsManager.GetQuestTagFlag(checkedTag);
    }
}
