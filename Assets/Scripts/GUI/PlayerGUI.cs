using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGUI : MonoBehaviour
{
    [SerializeField]
    private int playerID;

    private GameObject player;
    
    public bool IsValidPlayer()
    {
        return null != player;
    }

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        GameManager gm = GameManager.Instance;

        if (null != gm)
        {
            List<Platformer2DUserControl> controllers = null;
            gm.GetPlayers(out controllers);

            if (playerID < controllers.Count)
            {
                player = controllers[(int)playerID].gameObject;
            }
        }
    }

    public GameObject GetPlayer()
    {
        if (player == null)
            Initialize();

        if (player == null)
            Debug.LogError("PlayerGUI : Player still null after initialize, something went wrong.");

        return player;
    }

    public Stack GetPlayerStack()
    {
        if (player == null)
            Initialize();

        if (player == null)
            Debug.LogError("PlayerGUI : Player still null after initialize, something went wrong.");

        return player.GetComponent<Stack>();
    }
}
