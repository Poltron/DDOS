using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpening : MonoBehaviour 
{
    [SerializeField]
    private TriggerDetection    m_triggerDetection = null;

    

	// Update is called once per frame
	void Update () 
    {
        List<GameObject>    gos = null;

	    if(null != m_triggerDetection)
        {
            m_triggerDetection.GetGameObjects(out gos);

            //one player is inside the trigger
            if(0 != gos.Count)
            {
                OpenDoor();
            }
        }
	}

    private void OpenDoor()
    {
        GameObject.Destroy(transform.root.gameObject);
    }
}
