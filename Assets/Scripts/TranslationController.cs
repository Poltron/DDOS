using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationController : MonoBehaviour 
{
    public float    translationSpeed = 0f;
    public Vector2  translationVector = Vector2.zero;

    private Vector2 m_startPosition;
    private bool m_goToTarget = true;

    private void Awake()
    {
        m_startPosition = transform.position;
    }

	private void Update() 
    {
        if(true == m_goToTarget)
        {
            GoToTarget();
        }
        else
        {
            GoToStart();
        }
	}

    private void GoToTarget()
    {
        Vector2 endPosition = m_startPosition + translationVector;
        Vector2 fromMeToTarget = (endPosition - (Vector2)transform.position);
        Vector2 fromStartToTarget = (endPosition - m_startPosition);

        float dot = Vector2.Dot(fromMeToTarget, fromStartToTarget);

        if (dot <= 0)
        {
            m_goToTarget = false;
        }


        transform.Translate(fromMeToTarget.normalized * translationSpeed * Time.deltaTime, Space.World);
    }

    private void GoToStart()
    {
        Vector2 endPosition = m_startPosition + translationVector;
        Vector2 fromMeToStart = (m_startPosition - (Vector2)transform.position);
        Vector2 fromStartToTarget = (endPosition - m_startPosition);

        float dot = Vector2.Dot(fromMeToStart, fromStartToTarget);

        if (dot >= 0)
        {
            m_goToTarget = true;
        }


        transform.Translate(fromMeToStart.normalized * translationSpeed * Time.deltaTime, Space.World);
    }
}
