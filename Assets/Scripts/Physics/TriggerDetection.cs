using UnityEngine;
using System.Collections.Generic;
using System;

/*
 * This class is here to tracks all objects that are within triggers
 */
 [System.Serializable]
public sealed class TriggerDetection : MonoBehaviour
{
    [SerializeField] private List<GameObject>   _colliders = new List<GameObject>();

    private List<GameObject>                    _gos = new List<GameObject>();

    public void DisplayFOVList()
    {
        foreach (GameObject go in _gos)
        {
            Debug.Log("GO: " + go.name);
        }
    }

    public void Start()
    {
        foreach (GameObject goCollider in _colliders)
        {
            ColliderCallback colliderCallback = goCollider.GetComponent<ColliderCallback>();

            colliderCallback.triggerEnter += TriggerEnterCallback;
            colliderCallback.triggerExit += TriggerExitCallback;
        }
    }

    public bool ContainGameObject(string tag)
    {
        foreach (GameObject go in _gos)
        {
            if (tag == go.tag)
            {
                return true;
            }
        }

        return false;
    }

    public void GetGameObjects(Func<GameObject,bool> predicate, ref List<GameObject> list)
    {
        foreach(GameObject go in _gos)
        {
            if(predicate(go))
            {
                list.Add(go);
            }
        }
    }

    public void GetGameObjects(out List<GameObject> gos)
    {
        gos = _gos;
    }

    /*
     * TO BE IMPROVED, STORE COLLIDERS INSTEAD OF GAMEOBJECTS
     */

    private void TriggerEnterCallback(Collider2D collider)
    {
        if (IsPlayer(collider))
        {

            GameObject go = collider.gameObject;

            if (!ContainsGO(go))
            {
                //  Debug.Log("ADDING TO FOV: " + go.name);

                _gos.Add(go);
            }
            else
            {
                //  Debug.LogError("LOGIC ERROR: Why an object try to be added to your list insofar as it is  within it, objectName: " + go.name);
            }
        }
    }

    private void TriggerExitCallback(Collider2D collider)
    {
        if (IsPlayer(collider))
        {

            GameObject go = collider.gameObject;

            if (ContainsGO(go))
            {
             //   Debug.Log("REMOVING FROM TRIGGER: " + go.name);

                _gos.Remove(go);
            }
            else
            {
                //  Debug.LogError("LOGIC ERROR: Why an object try to be removed from your list insofar as it is not within it: objectName: " + go.name);
            }
        }
    }

    private bool ContainsGO(GameObject go)
    {
        return _gos.Contains(go);
    }

    private bool IsPlayer(Collider2D co)
    {
        return co.gameObject.tag == "Player";
    }
}
