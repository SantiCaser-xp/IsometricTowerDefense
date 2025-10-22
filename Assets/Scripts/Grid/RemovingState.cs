using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemovingState : IBuildingState
{
    private PreviewSystem previewSystem;
    private ObjectPlacer objectPlacer;

    public RemovingState(PreviewSystem previewSystem, ObjectPlacer objectPlacer)
    {
        this.previewSystem = previewSystem;
        this.objectPlacer = objectPlacer;
        previewSystem.StartShowingRemovePreview();
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3 position)
    {
        // Detecta el objeto bajo el cursor usando raycast
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            var obj = hit.collider.GetComponent<IsPlaced>();
            if (obj != null)
            {
                GameObject go = obj.gameObject;
                objectPlacer.RemoveObjectAt(objectPlacer.GetIndexOfPlacedObject(go));
            }
        }
        previewSystem.UpdatePosition(position, false);
    }

    public void UpdateState(Vector3 position)
    {
        previewSystem.UpdatePosition(position, true);
    }
}
