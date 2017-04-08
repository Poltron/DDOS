using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class DoubleJumpAction : Action {

    public override void Tick()
    {
        base.Tick();
    }

    public override void Do()
    {
        base.Do();
    }

    public override void OnEnabled()
    {
        base.OnEnabled();

        m_controller.CanDoubleJump = true;
    }

    public override void OnDisabled()
    {
        base.OnDisabled();

        m_controller.CanDoubleJump = false;
    }
}
