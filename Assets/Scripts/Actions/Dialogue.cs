using System;
using UnityEngine;

public class DialogueAction : InteractionComponent, IInteractionAction
{
    public static event Action dialogueStarted;
    public static event Action dialogueEnded;

    [SerializeReference] public BaseDialogSelector selector;
    public new void RunInteraction(IInteractable source_of_interaction)
    {
        dialogueStarted?.Invoke();
        DialogManager.Instance.StartDialogue(this);
    }

    public DialogObject GetDialogue()
    {
        return selector.GetSelectedDialog();
    }

    public void DialogueEnded()
    {
        dialogueEnded?.Invoke();
    }
}
