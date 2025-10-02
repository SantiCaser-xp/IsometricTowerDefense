using UnityEngine;

public class PlacementSystem : MonoBehaviour
{

    [SerializeField] InputManager inputManager;

    [SerializeField] Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;

    [SerializeField] private GameObject gridVisualization;

    private GridData floorData, structureData;

    [SerializeField]
    private PreviewSystem preview;

    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField]
    private ObjectPlacer objectPlacer;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField] private Transform playerTransform;
    [SerializeField] private float placementDistance = 2f; // Distancia frente al jugador
    [SerializeField] private float raycastHeight = 2f;     // Altura desde la que lanzar el raycast
    [SerializeField] private CharacterDeposit _deposit;
    private int _currentSelectedID;
    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
        floorData = new();
        structureData = new();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        _currentSelectedID = ID;
        buildingState = new PlacementState(ID, grid, preview, database, floorData, structureData, objectPlacer, layerMask);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        gridVisualization.SetActive(true);
        buildingState = new RemovingState(grid, preview, floorData, structureData, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }


    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3Int gridPosition = GetGridPositionInFrontOfPlayer();

        int price = database.objectsData[_currentSelectedID].Price;
         
        if (_deposit.CurrentGold >= price)
        {
            Debug.Log("Comprado!");
            _deposit.SubstructDeposit(price); // substruct money
            buildingState.OnAction(gridPosition);  // place object
        }
        else
        {
            StopPlacement();
            Debug.Log("Yo don´t have money!");
        }
        //buildingState.OnAction(gridPosition);
    }

    //private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    //{
    //    GridData selectedData = database.objectsData[selectedObjectIndex].ID == 0 ?
    //        floorData :
    //        structureData;

    //    return selectedData.CanPlaceObject(gridPosition, database.objectsData[selectedObjectIndex].Size);
    //}

    private void StopPlacement()
    {
        if (buildingState == null) return;

        gridVisualization.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        lastDetectedPosition = Vector3Int.zero;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
        {
            return;
        }
        Vector3Int gridPosition = GetGridPositionInFrontOfPlayer();
        buildingState.UpdateState(gridPosition);
        lastDetectedPosition = gridPosition;
    }

    private Vector3Int GetGridPositionInFrontOfPlayer()
    {
        Vector3 origin = playerTransform.position + playerTransform.forward * placementDistance + Vector3.up * raycastHeight;
        Vector3 direction = Vector3.down;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, raycastHeight * 2f, layerMask))
        {
            return grid.WorldToCell(hit.point);
        }
        // Si no golpea nada, devuelve la celda frente al jugador a nivel del suelo
        Vector3 fallback = playerTransform.position + playerTransform.forward * placementDistance;
        fallback.y = 0; // Ajusta segun tu sistema de grilla
        return grid.WorldToCell(fallback);
    }

}
