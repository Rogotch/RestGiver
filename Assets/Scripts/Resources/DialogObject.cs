using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogObject", menuName = "DialogueSystem/DialogObject")]
public class DialogObject : ScriptableObject
{
    [SerializeField] private string tag;
    [SerializeField] private DialogTagChecker[] reqDialogTags;
    [SerializeField] private StoryTagChecker[] reqStoryTags;
    [SerializeField] private QuestTagChecker[] reqQuestTags;
    [SerializeField] [TextArea] private string[] dialog;
    [SerializeField] private Response[] responses;
    [SerializeField] private bool repeatable = false;
    [SerializeField] private bool writeSelfTag = true;

    public static event Action dialogueStarted;
    public static event Action dialogueEnded;

    public string Tag => tag;
    public string[] Dialog => dialog;
    public Response[] Responses => responses;
    public bool Repeatable => repeatable;
    public int GetSize() => dialog.Length;

    public bool HasResponses() => responses != null && responses.Length > 0;

    public void DialogStarted()
    {
        dialogueStarted?.Invoke();
    }
    public void DialogEnded()
    {
        if (writeSelfTag)
        {
            FlagsManager.AddDialogTag(tag, true);
        }
        dialogueEnded?.Invoke();
    }
    private bool CheckTagsArr(TagChecker[] tags)
    {
        bool tagFlag = true;
        foreach (TagChecker tag in tags)
        {
            tagFlag = tagFlag && tag.CheckTag();
        }
        return tagFlag;
    }
    public bool CanBeStarted()
    {
        if (!IsRepeatable())
        {
            if (FlagsManager.GetDialogTagFlag(tag))
            {
                return false;
            }
        }
        return FullCheckTags();
    }
    private bool CheckDialogTags() { return CheckTagsArr(reqDialogTags); }
    private bool CheckStoryTags()  { return CheckTagsArr(reqStoryTags);  }
    private bool CheckQuestTags()  { return CheckTagsArr(reqQuestTags);  }
    private bool FullCheckTags()
    {
        return CheckDialogTags() && CheckStoryTags() && CheckQuestTags();
    }
    private bool IsRepeatable()
    {
        return repeatable;
    }
}
