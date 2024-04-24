using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAdditionnalMovement : MonoBehaviour
{
    GameObject      gMobj;
    MainGameManager gM;
    private bool    isRift;
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

    private void FixedUpdate()
    {
        gM.playerPos = this.gameObject.transform.position;
        
        //�v���C���[�����ɗ�������[���n�_�ɖ߂�
        if (this.gameObject.transform.position.y < -5)
        {
            this.gameObject.transform.position = new Vector3(0, 0, 0);
        }

        //���t�g�ɏ�����烊�t�g�̉����x���󂯎��
        if(isRift == true)
        {
            transform.position += gM.riftVec * Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //���t�g�ɏ�����炻�̃��t�g�ɒm�点��
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Player�������");
            isRift = true;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Player���~�肽");
            isRift = false;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = false;
        }
    }


}
