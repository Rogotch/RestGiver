using UnityEngine;

public class Challenger : BaseCharacter, IInteractable
{
    public void StartOfInteractionWith(IInteractable interacted_object)
    {
    }
    public void EndOfInteractionWith(IInteractable interacted_object)
    {
    }

    public void Interact()
    {
        Debug.Log("You challenge this NPC");
    }

}
