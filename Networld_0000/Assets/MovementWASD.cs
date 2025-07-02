using UnityEngine;
using UnityEngine.UI;

public class WASDKeyVisualizer : MonoBehaviour
{
    [System.Serializable]
    public class KeyImages
    {
        public Image unpressed;
        public Image pressed;
    }

    public KeyImages wKey;
    public KeyImages aKey;
    public KeyImages sKey;
    public KeyImages dKey;
    public KeyImages eKey;
    public KeyImages spaceKey;
    public KeyImages enterKey;


    void Update()
    {
        UpdateKey(wKey, KeyCode.W);
        UpdateKey(aKey, KeyCode.A);
        UpdateKey(sKey, KeyCode.S);
        UpdateKey(dKey, KeyCode.D);
        UpdateKey(eKey, KeyCode.E);
        UpdateKey(spaceKey, KeyCode.Space);
        UpdateKey(enterKey, KeyCode.Return);

    }

    void UpdateKey(KeyImages key, KeyCode code)
    {
        bool isPressed = Input.GetKey(code);
        if (key.unpressed != null) key.unpressed.enabled = !isPressed;
        if (key.pressed != null) key.pressed.enabled = isPressed;
    }
}