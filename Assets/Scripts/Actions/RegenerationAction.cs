using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegenerationAction : Action
{
    [SerializeField]
    private float MemoryRegenPerSec;

    public override void Tick()
    {
        base.Tick();

        if (!IsEnabled)
            return;
        
        m_controller.GetComponent<EntityBrain>().ApplyHeal(MemoryRegenPerSec * Time.deltaTime);
    }

    public override void Do()
    {
        base.Do();
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
