using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance { get; private set; }
    
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14; // sqrt(2) * 10
    
    private int width;
    private int height;
    private float cellSize;
    
    private GridSystem<PathNode> gridSystem;
    [SerializeField] private Transform gridDebugObjectPrefab;
    [SerializeField] private LayerMask obstacleLayerMask;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is more than one Pathfinding!" + transform + " - " + Instance);
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void Setup(int width, int height, float cellSize)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        
        gridSystem = new GridSystem<PathNode>(width, height, cellSize,
            (GridSystem<PathNode> gridSystem, GridPosition gridPosition) => new PathNode(gridPosition));
        gridSystem.CreateDebugObjects(gridDebugObjectPrefab);

        // set walkable to false for positions with obstacles
        for (int x = 0; x < this.width; x++)
        {
            for (int z = 0 ; z < this.height ; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                
                // fire the reaycast from below the ground
                float raycastOffsetDistance = 5f;
                if (Physics.Raycast(worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up,
                        raycastOffsetDistance * 2f, obstacleLayerMask))
                {
                    GetNode(x, z).SetIsWalkable(false);
                }

            }
        }
    }

    // use A* algorithm to find the shortest path
    public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
    {
        List<PathNode> openList = new List<PathNode>();
        List<PathNode> closedList = new List<PathNode>();
        
        PathNode startNode = gridSystem.GetGridObject(startGridPosition);
        PathNode endNode = gridSystem.GetGridObject(endGridPosition);
        openList.Add(startNode);

        // initialize grid
        for (int x = 0; x < gridSystem.GetWidth(); x++)
        {
            for (int z = 0; z < gridSystem.GetHeight(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);
                PathNode pathNode = gridSystem.GetGridObject(gridPosition);
                
                pathNode.SetGCost(int.MaxValue);
                pathNode.SetHCost(0);
                pathNode.CalculateFCost();
                pathNode.ResetCameFromPathNode();
            }
        }
        
        // initialize start node
        startNode.SetGCost(0);
        startNode.SetHCost(CalculateDistance(startGridPosition, endGridPosition));
        startNode.CalculateFCost();

        while (openList.Count > 0)
        {
            PathNode currentNode = GetLowestFCostNode(openList);

            if (currentNode == endNode)
            {
                // reached final node
                return CalculatePath(endNode);
            }
            
            openList.Remove(currentNode);
            closedList.Add(currentNode);
            
            // get all neighbors
            foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
            {
                if(closedList.Contains(neighbourNode)) continue;
                
                if (!neighbourNode.IsWalkable())
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                
                int tentativeGCost = currentNode.GetGCost() + CalculateDistance(currentNode.GetGridPosition(), neighbourNode.GetGridPosition());
                if (tentativeGCost < neighbourNode.GetGCost())
                {
                    neighbourNode.SetCameFromPathNode(currentNode);
                    neighbourNode.SetGCost(tentativeGCost);
                    neighbourNode.SetHCost(CalculateDistance(neighbourNode.GetGridPosition(), endGridPosition));
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }
        
        // no path found
        return null;
    }
    
    private List<GridPosition> CalculatePath(PathNode endNode)
    {
        List<PathNode> pathNodeList = new List<PathNode>();
        pathNodeList.Add(endNode);
        PathNode currentNode = endNode;
        while (currentNode.GetCameFromPathNode() != null)
        {
            pathNodeList.Add(currentNode.GetCameFromPathNode());
            currentNode = currentNode.GetCameFromPathNode();
        }
        
        pathNodeList.Reverse();
        
        List<GridPosition> gridPositionList = new List<GridPosition>();
        foreach (PathNode pathNode in pathNodeList)
        {
            gridPositionList.Add(pathNode.GetGridPosition());
        }

        return gridPositionList;
    }
    
    public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
    {
        GridPosition gridPositionDistance = gridPositionA - gridPositionB;
        int xDistance = Mathf.Abs(gridPositionDistance.x);
        int zDistance = Mathf.Abs(gridPositionDistance.z);
        int remainingDistance = Mathf.Abs(xDistance - zDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, zDistance) + Pathfinding.MOVE_STRAIGHT_COST * remainingDistance;
    }
    
    private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
    {
        PathNode lowestFCostPathNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].GetFCost() < lowestFCostPathNode.GetFCost())
            {
                lowestFCostPathNode = pathNodeList[i];
            }
        }

        return lowestFCostPathNode;
    }

    private PathNode GetNode(int x, int z)
    {
        return gridSystem.GetGridObject(new GridPosition(x, z));
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode)
    {
        List<PathNode> neighbourList = new List<PathNode>();

        GridPosition gridPosition = currentNode.GetGridPosition();

        if (gridPosition.x - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 0)); // add left node
            if (gridPosition.z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z - 1)); // add left down node
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(gridPosition.x - 1, gridPosition.z + 1)); // add left up node
            }
        }

        if (gridPosition.x + 1 < gridSystem.GetWidth())
        {
            neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 0)); // add right node
            if (gridPosition.z - 1 >= 0)
            {
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z - 1)); // add right down node
            }

            if (gridPosition.z + 1 < gridSystem.GetHeight())
            {
                neighbourList.Add(GetNode(gridPosition.x + 1, gridPosition.z + 1)); // add right up node
            }
        }

        if (gridPosition.z - 1 >= 0)
        {
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z - 1)); // add up node
        }

        if (gridPosition.z + 1 < gridSystem.GetHeight())
        {
            neighbourList.Add(GetNode(gridPosition.x + 0, gridPosition.z + 1)); // add down node
        }

        return neighbourList;
    }
}
