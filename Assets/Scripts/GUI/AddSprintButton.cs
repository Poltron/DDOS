using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddSprintButton : GUIButton
{
    protected override void DoOnClick()
    {
        InvincibleAction component = null;

        foreach (Action action in playerGUI.GetPlayerStack().possibleActions)
        {
            component = action.GetComponent<InvincibleAction>();

            if (component != null)
            {
                break;
            }
        }

        playerGUI.GetPlayerStack().AddAction(component);
    }
}
