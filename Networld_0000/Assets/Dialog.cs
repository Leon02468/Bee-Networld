using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Text.RegularExpressions;

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

    // Hyphenation settings
    private const int MinWordLengthForHyphenation = 8; // Only hyphenate words this long or longer
    private const int HyphenationChunkSize = 5;        // Insert soft hyphen every N chars

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

            // Preprocess the line for hyphenation
            string processedLine = HyphenateLongWords(lines[index]);
            typingCoroutine = StartCoroutine(TypeLine(processedLine));

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

    // Hyphenate long words in a line using soft hyphens (\u00AD)
    private string HyphenateLongWords(string line)
    {
        // Regex: match words with MinWordLengthForHyphenation or more characters
        return Regex.Replace(line, @"\w{" + MinWordLengthForHyphenation + @",}", match =>
        {
            string word = match.Value;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int i = 0;
            while (i + HyphenationChunkSize < word.Length)
            {
                sb.Append(word.Substring(i, HyphenationChunkSize));
                sb.Append("\u00AD"); // soft hyphen
                i += HyphenationChunkSize;
            }
            sb.Append(word.Substring(i)); // append the rest
            return sb.ToString();
        });
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
        // Force layout rebuild after the line is complete
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogText.rectTransform);
        isTyping = false;
    }

    void FinishCurrentLine()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        // Preprocess for hyphenation here as well
        dialogText.text = HyphenateLongWords(lines[index]);
        LayoutRebuilder.ForceRebuildLayoutImmediate(dialogText.rectTransform);
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