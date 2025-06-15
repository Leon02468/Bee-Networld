using UnityEngine;

public class OctetManager : MonoBehaviour
{
    [SerializeField] private HoneyBit[] honeyBits = new HoneyBit[8];

    public int GetDecimalValue()
    {
        int decimalValue = 0;

        for (int i = 0; i < honeyBits.Length; i++)
        {
            int bitValue = honeyBits[i].GetBitValue();
            decimalValue += bitValue * (1 << i);
        }

        return decimalValue;
    }

    //public void SetBits(bool[] bitStates)
    //{
    //    for (int i = 0; i < 8 && i < bitStates.Length; i++)
    //    {
    //        honeyBits[i].SetState(bitStates[i]);
    //    }
    //}
}
