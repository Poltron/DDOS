using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShareMemoryAction : Action
{
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
            if (gamepad.GetTriggerTap_L() && canCast && m_controller.IsInputEnabled)
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

        float memoryPlayer1 = controllers[0].GetComponent<Stack>().actualMemory;
        float memoryPlayer2 = controllers[1].GetComponent<Stack>().actualMemory;

        float moyenne = (memoryPlayer1 + memoryPlayer2) / 2.0f;

        controllers[0].GetComponent<Stack>().actualMemory = moyenne;
        controllers[1].GetComponent<Stack>().actualMemory = moyenne;

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
