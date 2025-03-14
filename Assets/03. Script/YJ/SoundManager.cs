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

    private float sfxSoundScale;
    public float SfxSoundScale
    {
        get { return sfxSoundScale; }
        set {sfxSoundScale = Mathf.Clamp01(value); }
    }

    public void Init()
    {
        bgms.Add(Resources.Load("24Portal_BGM", typeof(AudioClip)) as AudioClip);
        audioBGM = new GameObject("BGM").AddComponent<AudioSource>();
        audioSfx = new GameObject("SFX").AddComponent<AudioSource>();
        audioBGM.transform.SetParent(transform);
        audioSfx.transform.SetParent(transform);
        this.gameObject.AddComponent<AudioListener>();
    }
    public void PlayBGM(int index)
    {
        audioBGM.clip = bgms[index];
        audioBGM.Play();
    }

    public void StopBGM()
    {
        audioBGM.Stop();
    }

    public void PlaySFX(int index)
    {
        audioSfx.PlayOneShot(sfxs[index], SfxSoundScale);
    }

    public void BGMVolumeControll(float sliderValue)
    {
        BgmSoundScale = sliderValue;
        audioBGM.volume = BgmSoundScale;
    }

    public void SFXVolumeControll(float sliderValue)
    {
        SfxSoundScale = sliderValue;
    }
}
