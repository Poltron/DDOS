using System.Collections;
using XInputDotNetPure;
using System.Collections.Generic;
using UnityEngine;

// Stores states of a single gamepad button
public struct XButton
{
    public ButtonState prevState;
    public ButtonState state;
}

// Stores state of a single gamepad trigger
public struct TriggerState
{
    public float prevValue;
    public float currentValue;
}

// Rumble (vibration) event
class XRumble
{
    public float timer = 0;    // Rumble timer
    public float duration = 0; // Fade-out (in seconds)
    public Vector2 power = new Vector2();  // Intensity of rumble
}

public class Gamepad
{
    private GamePadState    prevState; // Previous gamepad state
    private GamePadState    state;      // Current gamepad state

    private int             gamepadIndex;        // Numeric index (1,2,3 or 4
    private PlayerIndex     playerIndex;    // XInput 'Player' index
    //private List<XRumble>   rumbleEvents; // Stores rumble events

    // Button input map
    private Dictionary<string, XButton> inputMap;

    // States for all buttons/inputs supported
    private XButton         A, B, X, Y; // Action (face) buttons
    private XButton         DPad_Up, DPad_Down, DPad_Left, DPad_Right;

    private XButton         Guide;       // Xbox logo button}
    private XButton         Back, Start;
    private XButton         L3, R3;      // Thumbstick buttons
    private XButton         LB, RB;      // 'Bumper' (shoulder) buttons
    private TriggerState    LT, RT; // Triggers

    //float leftStickX = 0f;
    //float leftStickY = 0f;

    // Constructor
    public Gamepad(int index)
    {
        // Set gamepad index
        gamepadIndex = index - 1;
        playerIndex = (PlayerIndex)gamepadIndex;

        // Create rumble container and input map
        //rumbleEvents = new List<XRumble>();
        inputMap = new Dictionary<string, XButton>();
        
        //leftStickX = GetStick_L().X;
        //leftStickY = GetStick_L().Y;
    }

    // Return numeric gamepad index
    public int GetIndex() { return gamepadIndex; }

    // Return gamepad connection state
    public bool IsConnected() { return state.IsConnected; }

    public bool GetButton(string buttonName)
    {
        return inputMap[buttonName].state == ButtonState.Pressed ? true : false;
    }

    public bool GetButtonDown(string button)
    {
        return (inputMap[button].prevState == ButtonState.Released &&
                inputMap[button].state == ButtonState.Pressed) ? true : false;
    }

    public void Update()
    {
        // Get current state
        state = GamePad.GetState(playerIndex);

        // Check gamepad is connected
        if (state.IsConnected)
        {
            A.state = state.Buttons.A;
            B.state = state.Buttons.B;
            X.state = state.Buttons.X;
            Y.state = state.Buttons.Y;

            DPad_Up.state = state.DPad.Up;
            DPad_Down.state = state.DPad.Down;
            DPad_Left.state = state.DPad.Left;
            DPad_Right.state = state.DPad.Right;

            Guide.state = state.Buttons.Guide;
            Back.state = state.Buttons.Back;
            Start.state = state.Buttons.Start;
            L3.state = state.Buttons.LeftStick;
            R3.state = state.Buttons.RightStick;
            LB.state = state.Buttons.LeftShoulder;
            RB.state = state.Buttons.RightShoulder;

            // Read trigger values into trigger states
            LT.currentValue = state.Triggers.Left;
            RT.currentValue = state.Triggers.Right;

            UpdateInputMap();
        }
    }


    // Refresh previous gamepad state
    public void Refresh()
    {
        // This 'saves' the current state for next update
        prevState = state;

        // Check gamepad is connected
        if (state.IsConnected)
        {
            A.prevState = prevState.Buttons.A;
            B.prevState = prevState.Buttons.B;
            X.prevState = prevState.Buttons.X;
            Y.prevState = prevState.Buttons.Y;

            DPad_Up.prevState = prevState.DPad.Up;
            DPad_Down.prevState = prevState.DPad.Down;
            DPad_Left.prevState = prevState.DPad.Left;
            DPad_Right.prevState = prevState.DPad.Right;

            Guide.prevState = prevState.Buttons.Guide;
            Back.prevState = prevState.Buttons.Back;
            Start.prevState = prevState.Buttons.Start;
            L3.prevState = prevState.Buttons.LeftStick;
            R3.prevState = prevState.Buttons.RightStick;
            LB.prevState = prevState.Buttons.LeftShoulder;
            RB.prevState = prevState.Buttons.RightShoulder;

            // Read previous trigger values into trigger states
            LT.prevValue = prevState.Triggers.Left;
            RT.prevValue = prevState.Triggers.Right;

            UpdateInputMap();
        }
    }

    #region THUNBSTICK
    // Return axes of left thumbstick
    public GamePadThumbSticks.StickValue GetStick_L()
    {
        return state.ThumbSticks.Left;
    }

    // Return axes of right thumbstick
    public GamePadThumbSticks.StickValue GetStick_R()
    {
        return state.ThumbSticks.Right;
    }
    #endregion

    #region TRIGGER
    public float GetTriggerL() 
    {
        return state.Triggers.Left; 
    }

    // Return axis of right trigger
    public float GetTriggerR() 
    { 
        return state.Triggers.Right; 
    }

    // Check if left trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_L()
    {
        return (LT.prevValue == 0f && LT.currentValue >= 0.1f) ? true : false;
    }

    // Check if right trigger was tapped - on CURRENT frame
    public bool GetTriggerTap_R()
    {
        return (RT.prevValue == 0f && RT.currentValue >= 0.1f) ? true : false;
    }
    #endregion

    // Update input map
    private void UpdateInputMap()
    {
        inputMap["A"] = A;
        inputMap["B"] = B;
        inputMap["X"] = X;
        inputMap["Y"] = Y;

        inputMap["DPad_Up"] = DPad_Up;
        inputMap["DPad_Down"] = DPad_Down;
        inputMap["DPad_Left"] = DPad_Left;
        inputMap["DPad_Right"] = DPad_Right;

        inputMap["Guide"] = Guide;
        inputMap["Back"] = Back;
        inputMap["Start"] = Start;

        // Thumbstick buttons
        inputMap["L3"] = L3;
        inputMap["R3"] = R3;

        // Shoulder ('bumper') buttons
        inputMap["LB"] = LB;
        inputMap["RB"] = RB;
    }

}
