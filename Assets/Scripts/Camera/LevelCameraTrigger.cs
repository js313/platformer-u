using UnityEngine;

public class LevelCameraTrigger : MonoBehaviour
{
    LevelCamera levelCamera;

    void Awake()
    {
        levelCamera = GetComponentInParent<LevelCamera>();
    }

    void Start()
    {
        levelCamera.EnableCamera(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            levelCamera.EnableCamera(true);
            levelCamera.SetNewTarget(collision.transform);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            levelCamera.EnableCamera(false);
        }
    }
}
