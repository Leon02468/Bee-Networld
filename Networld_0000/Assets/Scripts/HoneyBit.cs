using UnityEngine;

public class HoneyBit : MonoBehaviour
{
    public bool isActive = false;
    [SerializeField] private bool enableSfx = true;

    void Start()
    {
        UpdateSprite();
    }

    public void Toggle()
    {
        isActive = !isActive;
        UpdateSprite();
    }

    public int GetBitValue()
    {
        return isActive ? 1 : 0;
    }

    public void SetState(bool state)
    {
        if (isActive != state)
        {
            isActive = state;
            UpdateSprite();
        }
    }

    private void UpdateSprite()
    {
        GetComponent<SpriteRenderer>().color = isActive ? Color.white : Color.grey;

        if (enableSfx)
        {
            if (isActive)
                SoundEffectManager.Play("bitOn");
            else
                SoundEffectManager.Play("bitOff");
        }
    }
}