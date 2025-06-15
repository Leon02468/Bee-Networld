using TMPro;
using UnityEngine;

public class IpDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ipText;
    [SerializeField] private OctetManager[] octets = new OctetManager[4];

    private void Update()
    {
        string ip = $"{octets[0].GetDecimalValue()}.{octets[1].GetDecimalValue()}.{octets[2].GetDecimalValue()}.{octets[3].GetDecimalValue()}";
        ipText.text = ip;
    }
}
