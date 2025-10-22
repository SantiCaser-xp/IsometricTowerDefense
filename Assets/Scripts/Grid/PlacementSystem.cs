using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField] private GameObject _helpText;
    [SerializeField] InputManager inputManager;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private GameObject gridVisualization;
    [SerializeField] private PreviewSystem preview;
    [SerializeField] private ObjectPlacer objectPlacer;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float placementDistance = 2f;
    [SerializeField] private float raycastHeight = 2f;
    [SerializeField] private CharacterDeposit _deposit;
    private int _currentSelectedID;
    IBuildingState buildingState;

    private void Start()
    {
        StopPlacement();
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        _helpText.SetActive(true);
        _currentSelectedID = ID;
        buildingState = new PlacementState(ID, preview, database, objectPlacer, layerMask);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    public void StartRemoving()
    {
        StopPlacement();
        buildingState = new RemovingState(preview, objectPlacer);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnExit += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI()) return;

        Vector3 placementPosition = GetPlacementPositionInFrontOfPlayer();

        int price = database.objectsData[_currentSelectedID].Price;

        if (_deposit.CurrentGold >= price)
        {
            Debug.Log("Comprado!");
            _deposit.SubstructDeposit(price);
            buildingState.OnAction(placementPosition);
        }
        else
        {
            StopPlacement();
            Debug.Log("Yo don´t have money!");
        }
    }

    private void StopPlacement()
    {
        if (buildingState == null) return;

        _helpText.SetActive(false);
        buildingState.EndState();
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnExit -= StopPlacement;
        buildingState = null;
    }

    private void Update()
    {
        if (buildingState == null)
        {
            return;
        }
        Vector3 placementPosition = GetPlacementPositionInFrontOfPlayer();
        buildingState.UpdateState(placementPosition);
    }

    private Vector3 GetPlacementPositionInFrontOfPlayer()
    {
        Vector3 origin = playerTransform.position + playerTransform.forward * placementDistance + Vector3.up * raycastHeight;
        Vector3 direction = Vector3.down;
        RaycastHit hit;

        if (Physics.Raycast(origin, direction, out hit, raycastHeight * 2f, layerMask))
        {
            return hit.point;
        }
        Vector3 fallback = playerTransform.position + playerTransform.forward * placementDistance;
        fallback.y = 0;
        return fallback;
    }
}
