using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class UIMovement : MonoBehaviour
{

    GameObject gMobj;
    MainGameManager gM;
    SaveData data;

    [SerializeField] GameObject PoseCanvas;
    [SerializeField] GameObject GoalCanvas;
    [SerializeField] GameObject RecordCanvas;
    [SerializeField] GameObject GameOverCanvas;
    [SerializeField] GameObject ForcusedCanvus;
    [SerializeField] GameObject DarkImage;
    [SerializeField] TextMeshProUGUI TimeText;
    [SerializeField] TextMeshProUGUI ScoreText;
    [SerializeField] TextMeshProUGUI ClearTimeText;
    [SerializeField] TextMeshProUGUI HighScoreText;
    [SerializeField] Slider     HPMeter;
    [SerializeField] GameObject tNewRC;
    [SerializeField] GameObject sNewRC;
    [SerializeField] TextMeshProUGUI BestTimeText;
    [SerializeField] TextMeshProUGUI BestScoreText;
    [SerializeField] TextMeshProUGUI StageText;

    private RectTransform goalUITf;
    private Canvas poseCanvus;
    private Canvas recordCanvus;

    private float DarkAlpha = 1.0f;
    [SerializeField] private float darkAlphaSpeed = 0.001f;
    [SerializeField] private float goalDownSpeed = 10f;
    [SerializeField] GameObject[] disableUIList;

    // Start is called before the first frame update
    void Start()
    {
        gMobj = GameObject.Find("GameManager");
        gM = gMobj.GetComponent<MainGameManager>();
        //objectの取得
        goalUITf      =  GoalCanvas.GetComponent<RectTransform>();
        poseCanvus    =  PoseCanvas.GetComponent<Canvas>();
        recordCanvus  =  RecordCanvas.GetComponent<Canvas>();
        //HP表示は全快にしておく
        HPMeter.value = 1;
        

        //最初の明転
        gM.isFadeiIn = true;

        //セレクトシーンの時は余計なUIを消す
        if (SceneManager.GetActiveScene().name == "SelectScene")
        {
            foreach (GameObject disableObject in disableUIList)
            {
                Destroy(disableObject);
            }
        }

       
    }

    // Update is called once per frame
    void Update()
    {
        
        //画面明転処理
        if(gM.isFadeiIn == true)
        {
            DarkAlpha -= Time.unscaledDeltaTime * darkAlphaSpeed;
            DarkImage.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, DarkAlpha);
            if (DarkAlpha <= 0f)
            {
                DarkAlpha = 0f;
                gM.isFadeiIn = false;
            }
            
        }

        //画面暗転処理
        if (gM.isFadeOut == true)
        {
            DarkAlpha += Time.unscaledDeltaTime * darkAlphaSpeed;
            DarkImage.GetComponent<Image>().color = new Color(0.0f, 0.0f, 0.0f, DarkAlpha);
            if (DarkAlpha >= 1.0f)
            {
                DarkAlpha = 1.0f;
                gM.isFadeOut = false;
            }
            
        }

        //時間の表示
        //TimeText.SetText(gM.presentGameTime.ToString());
        TimeText.text = gM.presentGameTime.ToString();
        //スコア表示
        //ScoreText.SetText(gM.presentScore.ToString());
        ScoreText.text = gM.presentScore.ToString();

        //HP表示
        HPMeter.value = gM.charaHP/ gM.charaHP_MAX;

        //ポーズ画面処理
        if (gM.isPose == true && gM.isGoal != true)
        {
            poseCanvus.enabled = true;
        }
        else
        {
            poseCanvus.enabled = false;
        }

        //被発見処理
        if(gM.isForcused == true)
        {
            ForcusedCanvus.GetComponent<Canvas>().enabled = true;
        }
        else
        {
            ForcusedCanvus.GetComponent<Canvas>().enabled = false;
        }

        //ゴール処理
        if(gM.isGoal == true)
        {
            //ゴール画面を移動させる
            goalUITf.localPosition += new Vector3(0, goalDownSpeed, 0);
            if (goalUITf.localPosition.y >= 0)
            {
                goalDownSpeed = 0;
            }
            //クリア時のスコアを表示
            ClearTimeText.text = gM.presentGameTime.ToString();
            HighScoreText.text = gM.presentScore.ToString();

            //ベストスコアなら知らせる
            if(gM.isBestTime == true)
            {
                tNewRC.GetComponent<TextMeshProUGUI>().enabled = true;
            }

            if (gM.isBestScore == true)
            {
                sNewRC.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }

        //ゲームオーバー画面を出す
        if(gM.isGameOver == true)
        {
            GameOverCanvas.GetComponent<Canvas>().enabled = true;
        }

    }

    private void FixedUpdate()
    {
        //セレクトの時のスコア確認 
        Debug.Log(gM.isTransporting);
        data = gM.data;
        if (gM.isTransporting == true)
        {
            recordCanvus.enabled = true;
            switch (gM.selectingStage)
            {
                case 1:
                    StageText.text = "Stage 1";
                    BestTimeText.text = data.timeHighScore[0].ToString();
                    BestScoreText.text = data.scoreHighScore[0].ToString();
                    break;

                case 2:
                    StageText.text = "Stage 2";
                    BestTimeText.text = data.timeHighScore[1].ToString();
                    BestScoreText.text = data.scoreHighScore[1].ToString();
                    break;

                case 3:
                    StageText.text = "Stage 3";
                    BestTimeText.text = data.timeHighScore[2].ToString();
                    BestScoreText.text = data.scoreHighScore[2].ToString();
                    break;
            }

        }
        else
        {
            recordCanvus.enabled = false;
        }
    }
}
