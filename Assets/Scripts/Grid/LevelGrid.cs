using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    
    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private int width;
    [SerializeField] private int height;
    [SerializeField] private float cellSize;
    
    private GridSystem<GridObject> gridSystem;
    [SerializeField] private Transform gridDebugObjectPrefab;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one LevelGrid!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;

        gridSystem = new GridSystem<GridObject>(width, height, cellSize, (GridSystem<GridObject> gridSystem, GridPosition gridPosition) => new GridObject(gridSystem, gridPosition));
        // gridSystem.CreateDebugObjects(gridDebugObjectPrefab);
    }

    private void Start()
    {
        Pathfinding.Instance.Setup(width, height, cellSize);
    }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);
        AddUnitAtGridPosition(toGridPosition, unit);
        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) => gridSystem.GetGridPosition(worldPosition);

    public Vector3 GetWorldPosition(GridPosition gridPosition) => gridSystem.GetWorldPosition(gridPosition);

    public bool IsValidGridPosition(GridPosition gridPosition) => gridSystem.IsValidGridPosition(gridPosition);

    public int GetWidth() => gridSystem.GetWidth();

    public int GetHeight() => gridSystem.GetHeight();

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).HasAnyUnit();
    }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnit();
    }

    public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetInteractable();
    }
    
    public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable)
    {
        gridSystem.GetGridObject(gridPosition).SetInteractable(interactable);
    }

}
