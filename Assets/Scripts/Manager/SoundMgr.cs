using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using OSSC;

public enum BGMType
{
    bgm_jumpmikey_intro = 0,
    bgm_jumpmikey_ingame,
}

public enum SFXType
{
    coin = 0,
    enemy_die,
    getcha,
    hero_jump,
    gameover,
}

public class SoundMgr : SingletonComponentBase<SoundMgr>
{
    public static bool muteBackup = false;

    public bool audioMute { private set; get; }

    private SoundController soundController;

    private ISoundCue playBgmSound = null;
    private List<ISoundCue> playSfxSoundList = new();

    protected override void InitializeSingleton()
    {
        soundController = FindObjectOfType<SoundController>();
    }
    public override void ResetSingleton() { }

    public void PlaySFX(SFXType sfxType)
    {
        if (audioMute)
            return;

        PlaySoundSettings settings = new PlaySoundSettings();
        settings.Init();

        settings.name = sfxType.ToString();
        var sfxSound = soundController.Play(settings);

        playSfxSoundList.Add(sfxSound);
    }

    public void PlaySFXLoop(SFXType sfxType)
    {
        PlaySoundSettings settings = new PlaySoundSettings();
        settings.Init();

        settings.name = sfxType.ToString();
        settings.isLooped = true;
        var sfxLoopSound = soundController.Play(settings);

        playSfxSoundList.Add(sfxLoopSound);
    }

    public void PlayBGM(BGMType bgmType)
    {
        if (playBgmSound != null && playBgmSound.IsPlaying == true)
        {
            playBgmSound.Stop();
            playBgmSound = null;
        }

        PlaySoundSettings settings = new PlaySoundSettings();

        settings.Init();
        settings.name = bgmType.ToString();
        settings.categoryName = "BGM"; // search only in that category
        settings.parent = transform; // Use this parent to put the AudioSource's position here.
        settings.isLooped = true;

        playBgmSound = soundController.Play(settings);
    }

    //public void  SetMute(bool isMute = true)
    //{

    //}

    public void SetAllMute(bool isMute = true)
    {
        audioMute = isMute;
        soundController.SetMuteAll(isMute);

        if (isMute)
        {
            soundController.StopAll();
        }
        else
        {
            if (GameMgr.Instance.gameState == GameState.Title)
            {
                PlayBGM(BGMType.bgm_jumpmikey_intro);
            }
            else if (GameMgr.Instance.gameState == GameState.Game)
            {
                PlayBGM(BGMType.bgm_jumpmikey_ingame);
            }
        }
    }
}
