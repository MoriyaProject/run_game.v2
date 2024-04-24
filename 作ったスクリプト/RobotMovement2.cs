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
        //isZMove���L���Ȃ�ړ���Z�����݂̂ɍi��A�����łȂ��Ȃ�X�����̂�
        if (isZMove == true)
        {
            playerdestination = new Vector3(this.gameObject.transform.position.x, this.gameObject.transform.position.y, gM.playerPos.z);
        }
        else
        {
            playerdestination = new Vector3(gM.playerPos.x, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
        }
        
        //�v���C���[���G�ꂽ��
        if(tracking == true)
        {
            //�v���C���[��ǐ�
            transform.position = Vector3.MoveTowards(transform.position, playerdestination, robotMoveSpeed * Time.deltaTime);
            //����炷
            if(robotAS.isPlaying == false)
            {
                robotAS.PlayOneShot(alart);
            }
            
        }
        else
        {
            //�ڕW�n�_�����݂Ɉړ�
            if (flag == true)
            {
                transform.position = Vector3.MoveTowards(transform.position, pointB, robotMoveSpeed * Time.deltaTime);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, pointA, robotMoveSpeed * Time.deltaTime);
            }
        }

        //�v���C���[�������Ȃ�����ǐՏI��
        if(Vector3.Distance(this.gameObject.transform.position, playerdestination) > 0.5f)
        {
            tracking = false;
            gM.isForcused = false;
            robotAS.Stop();
        }

        //�ړ����E�ɗ�����ǐՏI��
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
        //�S�[��������~�܂�
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



