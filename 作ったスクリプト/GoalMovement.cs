using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;
    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {

        //接触したオブジェクトのタグが"Player"のときGMにゴールしたと伝える
        if (other.CompareTag("Player"))
        {
            Debug.Log("ゴールに触れた");
            gM.isGoal = true;
            gM.isPose = true;
            

        }
    }

}
