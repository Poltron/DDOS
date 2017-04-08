using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIButton : MonoBehaviour
{
    [SerializeField]
    protected PlayerGUI playerGUI;
    protected Button _button;

    void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(DoOnClick);
    }

    protected virtual void DoOnClick()
    {
    }
}
