using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            List<GridPosition> validGridPositionList = unit.GetComponent<MoveAction>().GetValidActionGridPositionList();
            GridSystemVisual.Instance.HideAllGridPositions();
            GridSystemVisual.Instance.ShowGridPositionList(validGridPositionList, GridSystemVisual.GridVisualType.White);
        }
    }
}
