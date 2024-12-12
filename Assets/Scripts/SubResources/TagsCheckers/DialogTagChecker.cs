using System;
using UnityEngine;

[Serializable]
public class DialogTagChecker : TagChecker
{
    protected override bool GetTagFlag()
    {
        return FlagsManager.GetDialogTagFlag(checkedTag);
    }
}
