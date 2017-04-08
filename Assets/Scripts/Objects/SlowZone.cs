using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class SlowZone : MonoBehaviour
{
    public float SlowMultiplier;
    public float ImmunityTime;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlatformerCharacter2D>().StartSlowAndImmuneFor(SlowMultiplier, ImmunityTime);
        }
    }
}
