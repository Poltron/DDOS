using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameManager : GenericSingleton<GameManager>
{
    [SerializeField]
    private List<GameObject>    m_playerPrefabs = new List<GameObject>();

    [SerializeField]
    private GameObject          m_cameraPrefab = null;

    [SerializeField]
    private List<Transform>     m_spawnPoints = new List<Transform>();

    [SerializeField]
    private List<string>        m_spawnPointsNames = new List<string>();

    [SerializeField]
    private float               m_camZOffset = 0f;

    //[SerializeField]
    //private float               m_camXOffset = 0f;

    [SerializeField]
    private string              m_menuSceneName;

    [SerializeField]
    private string              m_playSceneName;

    [SerializeField]
    private string              m_selectionSceneName;

    [SerializeField]
    private LayerMask           m_parallaxPlayer0;

    [SerializeField]
    private LayerMask           m_parallaxPlayer1;

    public enum ESTATE
    {
        MENU_PHASE,
        SELECTION_PHASE,
        PLAY_PHASE,
        END_PHASE
    }

    public ESTATE m_state = ESTATE.MENU_PHASE;

    private bool[] m_validatedPlayers = new bool[2];


    private List<Platformer2DUserControl> m_players = new List<Platformer2DUserControl>();
    private List<GameObject> m_cameras = new List<GameObject>();

    //private uint        m_playerCount = 2u;

    #region SELECTION
    private uint    m_currentPlayerIndex;
    private bool    m_isLoadingPlayPhase = false;
    #endregion

    #region END

    #endregion

    public delegate void OnStateChangement(ESTATE currentState, ESTATE newState);
    public event OnStateChangement StateChanged = (ESTATE prev, ESTATE next) => { };

    private void Awake()
    {
        GameManager gm = FindObjectOfType<GameManager>();

        if (null == gm || this == gm)
        {
            DontDestroyOnLoad(gameObject);
            InstantiatePlayers();
            m_currentPlayerIndex = 0;
        }
        else
        {
            enabled = false;
            Destroy(gameObject);
            return;
        }

        //code to start from the level
        if(m_state == ESTATE.PLAY_PHASE)
        {
            SetPlayerForPlayPhase();
        }
    }

    public void GetPlayers(out List<Platformer2DUserControl> playerControllers)
    {
        playerControllers = m_players;
    }

    public GameObject GetOther(GameObject playergo)
    {
        if (playergo == m_players[0].gameObject)
        {
            return m_players[1].gameObject;
        }
        else if (playergo == m_players[1].gameObject)
        {
            return m_players[0].gameObject;
        }

        return null;
    }

    public int GetCurrentPlayerSelectionIndex()
    {
        return (int) m_currentPlayerIndex;
    }

    public void SetState(ESTATE state)
    {
        if (state != m_state)
        {
            StateChanged(m_state, state);
            OnStateChanged(m_state, state);
            m_state = state;
        }
    }

    public bool AddCapacity(string actionName, float capacityCount)
    {
        var currentPlayerController = m_players[(int)m_currentPlayerIndex];

        var stack = currentPlayerController.GetComponent<Stack>();
        var remainingMemory = stack.GetRemainningAvailableMemory();

        if (remainingMemory < capacityCount)
        {
            return false;
        }

        Action action = null;

        if(stack.TryGetPossibleAction(actionName, ref action))
        {
            stack.AddAction(action);
            SwitchPlayerSelectionTurn();
            return true;
        }

        return false;
    }

    public void SetPlayerSelectionState(bool state)
    {
        m_validatedPlayers[(int)m_currentPlayerIndex] = state;
    }

    public bool ValidatePlayerSelection(int index)
    {
        if(index == (int)m_currentPlayerIndex)
        {
            m_validatedPlayers[index] = true;
            SwitchPlayerSelectionTurn();
            return true;
        }

        return false;
    }

    private void SwitchPlayerSelectionTurn()
    {
        if(2 == RemainingPlayerToSelect())
        {
            m_currentPlayerIndex = m_currentPlayerIndex == 0 ? 1u : 0u;
        }
        else if (1 == RemainingPlayerToSelect())
        {
            if(m_validatedPlayers[m_currentPlayerIndex])
            {
                m_currentPlayerIndex = m_currentPlayerIndex == 0 ? 1u : 0u;
            }
        }
    }

    private int RemainingPlayerToSelect()
    {
        int count = 0;

        if(!m_validatedPlayers[0])
        {
            ++count;
        }

        if(!m_validatedPlayers[1])
        {
            ++count;
        }

        return count;
    }

	// Update is called once per frame
	void Update ()
    {
		if(m_state == ESTATE.SELECTION_PHASE)
        {
            SelectionPhaseUpdate();
        }
        else if (m_state == ESTATE.PLAY_PHASE)
        {
            PlayPhaseUpdate();
        }
	}

    private void SelectionPhaseUpdate()
    {
        if (m_validatedPlayers[0] && m_validatedPlayers[1] && false == m_isLoadingPlayPhase)
        {
            m_isLoadingPlayPhase = true;
            SwitchToPlayPhase();
            Debug.Log("SWITCH TO PLAY PHASE");
        }
    }

    private void PlayPhaseUpdate()
    {
    }

    private void SwitchToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void SwitchToPlayPhase()
    {
        SwitchToScene(m_playSceneName);
        StartCoroutine(WaitForPlaySceneLoaded());
    }

    private IEnumerator WaitForPlaySceneLoaded()
    {
        while(SceneManager.GetActiveScene().name != m_playSceneName)
        {
            Debug.Log("WAITING FOR SCENE IS LOADED");
            yield return new WaitForSeconds(0);
        }

        Debug.Log("SCENE IS LOADED: " + SceneManager.GetActiveScene().name);

        SetPlayerForPlayPhase();

        m_state = ESTATE.PLAY_PHASE;

        Debug.Log("PLAYERS ARE INSTANTIATED");
    }

    private void SwitchFromPlayToEnd()
    {
        ResetAll();
        SwitchToScene(m_selectionSceneName);
        StartCoroutine(WaitForSelectionLoading());
    }

    private IEnumerator WaitForSelectionLoading()
    {
        while (SceneManager.GetActiveScene().name != m_selectionSceneName)
        {
            Debug.Log("WAITING FOR SCENE IS LOADED");
            yield return new WaitForSeconds(0);
        }

        Debug.Log("SCENE IS LOADED: " + SceneManager.GetActiveScene().name);


        m_state = ESTATE.SELECTION_PHASE;

        Debug.Log("PLAYERS ARE INSTANTIATED");
    }


    private void InstantiatePlayer(GameObject playerPrefab, Vector2 spawnPoint, int playerIndex, List<Platformer2DUserControl> outContainer)
    {
        GameObject              playerGO = GameObject.Instantiate(playerPrefab);
        Platformer2DUserControl playercontrol = playerGO.GetComponent<Platformer2DUserControl>();

        if (null != playercontrol)
        {
            playerGO.transform.position = spawnPoint;
            playercontrol.GamepadID = playerIndex;

            outContainer.Add(playercontrol);
        }
        else
        {
            Destroy(playerGO);
            Debug.LogError("GameManger::Awake: The player or the caera prefab is missing");
        }
    }

    private void AddCameraToPlayer(GameObject playerGO, Rect camRect, List<GameObject> outContainer)
    {
        GameObject playerCamera = GameObject.Instantiate(m_cameraPrefab);

        Camera2DFollow camFollowComponent = playerCamera.GetComponent<Camera2DFollow>();
        Parallaxing parallaxing = playerCamera.GetComponent<Parallaxing>();
        PlatformerCharacter2D character = playerGO.GetComponent<PlatformerCharacter2D>();
        
        Camera camComponent = playerCamera.GetComponent<Camera>();

        if (playerGO.transform.name.Contains("0"))
        {
            camComponent.cullingMask = m_parallaxPlayer0;
            parallaxing.backgrounds[0] = GameObject.Find("Furthest1").transform;
            parallaxing.backgrounds[1] = GameObject.Find("Far1").transform;
            parallaxing.backgrounds[2] = GameObject.Find("Midrange1").transform;
            parallaxing.backgrounds[3] = GameObject.Find("Close1").transform;
            character.fond = GameObject.Find("FondPlayer0");
            character.fond.layer = LayerMask.NameToLayer("BackgroundPlayer0");

        }
        else
        {
            camComponent.cullingMask = m_parallaxPlayer1;
            parallaxing.backgrounds[0] = GameObject.Find("Furthest2").transform;
            parallaxing.backgrounds[1] = GameObject.Find("Far2").transform;
            parallaxing.backgrounds[2] = GameObject.Find("Midrange2").transform;
            parallaxing.backgrounds[3] = GameObject.Find("Close2").transform;

            character.fond = GameObject.Find("FondPlayer1");
            character.fond.layer = LayerMask.NameToLayer("Backgroundplayer1");
        }

        camFollowComponent.target = playerGO.transform;
        camFollowComponent.damping = 0f;
        camFollowComponent.lookAheadFactor = 0f;
        camFollowComponent.lookAheadReturnSpeed = 0.5f;
        camFollowComponent.lookAheadMoveThreshold = 0.1f;

        Vector3 camInitPosition = playerGO.transform.position;

        camInitPosition.z -= 2f;

        playerCamera.transform.position = camInitPosition;
        camComponent.rect = camRect;
        camComponent.orthographicSize = m_camZOffset;


        outContainer.Add(playerCamera);
    }

    private void InstantiatePlayers()
    {
        if (null != m_cameraPrefab && m_playerPrefabs.Count != 0)
        {
            InstantiatePlayer(m_playerPrefabs[0], Vector3.zero, 1, m_players);
            InstantiatePlayer(m_playerPrefabs[0], Vector3.zero, 2, m_players);

            DontDestroyOnLoad(m_players[0].gameObject);
            DontDestroyOnLoad(m_players[1].gameObject);

            ResetPlayers();
        }
        else
        {
            Debug.LogError("GameManger::Awake: Something is wrong");
        }
    }

    private void ResetPlayers()
    {
        foreach (MonoBehaviour behaviour in m_players[0].GetComponents<MonoBehaviour>())
        {
            behaviour.enabled = false;
        }

        foreach (MonoBehaviour behaviour in m_players[1].GetComponents<MonoBehaviour>())
        {
            behaviour.enabled = false;
        }

        m_players[0].GetComponent<Rigidbody2D>().Sleep();
        m_players[1].GetComponent<Rigidbody2D>().Sleep();

        m_players[0].GetComponent<Rigidbody2D>().simulated = false;
        m_players[1].GetComponent<Rigidbody2D>().simulated = false;

        m_players[0].GetComponent<SpriteRenderer>().enabled = false;
        m_players[1].GetComponent<SpriteRenderer>().enabled = false;


        m_players[0].GetComponent<Stack>().ClearActionStack();
        m_players[1].GetComponent<Stack>().ClearActionStack();

        m_players[0].GetComponent<Platformer2DUserControl>().ResetAll();
        m_players[1].GetComponent<Platformer2DUserControl>().ResetAll();

        Animator animator0 = m_players[0].GetComponent<Animator>();
        Animator animator1 = m_players[1].GetComponent<Animator>();

        animator0.SetFloat("vSpeed", 0);
        animator0.SetFloat("Speed", 0);
        animator0.SetBool("Win", false);
        animator0.SetBool("Loose", false);
        animator0.SetBool("Stun", false);
        animator0.SetBool("Power", false);
        animator0.SetBool("CAC", false);
        animator0.SetBool("Crouch", false);


        animator1.SetFloat("vSpeed", 0);
        animator1.SetFloat("Speed", 0);
        animator1.SetBool("Win", false);
        animator1.SetBool("Loose", false);
        animator1.SetBool("Stun", false);
        animator1.SetBool("Power", false);
        animator1.SetBool("CAC", false);
        animator1.SetBool("Crouch", false);

        m_players[0].IsInputEnabled = false;
        m_players[1].IsInputEnabled = false;

        m_players[0].gameObject.name = "Player0";
        m_players[1].gameObject.name = "player1";
    }

    private void SetPlayerForPlayPhase()
    {
        foreach (MonoBehaviour behaviour in m_players[0].GetComponents<MonoBehaviour>())
        {
            behaviour.enabled = true;
        }

        foreach (MonoBehaviour behaviour in m_players[1].GetComponents<MonoBehaviour>())
        {
            behaviour.enabled = true;
        }

        m_players[0].GetComponent<Rigidbody2D>().WakeUp();
        m_players[1].GetComponent<Rigidbody2D>().WakeUp();

        m_players[0].GetComponent<Rigidbody2D>().simulated = true;
        m_players[1].GetComponent<Rigidbody2D>().simulated = true;

        m_players[0].GetComponent<SpriteRenderer>().enabled = true;
        m_players[1].GetComponent<SpriteRenderer>().enabled = true;
        
        m_players[0].IsInputEnabled = false;
        m_players[1].IsInputEnabled = false;

        StartCoroutine(Countdown());

        m_players[0].transform.position = GameObject.Find(m_spawnPointsNames[0]).transform.position;
        m_players[1].transform.position = GameObject.Find(m_spawnPointsNames[1]).transform.position;

        AddCameraToPlayer(m_players[0].gameObject, new Rect(0, 0.5f, 1, 0.5f), m_cameras);
        AddCameraToPlayer(m_players[1].gameObject, new Rect(0, 0, 1, 0.5f), m_cameras);
    }

    IEnumerator Countdown()
    {
        GameObject go3 = GameObject.Find("Countdown3");
        GameObject go2 = GameObject.Find("Countdown2");
        GameObject go1 = GameObject.Find("Countdown1");
        GameObject goGO = GameObject.Find("CountdownGO");

        if (goGO && go3 && go2 && go1)
        {

            go3.SetActive(true);
            go2.SetActive(false);
            go1.SetActive(false);
            goGO.SetActive(false);

            Debug.Log("3");

            yield return new WaitForSeconds(1.0f);

            go3.SetActive(false);
            go2.SetActive(true);
            Debug.Log("2");

            yield return new WaitForSeconds(1.0f);

            go2.SetActive(false);
            go1.SetActive(true);
            Debug.Log("1");

            yield return new WaitForSeconds(1.0f);

            go1.SetActive(false);
            goGO.SetActive(true);
            Debug.Log("GO");

            m_players[0].IsInputEnabled = true;
            m_players[1].IsInputEnabled = true;

            yield return new WaitForSeconds(1.0f);

            goGO.SetActive(false);
        }
        else
        {
            m_players[0].IsInputEnabled = true;
            m_players[1].IsInputEnabled = true;
        }
    }

    private void ResetAll()
    {
        ResetPlayers();
        m_validatedPlayers[0] = false;
        m_validatedPlayers[1] = false;
        m_isLoadingPlayPhase = false;
        m_currentPlayerIndex = 0;
    }

    private void OnStateChanged(ESTATE previousState, ESTATE nextState)
    {
        if(nextState == ESTATE.END_PHASE)
        {
            // SwitchFromPlayToEnd();
           // ResetPlayers();
        }

        if (nextState == ESTATE.SELECTION_PHASE)
        {
            ResetAll();
            SwitchToScene(m_selectionSceneName);
            StartCoroutine(WaitForSelectionLoading());
        }

        if (nextState == ESTATE.MENU_PHASE)
        {
            ResetAll();
            SwitchToScene(m_menuSceneName);
            StartCoroutine(WaitForSelectionLoading());
        }
    }
}

