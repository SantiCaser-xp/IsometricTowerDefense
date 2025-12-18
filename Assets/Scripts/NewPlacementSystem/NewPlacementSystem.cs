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
    private readonly List<IObserver> _observers = new List<IObserver>();

    private void Update()
    {
        if (!isPlacing)
        {
            return;
        }

        UpdateGhostPosition();
        isGhostColliding = currentGhost.GetComponent<GhostCollDetector>().isColliding;

#if UNITY_EDITOR || UNITY_STANDALONE
        // Editor / PC
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverUI_Mouse())
            {
                Debug.Log("[Placement] Click sobre UI (mouse), no coloco torre");
                return;
            }

            TryPlaceObject();
        }
#else
        // ANDROID / móvil
        if (Input.touchCount > 0)
        {
            // IMPORTANT: procesar TODOS los toques que empiezan este frame
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);

                if (touch.phase != TouchPhase.Began)
                {
                    continue;
                }

                bool overUI = IsPointerOverUI_TouchPosition(touch.position);
                Debug.Log($"[Placement] Touch Began index={i} pos={touch.position}, overUI={overUI}");

                if (overUI)
                {
                    Debug.Log("[Placement] Toque sobre UI, no coloco torre");
                    // No hacemos TryPlaceObject con ESTE toque, pero seguimos
                    // procesando otros toques Began del mismo frame si los hubiera.
                    continue;
                }

                // Toque válido en el mundo → intentamos colocar
                TryPlaceObject();
                // Si solo quieres colocar UNA torre por frame, puedes hacer break aquí:
                // break;
            }
        }
#endif
    }

    private void TryPlaceObject()
    {
        Debug.Log($"[Placement] TryPlaceObject gold={_deposit.CurrentGold} price={currentPrice} colliding={isGhostColliding}");

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

    public void StartPlacement(int id)
    {
        StopPlacement();
        isPlacing = true;
        helpText.SetActive(true);
        ObjectData data = database.objectsData.Find(obj => obj.ID == id);
        currentID = id;
        currentPrice = data.Price;
        if (data != null && data.GhostPrefab != null)
        {
            Vector3 placementPosition = GetPlacementPositionInFrontOfPlayer();
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
            Vector3 placementPosition = GetPlacementPositionInFrontOfPlayer();
            Notify();
            Instantiate(data.Prefab, placementPosition, Quaternion.identity);
            Debug.Log("[Placement] Torre colocada");
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

    // ----------- DETECCIÓN UI -----------

    private bool IsPointerOverUI_Mouse()
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("[Placement] EventSystem.current es null (mouse)");
            return false;
        }

        bool result = EventSystem.current.IsPointerOverGameObject();
        Debug.Log($"[Placement] IsPointerOverUI(mouse) => {result}");
        return result;
    }

    // Raycast manual para touch (independiente de fingerId)
    private bool IsPointerOverUI_TouchPosition(Vector2 screenPosition)
    {
        if (EventSystem.current == null)
        {
            Debug.LogWarning("[Placement] EventSystem.current es null (touch)");
            return false;
        }

        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        bool hitUI = results.Count > 0;
        Debug.Log($"[Placement] IsPointerOverUI_TouchPosition pos={screenPosition} hitUI={hitUI}");

        return hitUI;
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