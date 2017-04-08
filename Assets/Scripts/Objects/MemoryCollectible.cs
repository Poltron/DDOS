using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCollectible : MonoBehaviour
{
    public float Amount;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<EntityBrain>().ApplyHeal(Amount);
            Destroy(gameObject);
        }
    }
}
