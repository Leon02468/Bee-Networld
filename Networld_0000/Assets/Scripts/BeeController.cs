using TMPro;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    [SerializeField] private float enterSpeed = 3f;
    [SerializeField] private float exitSpeed = 5f;
    [SerializeField] private Vector3 enterTargetPosition;
    [SerializeField] private string ipAddress;
    [SerializeField] private TextMeshProUGUI ipText;

    private bool hasEntered = false;
    private bool isExiting = false;
}
