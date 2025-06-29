using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI dialogText;
    public string[] lines;
    public float textSpeed;
    public Button continueButton; // Assign in Inspector
    public DialogMover dialogMover; // Assign in Inspector

    private int index;
    private Coroutine typingCoroutine;
    private bool isTyping;

    void Start()
    {
        dialogText.text = string.Empty;
        if (continueButton != null)
            continueButton.onClick.AddListener(OnContinueClicked);
        StartDialog();
    }

    // Call this to set the dialog box position (UI Canvas must use Screen Space)
    public void SetPosition(Vector2 position)
    {
        RectTransform rect = GetComponent<RectTransform>();
        if (rect != null)
            rect.anchoredPosition = position;
    }

    void StartDialog()
    {
        index = 0;
        ShowLine();
    }

    void ShowLine()
    {
        if (index < lines.Length)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);
            typingCoroutine = StartCoroutine(TypeLine(lines[index]));

            // Move the dialog panel to the corresponding destination
            if (dialogMover != null)
                dialogMover.MoveDialogToIndex(index);

            // Ensure the continue button is selected for highlight
            if (continueButton != null)
                EventSystem.current.SetSelectedGameObject(continueButton.gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogText.text = "";
        foreach (char letter in line)
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
    }

    void FinishCurrentLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        dialogText.text = lines[index];
        isTyping = false;
    }

    // Called by the continue button
    public void OnContinueClicked()
    {
        if (isTyping)
        {
            FinishCurrentLine();
        }
        else
        {
            NextLine();
        }
    }

    void NextLine()
    {
        index++;
        ShowLine();
    }
}