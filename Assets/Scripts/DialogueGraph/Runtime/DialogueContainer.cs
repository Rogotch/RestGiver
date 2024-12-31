using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueContainer : ScriptableObject
{
    public List<BaseNodeLinkData> NodeLinks = new List<BaseNodeLinkData>();
    public List<BaseNodeData>     NodeData  = new List<BaseNodeData>();

}
