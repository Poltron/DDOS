using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotAction : Action 
{
    [SerializeField]
    private ProjectileLauncher m_projectileLauncher = null;

  //  [SerializeField]
  //  private Transform           m_shotTransform = null;

    [SerializeField]
    private List<Transform>     m_shotTransforms = new List<Transform>();

    [SerializeField]
    private List<Transform>     m_shotTransformsEnd = new List<Transform>();


    public float m_deactivatyionTime = 0f;

   // [SerializeField]
   // [Range(0.5f, 100f)]
   // private float               m_shotMinDistance = 0f;
    private Rigidbody2D         m_rigidBody = null;

    private Timer               m_deactivationTimer = new Timer();

    private void Update()
    {
        m_deactivationTimer.ElapseTime(Time.deltaTime);
    }

    public override void Tick()
    {
        base.Tick();


        if (false == IsEnabled || false == IsInitialized())
            return;

        Gamepad gamepad = null;

        if (TryGetValidGamepad(out gamepad))
        {
            if (gamepad.GetButtonDown("B") && m_controller.IsInputEnabled && true == m_deactivationTimer.HasElapsedAllTime())
            {
                m_controller.GetComponent<Animator>().SetBool("Shoot", true);

                LaunchProjectiles();
                m_deactivationTimer.SetTimer(m_deactivatyionTime);
                //    //var gpLeftAxis = gamepad.GetStick_L();

                //    //Vector2 axis = new Vector2(gpLeftAxis.X, gpLeftAxis.Y);
                //    Vector2 orientation = new Vector2(0, 0);
                //Vector2 startPosition = m_shotTransform.position;
                //Vector2 currentVel = m_rigidBody.velocity;

                //orientation.x = m_controller.GetLookDirection();
                //orientation.Normalize();
                //startPosition += (orientation * m_shotMinDistance);

                //currentVel.y = 0;
                //currentVel.x = 0;



                //m_projectileLauncher.LaunchProjectile(startPosition, currentVel, orientation, 5, m_controller.gameObject);
                Debug.Log("SHOT FIRED");
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

        m_rigidBody = gameObject.transform.root.GetComponent<Rigidbody2D>();
    }

    private bool IsInitialized()
    {
        bool res = false;

        res = null != m_projectileLauncher;
   //     res = true == res ? null != m_shotTransform : false;
        res = true == res ? null != m_rigidBody : false;

        return res;
    }

    public void LaunchProjectiles()
    {
        //Vector2 orientation = new Vector2(0, 0);
        //Vector2 startPosition = m_shotTransform.position;
        //Vector2 currentVel = m_rigidBody.velocity;
        //
        //orientation.x = m_controller.GetLookDirection();
        //orientation.Normalize();
        //startPosition += (orientation * m_shotMinDistance);
        //
        //currentVel.y = 0;
        //currentVel.x = 0;


        Vector2 currentVel = m_rigidBody.velocity;


        if (m_shotTransforms.Count == m_shotTransformsEnd.Count)
        {
            for (int index = 0; index < m_shotTransforms.Count; ++index)
            {
                Vector2 direction = m_shotTransformsEnd[index].position - m_shotTransforms[index].position;
                Vector2 startPosition = m_shotTransforms[index].position;

                //direction.x *= m_controller.GetLookDirection();

                m_projectileLauncher.LaunchProjectile(startPosition, currentVel, direction, 5, m_controller.gameObject);

            }
        }
    }

    private void OnDrawGizmos()
    {
      /*  if(m_shotTransforms.Count == m_shotTransformsEnd.Count)
        {
            for (int index = 0; index < m_shotTransforms.Count; ++index)
            {
                Debug.DrawLine(m_shotTransforms[index].position, m_shotTransformsEnd[index].position, Color.red);
            }
        }
        */
    }
}
