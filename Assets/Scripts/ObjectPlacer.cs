using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new List<GameObject>();

    public int PlaceObject(GameObject prefab, Vector3 position)
    {
        GameObject newObject = Instantiate(prefab);
        newObject.transform.position = position;
        SimpleTower simpleTower = newObject.GetComponent<SimpleTower>();
        BulletFactory bulletFactory = FindObjectOfType<BulletFactory>(); // O referencia directa si la tienes
        simpleTower.bulletFactory = bulletFactory;
        placedGameObjects.Add(newObject);
        newObject.GetComponent<SphereCollider>().enabled = true;
        newObject.GetComponentInChildren<BoxCollider>().enabled = true;
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
