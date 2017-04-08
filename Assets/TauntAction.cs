using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntAction : Action
{
    public override void Tick()
    {
        base.Tick();

        Gamepad gamepad = null;

        if (TryGetValidGamepad(out gamepad))
        {
            Debug.Log("can taunt");

            if (( gamepad.GetButtonDown("DPad_Up") || gamepad.GetButtonDown("DPad_Down") || gamepad.GetButtonDown("DPad_Left") || gamepad.GetButtonDown("DPad_Right")) && m_controller.IsInputEnabled)
            {
                Debug.Log("taunt");
                Do();
            }
        }
    }

    public override void Do()
    {
        base.Do();

        transform.parent.GetComponent<Animator>().SetTrigger("Taunt");
    }

    public override void OnEnabled()
    {
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        base.OnDisabled();
    }
}
