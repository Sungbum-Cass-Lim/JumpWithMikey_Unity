using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundObj : MonoBehaviour
{
    public AudioSource audioSource;
    public Action stopCallBack;

    [SerializeField]
    private AudioClip clip;
    private float audioLength;
    private bool loop;

    public void Init(AudioClip clip, bool isLoop = false)
    {
        this.clip = clip;
        audioLength = Time.realtimeSinceStartup + clip.length;
        loop = isLoop;
        stopCallBack = Stop;
    }

    public void Play(float minVolum = 0.0f, float maxVolum = 0.5f)
    {
        audioSource.clip = clip;
        audioSource.loop = loop;
        audioSource.volume = SoundMgr.isMute == true ? minVolum : maxVolum;

        audioSource.Play();
    }

    public void Stop()
    {
        SoundMgr.Instance.StopFx(audioSource);

        audioSource.Stop();
        ObjectPoolMgr.Instance.ReleasePool(gameObject);
    }

    //TODO: ���߿� Update���� ����ȭ ������ �������� ���� �ʿ�
    private void Update()
    {
        if (loop == false && audioLength <= Time.realtimeSinceStartup)
            stopCallBack.Invoke();
    }
}
