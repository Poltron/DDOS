using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfGame : MonoBehaviour 
{
    private bool m_triggerred = false;
    private GameObject m_winner = null;

    public GameObject GetWinner()
    {
        return m_winner;
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(collider.gameObject.tag == "Player")
        {
            GameManager gm = GameManager.Instance;

            if(gm && !m_triggerred)
            {
                m_winner = collider.gameObject.transform.root.gameObject;
                gm.SetState(GameManager.ESTATE.END_PHASE);
                //Destroy(gameObject);
                m_triggerred = true;
            }
        }
    }
}
