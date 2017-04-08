using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets._2D;

public class StunAction : Action
{
    [SerializeField]
    private float time;
    [SerializeField]
    private float cooldown;

    private Timer cooldownTimer = new Timer();
    private bool canCast = true;

    public override void Tick()
    {
        base.Tick();

        Gamepad gamepad = null;

        if (TryGetValidGamepad(out gamepad))
        {
            if (gamepad.GetButtonDown("X") && canCast && m_controller.IsInputEnabled)
            {
                Do();
            }
        }

        if (!canCast)
            cooldownTimer.ElapseTime(Time.deltaTime);
    }

    public override void Do()
    {
        base.Do();

        List<Platformer2DUserControl> controllers = null;
        GameManager.Instance.GetPlayers(out controllers);

        foreach (Platformer2DUserControl controller in controllers)
        {
            if (controller == m_controller)
                continue;
            
            controller.StopPlayerInput(time);
            cooldownTimer.SetTimer(cooldown);
            canCast = false;
            cooldownTimer.OnEndTimer += () => { canCast = true; };
        }

        m_controller.GetComponent<Animator>().SetBool("Power", true);
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
