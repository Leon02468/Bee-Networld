using UnityEngine;

public class HoneyBit : MonoBehaviour
{
    public bool isActive = false;

    void Start()
    {
        UpdateSprite();
    }

    public void Toggle()
    {
        isActive = !isActive;
        UpdateSprite();
    }

    //public void SetState(bool state)
    //{
    //    isActive = state;
    //    UpdateSprite();
    //}

    public int GetBitValue()
    {
        return isActive ? 1 : 0;
    }

    private void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().color = isActive ? Color.white : Color.grey;
    }
}
