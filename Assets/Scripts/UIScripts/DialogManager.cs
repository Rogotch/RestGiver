using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond;

    public static DialogManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void ShowDialog(Dialog showingDialogue)
    {
        dialogBox.SetActive(true);
        StartCoroutine(TypeDialog(showingDialogue.Lines[0]));
    }

    public IEnumerator TypeDialog(string line)
    {
        dialogText.text = "";
        foreach (char letter in line.ToCharArray()) 
        { 
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }

    }
}
