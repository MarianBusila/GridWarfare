using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAction : BaseAction
{
    private int maxInteractDistance = 1;

    private void Update()
    {
        if (!isActive)
            return;
    }

    public override string GetActionName()
    {
        return "Interact";
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        Door door = LevelGrid.Instance.GetDoorAtGridPosition(gridPosition);
        door.Interact(OnInteractComplete);
        ActionStart(onActionComplete);
    }
    
    private void OnInteractComplete()
    {
        ActionComplete();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition unitGridPosition = unit.GetGridPosition();
        
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        for (int x = -maxInteractDistance; x <= maxInteractDistance; x++)
        {
            for (int z = -maxInteractDistance; z <= maxInteractDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // is inside the grid
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;
                
                Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                if (door == null)
                {
                    // no door on this grid position
                    continue;
                }

                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction()
        {
            gridPosition = gridPosition,
            actionValue = 0
        };
    }
}