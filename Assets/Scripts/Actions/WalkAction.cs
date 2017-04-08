using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class WalkAction : Action
{
    public override void Tick()
    {
        base.Tick();
        /*
        float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
        if (horizontal != 0)
        {
            Do();
        }
        */
    }

    public override void Do()
    {
        base.Do();
    }

    public override void OnEnabled()
    {
        base.OnEnabled();

        Platformer2DUserControl controller = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
        controller.CanWalk = true;
    }

    public override void OnDisabled()
    {
        base.OnDisabled();

        Platformer2DUserControl controller = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
        controller.CanWalk = false;
    }
}