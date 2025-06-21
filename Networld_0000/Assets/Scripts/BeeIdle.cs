using UnityEngine;

public class BeeIdle : MonoBehaviour
{
    [SerializeField] private float horizontalSpeed = 1f;
    [SerializeField] private float verticalAmplitude = 0.5f;
    [SerializeField] private float verticalFrequency = 2f;
    [SerializeField] private float horizontalRange = 4f;
    [SerializeField] private Transform visualRoot; // Assign your sprite child here
    [SerializeField] private Vector3[] possibleOffsets; // Set in Inspector for random positions

    private Vector3 startPosition;
    private Animator animator;
    private bool facingLeft;
    private float lastXOffset = 0f;
    private float timeOffset;

    void Start()
    {
        // Randomize starting position from possibleOffsets, or use current position if none set
        if (possibleOffsets != null && possibleOffsets.Length > 0)
            startPosition = transform.position + possibleOffsets[Random.Range(0, possibleOffsets.Length)];
        else
            startPosition = transform.position;

        if (visualRoot == null && transform.childCount > 0)
            visualRoot = transform.GetChild(0);

        if (visualRoot != null)
            animator = visualRoot.GetComponent<Animator>();

        // Randomize initial facing direction
        facingLeft = Random.value > 0.5f;
        if (animator != null)
            animator.SetBool("FacingLeft", facingLeft);

        // Give each bee a unique time offset so their movement is not synchronized
        timeOffset = Random.Range(0f, 1000f);

        // Set initial xOffset and lastXOffset based on direction
        float pingPong = facingLeft ? horizontalRange : 0f;
        float xOffset = pingPong - (horizontalRange / 2f);
        lastXOffset = xOffset;

        // Place bee at the correct initial position
        float yOffset = Mathf.Sin(timeOffset * verticalFrequency) * verticalAmplitude;
        transform.position = startPosition + new Vector3(xOffset, yOffset, 0f);
    }

    void Update()
    {
        float time = Time.time + timeOffset;
        float pingPong = Mathf.PingPong(time * horizontalSpeed, horizontalRange);
        float xOffset = pingPong - (horizontalRange / 2f);
        float yOffset = Mathf.Sin(time * verticalFrequency) * verticalAmplitude;

        transform.position = startPosition + new Vector3(xOffset, yOffset, 0f);

        if (animator != null)
        {
            // Detect direction change
            if (xOffset < lastXOffset && !facingLeft)
            {
                facingLeft = true;
                animator.SetBool("FacingLeft", true);
            }
            else if (xOffset > lastXOffset && facingLeft)
            {
                facingLeft = false;
                animator.SetBool("FacingLeft", false);
            }
        }

        lastXOffset = xOffset;
    }
}