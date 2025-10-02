using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        placedGameObjects.Add(newObject);
        //newObject.GetComponent<SphereCollider>().enabled = true;
        newObject.GetComponentInChildren<BoxCollider>().enabled = true;
        newObject.GetComponent<IsPlaced>().isPlaced = true;
        newObject.GetComponent<SimpleTower>().enabled = true;
        TowerHealthBar healthBar = newObject.GetComponentInChildren<TowerHealthBar>(true);
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(true);
        }

        ITargetable targetable = newObject.GetComponent<ITargetable>();
        if (targetable != null)
        {
            EnemyTargetManager.Instance?.RegisterTarget(targetable);
        }
        


        return placedGameObjects.Count - 1;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null) 
        {
            return;
        }
        else
        {
            Destroy(placedGameObjects[gameObjectIndex]);
            placedGameObjects[gameObjectIndex] = null;
        }
    }
}
