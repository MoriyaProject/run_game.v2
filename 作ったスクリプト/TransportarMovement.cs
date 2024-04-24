using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportarMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    private bool isPlayer;
    [SerializeField] private bool isSelect;

    [Tooltip("0:�Z���N�g 1:1�� 2:2�� 3:3��")]
    [SerializeField] private int selectStageNum;

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }

    //�v���C���[�ɐG���ꂽ�Ƃ��A�����ɐU��ꂽ�ԍ���GM�ɑ���
    private void OnTriggerEnter(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player�ɐG��Ă���");
            gM.isTransporting = true;
            gM.selectingStage = selectStageNum;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player�͗��ꂽ");
            gM.isTransporting = false;

        }
    }

}
