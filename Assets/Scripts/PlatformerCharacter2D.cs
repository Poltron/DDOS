using System;
using System.Collections;
using UnityEngine;

namespace UnityStandardAssets._2D
{
    public class PlatformerCharacter2D : MonoBehaviour
    {
        public GameObject fond;

        [SerializeField] private GameObject ConfettiFX;
        [SerializeField] private Transform ConfettiFXPosition;

        [SerializeField] private GameObject ShockwavePowerFX;
        [SerializeField] private Transform ShockWavePowerPosition;

        [SerializeField] private ParticleSystem WalkDustFX;
        [SerializeField] private GameObject JumpDustFX;
        [SerializeField] private Transform JumpDustFXPosition;
        [SerializeField] private GameObject StunFX;

        public float m_MaxSpeed = 10f;                    // The fastest the player can travel in the x axis.
        [SerializeField] private float m_JumpForce = 400f;                  // Amount of force added when the player jumps.
        [SerializeField] private float m_DoubleJumpForce = 300f;                  // Amount of force added when the player jumps.
        [SerializeField] private float m_FlyJumpForce = 200f;                  // Amount of force added when the player jumps.
        [SerializeField] private bool m_AirControl = false;                 // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask m_WhatIsGround;                  // A mask determining what is ground to the character

        private Transform m_GroundCheck;    // A position marking where to check if the player is grounded.
        const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool m_Grounded;            // Whether or not the player is grounded.
        private Animator m_Anim;            // Reference to the player's animator component.
        private Rigidbody2D m_Rigidbody2D;
        private bool m_FacingRight = true;  // For determining which way the player is currently facing.

        private bool m_DoubleJumpUsed = false;
        private bool m_Invincible = false;

        public bool m_IsStunned;

        private void Awake()
        {
            // Setting up references.
            m_GroundCheck = transform.Find("GroundCheck");
            m_Anim = GetComponent<Animator>();
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void ResetAll()
        {
            m_Rigidbody2D.velocity = Vector2.zero;
            m_Anim.SetFloat("vSpeed", 0);
            m_Anim.SetFloat("Speed", 0);
            WalkDustFX.Stop();

            m_IsStunned = false;
            m_Invincible = false;
            m_Anim.SetBool("Stun", false);
            StunFX.SetActive(false);
        }

        private void FixedUpdate()
        {
            m_Grounded = false;

            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            // This can be done using layers instead but Sample Assets will not overwrite your project settings.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {
                    m_Grounded = true;
                    m_DoubleJumpUsed = false;
                }
            }

            m_Anim.SetBool("Ground", m_Grounded);

            // Set the vertical animation
            m_Anim.SetFloat("vSpeed", m_Rigidbody2D.velocity.y);
        }

        public void Move(float move, bool jump)
        {
            if (m_Grounded && (move != 0))
                WalkDustFX.Play();
            else
                WalkDustFX.Stop();

            //only control the player if grounded or airControl is turned on
            if (m_Grounded || m_AirControl)
            {
                // The Speed animator parameter is set to the absolute value of the horizontal input.
                m_Anim.SetFloat("Speed", Mathf.Abs(move));

                // Move the character
                if (m_IsStunned)
                    m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
                else
                    m_Rigidbody2D.velocity = new Vector2(move*m_MaxSpeed, m_Rigidbody2D.velocity.y);

                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && m_FacingRight)
                {
                    // ... flip the player.
                    Flip();
                }
            }
            
            // If the player should jump...
            if ((m_Grounded || (GetComponent<Platformer2DUserControl>().CanDoubleJump && !m_DoubleJumpUsed) || GetComponent<Platformer2DUserControl>().CanInfiniteJump) && jump && !m_IsStunned)
            {
                m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0);

                // if the player was grounded
                if (m_Grounded)
                {
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
                }
                // if the player wasn't grounded and can fly
                else if (GetComponent<Platformer2DUserControl>().CanInfiniteJump)
                {
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_FlyJumpForce));
                }
                // then it was a double jump
                else
                {
                    m_DoubleJumpUsed = true;
                    m_Rigidbody2D.AddForce(new Vector2(0f, m_DoubleJumpForce));
                }

                Instantiate(JumpDustFX, transform.position, Quaternion.identity);

                // Add a vertical force to the player.
                m_Grounded = false;
                m_Anim.SetBool("Ground", m_Grounded);
            }
        }

        private void Flip()
        {
            // Switch the way the player is labelled as facing. 
            m_FacingRight = !m_FacingRight;

            // Multiply the player's x local scale by -1.
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }

        public void StartSlowAndImmuneFor(float amount, float timer)
        {
            if (!m_Invincible)
                StartCoroutine(SlowedAndImmuneFor(amount, timer));
        }

        IEnumerator SlowedAndImmuneFor(float amount, float timer)
        {
            m_MaxSpeed *= amount;
            m_Invincible = true;
            gameObject.GetComponent<SpriteRenderer>().color = Color.red;
            yield return new WaitForSeconds(timer);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            m_MaxSpeed /= amount;
            m_Invincible = false;
        }

        public void SetStunState(bool enable)
        {
            if(enable && !m_Invincible)
            {
                m_Invincible = true;
                m_IsStunned = true;
                GetComponent<Platformer2DUserControl>().IsInputEnabled = false;
                m_Anim.SetBool("Stun", true);
                StunFX.SetActive(true);
            }
            else
            {
                m_Anim.SetBool("Stun", false);
                m_IsStunned = false;
                m_Invincible = false;
                GetComponent<Platformer2DUserControl>().IsInputEnabled = true;
                StunFX.SetActive(false);
            }
        }

        public void DisableCACAnim()
        {
            GetComponent<Animator>().SetBool("CAC", false);
        }

        public void DisablePowerAnim()
        {
            GetComponent<Animator>().SetBool("Power", false);
        }

        public void DisableShootAnim()
        {
            GetComponent<Animator>().SetBool("Shoot", false);
        }

        public void SpawnShowckwavePowerFX()
        {
            GameObject o = Instantiate(ShockwavePowerFX, ShockWavePowerPosition.position, Quaternion.identity);
            o.transform.parent = transform;
        }

        public void LateUpdate()
        {
            if (fond)
                fond.transform.position = new Vector3(transform.position.x, transform.position.y, fond.transform.position.z);
        }

        public void SpawnConfettis()
        {
            Instantiate(ConfettiFX, ConfettiFXPosition.position, Quaternion.identity);
        }
    }
}
