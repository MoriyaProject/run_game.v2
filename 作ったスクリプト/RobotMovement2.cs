using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMovement2 : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    [SerializeField] Vector3 pointA;
    [SerializeField] Vector3 pointB;
    [SerializeField] bool    isZMove;
    private bool flag;
    private GameObject player;
    Vector3 playerdestination;
    [SerializeField] float robotMoveSpeed = 5f;
    [SerializeField] bool  tracking       = false;

    private AudioSource robotAS;
    [SerializeField] AudioClip alart;

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
        robotAS = this.gameObject.GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //isZMoveが有効なら移動をZ方向のみに絞る、そうでないならX方向のみ
        if (isZMove == true)
        {
            playerdestination = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, gM.playerPos.z);
        }
        else
        {
            playerdestination = new Vector3(gM.playerPos.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
        
        //プレイヤーが触れたら
        if(tracking == true)
        {
            //プレイヤーを追跡
            transform.position = Vector3.MoveTowards(transform.position, playerdestination, robotMoveSpeed * Time.deltaTime);
            //音を鳴らす
            if(robotAS.isPlaying == false)
            {
                robotAS.PlayOneShot(alart);
            }
            
        }
        else
        {
            //目標地点を交互に移動
            if (flag == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointB, robotMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, pointA, robotMoveSpeed * Time.deltaTime);
            }
        }

        //プレイヤーが遠くなったら追跡終了
        if(Vector3.Distance(this.gameObject.transform.position, playerdestination) > 0.5f)
        {
            tracking = false;
            gM.isForcused = false;
            robotAS.Stop();
        }

        //移動限界に来たら追跡終了
        if (Vector3.Distance(this.gameObject.transform.position,pointB) < 0.2f)
        {
            flag = false;
            tracking = false;
            gM.isForcused = false;
            robotAS.Stop();
        }
        if (Vector3.Distance(this.gameObject.transform.position, pointA) < 0.2f)
        {
            flag = true;
            tracking = false;
            gM.isForcused = false;
            robotAS.Stop();
        }

    }

    void Update()
    {
        //ゴールしたら止まる
        if (gM.isGoal == true)
        {
            robotAS.Stop();
        }

        if (gM.isGameOver == true)
        {
            robotAS.Stop();
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player発見");
            //Playerが触れたら追跡開始
            tracking = true;
            gM.isForcused = true;

        }
    }

}



