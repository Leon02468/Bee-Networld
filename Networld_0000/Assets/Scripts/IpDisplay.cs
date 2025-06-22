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

    public string GetCurrentIP()
    {
        return ipText.text;
    }

    public void SetInitialIP(string ip)
    {
        string[] initialOctets = ip.Split('.');
        for (int i = 0; i < 4; i++)
        {
            int value = int.Parse(initialOctets[i]);
            octets[i].SetBitsFromValue(value);
        }
    }
}
