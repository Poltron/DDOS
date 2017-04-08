using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCACButton : GUIButton
{
    protected override void DoOnClick()
    {
        CACAction component = null;

        foreach (Action action in playerGUI.GetPlayerStack().possibleActions)
        {
            component = action.GetComponent<CACAction>();

            if (component != null)
            {
                break;
            }
        }

        playerGUI.GetPlayerStack().AddAction(component);

    }
}
