using System;
using UnityEngine;

public class DialogueAction : InteractionComponent, IInteractionAction
{
    [SerializeReference] public BaseDialogSelector selector;
    public new void RunInteraction(IInteractable source_of_interaction)
    {
        DialogManager.Instance.StartDialogue(GetDialogue());
    }

    public DialogObject GetDialogue()
    {
        return selector.GetSelectedDialog();
    }
}
