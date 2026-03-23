using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public static GameManager instance { get; private set; }
    public GameState currentState;

    //Money
    public static event Action<int> OnMoneyChanged;
    [SerializeField] int money;
    [SerializeField] int moneyWin;
    [SerializeField] private GameInput gameInput;
    private bool won;


    public int Money
    {
        get => money;
        set
        {
            money = value;
            OnMoneyChanged?.Invoke(money); // notify listeners (like UI)
        }
    }

    // === ORDER UI ===
    [Header("Order UI")]
    [SerializeField] private TextMeshProUGUI orderTextTemplate; // assign the disabled template
    [SerializeField] private Transform orderListContainer;      // assign the OrderListContainer transform
    [SerializeField] private Transform orderListHPair;      // assign the OrderListContainer transform
    [SerializeField] private OrderIconSetter orderIconTemplate;      // assign the OrderListContainer transform
    private Dictionary<Customer, TextMeshProUGUI> activeOrders = new Dictionary<Customer, TextMeshProUGUI>();
    private Dictionary<Customer, OrderIconSetter> activeIcons = new Dictionary<Customer, OrderIconSetter>();

    //WinUI
    [SerializeField] private GameObject winScreen;

    private void Awake()
    {
        if (instance) Destroy(this.gameObject);
        else instance = this;
    }

    private void Start()
    {
        StartCoroutine(begin());
    }
    private IEnumerator begin()
    {
        winScreen.SetActive(true);
        yield return new WaitForSeconds(3);
        winScreen.SetActive(false);
    }

    // --- Order UI management ---
    /// <summary>Call when a customer receives/places an order.</summary>
    public void AddOrderToUI(Customer customer)
    {
        if (customer == null) return;
        if (customer.GetOrder() == null) return;
        if (activeOrders.ContainsKey(customer)) return; // already shown

        // Instantiate a copy, enable it and set text
        Transform newHPair = Instantiate(orderListHPair, orderListContainer);

        OrderIconSetter icon = Instantiate(orderIconTemplate, newHPair);
        icon.gameObject.SetActive(true);

        TextMeshProUGUI text = Instantiate(orderTextTemplate, newHPair);
        text.gameObject.SetActive(true);
        if (customer.customerType != "cop")
        {
            icon.SetSprite(customer.customerType);
        }
        else
        {
            icon.SetSprite(customer.disguiseType);
        }
        // Use a clear display name: GameObject name (avoid ambiguous field names)
        string displayName = customer.gameObject.name;
        text.text = $"{displayName}: {customer.GetOrder().ToString()}";

        activeOrders[customer] = text;
        activeIcons[customer] = icon;
    }

    /// <summary>Call if order text needs to be refreshed (e.g., changed name/status).</summary>
    public void UpdateOrderUI(Customer customer)
    {
        if (customer == null) return;
        if (!activeOrders.ContainsKey(customer)) return;

        var text = activeOrders[customer];
        var icon = activeIcons[customer];
        text.text = $"{customer.gameObject.name}: {customer.GetOrder().ToString()}";
        if (customer.customerType != "cop")
        {
            icon.SetSprite(customer.customerType);
        }
        else
        {
            icon.SetSprite(customer.disguiseType);
        }
    }

    /// <summary>Call when the customer's order is completed / removed.</summary>
    public void RemoveOrderFromUI(Customer customer)
    {
        if (customer == null) return;
        if (activeOrders.TryGetValue(customer, out var text))
        {
            var row = text.transform.parent;
            if (row != null) Destroy(row.gameObject);
            activeOrders.Remove(customer);
        }
        activeIcons.Remove(customer);
        
    }



    public enum GameState
    {
        Start,
        MixDrinks,
        ServeCustomers,
        Win,
        Lose,
        Pause
    }

    // Update is called once per frame
    void Update()
    {
        if (!won)
        {
            if (money >= moneyWin){StateChanged(GameState.Win);}
        }
    }


    public void StateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Start:
                currentState = state;
                GameStart();
                break;
            case GameState.MixDrinks:
                currentState = state;
                MixDrinks();
                break;
            case GameState.ServeCustomers:
                currentState = state;
                ServeCustomers();
                break;
            case GameState.Win:
                currentState = state;
                Win();
                break;
            case GameState.Lose:
                currentState = state;
                Lose();
                break;
            case GameState.Pause:
                currentState = state;
                Pause();
                break;
        }
    }

    private void GameStart()
    {

    }
    private void MixDrinks()
    {

    }
    private void ServeCustomers()
    {

    }
    private void Win()
    {
        won = true;
        winScreen.SetActive(true);
        gameInput.LockInput(true);
    }
    private void Lose()
    {

    }
    private void Pause()
    {

    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public void SpendMoney(int amount)
    {
        Money -= amount;
    }
    
}


