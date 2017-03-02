﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 匍匐前進の手の動きを判定するスクリプト
/// </summary>
public class HohukuController : MonoBehaviour
{
    public enum HandState
    {
        IDLE,
        GROUND,
        HOHUKU
    }

    public enum HandType
    {
        RIGHT,
        LEFT,
    }

    [SerializeField]
    private HandState state;
    [SerializeField]
    private bool IsAdmit=false;
    [SerializeField]
    private HandType type;

    public Transform hand;
    public Transform mat;
    public MatCalibration calibration;

    /// <summary>
    /// マットと手の距離(深さ)
    /// </summary>
    private float depth;
    public float DepthDistance
    {
        set { depth = value; }
        get { return (depth - mat.position.y); }
    }

    /// <summary>
    /// 手が接地した初期位置
    /// </summary>
    private float start_pos;
    public float StartPos
    {
        private set { start_pos = value; }
        get { return start_pos; }
    }

    /// <summary>
    /// 現在の手の引き位置
    /// </summary>
    private float now_pull_hand_pos;
    public float NowPullHandPos
    {
        private set { now_pull_hand_pos = value; }
        get { return now_pull_hand_pos + (calibration.PullAreaMin - StartPos); }
    }

    private float before_pull_area;
    public float BeforePullArea
    {
        set { before_pull_area = value;}
        get { return before_pull_area; }
    }

    /// <summary>
    /// マットに手が接地しているかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsGround()
    {
        return DepthDistance <= (type==HandType.RIGHT ? calibration.RightHandDepth : calibration.LeftHandDepth);
    }

    /// <summary>
    /// 現在の手の位置(0.0~1.0)
    /// calibrationされた範囲内で手の接地した位置からの値
    /// </summary>
    /// <returns></returns>
    public float PullArea()
    {
        /*  Debug.Log("nowpull : " + NowPullHandPos +
              " min : " + calibration.PullAreaMin +
              " nax : " + calibration.PullAreaMax);*/
        return Mathf.Ceil((NowPullHandPos - calibration.PullAreaMin) / (calibration.PullAreaMax - calibration.PullAreaMin)*10f)/10f;
    }

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        DepthDistance = hand.position.y;

        if(IsAdmit)
        UpdateHandLogic();
	}

    public void UpdateHandLogic()
    {
        switch(state)
        {
            case HandState.IDLE:
                if (IsGround()) state = HandState.GROUND;
                break;
            case HandState.GROUND:
                StartPos = transform.position.z;
                state = HandState.HOHUKU;
                break;
            case HandState.HOHUKU:
                if (!IsGround())
                    state = HandState.IDLE;
                NowPullHandPos = transform.position.z;

                if (before_pull_area < PullArea())
                    Debug.Log("移動");

                before_pull_area = PullArea();
                break;
        }
    }
}