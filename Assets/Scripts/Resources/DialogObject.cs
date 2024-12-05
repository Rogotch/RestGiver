using UnityEngine;

[CreateAssetMenu(fileName = "DialogObject", menuName = "DialogueSystem/DialogObject")]
public class DialogObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialog;
    [SerializeField] private Response[] responses;
    public string[] Dialog => dialog;
    public Response[] Responses => responses;

    public int GetSize() => dialog.Length;

    public bool HasResponses() => responses != null && responses.Length > 0;
}
