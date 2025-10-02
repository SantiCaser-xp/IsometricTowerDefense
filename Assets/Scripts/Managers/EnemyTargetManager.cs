using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTargetManager : MonoBehaviour
{
    private static EnemyTargetManager instance;
    public static EnemyTargetManager Instance => instance;

    [Header("Target lists")]
    private List<ITargetable> allTargets = new List<ITargetable>();
    private List<ITargetable> towers = new List<ITargetable>();
    private ITargetable playerBase;

    [Header("Optimization")]
    [SerializeField] private float spatialGridCellSize = 10f;
    private Dictionary<Vector2Int, List<ITargetable>> spatialGrid;
    [SerializeField] private bool showDebugInfo = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitializeSpatialGrid();
    }

    private void InitializeSpatialGrid()
    {
        spatialGrid = new Dictionary<Vector2Int, List<ITargetable>>();
    }

    public void RegisterTarget(ITargetable target)
    {
        if (target == null || allTargets.Contains(target)) return;

        allTargets.Add(target);

        if (target.TargetType == TargetType.Tower) towers.Add(target);
        else if (target.TargetType == TargetType.PlayerBase) playerBase = target;

        UpdateSpatialGrid(target);
    }

    public void UnregisterTarget(ITargetable target)
    {
        if (target == null) return;
        allTargets.Remove(target);
        towers.Remove(target);

        if (target == playerBase) playerBase = null;
        RemoveFromSpatialGrid(target);

        // Notify all enemies about the loss of the target
        EnemyManager.Instance?.NotifyTargetLost(target);
    }

    public ITargetable GetOptimalTarget(Vector3 fromPosition, TargetingStrategy strategy)
    {
        switch (strategy)
        {
            case TargetingStrategy.Nearest:

                return GetNearestTarget(fromPosition);
            case TargetingStrategy.Weakest:
                return GetWeakestTarget(fromPosition);
            case TargetingStrategy.Strongest:
                return GetStrongestTarget(fromPosition);
            case TargetingStrategy.PlayerBase:
                return playerBase;
            default:
                return GetNearestTarget(fromPosition);
        }
    }
    private ITargetable GetNearestTarget(Vector3 fromPosition)
    {
        ITargetable nearest = null;
        float minDistance = float.MaxValue;

        Vector2Int gridPos = GetGridPosition(fromPosition);
        List<ITargetable> nearbyTargets = GetNearbyTargets(gridPos, 2); // Check neighboring cells
        foreach (var target in nearbyTargets)
        {
            if (target == null) continue;

            //  float distance = Vector3.Distance(fromPosition, target.GetPos());
            float distance = (fromPosition - target.GetPos()).sqrMagnitude;
            if (distance < minDistance * minDistance) { minDistance = distance; nearest = target; }
        }
        // If not found in the nearest cells, search globally
        if (nearest == null)
        {
            foreach (var target in allTargets)
            {
                if (target == null) continue;
                float distance = (fromPosition - target.GetPos()).sqrMagnitude;
                if (distance < minDistance * minDistance) { minDistance = distance; nearest = target; }
            }
        }
        return nearest;
    }
    private ITargetable GetWeakestTarget(Vector3 fromPosition)
    {
        return allTargets
             .Where(t => t != null)
             //.OrderBy(t => t.CurrentHealth)
             //.ThenBy(t => Vector3.Distance(fromPosition, t.Position))
             .FirstOrDefault();
    }


    private ITargetable GetStrongestTarget(Vector3 fromPosition)
    {
        return allTargets
           .Where(t => t != null)
           //.OrderByDescending(t => t.CurrentHealth)
           //.ThenBy(t => Vector3.Distance(fromPosition, t.Position))
           .FirstOrDefault();
    }
    #region Spatial Grid Optimization
    private Vector2Int GetGridPosition(Vector3 worldPosition)
    {
        return new Vector2Int(
            Mathf.FloorToInt(worldPosition.x / spatialGridCellSize),
            Mathf.FloorToInt(worldPosition.z / spatialGridCellSize));
    }
    private void UpdateSpatialGrid(ITargetable target)
    {
        Vector2Int gridPos = GetGridPosition(target.GetPos());
        if (!spatialGrid.ContainsKey(gridPos))
            spatialGrid[gridPos] = new List<ITargetable>();
        spatialGrid[gridPos].Add(target);
    }
    private void RemoveFromSpatialGrid(ITargetable target)
    {
        Vector2Int gridPos = GetGridPosition(target.GetPos());
        if (spatialGrid.ContainsKey(gridPos))
            spatialGrid[gridPos].Remove(target);
    }

    private List<ITargetable> GetNearbyTargets(Vector2Int centerGrid, int radius)
    {
        List<ITargetable> nearby = new List<ITargetable>();
        for (int x = -radius; x <= radius; x++)
        {
            for (int z = -radius; z <= radius; z++)
            {
                Vector2Int checkPos = new Vector2Int(centerGrid.x + x, centerGrid.y + z);
                if (spatialGrid.ContainsKey(checkPos))
                {
                    nearby.AddRange(spatialGrid[checkPos]);
                }
            }
        }
        return nearby;
    }
    #endregion
    private void OnGUI()
    {
        if (showDebugInfo)
        {
            GUI.Label(new Rect(10, 30, 200, 20), $"Registred targets: {allTargets.Count}");
        }
    }



}
