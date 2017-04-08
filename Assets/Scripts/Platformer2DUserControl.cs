using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets._2D;

[RequireComponent(typeof (PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour
{
    [SerializeField]
    private GameObject BubbleFX;

    [SerializeField]
    private float AlphaWhenJusthit;

    public bool CanJump;
    public bool CanWalk;
    public bool CanDoubleJump;
    public bool CanInfiniteJump;

    public int  GamepadID;

    [SerializeField]
    private float   m_stunTime;

    [SerializeField]
    private float   m_additionalInvincibleTime;

    [SerializeField]
    private float   m_invincibleAlphaToggleFrequency;

    private PlatformerCharacter2D m_Character;
    private bool m_Jump;
    private Gamepad m_gamepad;
    [HideInInspector]
    public float m_lookDirection;

    private Timer   m_stunTimer = new Timer();
    private Timer   m_invincibleTimer = new Timer();
    private Stack   m_stack = null;
    private bool    m_hit = false;
    public bool    m_invincible = false;

    public bool IsInputEnabled = true;
    private Timer m_stopInputTimer = new Timer();

    public bool HasValidGamepad()
    {
        return null != m_gamepad && true == m_gamepad.IsConnected();
    }

    public bool IsHit()
    {
        return m_hit;
    }

    public float GetLookDirection()
    {
        return m_lookDirection;
    }

    public Gamepad  GetGamepad()
    {
        return m_gamepad;
    }

    public void SetInvincible(float invincibleTime)
    {
        invincibleTime = Mathf.Clamp(invincibleTime, 0, float.MaxValue);

        if (false == m_invincible)
        {
            BubbleFX.SetActive(true);
            m_invincible = true;
            m_invincibleTimer.SetTimer(invincibleTime);
        }
    }


    public void ResetAll()
    {
        
        m_Character.ResetAll();
        StopCoroutine("StunCoroutine");

        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        Color c = sprite.color;
        c.a = 1;
        sprite.color = c;
        BubbleFX.SetActive(false);
        m_invincible = false;
        m_hit = false;

        m_stunTimer.Reset();
        m_invincibleTimer.Reset();
        m_stopInputTimer.Reset();
    }

    private void Awake()
    {
        m_Character = GetComponent<PlatformerCharacter2D>();
        m_lookDirection = 1f;

        EntityBrain brain = GetComponent<EntityBrain>();
        Stack stack = GetComponent<Stack>();

        if(stack && brain)
        {
            m_stack = stack;
            brain.OnApplyDamage += ApplyDamage;
            brain.OnApplyHeal += ApplyHeal;
        }
    }

    private void Start()
    {
        GamepadManager gpm = GamepadManager.Instance;

        m_gamepad = gpm != null ? gpm.GetGamepad(GamepadID) : null;

        var layerVal = LayerMask.NameToLayer(gameObject.name);

        gameObject.layer = layerVal;

        if (!HasValidGamepad())
        {
            return;
        }
    }

    private void Update()
    {
        if(!HasValidGamepad())
        {
            return;
        }

        m_stunTimer.ElapseTime(Time.deltaTime);
        m_invincibleTimer.ElapseTime(Time.deltaTime);
        m_stopInputTimer.ElapseTime(Time.deltaTime);

        if (!m_Jump && CanJump)
        {
            // Read the jump input in Update so button presses aren't missed.
            m_Jump = m_gamepad.GetButtonDown("A");
        }

        if(true == m_hit && true == m_stunTimer.HasElapsedAllTime())
        {
            m_hit = false;
            m_Character.SetStunState(false);
        }

        if(true == m_invincible && true == m_invincibleTimer.HasElapsedAllTime())
        {
            m_invincible = false;
            Color c = GetComponent<SpriteRenderer>().color;
            c.a = 1;
            GetComponent<SpriteRenderer>().color = c;
            BubbleFX.SetActive(false);
        }
    }


    private void FixedUpdate()
    {
        if (!HasValidGamepad() && true == m_hit)
        {
            return;
        }

        // Read the inputs.
        float h = m_gamepad.GetStick_L().X;

        if (!CanWalk)
            h = 0;
        
        if(false == Mathf.Approximately(h, 0.0f))
        {
            m_lookDirection = h > 0 ? 1f : -1f;
        }

        if (IsInputEnabled || m_Character.m_IsStunned)
        {
            // Pass all parameters to the character control script.
            m_Character.Move(h, m_Jump);
        }
        
        m_Jump = false;
    }

    private void ApplyDamage(float damage)
    {
        if (!IsInputEnabled)
        {
            return;
        }

        //not invincible
        if (false == m_invincible && true == m_invincibleTimer.HasElapsedAllTime())
        {
            m_hit = true;
            m_invincible = true;
            
            Color c = GetComponent<SpriteRenderer>().color;
            c.a = AlphaWhenJusthit;
            GetComponent<SpriteRenderer>().color = c;
            
            m_stack.DamageMemory(damage);
            m_stunTimer.SetTimer(m_stunTime);
            m_Character.SetStunState(true);
            m_invincibleTimer.SetTimer(m_additionalInvincibleTime);

            StartCoroutine(BlinkAlpha());
        }
    }

    private bool m_blinking;

    private IEnumerator BlinkAlpha()
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();
        Color c = sprite.color;

        m_blinking = true;

        float timer = 0.0f;

        float nextAlpha = AlphaWhenJusthit;

        while (m_invincible)
        {
            if (timer > m_invincibleAlphaToggleFrequency)
            {
                sprite.color = c;
                timer = 0.0f;

                if (sprite.color.a == AlphaWhenJusthit)
                    c.a = 1.0f;
                else
                    c.a = AlphaWhenJusthit;
            }

            yield return new WaitForSeconds(Time.deltaTime);

            timer += Time.deltaTime;
        }

        
        c.a = 1;
        sprite.color = c;

        m_blinking = false;
    }

    private void ApplyHeal(float heal)
    {
        if (!IsInputEnabled)
        {
            return;
        }

        m_stack.HealMemory(heal);
    }

    public void StopPlayerInput(float time)
    {
        m_stopInputTimer.SetTimer(time);
        IsInputEnabled = false;

        m_stopInputTimer.OnEndTimer += () => { IsInputEnabled = true; };
    }
}
