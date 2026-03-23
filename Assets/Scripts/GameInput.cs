using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{

    public static GameInput instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler<int> OnInventoryAction;
    public event EventHandler OnOrderList;
    public event EventHandler OnThrowDrink;
    public event EventHandler OnTutorial;

    private PlayerInputActions playerInputActions;
    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;

        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Enable();
        playerInputActions.OrderList.Enable();
        playerInputActions.Tutorial.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.Inventory1.performed += Inventory1_performed;
        playerInputActions.Player.Inventory2.performed += Inventory2_performed;
        playerInputActions.Player.Inventory3.performed += Inventory3_performed;
        playerInputActions.Player.Inventory4.performed += Inventory4_performed;
        playerInputActions.Player.Inventory5.performed += Inventory5_performed;
        playerInputActions.OrderList.OrderList.performed += OrderList_performed;
        playerInputActions.Tutorial.Tutorial.performed += Tutorial_performed;
        playerInputActions.Player.Throw.performed += Throw_performed;
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    private void Inventory1_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Inventory_performed(1);
    }
    private void Inventory2_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Inventory_performed(2);
    }
    private void Inventory3_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Inventory_performed(3);
    }
    private void Inventory4_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Inventory_performed(4);
    }
    private void Inventory5_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Inventory_performed(5);
    }
    private void Inventory_performed(int i)
    {
        OnInventoryAction?.Invoke(this, i);
    }
    private void OrderList_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnOrderList?.Invoke(this, EventArgs.Empty);
    }
    private void Tutorial_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnTutorial?.Invoke(this, EventArgs.Empty);
    }

    private void Throw_performed(InputAction.CallbackContext obj)
    {
        OnThrowDrink?.Invoke(this, EventArgs.Empty);
    }

    public void LockInput(bool locked)
    {
        if (locked)
        {
            playerInputActions.Player.Disable();
            playerInputActions.Locked.Enable();
            playerInputActions.OrderList.Disable();
            playerInputActions.Tutorial.Disable();
        }
        else
        {
            playerInputActions.Player.Enable();
            playerInputActions.Locked.Disable();
            playerInputActions.OrderList.Enable();
            playerInputActions.Tutorial.Enable();
        }
    }

    public void LockInputWithReturn(bool locked)
    {
        if (locked)
        {
            playerInputActions.Player.Disable();
            playerInputActions.Locked.Enable();
            playerInputActions.OrderList.Enable();
            playerInputActions.Tutorial.Enable();
        }
        else
        {
            playerInputActions.Player.Enable();
            playerInputActions.Locked.Disable();
            playerInputActions.OrderList.Enable();
            playerInputActions.Tutorial.Enable();
        }
    }
    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 moveInput = playerInputActions.Player.Move.ReadValue<Vector2>();
        return moveInput;
    }
}
