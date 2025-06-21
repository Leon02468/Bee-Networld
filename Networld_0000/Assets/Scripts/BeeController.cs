using TMPro;
using UnityEngine;

public class BeeController : MonoBehaviour
{
    public string targetIP;
    public bool isActiveBee = false;
    private TextMeshPro ipText;
    private Transform visualRoot;

    [SerializeField] private Vector3 activeScale;
    [SerializeField] private Vector3 idleScale;

    [SerializeField] private float enterSpeed;
    [SerializeField] private float exitSpeed;

    private Vector3 targetPosition;
    private bool entering = true;
    private bool exiting = false;
    private Animator animator;

    void Awake()
    {
        visualRoot = transform.GetChild(1);
        ipText = GetComponentInChildren<TextMeshPro>();

        // Unflip the sprite and ensure scale is positive
        //var spriteRenderer = visualRoot.GetComponent<SpriteRenderer>();
        //if (spriteRenderer != null)
        //    spriteRenderer.flipX = false;

        Vector3 scale = visualRoot.localScale;
        scale.x = Mathf.Abs(scale.x);
        visualRoot.localScale = scale;

        // Ensure Animator always faces right
        animator = visualRoot.GetComponent<Animator>();
        if (animator != null)
            animator.SetBool("FacingLeft", false);
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
        if (ipText != null)
            ipText.gameObject.SetActive(isActive);
    }

    public void Setup(string ip, Transform targetPos)
    {
        targetIP = ip;
        ipText.text = ip;
        targetPosition = targetPos.position;
        entering = true;
        exiting = false;

        // Ensure Animator always faces right on setup
        if (animator != null)
            animator.SetBool("FacingLeft", false);
    }

    public void FlyAway()
    {
        exiting = true;
    }
}
