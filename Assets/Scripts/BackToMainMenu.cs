using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour 
{
    public void OnBackToMainMenu()
    {
        GameManager gm = GameManager.Instance;

        if(gm)
        {
            gm.SetState(GameManager.ESTATE.MENU_PHASE);
        }
    }
}
