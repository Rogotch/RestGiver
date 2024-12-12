using System;
using System.Collections.Generic;
using UnityEngine;

public class FlagsManager : MonoBehaviour
{
    private static FlagsManager Instance;

    private static Dictionary<string, bool> dialogsTags  = new Dictionary<string, bool>();
    private static Dictionary<string, bool> questsTags   = new Dictionary<string, bool>();
    private static Dictionary<string, bool> storyTags    = new Dictionary<string, bool>();

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("Found more than one instance of FlagsManager");
        }
        Instance = this;
    }

    public static void AddDialogTag(string tag, bool flag)
    {
        AddTagToDictionary(tag, flag, dialogsTags);
    }
    public static void AddQuestTag(string tag, bool flag)
    {
        AddTagToDictionary(tag, flag, questsTags);
    }
    public static void AddStoryTag(string tag, bool flag)
    {
        AddTagToDictionary(tag, flag, storyTags);
    }
    public static bool GetDialogTagFlag(string tag)
    {
        return GetTagFlagFromDictionary(tag, dialogsTags);
    }
    public static bool GetQuestTagFlag(string tag)
    {
        return GetTagFlagFromDictionary(tag, questsTags);
    }
    public static bool GetStoryTagFlag(string tag)
    {
        return GetTagFlagFromDictionary(tag, storyTags);
    }

    private static void AddTagToDictionary(string tag, bool flag, Dictionary<string, bool> tags_dict)
    {
        tags_dict[tag] = flag;
    }
    private static bool GetTagFlagFromDictionary(string tag, Dictionary<string, bool> tags_dict)
    {
        if (!tags_dict.ContainsKey(tag))
        {
            return false;
        }
        return tags_dict[tag];
    }
}
