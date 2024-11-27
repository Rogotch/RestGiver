using System;
using UnityEngine;
using UnityEngine.Events;


public enum GameState { FreeRoam, Dialog, Battle}
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerController playerController;
    public static GameState  state;
    public static event Action<GameState> stateChanged;

    private void OnEnable()
    {
        DialogueAction.dialogueStarted += DialogueStarted;
        DialogueAction.dialogueEnded   += DialogueEnded;
    }

    private void OnDisable()
    {
        DialogueAction.dialogueStarted -= DialogueStarted;
        DialogueAction.dialogueEnded   -= DialogueEnded;
    }

    private void DialogueStarted()
    {
        ChangedState(GameState.Dialog);
    }
    private void BattleStarted()
    {
        ChangedState(GameState.Battle);
    }
    private void DialogueEnded()
    {
        ChangedState(GameState.FreeRoam);
    }
    private void BattleEnded()
    {
        ChangedState(GameState.FreeRoam);
    }
    private void Example()
    {
        Debug.Log("Test input action!");
    }

    private void ChangedState(GameState new_state)
    {
        state = new_state;
        stateChanged?.Invoke(state);
    }
}
