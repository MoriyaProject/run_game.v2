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

        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�GM�ɃS�[�������Ɠ`����
        if (other.CompareTag("Player"))
        {
            Debug.Log("�S�[���ɐG�ꂽ");
            gM.isGoal = true;
            gM.isPose = true;
            

        }
    }

}
