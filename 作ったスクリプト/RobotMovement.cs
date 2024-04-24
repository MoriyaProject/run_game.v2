using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


//�I�u�W�F�N�g��NavMeshAgent�R���|�[�l���g��ݒu
[RequireComponent(typeof(NavMeshAgent))]

public class RobotMovement : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;

    public   Vector3[]     points;
    private  int           destPoint = 0;
    private  NavMeshAgent  agent;

    private  GameObject    player;
    private  float         distance;

    [SerializeField] float trackingRange = 3f;
    [SerializeField] float quitRange     = 5f;
    [SerializeField] float robotMoveSpeed = 3f;
    [SerializeField] bool  tracking      = false;

    [SerializeField] GameObject bikkuriBall;
    [SerializeField] GameObject bikkuriPoal;

    private AudioSource robotAS;
    [SerializeField] AudioClip alart;

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();

        robotAS = this.gameObject.GetComponent<AudioSource>();

        // autoBraking �𖳌��ɂ���ƁA�ڕW�n�_�̊Ԃ��p���I�Ɉړ�����
        //�G�[�W�F���g�͖ڕW�n�_�ɋ߂Â��Ă����x�𗎂Ƃ��Ȃ�
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false;

        robotAS = this.GetComponent<AudioSource>();

        GotoNextPoint();

    }

    void GotoNextPoint()
    {
        // �n�_���Ȃɂ��ݒ肳��Ă��Ȃ��Ƃ��ɕԂ�
        if (points.Length == 0)
            return;

        //�G�[�W�F���g�����ݐݒ肳�ꂽ�ڕW�n�_�ɍs���悤�ɐݒ�
        agent.destination = points[destPoint];

        // �z����̎��̈ʒu��ڕW�n�_�ɐݒ肵�A�K�v�Ȃ�Ώo���n�_�ɖ߂�
        destPoint = (destPoint + 1) % points.Length;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Player�Ƃ��̃I�u�W�F�N�g�̋����𑪂�
        distance = Vector3.Distance(this.gameObject.transform.position, gM.playerPos);
        
        if (tracking == true)
        {
            //�ǐՂ̎��AquitRange��苗�������ꂽ�璆�~
            if (distance > quitRange)
            {
                tracking = false;
                gM.isForcused = false;
            }

            //Player��ڕW�Ƃ���
            agent.destination = gM.playerPos;

            //bikkuri��\��
            bikkuriBall.GetComponent<MeshRenderer>().enabled = true;
            bikkuriPoal.GetComponent<MeshRenderer>().enabled = true;

            //����炷
            if (robotAS.isPlaying == false)
            {
                robotAS.PlayOneShot(alart);
            }

        }
        else
        {
            //����������
            //bikkuri������
            bikkuriBall.GetComponent<MeshRenderer>().enabled = false;
            bikkuriPoal.GetComponent<MeshRenderer>().enabled = false;
            //�����~�߂�
            robotAS.Stop();

            // �G�[�W�F���g�����ڕW�n�_�ɋ߂Â��Ă�����A���̖ڕW�n�_��I��
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                GotoNextPoint();
            }
               
        }

    }

    void Update()
    {
        //�S�[�������瓮�����~�߂�
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
        //�ڐG�����I�u�W�F�N�g�̃^�O��"Player"�̂Ƃ�
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player����");
            //Player���G�ꂽ��ǐՊJ�n
            tracking = true;
            gM.isForcused = true;

        }
    }
}
