using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using System;

public class TypewriterEffect : MonoBehaviour
{
    //public static event Action dialogueStarte;
    public static event Action DialogueShowing;
    public static event Action DialogueShowed;
    public static event Action DialogueCanceled;

    private Coroutine writeCoroutine;
    public void Run(string textToType, TMP_Text textLabel, int lettersPerSecond)
    {
        writeCoroutine = StartCoroutine(TypeText(textToType, textLabel, lettersPerSecond));
    }
    public void Stop()
    {
        if (writeCoroutine != null)
        {
            DialogueCanceled?.Invoke();
            StopCoroutine(writeCoroutine);
        }
    }

    private IEnumerator TypeText(string textToType, TMP_Text textLabel, int lettersPerSecond)
    {
        DialogueShowing?.Invoke();
        foreach (char letter in textToType.ToCharArray())
        {
            textLabel.text += letter;
            //textLabel.sizeDelta = textLabel.autoSizeTextContainer
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
        textLabel.text = textToType;
        DialogueShowed?.Invoke();
    }
}
