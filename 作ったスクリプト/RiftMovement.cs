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
        // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ�
        if (points.Length == 0)
            return;

        // �z����̎��̈ʒu��ڕW�n�_�ɐݒ肵�A�K�v�Ȃ�Ώo���n�_�ɖ߂�
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //�ڕW�Ɍ������Đi��
        Vector3 vec = (points[destPoint] - transform.position).normalized;
        transform.position += vec * riftMoveSpeed * Time.deltaTime;

        //�v���C���[�������������x��GM�ɑ���
        if (isRide == true)
        {
            gM.riftVec = vec * riftMoveSpeed;
        }

        //�ڕW���߂��Ȃ�����A�ڕW��ς���
        if (Vector3.Distance(this.gameObject.transform.position, points[destPoint]) < 0.1f)
        {
            transform.position = points[destPoint];
            GotoNextPoint();
        }
    }
}
