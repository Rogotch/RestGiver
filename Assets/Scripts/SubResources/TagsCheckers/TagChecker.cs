using System;
using UnityEngine;

[Serializable]
public class TagChecker
{
    [SerializeField] protected string checkedTag;
    [SerializeField] protected bool  checkedFlag;

    public bool CheckTag()
    {
        return GetTagFlag() == checkedFlag;
    }
    protected virtual bool GetTagFlag()
    {
        return false;
    }
}
