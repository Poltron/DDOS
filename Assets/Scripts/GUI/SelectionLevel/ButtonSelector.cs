using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonSelector : MonoBehaviour 
{
    [SerializeField]
    private List<GameObject> m_capacityButtons = new List<GameObject>();

    [SerializeField]
    private List<GameObject> m_validationButtons = new List<GameObject>();

	// Update is called once per frame
	void Update () 
    {
        EventSystem ev = EventSystem.current;

        GameObject availableButton = null;

        if(ev.currentSelectedGameObject != null)
        {
            return;
        }

        if(TryFindAvailableCapacityButton(out availableButton))
        {
            ev.SetSelectedGameObject(availableButton);
        }
        else
        {
            GameManager gm = GameManager.Instance;

            if(gm)
            {
                var playerIndex = gm.GetCurrentPlayerSelectionIndex();

                ev.SetSelectedGameObject(m_validationButtons[playerIndex]);
            }
        }

	}


    private bool TryFindAvailableCapacityButton(out GameObject go)
    {
        bool res = false;

        go = null;


        foreach(var capButton in m_capacityButtons)
        {
            Button button = capButton.GetComponent<Button>();

            if (button.IsInteractable())
            {
                go = button.gameObject;
                return true;
            }
        }

        return res;
    }
}
