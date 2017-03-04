using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 触覚刺激を操作するスクリプト
/// </summary>
public class StimulusController : MonoBehaviour
{
    /// <summary>
    /// チャンネル
    /// </summary>
    public enum Stimulus_Ch
    {
        FIVE = 0,
        SIX = 1,
        SEVEN = 2,
        EIGHT = 3
    };

    public enum Stimulus_Type
    {
        STIMULUS,
        HERATBEAT
    }

    [SerializeField]
    private List<AudioSource> StimulusSource;
    [SerializeField]
    private List<AudioSource> HeartbeatSource;

    // Use this for initialization
    void Start()
    {
        Init();
    }

    public void Init()
    {
        StopAll(Stimulus_Type.HERATBEAT);
        StopAll(Stimulus_Type.STIMULUS);
    }

    /// <summary>
    /// 刺激開始
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ch"></param>
    public void Play(Stimulus_Type type, Stimulus_Ch ch)
    {
        if (type == Stimulus_Type.STIMULUS)
            StimulusSource[(int)type].Play();
        else HeartbeatSource[(int)type].Play();
    }

    /// <summary>
    /// 刺激停止
    /// </summary>
    /// <param name="type"></param>
    /// <param name="ch"></param>
    public void Stop(Stimulus_Type type, Stimulus_Ch ch)
    {
        if (type == Stimulus_Type.STIMULUS)
        {
            StimulusSource[(int)ch].Stop();
            StimulusSource[(int)ch].pitch = 0f;
            StimulusSource[(int)ch].volume = 0f;
        }
        else
        {
            HeartbeatSource[(int)ch].Stop();
            HeartbeatSource[(int)ch].pitch = 0f;
            HeartbeatSource[(int)ch].volume = 0f;
        }
    }

    public void StopAll(Stimulus_Type type = Stimulus_Type.STIMULUS)
    {

        for (int i = 0; i <= (int)Stimulus_Ch.EIGHT; i++)
        {
            if (type == Stimulus_Type.STIMULUS)
                Stop(Stimulus_Type.STIMULUS, (Stimulus_Ch)i);
            else Stop(Stimulus_Type.HERATBEAT, (Stimulus_Ch)i);
        }

    }

    /// <summary>
    /// 移動量に応じて刺激の強さを調整する
    /// 移動量が0~0.5で腹を刺激(0.25でmax,0.5でmin)
    /// 移動量が0.25=0.75で足を刺激(0.5でmax,0.75でmin)
    /// </summary>
    /// <param name="pitch"></param>
    public void UpdateStrength(float strength)
    {
        StopAllCoroutines();

        if (strength >= 0.75)
        {
            StimulusSource[(int)Stimulus_Ch.FIVE].volume = 0f;
            StimulusSource[(int)Stimulus_Ch.SIX].volume = 0f;
            StimulusSource[(int)Stimulus_Ch.SEVEN].volume = 0f;
            StimulusSource[(int)Stimulus_Ch.EIGHT].volume = 0f;
        }
        else if (strength >= 0.25)
        {
            if (!StimulusSource[(int)Stimulus_Ch.SIX].isPlaying)
            {
                Play(Stimulus_Type.STIMULUS, Stimulus_Ch.SIX);
                Play(Stimulus_Type.STIMULUS, Stimulus_Ch.EIGHT);
            }
            //移動量が0.75を超えたら腹の刺激はしない
            if (strength <= 0.5f)
            {
                //腹部刺激弱める
                StimulusSource[(int)Stimulus_Ch.FIVE].volume = 2.0f - (strength / 0.25f);
                StimulusSource[(int)Stimulus_Ch.SEVEN].volume = 2.0f - (strength / 0.25f);
                //脚部刺激強める
                StimulusSource[(int)Stimulus_Ch.SIX].volume = ((strength - 0.25f) / 0.25f);
                StimulusSource[(int)Stimulus_Ch.EIGHT].volume = ((strength - 0.25f) / 0.25f);
            }
            else
            {
                //脚部刺激弱める
                StimulusSource[(int)Stimulus_Ch.SIX].volume = 2.0f - ((strength - 0.25f) / 0.25f);
                StimulusSource[(int)Stimulus_Ch.EIGHT].volume = 2.0f - ((strength - 0.25f) / 0.25f);
            }
        }
        else
        {
            if (!StimulusSource[(int)Stimulus_Ch.FIVE].isPlaying)
            {
                Play(Stimulus_Type.STIMULUS, Stimulus_Ch.FIVE);
                Play(Stimulus_Type.STIMULUS, Stimulus_Ch.SEVEN);
            }

            StimulusSource[(int)Stimulus_Ch.FIVE].volume = (strength / 0.25f);
            StimulusSource[(int)Stimulus_Ch.SEVEN].volume = (strength / 0.25f);
        }
        /*    Debug.Log("腹部 " + StimulusSource[(int)Stimulus_Ch.FIVE].volume +
                " 脚部 " + StimulusSource[(int)Stimulus_Ch.SIX].volume);*/

        StartCoroutine(StopEvent());

    }

    public IEnumerator StopEvent()
    {
        int frame = 0;

        while (frame <= 8)
        {
            frame++;
            yield return null;
        }
        StopAll();
    }
}
