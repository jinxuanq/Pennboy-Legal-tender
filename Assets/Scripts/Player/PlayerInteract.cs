using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract instance;

    private Vector2 lastInteractInput;
    [SerializeField] private GameInput gameInput;

    [SerializeField] private GameObject eButton;
    [SerializeField] private GameObject orderList;
    [SerializeField] private GameObject tutorial;

    private GlasswareCounter currentGlassware;
    public Customer currentCustomer;
    private bool isPreviousHit = false;
    private RaycastHit2D previousHit;

    public Drink selectedDrink;


    private void Awake()
    {
        if (instance) Destroy(gameObject);
        else instance = this;
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInventoryAction += GameInput_OnInventoryAction;
        gameInput.OnOrderList += GameInput_OnOrderList;
        gameInput.OnTutorial += GameInput_OnTutorial;
        previousHit = Physics2D.Raycast(transform.position, gameInput.GetMovementVectorNormalized(), 0);

        eButton.SetActive(false);
    }


    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (currentCustomer != null)
        {
            currentCustomer.Interact();
        }
        else if (currentGlassware != null)
        {
            currentGlassware.Interact();
        }
    }
    private void GameInput_OnInventoryAction(object sender, int i)
    {
        Inventory.instance.SelectIndex(i);
    }

    void GameInput_OnOrderList(object sender, System.EventArgs e)
    {
        if (orderList.activeSelf)
        {
            GameInput.instance.LockInputWithReturn(false);
            orderList.SetActive(false);
        }
        else
        {
            GameInput.instance.LockInputWithReturn(true);
            orderList.SetActive(true);
        }
    }

    void GameInput_OnTutorial(object sender, System.EventArgs e)
    {
        if (tutorial.activeSelf)
        {
            GameInput.instance.LockInputWithReturn(false);
            tutorial.SetActive(false);
        }
        else
        {
            GameInput.instance.LockInputWithReturn(true);
            tutorial.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        HandleDetection();
    }

    private void HandleDetection()
    {
        int layer_mask = LayerMask.GetMask("Interactable"); 

        Vector2 moveInput = gameInput.GetMovementVectorNormalized();

        if (moveInput != Vector2.zero)
        {
            lastInteractInput = moveInput;
        }
        float raycastDistance = 1f;
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, lastInteractInput, raycastDistance, layer_mask);
        if (raycastHit)
        {
            if (isPreviousHit)
            {
                if (raycastHit.collider != previousHit.collider)
                {
                    //Deselects the previous counter before new counter is assigned
                    DeSelect();
                }
            }
            previousHit = raycastHit;
            //Customer Interact
            if (raycastHit.transform.TryGetComponent(out Customer customer))
            {
                currentCustomer = customer;
                eButton.SetActive(true);
                isPreviousHit = true;
            }
            //Glassware Interact
            else if (raycastHit.transform.TryGetComponent(out GlasswareCounter glasswareCounter))
            {
                currentGlassware = glasswareCounter;
                currentGlassware.GlasswareHighlight(true);
                eButton.SetActive(true);
                isPreviousHit = true;
            }
            else
            {
                DeSelect();
            }
        }
        else
        {
            DeSelect();
        }
    }

    private void DeSelect()
    {
        if (currentGlassware != null)
        {
            currentGlassware.GlasswareHighlight(false);
            currentGlassware = null;
        }
            currentCustomer = null;
        eButton.SetActive(false);
        isPreviousHit = false;
    }

    private void Serve()
    {
        if (currentCustomer != null)
        {
            if(selectedDrink != null)
            {
                currentCustomer.CompleteOrder(selectedDrink);
                Destroy(selectedDrink.gameObject);
            }
        }
    }

}
