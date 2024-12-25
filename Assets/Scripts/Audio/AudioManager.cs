using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource[] sfx;
    [SerializeField] AudioSource[] music;

    int bgmIndex;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        InvokeRepeating(nameof(PlayMusicIfNeeded), 0, 3);
    }

    void PlayMusicIfNeeded()
    {
        if (music[bgmIndex].isPlaying) return;

        PlayRandomBgm();
    }

    public void PlayRandomBgm()
    {
        bgmIndex = Random.Range(0, music.Length);
        PlayBgm(bgmIndex);
    }

    public void PlayBgm(int bgmToPlay)
    {
        foreach (AudioSource bgm in music)
        {
            bgm.Stop();
        }

        bgmIndex = bgmToPlay;

        music[bgmToPlay].Play();
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
