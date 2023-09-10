using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveItem : MonoBehaviour
{
    
    public event EventHandler OnScrambleAction;
    public event EventHandler OnReturnToOrigin;

    [SerializeField] private InputAction scrambleAction;
    [SerializeField] private InputAction returnToOriginAction;
    private bool canInput = true;
    private float animationDuration = 5f;
    private float inputDisableTime = 0f;

    private void Awake()
    {
        scrambleAction.Enable();
        returnToOriginAction.Enable();
    }

    private void Update()
    {
        if (!canInput)
        {
            float elapsedTime = Time.time - inputDisableTime;

            if (elapsedTime >= animationDuration)
            {
                print("Press a key to scramble or return to original place");
                canInput = true;
            }
            return;
        }

        if (scrambleAction.IsPressed())
        {
            OnScrambleAction?.Invoke(this, EventArgs.Empty);
            DisableInput();
        }

        if (returnToOriginAction.IsPressed())
        {
            OnReturnToOrigin?.Invoke(this, EventArgs.Empty);
            DisableInput();
        }
    }
    private void DisableInput()
    {
        canInput = false;
        inputDisableTime = Time.time;
    }
}