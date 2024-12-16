using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject    dialogBox;
    [SerializeField] TMP_Text      dialogText;
    [SerializeField] RectTransform scrollContent;
    [SerializeField] RectTransform responsesBox;

    [SerializeField] int lettersPerSecond;

    public static DialogManager Instance { get; private set; }

    private ResponseHandler  responseHandler;
    private TypewriterEffect typewriterEffect;
    private DialogObject     activeDialog;
    private Coroutine        currentCoroutine;
    private int              lineNum;
    private bool             showInProgress;

    private void Awake()
    {
        Instance = this;
    }

    private void OnEnable()
    {
        TypewriterEffect.DialogueShowing  += DialogueShowing;
        TypewriterEffect.DialogueShowed   += DialogueShowed;
        TypewriterEffect.DialogueCanceled += DialogueCanceled;
    }

    private void OnDisable()
    {
        TypewriterEffect.DialogueShowing  -= DialogueShowing;
        TypewriterEffect.DialogueShowed   -= DialogueShowed;
        TypewriterEffect.DialogueCanceled -= DialogueCanceled;
    }
    private void Start()
    {
        responseHandler  = GetComponent<ResponseHandler>();
        typewriterEffect = GetComponent<TypewriterEffect>();
    }

    private void DialogueShowing()
    {
        dialogText.text = string.Empty;
        showInProgress = true;
        dialogBox.SetActive(true);
    }
    private void DialogueShowed()
    {
        showInProgress = false;
    }
    private void DialogueCanceled()
    {
        showInProgress = false;
        dialogText.text = activeDialog.Dialog[lineNum];
    }

    public void StartDialogue(DialogObject dialogue)
    {
        if (activeDialog == null)
        {
            SetActiveDialogObject(dialogue);
        }
        else IncreaseLineNum();
    }
    public void SetActiveDialogObject(DialogObject dialogObject)
    {
        activeDialog = dialogObject;
        activeDialog.DialogStarted();
        ShowDialog();
    }
    public void ShowDialog()
    {
        string currentLine = activeDialog.Dialog[lineNum];

        if (showInProgress)
        {
            typewriterEffect.Stop();
        }
        else
        {
            typewriterEffect.Run(currentLine, dialogText, lettersPerSecond);
        }
    }

    public void CloseDialog()
    {
        //Debug.Log("line ", lineNum);
        activeDialog.DialogEnded();
        activeDialog = null;
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
                lineNum = 0;
                if (activeDialog.HasResponses())
                {
                    responseHandler.ShowResponses(activeDialog.Responses);
                    //scrollContent.sizeDelta = new Vector2(scrollContent.sizeDelta.x, dialogText.GetComponent<RectTransform>().sizeDelta.y + responsesBox.sizeDelta.y);
                }
                else
                {
                    CloseDialog();
                }
                return;
            }
        }
        ShowDialog();
    }
}
