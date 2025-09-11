using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO dataBase;
    GridData floorData;
    GridData structureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD,
                          Grid grid,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO dataBase,
                          GridData floorData,
                          GridData structureData,
                          ObjectPlacer objectPlacer,
                          LayerMask layerMask)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.dataBase = dataBase;
        this.floorData = floorData;
        this.structureData = structureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = dataBase.objectsData.FindIndex(obj => obj.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(dataBase.objectsData[selectedObjectIndex].Prefab, dataBase.objectsData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception("Object with ID " + iD + " not found in database.");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            return;
        }

        int index = objectPlacer.PlaceObject(dataBase.objectsData[selectedObjectIndex].Prefab, grid.CellToWorld(gridPosition));


        GridData selectedData = dataBase.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            structureData;
        selectedData.AddObject(gridPosition, dataBase.objectsData[selectedObjectIndex].Size, dataBase.objectsData[selectedObjectIndex].ID, index);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = dataBase.objectsData[selectedObjectIndex].ID == 0 ?
            floorData :
            structureData;

        bool gridValid = selectedData.CanPlaceObject(gridPosition, dataBase.objectsData[selectedObjectIndex].Size);

        bool enemyCollision = CheckEnemyCollision(grid.CellToWorld(gridPosition), dataBase.objectsData[selectedObjectIndex].Size);

        return gridValid && !enemyCollision;
    }

    private bool CheckEnemyCollision(Vector3 worldPosition, Vector2Int size)
    {
        // Ajusta la altura según el tamaño real de los enemigos
        float yCenter = 0.5f; // O la altura media de los enemigos
        float yExtent = 0.5f; // O la mitad de la altura de los enemigos

        Vector3 center = worldPosition + new Vector3(size.x / 2f, yCenter, size.y / 2f);
        Vector3 halfExtents = new Vector3(size.x / 2f, yExtent, size.y / 2f);

        // Dibuja el OverlapBox en la escena para depuración
#if UNITY_EDITOR
        Debug.DrawLine(center - halfExtents, center + halfExtents, Color.red, 0.5f);
#endif

        Collider[] colliders = Physics.OverlapBox(center, halfExtents, Quaternion.identity, LayerMask.GetMask("Enemy"));

        return colliders.Length > 0;
    }



    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
