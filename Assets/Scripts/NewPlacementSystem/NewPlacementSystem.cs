using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine;
using System;

public class NewPlacementSystem : MonoBehaviour, IObservable
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float placementDistance = 2f;
    [SerializeField] private float raycastHeight = 2f;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private bool isPlacing = false;
    [SerializeField] private GameObject helpText;
    [SerializeField] private GameObject cantPlaceText;
    [SerializeField] private GameObject outOfCashText;
    [SerializeField] private CharacterDeposit _deposit;
    [SerializeField] private bool isGhostColliding;
    [SerializeField] private int currentPrice;
    [SerializeField] private bool _tutorialMode = false;
    private GameObject currentGhost;
    private int currentID;
    private List<IObserver> _observers = new List<IObserver>();

    void Update()
    {
        //Debug.DrawRay(playerTransform.position + playerTransform.forward * placementDistance + Vector3.up * raycastHeight, Vector3.down * raycastHeight * 2f, Color.red);
        if (isPlacing)
        {
            UpdateGhostPosition();
            isGhostColliding = currentGhost.GetComponent<GhostCollDetector>().isColliding;
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                if (_deposit.CurrentGold >= currentPrice && !isGhostColliding)
                {
                    PlaceObject();
                    _deposit.SubstructDeposit(currentPrice);
                }
                else
                {
                    if (isGhostColliding)
                    {
                        cantPlaceText.SetActive(true);
                        StartCoroutine(FadeOutText(cantPlaceText));
                    }

                    if (_deposit.CurrentGold < currentPrice)
                    {
                        outOfCashText.SetActive(true);
                        StartCoroutine(FadeOutText(outOfCashText));
                        StopPlacement();
                    }
                }
            }
        }
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        isPlacing = true;
        helpText.SetActive(true);
        ObjectData data = database.objectsData.Find(obj => obj.ID == ID);
        currentID = ID;
        currentPrice = data.Price;
        if (data != null && data.GhostPrefab != null)
        {
            Vector3 placementPosition = GetPlacementPositionFromMouse();
            currentGhost = Instantiate(data.GhostPrefab, placementPosition, Quaternion.identity);
        }

        if (_tutorialMode)
        {
            EventManager.Trigger(EventType.OpenBuildMenu, EventType.OpenBuildMenu);
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
            Vector3 placementPosition = GetPlacementPositionFromMouse();
            Notify();
            Instantiate(data.Prefab, placementPosition, Quaternion.identity);
        }

        if (_tutorialMode)
        {
            EventManager.Trigger(EventType.PlaceBuilding, EventType.PlaceBuilding);
        }
    }

    public void UpdateGhostPosition()
    {
        if (currentGhost != null)
        {
            Vector3 placementPosition = GetPlacementPositionFromMouse();
            currentGhost.transform.position = placementPosition;
        }
        else
        {
            Debug.Log("Theres not currentGhost");
        }
    }

    /// <summary>
    /// Calcula la posición de colocación usando un raycast desde la cámara a la posición del ratón.
    /// Respeta el layerMask definido para el terreno/suelo de construcción.
    /// </summary>
    private Vector3 GetPlacementPositionFromMouse()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null)
        {
            // Fallback si no hay cámara principal
            Vector3 fallbackNoCamera = playerTransform.position + playerTransform.forward * placementDistance;
            fallbackNoCamera.y = 0f;
            return fallbackNoCamera;
        }

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast contra el suelo/escenario usando el layerMask
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            return hit.point;
        }

        // Fallback si no hay nada bajo el ratón: proyectamos hacia delante del jugador
        Vector3 origin = playerTransform.position + Vector3.up * raycastHeight;
        Vector3 direction = playerTransform.forward;
        if (Physics.Raycast(origin, direction, out hit, placementDistance + raycastHeight, layerMask))
        {
            return hit.point;
        }

        Vector3 fallback = playerTransform.position + playerTransform.forward * placementDistance;
        fallback.y = 0f;
        return fallback;
    }

    public IEnumerator FadeOutText(GameObject Advise)
    {
        yield return new WaitForSeconds(2);
        Advise.SetActive(false);
    }

    #region Observable
    public void Subscribe(IObserver observer)
    {
        if (!_observers.Contains(observer))
        {
            _observers.Add(observer);
        }
    }

    public void Unsubscribe(IObserver observer)
    {
        if (_observers.Contains(observer))
        {
            _observers.Remove(observer);
        }
    }

    public void Notify()
    {
        foreach (var obs in _observers)
        {
            obs.UpdateData();
        }
    }
    #endregion
}