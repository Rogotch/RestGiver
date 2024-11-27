using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] Text dialogText;

    [SerializeField] int lettersPerSecond;

    public static DialogManager Instance { get; private set; }

    private DialogueAction activeDialogueAction;
    private Dialog         activeDialog;
    private Coroutine      currentCoroutine;
    private int            lineNum;
    private bool           showInProgress;

    private void Awake()
    {
        Instance = this;
    }

    public void StartDialogue(DialogueAction action)
    {
        if (activeDialogueAction == null)
        {
            activeDialogueAction = action;
            activeDialog = action.GetDialogue();
            ShowDialog();
        }
        else IncreaseLineNum();
    }
    public void ShowDialog()
    {
        string currentLine = activeDialog.Lines[lineNum];

        if (showInProgress)
        {
            StopCoroutine(currentCoroutine);
            FullShowLine(currentLine);
        }
        else
        {
            dialogBox.SetActive(true);
            currentCoroutine = StartCoroutine(TypeDialog(currentLine));
        }
    }

    public IEnumerator TypeDialog(string line)
    {
        showInProgress = true;
        dialogText.text = "";
        foreach (char letter in line.ToCharArray()) 
        { 
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        FullShowLine(line);
    }

    private void FullShowLine(string line)
    {
        showInProgress = false;
        dialogText.text = line;
    }

    public void CloseDialog()
    {
        //Debug.Log("line ", lineNum);
        activeDialog = null;
        lineNum = 0;
        activeDialogueAction.DialogueEnded();
        activeDialogueAction = null;
        dialogBox.SetActive(false);
    }

    private void IncreaseLineNum()
    {
        if (!showInProgress)
        {
            if (activeDialog.GetSize() > lineNum + 1)
            {
                lineNum++;
            }
            else 
            { 
                CloseDialog();
                return;
            }
        }
        ShowDialog();
    }
}
