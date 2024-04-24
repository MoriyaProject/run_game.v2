using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiftMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    public bool isRide    = false;
    private int destPoint = 0;
    [SerializeField] Vector3[]  points;
    [SerializeField] float      riftMoveSpeed = 3f;


    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();

        GotoNextPoint();
    }

    void GotoNextPoint()
    {
        // 地点がなにも設定されていないときに返す
        if (points.Length == 0)
            return;

        // 配列内の次の位置を目標地点に設定し、必要ならば出発地点に戻る
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //目標に向かって進む
        Vector3 vec = (points[destPoint] - transform.position).normalized;
        transform.position += vec * riftMoveSpeed * Time.deltaTime;

        //プレイヤーが乗ったら加速度をGMに送る
        if (isRide == true)
        {
            gM.riftVec = vec * riftMoveSpeed;
        }

        //目標が近くなったら、目標を変える
        if (Vector3.Distance(this.gameObject.transform.position, points[destPoint]) < 0.1f)
        {
            transform.position = points[destPoint];
            GotoNextPoint();
        }
    }
}
