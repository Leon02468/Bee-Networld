using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeManager : MonoBehaviour
{
    [SerializeField] private GameObject beePrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform[] beeTargetPositions = new Transform[3];
    [SerializeField] private string[] addressPool;

    private Queue<string> beeIPQueue = new Queue<string>();
    private List<BeeController> activeBees = new List<BeeController>();
    private int currentBeeIndex = 0;
    private float delaySpawn = 2.5f;

    public BeeController CurrentBee => (activeBees.Count > 0 && currentBeeIndex >= 0) ? activeBees[currentBeeIndex] : null;

    public void InitBeeWave(string[] ipList)
    {
        beeIPQueue.Clear();
        activeBees.Clear();
        currentBeeIndex = 0;

        foreach (string ip in ipList)
        {
            beeIPQueue.Enqueue(ip);
        }
    }

    private void SpawnBee(int slotIndex)
    {
        if (beeIPQueue.Count == 0) return;
        
        string newIP = beeIPQueue.Dequeue();
        GameObject beeObj = Instantiate(beePrefab, spawnPoint.position, Quaternion.identity);
        BeeController bee = beeObj.GetComponent<BeeController>();
        bee.Setup(newIP, beeTargetPositions[slotIndex]);

        activeBees.Insert(slotIndex, bee);

        SetActiveBee(currentBeeIndex);
    }

    public void StartBeeSequence()
    {
        StartCoroutine(SpawnBeeOneByOne());
    }

    private IEnumerator SpawnBeeOneByOne()
    {
        int beesToSpawn = Mathf.Min(3, beeIPQueue.Count);
        for (int i = 0; i < beesToSpawn; i++)
        {
            SpawnBee(i);
            yield return new WaitForSeconds(delaySpawn);
        }

        if(activeBees.Count > 0)
            SetActiveBee(0);
    }

    public void SetActiveBee(int index)
    {
        if (index < 0 || index >= activeBees.Count) return;

        for (int i = 0; i < activeBees.Count; i++)
        {
            activeBees[i].isActiveBee = false;
            activeBees[i].SetVisualActive(false);
        }
        
        currentBeeIndex = index;
        activeBees[index].isActiveBee = true;
        activeBees[index].SetVisualActive(true);
    }

    public void CycleBee()
    {
        if (activeBees.Count == 0) return;
        int next = (currentBeeIndex + 1) % activeBees.Count;
        SetActiveBee(next);
    }

    public void RemoveAndReplaceCurrentBee()
    {
        if (CurrentBee == null) return;
        
        int slotIndex = currentBeeIndex;
        CurrentBee.FlyAway();
        activeBees.RemoveAt(slotIndex);

        //fill the gap
        if (beeIPQueue.Count > 0)
        {
            StartCoroutine(SpawnBeeDelayed(slotIndex));

            //fix selection
            if (activeBees.Count > 0)
                SetActiveBee(currentBeeIndex % activeBees.Count);
        }
        else
        {
            Debug.Log("No more bees to spawn.");
            if (activeBees.Count > 0)
                SetActiveBee(currentBeeIndex % activeBees.Count);
            else
                currentBeeIndex = -1;
        }
        
    }

    private IEnumerator SpawnBeeDelayed(int slotIndex)
    {
        yield return new WaitForSeconds(delaySpawn);
        SpawnBee(slotIndex);
    }
}
