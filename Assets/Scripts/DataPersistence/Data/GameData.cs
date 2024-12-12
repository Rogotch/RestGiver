using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public string currentEpisodeTag;

    public Dictionary<string, bool> dialogsTags;
    public Dictionary<string, bool> questsTags;
    public Dictionary<string, bool> storyTags;
    public Vector2Int mapPosition;

    public GameData()
    {
        this.currentEpisodeTag =  null;
        this.dialogsTags       =  new Dictionary<string, bool>();
        this.questsTags        =  new Dictionary<string, bool>();
        this.storyTags         =  new Dictionary<string, bool>();
        this.mapPosition       =  new Vector2Int(0, 0);
    }
}
