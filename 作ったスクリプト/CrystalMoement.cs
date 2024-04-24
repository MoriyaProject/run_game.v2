using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrystalMoement : MonoBehaviour
{
    GameObject CryOb;
    Transform  CryTf;
    [SerializeField] private bool isGold;

    GameObject gMobj;
    MainGameManager gM;

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
 
        //自身のobjectの取得
        CryOb = this.gameObject;
        CryTf = CryOb.transform;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //1秒にn度回転
        CryTf.Rotate(0f, 120.0f * Time.deltaTime, 0f);
    }

    void OnTriggerEnter(Collider other)
    {
        
        //接触したオブジェクトのタグが"Player"のとき
        if (other.CompareTag("Player"))
        {
            Debug.Log("クリスタルに触れた"); 

            //点数をGMに送って消える
            if(isGold == true)
            {
                gM.ScoreManager(1000,isGold);
                
            }
            else
            {
                gM.ScoreManager(100,isGold);
                
            }

            Destroy(this.gameObject);

        }
    }

}
