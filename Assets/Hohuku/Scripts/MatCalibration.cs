using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// マットのキャリブレーションをするスクリプト
/// 手順
/// 1 対応するviveコンの割り当てを調整する.
/// 2 体験者にマットの中央に手を置いてもらいCキーでキャリブレーション
/// 3 キャリブレーションで設定するのは左右の手ごとの接地を判断するためのdepthと引手の範囲(viveコン ~ hmd)
/// 4 キャリブレーション後の数値は保存してリスタート時に反映
/// </summary>
public class MatCalibration : MonoBehaviour
{
    #region keys

    public const string RIGHT_HAND_DEPTH = "RIGHT_HAND_DEPTH";
    public const string LEFT_HAND_DEPTH = "LEFT_HAND_DEPTH";
    public const string PULL_AREA_MAX = "PULL_AREA_MAX";
    public const string PULL_AREA_MIN = "PULL_AREA_MIN";

    #endregion

    #region values

    private float right_hand_depth;
    public float RightHandDepth
    {
        private set
        {
            right_hand_depth = value;
            SaveCalibrationData(RIGHT_HAND_DEPTH, value);
        }
        get { return right_hand_depth; }
    }

    private float left_hand_depth;
    public float LeftHandDepth
    {
        private set
        {
            left_hand_depth = value;
            SaveCalibrationData(LEFT_HAND_DEPTH, value);
        }
        get { return left_hand_depth; }
    }

    private float pull_area_max;
    public float PullAreaMax
    {
        private set
        {
            pull_area_max = value;
            SaveCalibrationData(PULL_AREA_MAX, value);
        }
        get { return pull_area_max; }
    }

    private float pull_area_min;
    public float PullAreaMin
    {
        private set
        {
            pull_area_min = value;
            SaveCalibrationData(PULL_AREA_MIN, value);
        }
        get { return pull_area_min; }
    }

    #endregion

    [SerializeField]
    private MeshRenderer mat_mesh;
   /* [SerializeField]
    private Transform max_look_point;
    [SerializeField]
    private Transform min_look_point;*/

    public Transform hmd;
    public Transform right_hand;
    public Transform left_hand;

    // Use this for initialization
    void Start()
    {
        LoadAll();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.C))
            Calibration();


       // Debug.Log(GetNowDistance(right_hand));
      //  if (GetNowDistance(right_hand) <= RightHandDepth)
         //   Debug.Log("接地");
    }

    public void Calibration()
    {
        RightHandDepth = (right_hand.position.y - transform.position.y) *1.04f;
        LeftHandDepth = (left_hand.position.y - transform.position.y)* 1.04f;
        PullAreaMax = hmd.transform.localPosition.z;
        PullAreaMin = transform.localPosition.z;
    }

    public void SaveCalibrationData(string key, float value)
    {
        PlayerPrefs.SetFloat(key, value);
        Debug.Log("Save : " + key + " : " + value);
    }

    /// <summary>
    /// 前回キャリブレーションしたサイズでキャリブレーションする
    /// </summary>
    public float LoadCalibrationData(string key)
    {
        return PlayerPrefs.GetFloat(key, -1);
    }

    public void LoadAll()
    {
        RightHandDepth = LoadCalibrationData(RIGHT_HAND_DEPTH);
        LeftHandDepth = LoadCalibrationData(LEFT_HAND_DEPTH);
        PullAreaMax = LoadCalibrationData(PULL_AREA_MAX);
        PullAreaMin = LoadCalibrationData(PULL_AREA_MIN);
    }
}
