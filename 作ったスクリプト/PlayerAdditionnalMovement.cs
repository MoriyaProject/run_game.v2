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
        
        //プレイヤーが下に落ちたらゼロ地点に戻す
        if (this.gameObject.transform.position.y < -5)
        {
            this.gameObject.transform.position = new Vector3(0, 0, 0);
        }

        //リフトに乗ったらリフトの加速度を受け取る
        if(isRift == true)
        {
            transform.position += gM.riftVec * Time.deltaTime;
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        //リフトに乗ったらそのリフトに知らせる
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Playerが乗った");
            isRift = true;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = true;

        }
    }

    private void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Rift"))
        {
            Debug.Log("Playerが降りた");
            isRift = false;
            other.transform.parent.gameObject.GetComponent<RiftMovement>().isRide = false;
        }
    }


}
