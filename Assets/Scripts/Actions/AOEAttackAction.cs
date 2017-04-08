using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AOEAttackAction : Action
{
    [SerializeField]
    private GameObject AOEAttackFX;

    [SerializeField]
    private Transform AoeAttackFXPosition;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float m_activationTime;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float m_cooldown;

    private float m_activationTimer;
    private float m_cooldownElapsedTime;

    [SerializeField]
    private float m_damage;

    private Collider2D m_boxCollider;

    public override void Tick()
    {
        base.Tick();
        
        if (false == IsInitialized())
            return;

        Gamepad gamepad = null;

        if (TryGetValidGamepad(out gamepad))
        {
            if (gamepad.GetButtonDown("B") && m_controller.IsInputEnabled)
            {
                ActivateAEO();
            }
        }
    }

    private void Update()
    {
        if (false == IsInitialized())
        {
            return;
        }

        //decreasing the cooldown time
        if (0 < m_cooldownElapsedTime)
        {
            m_cooldownElapsedTime -= Time.deltaTime;

            if (0f >= m_cooldownElapsedTime)
            {
                m_cooldownElapsedTime = -1f;
            }
        }

        //decreasing the activation time
        if (0 < m_activationTimer)
        {
            m_activationTimer -= Time.deltaTime;

            if (0.0f >= m_activationTimer)
            {
                DeactivateTriggerPhysic();
                m_cooldownElapsedTime = m_cooldown;
                m_activationTimer = -1f;
            }
        }
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

    protected override void Awake()
    {
        base.Awake();

        m_boxCollider = GetComponent<Collider2D>();

        m_cooldownElapsedTime = -1f;
        m_activationTimer = -1f;

        DeactivateTriggerPhysic();
    }

    private bool IsInitialized()
    {
        bool res = false;

        res = null != m_boxCollider;

        return res;
    }

    private void ActivateAEO()
    {
        //the aoe has not been activated yet AND the cooldown time has been elapsed
        if (0 > m_cooldownElapsedTime &&
            0 > m_activationTimer)
        {
            m_activationTimer = m_activationTime;

            ActivateTriggerPhysic();

            GameObject o = Instantiate(AOEAttackFX, AoeAttackFXPosition.position, Quaternion.identity);
            o.transform.parent = transform;

        }
    }

    private void ActivateTriggerPhysic()
    {
        m_boxCollider.enabled = true;
        m_controller.GetComponent<Animator>().SetBool("Power", true);
    }

    private void DeactivateTriggerPhysic()
    {
        m_boxCollider.enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EntityBrain brain = other.gameObject.GetComponent<EntityBrain>();

        if (null != brain)
        {
            brain.ApplyDamage(m_damage);
        }
    }
}
