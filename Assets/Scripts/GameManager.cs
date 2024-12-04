using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Player player;
    public int fruitsCollected = 0;
    public bool randomizeFruits = false;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void FruitCollected() => fruitsCollected++;

    public bool GetRandomizeFruits() => randomizeFruits;
}
