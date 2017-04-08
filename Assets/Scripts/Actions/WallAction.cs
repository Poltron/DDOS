using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallAction : Action
{
    [SerializeField]
    private float cooldown;

    private Timer cooldownTimer = new Timer();
    private bool canCast = true;

    [SerializeField]
    private GameObject wall;
    [SerializeField]
    private float distanceToSpawn;

    public override void Tick()
    {
        base.Tick();

        Gamepad gamepad = null;

        if (TryGetValidGamepad(out gamepad))
        {
            if (gamepad.GetButtonDown("Y") && canCast && m_controller.IsInputEnabled)
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
        
        Instantiate(wall, transform.position - new Vector3(distanceToSpawn * m_controller.m_lookDirection, 0, 0), Quaternion.identity);
        cooldownTimer.SetTimer(cooldown);
        canCast = false;
        cooldownTimer.OnEndTimer += () => { canCast = true; };

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
