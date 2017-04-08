using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CACAction : Action 
{
    [SerializeField]
    private GameObject HitFX;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float           m_activationTime;

    [SerializeField]
    [Range(0.1f, 10f)]
    private float           m_cooldown;

    private float           m_activationTimer;
    private float           m_cooldownElapsedTime;

    [SerializeField]
    private float           m_damage;

    private BoxCollider2D   m_boxCollider;
    private List<Collider2D> collidersAlreadyHit = new List<Collider2D>();

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
                bool isOverriden = false;

                foreach (Action action in m_controller.GetComponent<Stack>().actions)
                {
                    if ((action is ShotAction || action is AOEAttackAction) && action.IsEnabled)
                    {
                        isOverriden = true;
                        break;
                    }
                }

                if (!isOverriden)
                    ActivateAEO();
            }
        }
    }

    private bool onetwo;

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

            if(0f >= m_cooldownElapsedTime)
            {
                m_cooldownElapsedTime = -1f;
            }
        }

        //decreasing the activation time
        if (0 < m_activationTimer)
       {
            m_activationTimer -= Time.deltaTime;

            if(0.0f >= m_activationTimer)
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

        m_boxCollider = GetComponent<BoxCollider2D>();

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
        if( 0 > m_cooldownElapsedTime && 
            0 > m_activationTimer)
        {
            m_activationTimer = m_activationTime;

            ActivateTriggerPhysic();

            m_controller.GetComponent<Animator>().SetBool("CAC", true);

            Debug.Log("CACACTION");
        }
    }

    private void ActivateTriggerPhysic()
    {
        m_boxCollider.enabled = true;
    }

    private void DeactivateTriggerPhysic()
    {
        m_boxCollider.enabled = false;
        collidersAlreadyHit.Clear();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EntityBrain brain = other.gameObject.GetComponent<EntityBrain>();
        
        /*foreach (Collider2D collider in collidersAlreadyHit)
        {
            if (collider == other)
                return;
        }

        if (other == transform.parent.GetComponent<Collider2D>())
            return;*/

        if(null != brain)
        {

            Platformer2DUserControl comp = other.GetComponent<Platformer2DUserControl>();

            if (comp != null && !comp.m_invincible)
                Instantiate(HitFX, other.transform.position, Quaternion.identity);

            //collidersAlreadyHit.Add(other);

            brain.ApplyDamage(m_damage);
        }
    }
}
