using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuteData
{
    public bool mute;
}

public enum SoundType
{
    bgm_jumpmikey_intro = 0,
    bgm_jumpmikey_ingame = 1,
    coin = 2,
    enemy_die = 3,
    getcha = 4,
    hero_jump = 5,
    gameover = 6,
}

public class SoundMgr : SingletonComponentBase<SoundMgr>
{
    public static bool isMute = false;

    public AudioClip defultBgm;
    public List<AudioClip> bgmClipList = new List<AudioClip>();
    public List<AudioClip> fxClipList = new List<AudioClip>();

    private Dictionary<string, AudioClip> bgmDictionay = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> fxDictionay = new Dictionary<string, AudioClip>();

    private Transform bgmContainer;
    private Transform fxContainer;

    private List<AudioSource> playSoundList = new List<AudioSource>();

    public SoundObj SoundComponent;

    protected override void InitializeSingleton() { }
    private void Awake()
    {
        BgmInit();
        FxInit();

        PlayDefultBgm();
    }

    public void BgmInit()
    {
        bgmContainer = new GameObject().transform;
        bgmContainer.name = "BgmContainer";
        bgmContainer.SetParent(transform);

        AudioSource bgm = bgmContainer.gameObject.AddComponent<AudioSource>();
        bgm.loop = true;
        bgm.volume = isMute == true ? 0.0f : 0.5f; ;

        foreach (var bgmClip in bgmClipList)
        {
            bgmDictionay.Add(bgmClip.name, bgmClip);
        }
    }
    public void FxInit()
    {
        fxContainer = new GameObject().transform;
        fxContainer.name = "FxContainer";
        fxContainer.SetParent(transform);

        foreach (var fxClip in fxClipList)
        {
            fxDictionay.Add(fxClip.name, fxClip);
        }
    }

    public void PlayDefultBgm()
    {
        AudioSource bgm = bgmContainer.GetComponent<AudioSource>();
        playSoundList.Add(bgm);

        bgm.clip = defultBgm;
        bgm.Play();
    }
    public void ChangeBgm(string bgmName)
    {
        if (bgmDictionay.TryGetValue(bgmName, out var clip))
        {
            AudioSource bgm = bgmContainer.GetComponent<AudioSource>();

            bgm.clip = clip;
            bgm.Play();
        }
    }
    public void ChangeBgm(SoundType bgmType)
    {
        string name = SoundTypeToName(bgmType);
        ChangeBgm(name);
    }

    public void PlayFx(string fxName, bool isLoop = false)
    {
        SoundObj fxObj = ObjectPoolMgr.Instance.Load<SoundObj>(PoolObjectType.Effect, "SoundComponent");
        fxObj.name = fxName;
        fxObj.transform.SetParent(fxContainer);

        playSoundList.Add(fxObj.audioSource);

        if (fxDictionay.TryGetValue(fxName, out var clip))
        {
            fxObj.Init(clip, isLoop);
            fxObj.Play();
        }
    }

    public void PlayFx(SoundType fxType, bool isLoop = false)
    {
        PlayFx(SoundTypeToName(fxType), isLoop);
    }

    public void PlayGameOver(SoundType fxType, bool isLoop = false)
    {
        SoundObj fxObj = ObjectPoolMgr.Instance.Load<SoundObj>(PoolObjectType.Effect, "SoundComponent");
        Debug.Log("GameOver");

        if (fxDictionay.TryGetValue(SoundTypeToName(fxType), out var clip))
        {
            fxObj.Init(clip, isLoop);
            fxObj.Play(0.5f);
        }
    }

    public void StopFx(AudioSource source)
    {
        if (!playSoundList.Contains(source))
            return;

        playSoundList.Remove(source);
    }

    public void SetMute(bool show)
    {
        isMute = show;

        if (show)
        {
            foreach (var audio in playSoundList)
            {
                audio.volume = 0.0f;
            }
        }
        else
        {
            foreach (var audio in playSoundList)
            {
                audio.volume = 0.5f;
            }
        }
    }

    public string SoundTypeToName(SoundType type)
    {
        switch (type)
        {
            case SoundType.bgm_jumpmikey_intro: return "bgm_jumpmikey_intro";
            case SoundType.bgm_jumpmikey_ingame: return "bgm_jumpmikey_ingame";
            case SoundType.coin: return "coin";
            case SoundType.enemy_die: return "enemy_die";
            case SoundType.getcha: return "getcha";
            case SoundType.hero_jump: return "hero_jump";
            case SoundType.gameover: return "gameover";
            default: return string.Empty;
        }
    }
}
