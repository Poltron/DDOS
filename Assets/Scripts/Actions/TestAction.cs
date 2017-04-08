using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAction : Action {

    public override void Tick()
    {
        base.Tick();

        if (Input.GetKeyDown(KeyCode.R))
        {
            Do();
        }
    }

    public override void Do()
    {
        base.Do();

        Debug.Log("Test Action");
    }
}
