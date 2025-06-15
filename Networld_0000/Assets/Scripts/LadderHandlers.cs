using UnityEngine;

public class LadderHandlers : MonoBehaviour
{
    private float width;
    private float height;
    private float ladderX;
    private Transform topHandler;
    private Transform bottomHandler;

    void Awake()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        height = GetComponent<SpriteRenderer>().bounds.size.y;
        ladderX = transform.position.x;
        topHandler = transform.GetChild(0).transform;
        bottomHandler = transform.GetChild(1).transform;

        topHandler.position = new Vector3(transform.position.x, transform.position.y + (float)(height / 2 + 0.5), 0);
        bottomHandler.position = new Vector3(transform.position.x, transform.position.y - (height / 2), 0);
        GetComponent<BoxCollider2D>().offset = Vector2.zero;
        GetComponent<BoxCollider2D>().size = new Vector2(width, height);
    }

    public float getCenterX()
    {
        return ladderX ;
    }
    public float GetTopY()
    {
        return topHandler.position.y;
    }
    public float GetBottomY()
    {
        return bottomHandler.position.y;
    }
}
