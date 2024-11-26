using UnityEngine;

public interface IInteractable
{
    public void StartOfInteractionWith(IInteractable interacted_object);
    public void EndOfInteractionWith(IInteractable interacted_object);
    public void Interact(IInteractable interacted_object);
}
