using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private int gameObjectIndex = -1;
    Grid grid;
    NewPreviewSystem previewSystem;
    GridData floorData;
    GridData structureData;
    ObjectPlacer objectPlacer;

    public RemovingState(Grid grid, NewPreviewSystem previewSystem, GridData floorData, GridData structureData, ObjectPlacer objectPlacer)
    {
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.floorData = floorData;
        this.structureData = structureData;
        this.objectPlacer = objectPlacer;

        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition)
    {
        GridData selectedData = null;
        if (structureData.CanPlaceObject(gridPosition, Vector2Int.one) == false)
        {
            selectedData = structureData;
        }
        else if (floorData.CanPlaceObject(gridPosition, Vector2Int.one) == false)
        {
            selectedData = floorData;
        }

        if (selectedData == null)
        {
            //sound
            return;
        }
        else
        {
            gameObjectIndex = selectedData.GetRepresentationIndex(gridPosition);
            if (gameObjectIndex == -1)
            {
                return;
            }
            else
            {
                selectedData.RemoveObjectAt(gridPosition);
                objectPlacer.RemoveObjectAt(gameObjectIndex);
            }
            Vector3 cellPosition = grid.CellToWorld(gridPosition);
            previewSystem.UpdatePosition(cellPosition, CheckIfSelectionIsValid(gridPosition));

        }
    }

    private bool CheckIfSelectionIsValid(Vector3Int gridPosition)
    {
        return !(structureData.CanPlaceObject(gridPosition, Vector2Int.one) == false &&
            floorData.CanPlaceObject(gridPosition, Vector2Int.one) == false);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool validity = CheckIfSelectionIsValid(gridPosition);
        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), validity);
    }
}
