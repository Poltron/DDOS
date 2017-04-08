using UnityEngine;
using System.Collections.Generic;

/*
 * It allows other component to register callback on it to detect if something enter or exit the trigger
 */
public class ColliderCallback : MonoBehaviour {


    public delegate void ColliderDelegate(Collider2D other);

    public event ColliderDelegate triggerEnter = (Collider) => { };
    public event ColliderDelegate triggerExit = (Collider) => { };


    private void OnTriggerEnter2D(Collider2D other)
    {
       //     Debug.Log("ENTER CALLBACK: " + name + " with " + other.name);
        triggerEnter(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        //    Debug.Log("EXIT CALLBACK: " + name + " with " + other.name);
        triggerExit(other);
    }
}
