using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using System.Linq;
using UnityEngine.EventSystems;

public class Dialogue : MonoBehaviour
{

    public TextMeshProUGUI textComponent;

    public TextMeshProUGUI nameText;
    public IconSelector icon;
    public GameObject serveButton;
    public List<string> lines = new List<string>();
    public float textSpeed;
    private bool typing;

    private int index;
    private string gameState;

    public event System.Action<Dialogue> OnDialogueEnded;
    [SerializeField] private GameInput gameInput;


    // Start is called before the first frame update
    void Start()
    {
        textComponent.text = string.Empty;
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.E))
        {
            if (ClickIsOn(serveButton)) return;

            if (lines.Count > 0)
            {
                //if line written, move to next line in list, else skip ahead and complete line
                if (!typing)
                {
                    NextLine();
                }
                else
                {
                    StopAllCoroutines();
                    typing = false;
                    textComponent.text = lines[index];
                    lines.RemoveAt(0);
                }
            }
            else
            {
                OnDialogueEnded?.Invoke(this);
            }

        }
    }
    
    public void clearLines()
    {
        lines.Clear();
        StopAllCoroutines();
    }
    public void SetState(string state)
    {
        gameState = state;
    }

    public void AddText(String newText)
    {
        lines.Add(newText);
    }

    public void SetSprite(String sprite, String name)
    {
        icon.Sprite(sprite);
        nameText.text = name;
    }

    public void showCop()
    {
        icon.showCop();
    }

    public void StartDialogue()
    {
        OnDialogueEnded = null;

        gameObject.SetActive(true);
        if (gameState == "first order")
        {
            serveButton.SetActive(false);
        }
        if (gameState == "waiting")
        {
            serveButton.SetActive(true);
        }
        if (gameState == "served")
        {
            serveButton.SetActive(false);
        }
        textComponent.text = string.Empty;
        index = 0;
        StartCoroutine(TypeLine());
        
    }

    IEnumerator TypeLine()
    {
        typing = true;
        textComponent.text = string.Empty;
        yield return null;
        foreach (char c in (lines[index].ToCharArray()))
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        lines.RemoveAt(0);
        typing = false;
    }

    void NextLine()
    {
        textComponent.text = string.Empty;
        StartCoroutine(TypeLine());
    }
    
    private bool ClickIsOn(GameObject target)
    {
        if (target == null || EventSystem.current == null) return false;

        var ped = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(ped, results);

        for (int i = 0; i < results.Count; i++)
        {
            var go = results[i].gameObject;
            if (go == target || go.transform.IsChildOf(target.transform))
                return true;
        }
        return false;
    }
}
