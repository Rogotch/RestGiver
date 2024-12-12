using System;
using UnityEngine;

[Serializable]
public class StoryTagChecker : TagChecker
{
    protected override bool GetTagFlag()
    {
        return FlagsManager.GetStoryTagFlag(checkedTag);
    }
}
