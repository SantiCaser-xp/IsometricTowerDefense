using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NewPlacementSystem : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float placementDistance = 2f;
    [SerializeField] private float raycastHeight = 2f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private bool isPlacing = false;
    [SerializeField] private GameObject helpText;
    private GameObject currentGhost;
    private int currentID;


    void Update()
    {
        Debug.DrawRay(playerTransform.position + playerTransform.forward * placementDistance + Vector3.up * raycastHeight, Vector3.down * raycastHeight * 2f, Color.red);
        if (isPlacing)
        {
            UpdateGhostPosition();
            if (Input.GetMouseButtonDown(0))
            {
                PlaceObject();
                StopPlacement();
            }
        }
    }

    public void StartPlacement(int ID)
    {
        isPlacing = true;
        helpText.SetActive(true);
        ObjectData data = database.objectsData.Find(obj => obj.ID == ID);
        currentID = ID;
        if (data != null && data.GhostPrefab != null)
        {
            // Instancia el ghostPrefab en la escena
            Vector3 placementPosition = GetPlacementPositionInFrontOfPlayer();
            currentGhost = Instantiate(data.GhostPrefab, placementPosition, Quaternion.identity);
            // Puedes guardar la referencia si necesitas moverlo después
        }
    }

    public void StopPlacement()
    {
        isPlacing = false;
        helpText.SetActive(false);
        if (currentGhost != null)
        {
            Destroy(currentGhost);
        }
    }

    public void PlaceObject()
    {
        ObjectData data = database.objectsData.Find(obj => obj.ID == currentID);
        if (data != null && data.Prefab != null)
        {
            // Instancia el ghostPrefab en la escena
            Vector3 placementPosition = GetPlacementPositionInFrontOfPlayer();
            Instantiate(data.Prefab, placementPosition, Quaternion.identity);

            // Puedes guardar la referencia si necesitas moverlo después
        }
    }

    public void UpdateGhostPosition()
    {
        if (currentGhost != null)
        {
            Vector3 placementPosition = GetPlacementPositionInFrontOfPlayer();
            currentGhost.transform.position = placementPosition;
        }
        else
        {
            Debug.Log("Theres not currentGhost");
        }
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
