using System.Collections;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class DroppedFruit : Fruit
{
    SpriteRenderer sr;

    [SerializeField] float canPickupAfter = 1;
    [SerializeField] float blinkSpeed = 2;
    [SerializeField] float ttl = 10;
    [SerializeField] Vector2 velocity;

    bool canPickup = false;

    protected override void Awake()
    {
        base.Awake();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        base.Start();

        Destroy(gameObject, ttl);
        StartCoroutine(NotInteractable());
    }

    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;
    }

    IEnumerator NotInteractable()
    {
        float currTime = 0;

        while (currTime < canPickupAfter)
        {
            float pingPong = Mathf.PingPong(currTime * blinkSpeed, 1);
            sr.color = Color.Lerp(Color.white, Color.clear, pingPong);
            currTime += Time.deltaTime;
            yield return null;
        }
        canPickup = true;
        sr.color = Color.white;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (!canPickup) return;
        base.OnTriggerEnter2D(collision);
    }
}
