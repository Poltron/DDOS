using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure;

public class GamepadManager : GenericSingleton<GamepadManager>
{
    public int              gamepadCount = 2; // Number of gamepads to support
    private List<Gamepad>   gamepads;     // Holds gamepad instances
    private string[]        gamepadNames = null;


    public void Refresh()
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Refresh();
        }
    }

    // Return specified gamepad
    // (Pass index of desired gamepad, eg. 1)
    public Gamepad GetGamepad(int index)
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            // Indexes match, return this gamepad
            if (gamepads[i].GetIndex() == (index -1))
            {
                return gamepads[i];
            }
        }

        Debug.LogError("[GamepadManager]: " + index + " is not a valid gamepad index!");

        return null;
    }

    // Check across all gamepads for button press.
    // Return true if the conditions are met by any gamepad
    public bool GetButtonAny(string button)
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            // Gamepad meets both conditions
            if (gamepads[i].IsConnected() && gamepads[i].GetButton(button))
            {
                return true;
            }
        }

        return false;
    }

    // Check across all gamepads for button press - CURRENT frame.
    // Return true if the conditions are met by any gamepad
    public bool GetButtonDownAny(string button)
    {
        for (int i = 0; i < gamepads.Count; ++i)
        {
            // Gamepad meets both conditions
            if (gamepads[i].IsConnected() && gamepads[i].GetButtonDown(button))
            {
                return true;
            }
        }

        return false;
    }

    // Return number of connected gamepads
    public int ConnectedTotal()
    {
        int total = 0;

        for (int i = 0; i < gamepads.Count; ++i)
        {
            if (gamepads[i].IsConnected())
            {
                total++;
            }
        }

        return total;
    }

    // Use this for initialization
    private void Awake()
    {
        gamepadNames = Input.GetJoystickNames();


        // Lock GamepadCount to supported range
        gamepadCount = Mathf.Clamp(gamepadCount, 1, 4);

        gamepads = new List<Gamepad>();

        // Create specified number of gamepad instances
        for (int i = 0; i < gamepadCount; ++i)
        {
            gamepads.Add(new Gamepad(i + 1));
        }

    }

    // Update is called once per frame
    private void Update()
    {
        //CheckGamepadPlugChangementss();

        for (int i = 0; i < gamepads.Count; ++i)
        {
            gamepads[i].Update();
        }
    }

    private void CheckGamepadPlugChangementss()
    {
        string[] gamepadNames = Input.GetJoystickNames();

        //one of the gamepad has been plugged or unplugged
        if(gamepadNames.Length != gamepads.Count)
        {
            //a gamepad has been plugged
            if(gamepadNames.Length > gamepads.Count)
            {
                
            }
            //a gamepad has been unplugged
            else
            {
                
            }
        }
    }

    private int GetConnectedPadCount()
    {
        int      padCount = 0;

        foreach (var padName in gamepadNames)
        {
            if(padName != "")
            {
                ++padCount;
            }
        }

        return padCount;
    }
}
