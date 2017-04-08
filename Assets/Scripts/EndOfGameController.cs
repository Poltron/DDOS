using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EndOfGameController : MonoBehaviour 
{

    [SerializeField]
    private List<GameObject> m_player0Panels = new List<GameObject>();

    [SerializeField]
    private List<GameObject> m_player1Panels = new List<GameObject>();

    [SerializeField]
    private GameObject m_backToMenuButton = null;

    // Use this for initialization
    void Start () 
    {
        if (GameManager.Instance)
            GameManager.Instance.StateChanged += OnStateChanged;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnStateChanged(GameManager.ESTATE currentState, GameManager.ESTATE newState)
    {
        if(newState == GameManager.ESTATE.END_PHASE)
        {
            GameObject winner = FindObjectOfType<EndOfGame>().GetWinner();
            GameObject looser = GameManager.Instance.GetOther(winner);

            winner.GetComponent<Animator>().SetBool("Win", true);
            looser.GetComponent<Animator>().SetBool("Loose", true);

            winner.GetComponent<Stack>().enabled = false;
            looser.GetComponent<Stack>().enabled = false;

            winner.GetComponent<Platformer2DUserControl>().IsInputEnabled = false;
            winner.GetComponent<Platformer2DUserControl>().ResetAll();
            looser.GetComponent<Platformer2DUserControl>().IsInputEnabled = false;
            looser.GetComponent<Platformer2DUserControl>().ResetAll();

            StartCoroutine(ActivateEndPanels(winner, looser));

        //    GameManager.Instance.SetState(GameManager.ESTATE.MENU_PHASE);
        }
    }

    private IEnumerator ActivateEndPanels(GameObject winner, GameObject looser)
    {
        yield return new WaitForSeconds(3);

        if(winner.name == "Player0")
        {
            ActivateWinPanel(m_player0Panels);
            ActivateLoosePanel(m_player1Panels);
        }
        else 
        {
            ActivateWinPanel(m_player1Panels);
            ActivateLoosePanel(m_player0Panels);
        }

        m_backToMenuButton.SetActive(true);
        EventSystem.current.SetSelectedGameObject(m_backToMenuButton);
    }

    private void ActivateLoosePanel(List<GameObject> panels)
    {
        panels[1].gameObject.SetActive(true);
    }

    private void ActivateWinPanel(List<GameObject> panels)
    {
        panels[0].gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
    if(GameManager.Instance)
        GameManager.Instance.StateChanged -= OnStateChanged;

    }
}
