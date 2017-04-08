using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddShootButton : GUIButton
{
    protected override void DoOnClick()
    {
        ShotAction component = null;

        foreach (Action action in playerGUI.GetPlayerStack().possibleActions)
        {
            component = action.GetComponent<ShotAction>();

            if (component != null)
            {
                break;
            }
        }

        playerGUI.GetPlayerStack().AddAction(component);
    }
}
