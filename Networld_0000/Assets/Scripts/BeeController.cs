using TMPro;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    public string targetIP;
    public bool isActiveBee = false;
    private TextMeshPro ipText;
    private Transform visualRoot;

    [SerializeField] private Vector3 activeScale = new Vector3(2f, 2f, 2f);
    [SerializeField] private Vector3 idleScale = new Vector3(1.4f, 1.4f, 1.4f);

    [SerializeField] private float enterSpeed = 2f;
    [SerializeField] private float exitSpeed = 5f;

    private Vector3 targetPosition;
    private bool entering = true;
    private bool exiting = false;

    void Awake()
    {
        visualRoot = transform.GetChild(1);
        ipText = GetComponentInChildren<TextMeshPro>();
    }
    void Update()
    {
        if (entering)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, enterSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                entering = false;
        }
        else if (exiting)
        {
            transform.Translate(Vector3.right * exitSpeed * Time.deltaTime);
        }
    }

    public void SetVisualActive(bool isActive)
    {
        visualRoot.localScale = isActive ? activeScale : idleScale;
        if(ipText != null)
            ipText.gameObject.SetActive(isActive);
    }

    public void Setup(string ip, Transform targetPos)
    {
        targetIP = ip;
        ipText.text = ip;
        targetPosition = targetPos.position;
        entering = true;
        exiting = false;
    }

    public void FlyAway()
    {
        exiting = true;
    }
}
