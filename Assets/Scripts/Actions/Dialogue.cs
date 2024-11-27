using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class DialogueAction : InteractionComponent, IInteractionAction
{
    public static event Action dialogueStarted;
    public static event Action dialogueEnded;

    [SerializeField] Dialog dialog;

    public new void RunInteraction(IInteractable source_of_interaction)
    {
        dialogueStarted?.Invoke();
        DialogManager.Instance.StartDialogue(this);
    }

    public Dialog GetDialogue()
    {
        return dialog;
    }

    public void DialogueEnded()
    {
        dialogueEnded?.Invoke();
    }
}
