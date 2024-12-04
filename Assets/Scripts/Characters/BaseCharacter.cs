using UnityEngine;
using UnityEngine.Windows;
using System.Collections;
using System.Collections.Generic;
using System;

public class BaseCharacter : MonoBehaviour
{

    public float movement_speed;
    public Grid grid_component;
    public GameObject sprite_object;
    public LayerMask solid_objects_layer;
    public LayerMask interactable_layer;

    protected Vector2Int map_pos;
    protected Vector3 target_position;
    protected bool is_moving;

    protected Animator character_animator;
    protected Vector2 character_direction;

    private void Awake()
    {
        character_animator = sprite_object.GetComponent<Animator>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected void Start()
    {
        UpdateMapPosition();
    }

    // Update is called once per frame
    protected void Update()
    {

    }


    protected void UpdateCharacterDirection()
    {

    }

    public Vector2Int GetMapPosition()
    {
        return map_pos;
    }

    public void Interact(IInteractable interacted_object)
    {
    }

    protected bool IsWalkable(Vector3 checked_position)
    {
        return !(Physics2D.OverlapCircle(checked_position, 0.2f, solid_objects_layer | interactable_layer) != null);
    }

    protected IEnumerator Move()
    {
        while ((target_position - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, target_position, movement_speed * Time.deltaTime);

            yield return null;
        }

        UpdateMapPosition();
        UpdateTargetPosition();
    }

    protected Vector3Int GetCurrentCellPositionVector3Int()
    {
        Vector3Int global_grid_position = grid_component.WorldToCell(transform.position);
        return global_grid_position;
    }
    protected Vector2Int GetCurrentCellPositionVector2Int()
    {
        Vector3Int global_grid_position = GetCurrentCellPositionVector3Int();
        Vector2Int cell_position = new Vector2Int(global_grid_position.x, global_grid_position.y);
        return cell_position;
    }

    protected void UpdateMapPosition()
    {
        map_pos = GetCurrentCellPositionVector2Int();
    }

    protected void UpdateTargetPosition()
    {
    }
}
