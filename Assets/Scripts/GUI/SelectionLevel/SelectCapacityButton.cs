using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectCapacityButton : MonoBehaviour 
{
    public string   actionName;
    public float    actionCount;

    private bool    m_isEnabled = true;

    public bool IsEnabled
    {
    get { return m_isEnabled; }
    }

    public void AddAction()
    {
        GameManager gm = GameManager.Instance;

        if(true == m_isEnabled && null != gm)
        {
            if(gm.AddCapacity(actionName, actionCount))
            {
                m_isEnabled = false;
                GetComponent<Button>().interactable = false;
            }
        }
    }

}
