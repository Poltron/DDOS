using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stack : MonoBehaviour
{
    [SerializeField]
    private List<Action> permanentActions = new List<Action>();
    
    [SerializeField]
    public List<Action> actions = new List<Action>();

    [SerializeField]
    public List<Action> possibleActions = new List<Action>();

    [SerializeField]
    public float stackMemoryThreshold = 100.0f;
    [SerializeField]
    private float stackTotalMemory = 0;

    public float actualMemory = 0;

    public delegate void StackCompositionUpdated();
    public StackCompositionUpdated OnStackCompositionUpdate = () => { };

    public delegate void StackMemoryUpdated();
    public StackMemoryUpdated OnStackMemoryUpdate = () => { };

    public float GetRemainningAvailableMemory()
    {
        return stackMemoryThreshold - stackTotalMemory;
    }

    public void ClearActionStack()
    {
        stackTotalMemory = 0f;
        actualMemory = stackMemoryThreshold;
        actions.Clear();
    }

    public bool TryGetPossibleAction(string actionName, ref Action outAction)
    {
        foreach (Action action in possibleActions)
        {
            Action currentAction = action.GetComponent<Action>();

            if (currentAction && currentAction.name == actionName)
            {
                outAction = currentAction;
                return true;
            }
        }

        Debug.LogError("The action " + actionName + " is not available in possible actions");

        return false;
    }

    private void Awake()
    {
        actualMemory = stackMemoryThreshold;
    }

    private void OnEnable()
    {
        foreach (Action permanentAction in permanentActions)
        {
            if (!permanentAction)
                continue;

            permanentAction.IsEnabled = true;
        }

        OnStackCompositionUpdate();
    }

    private void Start()
    {
        //CODE MOVED TO OnEnable()
      //  foreach (Action permanentAction in permanentActions)
      //  {
      //      if (!permanentAction)
      //          continue;
      //
      //      permanentAction.IsEnabled = true;
      //  }
      //
      //  OnStackCompositionUpdate();
    }

    public void AddAction(Action action)
    {
        if (stackTotalMemory + action.MemoryCost > stackMemoryThreshold)
            return;

        actions.Add(action);
        action.IsEnabled = true;

        stackTotalMemory += action.MemoryCost;

        OnStackCompositionUpdate();
    }

    void Update()
    {
        foreach (Action permanentAction in permanentActions)
        {
            if (!permanentAction)
                continue;

            if (permanentAction.IsEnabled)
                permanentAction.Tick();
        }

        foreach (Action action in actions)
        {
            if (!action)
                continue;

            if (action.IsEnabled && !action.isHacked)
                action.Tick();
        }
	}

    public void DamageMemory(float amount)
    {
        actualMemory -= amount;

        if (actualMemory < 0)
        {
            actualMemory = 0;
        }

        UpdateActionEnabled();
        OnStackMemoryUpdate();
    }

    public void HealMemory(float amount)
    {
        actualMemory += amount;

        if (actualMemory > stackMemoryThreshold)
        {
            actualMemory = stackMemoryThreshold;
        }

        UpdateActionEnabled();
        OnStackMemoryUpdate();
    }

    void UpdateActionEnabled()
    {
        float thresholdMin = 0;
        float thresholdMax = 0;

        foreach (Action action in actions)
        {
            thresholdMax = thresholdMin + action.MemoryCost;

            if (actualMemory < thresholdMax)
                action.IsEnabled = false;

            thresholdMin = thresholdMax;
        }
    }

    public void HackFirstModule(float time)
    {
        for (int i = actions.Count - 1; i >= 0; --i)
        {
            Debug.Log(actions[i].gameObject.name);

            // l'action est enabled
            if (actions[i].IsEnabled)
            {
                actions[i].HackedFor(time);
                OnStackMemoryUpdate();
                break;
            }
        }
    }
}
