using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

/** Author: Nick Zimanski
*   Version: 10/24/22
*
*/
public class PromptController : MonoBehaviour
{
    private TextMeshProUGUI _promptText;
    private static event Action OnInteract;
    private Managers.InputManager _im;
    private string _currentInteractAxis = "";

    void Start()
    {
        OnInteract = () => { };
        _im = GameManager.Instance.Get<Managers.InputManager>();
        _promptText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_currentInteractAxis == "") return;

        if (_im.GetButtonDown(_currentInteractAxis)) OnInteract();
    }

    public void AssignInteractable(string axis, Action callback)
    {
        OnInteract += callback;
        _currentInteractAxis = axis;
        UpdateText("Press " + axis + " to interact");

        UpdateText("Press ENTER to interact");

    }

    public void UnassignInteractable()
    {
        OnInteract = () => { };
        _currentInteractAxis = "";
        UpdateText("");
    }

    private void UpdateText(string str)
    {
        _promptText.text = str;
    }
}
