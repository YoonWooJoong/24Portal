using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    [SerializeField] List<AudioClip> bgms = new List<AudioClip>();
    [SerializeField] List<AudioClip> sfxs = new List<AudioClip>();

    [SerializeField] AudioSource audioBGM;
    [SerializeField] AudioSource audioSfx;
    private float bgmSoundScale;
    public float BgmSoundScale
    {
        get { return bgmSoundScale; }
        set { bgmSoundScale = Mathf.Clamp01(value); }
    }

    private float sfxSoundScale =1;
    public float SfxSoundScale
    {
        get { return sfxSoundScale; }
        set {sfxSoundScale = Mathf.Clamp01(value); }
    }

    /// <summary>
    /// 초기화
    /// </summary>
    public void Init()
    {
        bgms.Add(Resources.Load("Sound/24Portal_BGM", typeof(AudioClip)) as AudioClip);
        sfxs.Add(Resources.Load("Sound/ShootSFX", typeof(AudioClip)) as AudioClip);
        sfxs.Add(Resources.Load("Sound/FindPlayerSFX", typeof(AudioClip)) as AudioClip);
        sfxs.Add(Resources.Load("Sound/HitSFX", typeof(AudioClip)) as AudioClip);
        sfxs.Add(Resources.Load("Sound/achievement", typeof(AudioClip)) as AudioClip);
        sfxs.Add(Resources.Load("Sound/Glass", typeof(AudioClip)) as AudioClip);


        audioBGM = new GameObject("BGM").AddComponent<AudioSource>();
        audioSfx = new GameObject("SFX").AddComponent<AudioSource>();
        audioBGM.transform.SetParent(transform);
        audioSfx.transform.SetParent(transform);
        this.gameObject.AddComponent<AudioListener>();
    }
    /// <summary>
    /// 배경음악 재생
    /// </summary>
    /// <param name="index"></param>
    public void PlayBGM(int index)
    {
        audioBGM.clip = bgms[index];
        audioBGM.Play();
    }

    /// <summary>
    /// 배경음악 멈춤
    /// </summary>
    public void StopBGM()
    {
        audioBGM.Stop();
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    /// <param name="index"></param>
    public void PlaySFX(int index)
    {
        audioSfx.clip = sfxs[index];
        audioSfx.PlayOneShot(sfxs[index], SfxSoundScale);
    }

    /// <summary>
    /// 배경음악 소리조절
    /// </summary>
    /// <param name="sliderValue"></param>
    public void BGMVolumeControll(float sliderValue)
    {
        BgmSoundScale = sliderValue;
        audioBGM.volume = BgmSoundScale;
    }

    /// <summary>
    /// 효과음 소리 조절
    /// </summary>
    /// <param name="sliderValue"></param>
    public void SFXVolumeControll(float sliderValue)
    {
        SfxSoundScale = sliderValue;
    }
}
