using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class JumpAction : Action
{
    public override void Tick()
    {
        base.Tick();

        Gamepad gamepad = null;

        if(TryGetValidGamepad(out gamepad))
        {
            if(gamepad.GetButton("A") && m_controller.IsInputEnabled)
            {
                Do();
            }
        }

    }

    public override void Do()
    {
        base.Do();
    }

    public override void OnEnabled()
    {
        base.OnEnabled();

        Platformer2DUserControl controller = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
        controller.CanJump = true;
    }

    public override void OnDisabled()
    {
        base.OnDisabled();

        Platformer2DUserControl controller = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
        controller.CanJump = false;
    }
}
