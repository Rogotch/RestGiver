using UnityEngine;

[CreateAssetMenu(fileName = "DialogObject", menuName = "DialogueSystem/DialogObject")]
public class DialogObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialog;

    public string[] Dialog => dialog;

    public int GetSize() => dialog.Length;
}
