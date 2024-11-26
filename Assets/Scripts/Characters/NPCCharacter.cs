using UnityEngine;

public class NPCCharacter : BaseCharacter, IInteractable
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected new void Start()
    {

    }

    // Update is called once per frame
    protected new void Update()
    {

    }
    public new void Interact(IInteractable interacted_object)
    {
        interacted_object.StartOfInteractionWith(GetComponent<IInteractable>());

        IInteractionAction interaction_action = GetComponent<IInteractionAction>();
        //GetComponent<InteractionComponent>().RunInteraction(GetComponent<IInteractable>());
        interaction_action.RunInteraction(GetComponent<IInteractable>());

        interacted_object.EndOfInteractionWith(GetComponent<IInteractable>());
    }

    public void StartOfInteractionWith(IInteractable interacted_object)
    {
    }

    public void EndOfInteractionWith(IInteractable interacted_object)
    {
    }
}
