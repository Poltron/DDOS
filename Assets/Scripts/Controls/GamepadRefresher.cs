using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamepadRefresher : GenericSingleton<GamepadRefresher> 
{
	// Update is called once per frame
	private void Update () 
    {
        GamepadManager gpm = GamepadManager.Instance;

        if(null != gpm)
        {
            gpm.Refresh();
        }
    }
}
