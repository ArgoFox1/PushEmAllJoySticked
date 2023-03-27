using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameManager gameManager;

    public int deadZombieCount;

    public List<GameObject> golds;
    public List<GameObject> zombies;

    private GameObject currentZombie;
    private GameObject currentGold;

    public List<GameObject> pooledGolds;
    public List<GameObject> pooledZombies;
    private void Start()
    {
        deadZombieCount = 0;
        for (int i = 0; i < zombies.Count; i++)
        {
            zombies[i].SetActive(true);
        }
    }
    private void Update()
    {
        if (pooledZombies.Count > 0)
        {
            for (int i = 0; i < pooledZombies.Count; i++)
            {
                pooledZombies[i].SetActive(false);
                currentZombie = pooledZombies[i];
                zombies.Add(currentZombie);
                pooledZombies.RemoveAt(i);
            }
        }       
        if(pooledGolds.Count > 0)
        {
            for (int i = 0; i < pooledGolds.Count; i++)
            {
                pooledGolds[i].SetActive(false);
                currentGold = pooledGolds[i];
                golds.Add(currentGold);
                pooledGolds.RemoveAt(i);
            }
        }
        if(deadZombieCount == zombies.Count)
        {
            SpawnGems();
            deadZombieCount = 0;
        }
        if(gameManager.goldCount == golds.Count)
        {
            gameManager.isFinal = true;
        }
    }
    private void SpawnGems()
    {
        for (int i = 0; i < golds.Count; i++)
        {
            golds[i].SetActive(true);
        }
    }  
}
