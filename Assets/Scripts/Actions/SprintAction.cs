using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

public class SprintAction : Action
{
    [SerializeField]
    private float SprintSpeed;

    private float BackupNormalSpeed;

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

        PlatformerCharacter2D controller = gameObject.transform.parent.GetComponent<PlatformerCharacter2D>();
        BackupNormalSpeed = controller.m_MaxSpeed;

        controller.m_MaxSpeed = SprintSpeed;
    }

    public override void OnDisabled()
    {
        base.OnDisabled();

        PlatformerCharacter2D controller = gameObject.transform.parent.GetComponent<PlatformerCharacter2D>();
        controller.m_MaxSpeed = BackupNormalSpeed;
    }
}
