using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FruitType { Apple, Banana, Cherry, Kiwi, Melon, Orange, Pineapple, Strawberry }

public class Fruit : MonoBehaviour
{
    GameManager gameManager;
    Animator anim;

    [SerializeField]
    FruitType type;
    [SerializeField]
    GameObject pickupVFX;

    protected virtual void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()    // Don't assign in Awake() as it might get called before GameManager's Awake()
    {
        gameManager = GameManager.instance;
        if (GameManager.instance.GetRandomizeFruits()) SetRandomLook();
        else SetLook((int)type);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            AudioManager.instance.PlaySfx(8);

            gameManager.FruitCollected();
            Destroy(gameObject);
            Instantiate(pickupVFX, transform.position, Quaternion.identity);
        }
    }

    void SetRandomLook()
    {
        int fruitIndex = Random.Range(0, 8);
        SetLook(fruitIndex);
    }

    void SetLook(int fruitIndex)
    {
        anim.SetFloat("fruitIndex", fruitIndex);
    }
}
