using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class AccelerationZone : MonoBehaviour
{
    public float Multiplier;

    private bool accelerated;


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlatformerCharacter2D>().m_MaxSpeed *= Multiplier;
            other.GetComponent<Animator>().SetBool("Boosted", true);
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<Animator>().SetBool("Boosted", true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlatformerCharacter2D>().m_MaxSpeed /= Multiplier;
            other.GetComponent<Animator>().SetBool("Boosted", false);
        }
    }

    public void Lol() { }
}
