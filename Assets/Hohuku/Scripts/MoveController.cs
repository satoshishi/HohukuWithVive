using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    float distance = 0.0f;
    bool already_start;
    bool is_move;

    // Use this for initialization
    void Start()
    {

    }

    void Move(iTween.EaseType easing)
    {
        is_move = false;
        already_start = true;

        iTween.ValueTo(
gameObject, iTween.Hash(
    "from", 0f,
    "to", 0.01f,
    "time", 0f,
    "onupdate", "move",
    "oncomplete","complete",
    "easing" , easing)
            );
    }

    void add()
    {
        if (is_move)
        {
            Move(iTween.EaseType.linear);
        }
    }

    void complete()
    {
        already_start = false;
    }

    public void Move(float _distance)
    { 

        distance += _distance;
        float pathLength = iTween.PathLength(iTweenPath.GetPath("path"));
        float percent = distance / pathLength;
        if (percent < 0.0f) percent = 0.0f;
        if (percent > 1.0f) percent = 1.0f;
        iTween.PutOnPath(gameObject, iTweenPath.GetPath("path"), percent);
        // 少し先の位置(percent+0.01←この数値は任意)を取得(戦略1)
        Vector3 fpos = iTween.PointOnPath(iTweenPath.GetPath("path"), percent + 0.01f);
        // 少し先の位置を向かせる(戦略2)
        gameObject.transform.LookAt(fpos);

      //  add();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            Move(0.03f);
        }
    }
}
