using UnityEngine;

[CreateAssetMenu(fileName = "DialogObject", menuName = "DialogueSystem/DialogObject")]
public class DialogObject : ScriptableObject
{
    [SerializeField] private string tag;
    [SerializeField] [TextArea] private string[] dialog;
    [SerializeField] private Response[] responses;
    [SerializeField] private bool repeatable = false;

    public string Tag => tag;
    public string[] Dialog => dialog;
    public Response[] Responses => responses;
    public bool Repeatable => repeatable;
    public int GetSize() => dialog.Length;

    public bool HasResponses() => responses != null && responses.Length > 0;

    public bool IsRepeatable()
    {
        return repeatable;
    }
    private void DialogEnded()
    {

    }
}
