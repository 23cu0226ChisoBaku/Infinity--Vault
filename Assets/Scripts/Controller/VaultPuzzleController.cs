using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MLibrary;

public class VaultPuzzleController : MonoBehaviour
{
    private Button[] _buttons;
    private event System.Action _onFinish;
    private int _currentButtonIndex;

    private void Awake()
    {
        _currentButtonIndex = 0;

        var childrenButton = GetComponentsInChildren<Button>();
        
        // 子Buttonをシャッフルする
        childrenButton.Shuffle();

        _buttons = new Button[childrenButton.Length];

        for (int i = 0; i < _buttons.Length; ++i)
        {
            _buttons[i] = childrenButton[i];
            _buttons[i].onClick.AddListener(OnButtonClick);
            _buttons[i].gameObject.SetActive(false);

            var children = _buttons[i].GetComponentInChildren<TMP_Text>();
            if (children != null)
            {
                children.text = (i + 1).ToString();
            }
        }

        _buttons[0].gameObject.SetActive(true);
    }

    private void OnButtonClick()
    {
        if (_currentButtonIndex >= _buttons.Length - 1)
        {
            _onFinish?.Invoke();
            return;
        }

        _buttons[_currentButtonIndex].gameObject.SetActive(false);
        ++_currentButtonIndex;

        _buttons[_currentButtonIndex].gameObject.SetActive(true);
    }

    public void RegisterFinishEvent(Action finishEvent)
    {
        _onFinish += finishEvent;
    }

    public void UnregisterFinishEvent(Action finishEvent)
    {
        _onFinish -= finishEvent;
    }
}