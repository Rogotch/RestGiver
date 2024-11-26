using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerController : BaseCharacter, IInteractable
{
    private Vector2 input;

    protected new void Start()
    {
        base.Start();
    }

    private void FixedUpdate()
    {
    }

    public void TryMove(InputAction.CallbackContext context)
    {
        if (GameController.state != GameState.FreeRoam)
        {
            return;
        }
        input = context.ReadValue<Vector2>();
        if (input.x != 0) input.y = 0;

        if (input != Vector2.zero)
        {
            character_direction = input;
            character_animator.SetFloat("direction_x", input.x);
            character_animator.SetFloat("direction_y", input.y);

            if (!is_moving)
            {
                UpdateTargetPosition();
                StartCoroutine(Move());
            }
        }
    }

    #region Moving
    private new bool IsWalkable(Vector3 checked_position)
    {
        return !(Physics2D.OverlapCircle(checked_position, 0.2f, solid_objects_layer | interactable_layer) != null);
    }

    new IEnumerator Move()
    {
        while ((target_position - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_position, movement_speed * Time.deltaTime);

            yield return null;
        }

        UpdateMapPosition();
        UpdateTargetPosition();
    }

    private new void UpdateTargetPosition()
    {
        bool old_value = is_moving;
        if (input == Vector2.zero || !IsWalkable(target_position))
        {
            is_moving = false;
            StopCoroutine(Move());
        }
        else
        {
            is_moving = true;
            Vector3Int grid_position = GetCurrentCellPositionVector3Int();
            Vector3Int grid_target_position = new Vector3Int(grid_position.x + (int)input.x, grid_position.y + (int)input.y, grid_position.z);
            Vector3 predicted_target_position = grid_component.GetCellCenterWorld(grid_target_position);

            if (!IsWalkable(predicted_target_position))
            {
                is_moving = false;
                StopCoroutine(Move());
            }
            else
            {
                target_position = predicted_target_position;
                StartCoroutine(Move());
            }
        }
        if (is_moving != old_value)
        {
            character_animator.SetBool("is_moving", is_moving);
        }
    } 
    #endregion

    #region Interactions
    public void TryInteract(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }


        Vector3 facing_direction = new Vector3(character_direction.x, character_direction.y);
        Vector3 interact_pos = transform.position + facing_direction;
        Debug.DrawLine(transform.position, interact_pos, Color.red, 2f);

        var collider = Physics2D.OverlapCircle(interact_pos, 0.2f, interactable_layer);
        if (collider != null && collider.GetComponent<IInteractable>() != null)
        {
            Debug.Log("Try interact! " + context.phase);
            collider.GetComponent<IInteractable>().Interact(GetComponent<IInteractable>());
        }
    }
    public void Interact()
    {
        Debug.Log("You was interacted");
    }

    public void StartOfInteractionWith(IInteractable interacted_object)
    {
    }

    public void EndOfInteractionWith(IInteractable interacted_object)
    {
    } 
    #endregion
}
