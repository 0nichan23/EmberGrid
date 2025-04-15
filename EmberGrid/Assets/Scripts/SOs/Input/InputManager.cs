using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{

    public UnityEvent RotRight;
    public UnityEvent RotLeft;


    private Player playerActions;


    public void Awake()
    {
        playerActions = new Player();
        playerActions.Enable();
        playerActions.BasicActions.RotRight.performed += InvokeRotRight;
        playerActions.BasicActions.RotLeft.performed += InvokeRotLeft;
    }


    public void InvokeRotRight(InputAction.CallbackContext obj)
    {
        RotRight?.Invoke();
    }
    public void InvokeRotLeft(InputAction.CallbackContext obj)
    {
        RotLeft?.Invoke();
    }


}
