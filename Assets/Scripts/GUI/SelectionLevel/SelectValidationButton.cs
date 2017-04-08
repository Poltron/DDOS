using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectValidationButton : MonoBehaviour 
{
    public int playerIndex = 0;

    // Update is called once per frame
    public void SelectValidation()
    {
        GameManager gm = GameManager.Instance;

        if (null != gm)
        {
            if(gm.ValidatePlayerSelection(playerIndex))
            {
                GetComponent<Button>().interactable = false;
            }
        }
    }
}
