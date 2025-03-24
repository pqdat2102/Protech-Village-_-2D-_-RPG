using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpSpawner : MonoBehaviour
{
    [SerializeField] private GameObject goldCoin, healthGlobe, staminaGlobe;

    [SerializeField] private float healthGlobeWeight = 5; 
    [SerializeField] private float staminaGlobeWeight = 15f; 
    [SerializeField] private float goldCoinWeight = 60f;     
    [SerializeField] private float nothingWeight = 20f;      

    public void DropItems()
    {
        float totalWeight = healthGlobeWeight + staminaGlobeWeight + goldCoinWeight + nothingWeight + 1;


        float randomValue = Random.Range(0f, totalWeight);

        // Quyết định vật phẩm nào sẽ xuất hiện dựa trên trọng số
        if (randomValue < healthGlobeWeight)
        {
            // Spawn healthGlobe
            Instantiate(healthGlobe, transform.position, Quaternion.identity);
        }
        else if (randomValue < healthGlobeWeight + staminaGlobeWeight)
        {
            // Spawn staminaGlobe
            Instantiate(staminaGlobe, transform.position, Quaternion.identity);
        }
        else if (randomValue < healthGlobeWeight + staminaGlobeWeight + goldCoinWeight)
        {
            // Spawn goldCoin (với số lượng ngẫu nhiên từ 1 đến 3)
            int randomAmountOfGold = Random.Range(1, 4);
            for (int i = 0; i < randomAmountOfGold; i++)
            {
                Instantiate(goldCoin, transform.position, Quaternion.identity);
            }
        }
    }
}