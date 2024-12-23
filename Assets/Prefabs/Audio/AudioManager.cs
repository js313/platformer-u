using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    AudioSource[] sfx;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySfx(int sfxIndex, bool randomizePitch = true)
    {
        if (sfxIndex >= sfx.Length) return;

        if (randomizePitch)
            sfx[sfxIndex].pitch = Random.Range(0.9f, 1.1f);

        sfx[sfxIndex].Play();
    }

    public void StopSfx(int sfxIndex)
    {
        if (sfxIndex >= sfx.Length) return;

        sfx[sfxIndex].Stop();
    }
}
