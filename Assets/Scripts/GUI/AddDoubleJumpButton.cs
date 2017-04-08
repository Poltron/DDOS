using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddDoubleJumpButton : GUIButton
{
    protected override void DoOnClick() 
    {
        DoubleJumpAction component = null;

        foreach (Action action in playerGUI.GetPlayerStack().possibleActions)
        {
            component = action.GetComponent<DoubleJumpAction>();

            if (component != null)
            {
                break;
            }
        }
        
        playerGUI.GetPlayerStack().AddAction(component);
    }
}
