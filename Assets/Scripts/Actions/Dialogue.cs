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

    [SerializeField] Dialog dialog;

    public new void RunInteraction(IInteractable source_of_interaction)
    {
        dialogueStarted?.Invoke();
        DialogManager.Instance.ShowDialog(dialog);
    }
}
