using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//保存するデータの型
[System.Serializable]
public class SaveData
{
    public float[] timeHighScore  = new float[3];
    public float[] scoreHighScore = new float[3];

}

public class MainGameManager : MonoBehaviour
{
    [SerializeField] private SEManager   SEm;
    [SerializeField] private BGMManager  BGMm;

    //save関連
    public SaveData data;
    public bool isBestTime = false;
    public bool isBestScore = false;

    SaveData MyLoad()
    {
        string datastr;

        if (PlayerPrefs.HasKey("PlayerData"))
        {
            //データがあれば読み込む
            datastr = PlayerPrefs.GetString("PlayerData");
            return JsonUtility.FromJson<SaveData>(datastr);
        }
        else
        {
            //データがなかったら新しく作る
            Debug.Log("new");
            SaveData newData = new SaveData();
            return newData;
        }

        
    }

    void MySave(SaveData myData)
    {
        //変更されたmydataをjsonにしてPlayerPrefsに上書き保存
        string datastr = JsonUtility.ToJson(myData);
        PlayerPrefs.SetString("PlayerData", datastr);
        PlayerPrefs.Save();
    }


    //scene関連
    public bool goalFlag = false;
    
    //Sceneを読み込む関数
        public void MainSceneManager(string roadSceneName)
        {
            isFadeOut = true;
            SEm.SEPlay(4);
            StartCoroutine(Scenechange(roadSceneName));
        }

        IEnumerator Scenechange(string sceneName)
        {
            //delay秒待つ
            yield return new WaitForSecondsRealtime(3.0f);
            /*処理*/
            SceneManager.LoadScene(sceneName);
        }

        //UI関連
        public bool isFadeiIn;
        public bool isFadeOut;

        //time関連
        public float presentGameTime = 0f;
        public bool isPose = false;


        //position関連
        public Vector3 playerPos;

        //HP関連
        public int    charaHP_MAX = 100;
        public float  charaHP     = 0;
        public bool   isForcused  = false;

        [SerializeField] private int   damageScale = 10;
        [SerializeField] private float healScale   = 0.5f;

        //score関連
        public int  presentScore = 0;
        
    //スコアの累積と獲得時のSEを鳴らす
        public void ScoreManager(int addingScore, bool isGold)
        {            
            {
                presentScore += addingScore;
            }
            
            if(isGold == true) {
                SEm.SEPlay(2);
            }
            else
            {
                SEm.SEPlay(1);
            }
            
        }

     //rift関連
    public Vector3 riftVec;

    //goal関連
    public bool isGoal      = false;
    private int issmallgoal = 1;

    //gameover関連
    public bool isGameOver;

    //transport関連
    public bool isTransporting;
    public int  selectingStage;

    private void Awake()
    {

        //ファイル読み込み
        data = MyLoad();

        //時間を進める
        Time.timeScale = 1.0f;
    }



    private void Start()
        {

        Debug.Log(JsonUtility.ToJson(data, true));

        //HPは開始時にMAX
        charaHP = charaHP_MAX;

        //BGMをシーンに合わせて変える
        switch (SceneManager.GetActiveScene().name)
        {
            case "TitleScene":
                BGMm.BGMPlay(0);
                break;

            case "SelectScene":
                BGMm.BGMPlay(1);
                break;

            case "Stage1":
                BGMm.BGMPlay(2);
                break;

            case "Stage2":
                BGMm.BGMPlay(3);
                break;

            case "Stage3":
                BGMm.BGMPlay(4);
                break;

            case "testscene":
                BGMm.BGMPlay(1);
                break;

            default:

                break;
        }



        }

        private void Update()
        {

        //HPが０になったらゲームオーバー
        if(charaHP <= 0f)
        {
            isGameOver = true;
            Time.timeScale = 0f;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                MainSceneManager("SelectScene");
                SEm.SEPlay(0);
            }
            
        }
        
