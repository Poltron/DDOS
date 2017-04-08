using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AUtomaticSwing : MonoBehaviour 
{
    [SerializeField]
    private TriggerDetection    m_triggerDetection = null;

    [SerializeField]
    private ProjectileLauncher  m_projectileLauncher = new ProjectileLauncher();

    [SerializeField]
    private List<Transform>     m_spawnpoints = new List<Transform>();

    [SerializeField]
    private List<Transform>     m_directionAnchors = new List<Transform>();

    private Timer               m_timer = new Timer();

    // Use this for initialization
    private void Start () 
    {
        m_timer.Reset();
	}

    // Update is called once per frame
    private void Update () 
    {


        if (false == HasEnoughtPoints())
        {
            return;
        }

        //GameObject target = null;

        m_timer.ElapseTime(Time.deltaTime);

        if(false == m_timer.HasElapsedAllTime())
        {
            return;
        }

        List<GameObject> targets = null;

        m_triggerDetection.GetGameObjects(out targets);

        if(0 < targets.Count)
        {
            for(int index = 0; index < m_spawnpoints.Count; ++index)
            {
                Vector3 direction = m_directionAnchors[index].position - m_spawnpoints[index].position;

                m_projectileLauncher.LaunchProjectile(m_spawnpoints[index].position, Vector2.zero, direction, 5, gameObject);
            }

            m_timer.SetTimer(m_projectileLauncher.cooldown);
        }

        /*
		if(TryGetNearestTarget(out target))
        {
            Vector2 direction = new Vector2();

            direction = target.transform.position - m_spawnPoint.position;

            m_projectileLauncher.LaunchProjectile(m_spawnPoint.position, Vector2.zero, direction, 5);

            m_timer.SetTimer(m_projectileLauncher.cooldown);
        }*/
    }

    private bool TryGetNearestTarget(out GameObject nearestGO)
    {
        List<GameObject> targets = null;

        m_triggerDetection.GetGameObjects(out targets);

        float shorterquaredLength = float.MaxValue;

        nearestGO = null;

        foreach (GameObject go in targets)
        {
            float                   currentSquaredLength = (go.transform.root.position - transform.root.position).sqrMagnitude;

            if(currentSquaredLength < shorterquaredLength)
            {
                nearestGO = go;
                shorterquaredLength = currentSquaredLength;
            }
        }

        return nearestGO != null;
    }

    private bool HasEnoughtPoints()
    {
        return m_spawnpoints.Count == m_directionAnchors.Count;
    }

    private void OnDrawGizmos()
    {
     /*   if (false == HasEnoughtPoints())
        {
            return;
        }

     //   DisplayDebug();
    */
    }

    private void DisplayDebug()
    {
        for (int index = 0; index < m_spawnpoints.Count; ++index)
        {
            if (m_directionAnchors[index] != null && m_spawnpoints[index] != null)
            {
                Vector3 direction = m_directionAnchors[index].position - m_spawnpoints[index].position;

                Debug.DrawRay(m_spawnpoints[index].position, direction, Color.red);
            }
        }
    }
}
