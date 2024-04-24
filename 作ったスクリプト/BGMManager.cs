using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    GameObject gMobj;
    MainGameManager gM;
    [SerializeField] private AudioSource BGMaudioSource;
    [Tooltip("0:タイトル　１：セレクト　２：１面　３：２面　４：３面")]
    [SerializeField] private AudioClip[] BGMlist;
    
    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
    }

    public void BGMPlay(int BGMnumber)
    {
        //GameManagerからどのSceneか受け取る
        BGMaudioSource.clip = BGMlist[BGMnumber];

        //最初からBGMを鳴らす
        BGMaudioSource.Play();
    }

    private void Update()
    {
        //ゴールしたら音を止める
        if(gM.isGoal == true)
        {
            BGMaudioSource.Stop();
        }


        if (gM.isGameOver == true)
        {
            BGMaudioSource.Stop();
        }
    }

}
