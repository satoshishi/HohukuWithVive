using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

/// <summary>
/// マットのキャリブレーションをするスクリプト
/// </summary>
public class MatCalibration : MonoBehaviour
{
    #region keys

    public const string POSITION_Z = "POSITION_Z";
    public const string SIZE_X = "SIZE_X";
    public const string SIZE_Z = "SIZE_Z";
    public const string DEPTH = "DEPTH";

    #endregion

    public GameObject test;
    [SerializeField]
    private MeshRenderer mat_mesh;

   // private Vector3 Hand_Calibration

    // Use this for initialization
    void Start()
    {
        // LoadCalibrationData();
      //  ResetCalibration();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.UpArrow))
            test.transform.position = new Vector3(test.transform.position.x, test.transform.position.y, test.transform.position.z+1);
        if (Input.GetKeyUp(KeyCode.DownArrow))
            test.transform.position = new Vector3(test.transform.position.x, test.transform.position.y, test.transform.position.z-1);

        //  StartCoroutine(PlayCalibration());
    }

    IEnumerator PlayCalibration()
    {
        mat_mesh.enabled = true;

        Debug.Log("幅を設定します");

        while (!Input.GetKeyUp(KeyCode.Return))
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
                transform.localScale = new Vector3(transform.localScale.x + 0.1f, transform.localScale.y, transform.localScale.z);
            if (Input.GetKeyUp(KeyCode.DownArrow))
                transform.localScale = new Vector3(transform.localScale.x - 0.1f, transform.localScale.y, transform.localScale.z);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Debug.Log("高さを設定します");

        while (!Input.GetKeyUp(KeyCode.Return))
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z + 0.1f);
           
            if (Input.GetKeyUp(KeyCode.DownArrow))
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, transform.localScale.z - 0.1f);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);

        Debug.Log("深さを設定します");

        while (!Input.GetKeyUp(KeyCode.Return))
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y + 0.1f, transform.localScale.z);
            if (Input.GetKeyUp(KeyCode.DownArrow))
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y - 0.1f, transform.localScale.z);
            yield return null;
        }

        SaveCalibrationData();
        Debug.Log("キャリブレーション完了!");
    }

    /// <summary>
    /// 初期状態でキャリブレーションする
    /// </summary>
    public void ResetCalibration()
    {
        transform.position = Vector3.zero;
        transform.localScale = new Vector3(1f, 1f, 1f);
        SaveCalibrationData();
    }

    public void SaveCalibrationData()
    {
        PlayerPrefs.SetFloat(POSITION_Z, transform.position.z);
        PlayerPrefs.SetFloat(SIZE_X, transform.localScale.x);
        PlayerPrefs.SetFloat(SIZE_Z, transform.localScale.z);
        PlayerPrefs.SetFloat(DEPTH, transform.localScale.y);
    }

    /// <summary>
    /// 前回キャリブレーションしたサイズでキャリブレーションする
    /// </summary>
    public void  LoadCalibrationData()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, GetCalibrationData(POSITION_Z));
        transform.localScale = new Vector3(GetCalibrationData(SIZE_X), GetCalibrationData(DEPTH), GetCalibrationData(SIZE_Z));
    }

    public float GetCalibrationData(string key)
    {
        return PlayerPrefs.GetFloat(key, -1);
    }
}
