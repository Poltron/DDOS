using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class FlyAction : Action
{
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

        Platformer2DUserControl controller = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
        controller.CanInfiniteJump = true;
    }

    public override void OnDisabled()
    {
        base.OnDisabled();

        Platformer2DUserControl controller = gameObject.transform.parent.GetComponent<Platformer2DUserControl>();
        controller.CanInfiniteJump = false;
    }
}