            //ポーズ画面処理
            //Qが押されたらポーズ画面
            //ポーズ中にQが押されたらポーズ解除
            if (Input.GetKeyDown(KeyCode.Q)) {

                if(isPose == false)
                {
                    isPose = true;
                    SEm.SEPlay(3);
                    
                }
                else
                {
                    isPose = false;
                    Time.timeScale = 1.0f;
                }
                
            }

            //ポーズ中は時間停止
            //1が押されたら再開、２が押されたらセレクト画面へ、３が押されたらゲーム終了
            if (isPose == true)
            {
                Time.timeScale = 0f;

                if (Input.GetKeyDown(KeyCode.Alpha1)) { 
                    isPose = false;
                    SEm.SEPlay(0);
                    Time.timeScale = 1.0f;
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    SEm.SEPlay(0);
                    MainSceneManager("SelectScene");
                }

                if (Input.GetKeyDown(KeyCode.Alpha3))
                {
                    Application.Quit();//ゲームプレイ終了
                }

            }

            //ゴール処理
            if(isGoal == true)
            {
                if(issmallgoal == 1)
            {
                SEm.SEPlay(6);
                issmallgoal = 2;
            }

            //スコアを調べてベストスコアだったら記録
            switch (SceneManager.GetActiveScene().name)
            {
                case "Stage1":

                    if(data.timeHighScore[0] > presentGameTime || data.timeHighScore[0] == 0)
                    {
                        data.timeHighScore[0] = presentGameTime;
                        isBestTime = true;
                        Debug.Log(data.timeHighScore[0]);
                        MySave(data);
                    }

                    if (data.scoreHighScore[0] < presentScore || data.scoreHighScore[0] == 0)
                    {
                        data.scoreHighScore[0] = presentScore;
                        isBestScore = true;
                        MySave(data);
                    }

                    Debug.Log(JsonUtility.ToJson(data, true));

                    break;

                case "Stage2":

                    if (data.timeHighScore[1] > presentGameTime || data.timeHighScore[1] == 0)
                    {
                        data.timeHighScore[1] = presentGameTime;
                        isBestTime = true;
                        MySave(data);
                    }

                    if (data.scoreHighScore[1] < presentScore || data.scoreHighScore[1] == 0)
                    {
                        data.scoreHighScore[1] = presentScore;
                        isBestScore = true;
                        MySave(data);
                    }
                    break;

                case "Stage3":

                    if (data.timeHighScore[2] > presentGameTime || data.timeHighScore[2] == 0)
                    {
                        data.timeHighScore[2] = presentGameTime;
                        isBestTime = true;
                        MySave(data);
                    }

                    if (data.scoreHighScore[2] < presentScore || data.scoreHighScore[2] == 0)
                    {
                        data.scoreHighScore[2] = presentScore;
                        isBestScore = true;
                        MySave(data);
                    }
                    break;

                default:

                    break;
            }
            Debug.Log(isBestScore + " : " + isBestTime);

            //Spaceでセレクトシーンに戻る
            if (Input.GetKeyDown(KeyCode.Space))
                {
                    SEm.SEPlay(0);
                    MainSceneManager("SelectScene");
                }
            }
            //ゴール処理ここで終わり


    }

        private void FixedUpdate()
        {
            //Timeを数える
            presentGameTime += Time.deltaTime;


            //HPの計算
            if (isForcused == true)
            {
                charaHP -= damageScale * Time.deltaTime;
                
            }
            else
            {
                if(charaHP < charaHP_MAX)
                charaHP += healScale;
            }

        //transportを使うとき、振られた番号に応じたステージに飛ばす
        if (isTransporting == true)
        {
            

            if (Input.GetKeyDown(KeyCode.F))
            {
                Debug.Log("staged");
                SEm.SEPlay(4);

                switch (selectingStage)
                {
                    case 0:
                        MainSceneManager("SelectScene");
                        break;

                    case 1:
                        MainSceneManager("Stage1");
                        break;

                    case 2:
                        MainSceneManager("Stage2");
                        break;

                    case 3:
                        MainSceneManager("Stage3");
                        break;

                }
            }
        }

    }

}


