using UnityEngine;

public class SkinManager : MonoBehaviour
{
    public static SkinManager instance;
    
    public int selectedSkinIndex;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
