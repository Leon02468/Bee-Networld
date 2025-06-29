using UnityEngine;

public class DialogMover : MonoBehaviour
{
    public Vector2[] destinations; // Set multiple positions in Inspector

    private Dialog dialog;

    void Awake()
    {
        dialog = GetComponent<Dialog>();
    }

    void Start()
    {
        // Optionally move to the first destination at start
        if (dialog != null && destinations != null && destinations.Length > 0)
        {
            dialog.SetPosition(destinations[0]);
        }
    }

    // Move dialog to a specific destination by index
    public void MoveDialogToIndex(int index)
    {
        if (dialog != null && destinations != null && index >= 0 && index < destinations.Length)
        {
            dialog.SetPosition(destinations[index]);
        }
    }

    // Move dialog to a specific position directly
    public void MoveDialog(Vector2 newPosition)
    {
        if (dialog != null)
        {
            dialog.SetPosition(newPosition);
        }
    }
}