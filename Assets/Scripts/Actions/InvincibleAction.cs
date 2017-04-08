using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleAction : Action
{
    [SerializeField]
    private float m_activationTime;

    [SerializeField]
    private float m_deactivationTime;
    
    private Timer m_activationTimer = new Timer();
    private Timer m_deactivationTimer = new Timer();

    private bool m_isActivated = false;

    public override void Tick()
    {
        base.Tick();

        Gamepad gamepad = null;

        if (TryGetValidGamepad(out gamepad))
        {
            if (gamepad.GetButtonDown("L3") && m_controller.IsInputEnabled)
            {
                Do();
            }
        }

    }

    private void Update()
    {
        m_activationTimer.ElapseTime(Time.deltaTime);
        m_deactivationTimer.ElapseTime(Time.deltaTime);

        if(m_isActivated)
        {
            if(true == m_activationTimer.HasElapsedAllTime())
            {
                m_deactivationTimer.SetTimer(m_deactivationTime);
                m_isActivated = false;
            }
        }
    }

    public override void Do()
    {
        base.Do();

        if(false == m_isActivated && true == m_deactivationTimer.HasElapsedAllTime())
        {
            m_controller.GetComponent<Animator>().SetBool("Power", true);
            m_activationTimer.SetTimer(m_activationTime);
            m_isActivated = true;
            m_controller.SetInvincible(m_activationTime);
        }
    }

}
