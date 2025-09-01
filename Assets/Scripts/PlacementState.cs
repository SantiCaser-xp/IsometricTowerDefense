using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : MonoBehaviour
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO dataBase;
    GridData floorData;
    GridData structureData;
    ObjectPlacer objectPlacer;
}
