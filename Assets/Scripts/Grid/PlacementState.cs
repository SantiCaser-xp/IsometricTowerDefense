using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class PlacementState : IBuildingState
{
    int ID;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO dataBase;
    ObjectPlacer objectPlacer;
    int selectedObjectIndex = -1;

    public PlacementState(int iD,
                          PreviewSystem previewSystem,
                          ObjectsDatabaseSO dataBase,
                          ObjectPlacer objectPlacer,
                          LayerMask layerMask)
    {
        ID = iD;
        this.previewSystem = previewSystem;
        this.dataBase = dataBase;
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

    public void OnAction(Vector3 position)
    {
        var detector = previewSystem.GetPreviewCollisionDetector();
        bool enemyCollision = detector != null && detector.IsColliding;
        if (enemyCollision) return;

        int index = objectPlacer.PlaceObject(dataBase.objectsData[selectedObjectIndex].Prefab, position);
        previewSystem.UpdatePosition(position, false);
    }

    public void UpdateState(Vector3 position)
    {
        var detector = previewSystem.GetPreviewCollisionDetector();
        bool placementValidity = detector != null && !detector.IsColliding;
        previewSystem.UpdatePosition(position, placementValidity);
    }
}
