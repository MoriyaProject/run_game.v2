using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

//�ۑ�����f�[�^�̌^
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

    //save�֘A
    public SaveData data;
    public bool isBestTime = false;
    public bool isBestScore = false;

    SaveData MyLoad()
    {
        string datastr;

        if (PlayerPrefs.HasKey("PlayerData"))
        {
            //�f�[�^������Γǂݍ���
            datastr = PlayerPrefs.GetString("PlayerData");
            return JsonUtility.FromJson<SaveData>(datastr);
        }
        else
        {
            //�f�[�^���Ȃ�������V�������
            Debug.Log("new");
            SaveData newData = new SaveData();
            return newData;
        }

        
    }

    void MySave(SaveData myData)
    {
        //�ύX���ꂽmydata��json�ɂ���PlayerPrefs�ɏ㏑���ۑ�
        string datastr = JsonUtility.ToJson(myData);
        PlayerPrefs.SetString("PlayerData", datastr);
        PlayerPrefs.Save();
    }


    //scene�֘A
    public bool goalFlag = false;
    
    //Scene��ǂݍ��ފ֐�
        public void MainSceneManager(string roadSceneName)
        {
            isFadeOut = true;
            SEm.SEPlay(4);
            StartCoroutine(Scenechange(roadSceneName));
        }

        IEnumerator Scenechange(string sceneName)
        {
            //delay�b�҂�
            yield return new WaitForSecondsRealtime(3.0f);
            /*����*/
            SceneManager.LoadScene(sceneName);
        }

        //UI�֘A
        public bool isFadeiIn;
        public bool isFadeOut;

        //time�֘A
        public float presentGameTime = 0f;
        public bool isPose = false;


        //position�֘A
        public Vector3 playerPos;

        //HP�֘A
        public int    charaHP_MAX = 100;
        public float  charaHP     = 0;
        public bool   isForcused  = false;

        [SerializeField] private int   damageScale = 10;
        [SerializeField] private float healScale   = 0.5f;

        //score�֘A
        public int  presentScore = 0;
        
    //�X�R�A�̗ݐςƊl������SE��炷
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

     //rift�֘A
    public Vector3 riftVec;

    //goal�֘A
    public bool isGoal      = false;
    private int issmallgoal = 1;

    //gameover�֘A
    public bool isGameOver;

    //transport�֘A
    public bool isTransporting;
    public int  selectingStage;

    private void Awake()
    {

        //�t�@�C���ǂݍ���
        data = MyLoad();

        //���Ԃ�i�߂�
        Time.timeScale = 1.0f;
    }



    private void Start()
        {

        Debug.Log(JsonUtility.ToJson(data, true));

        //HP�͊J�n����MAX
        charaHP = charaHP_MAX;

        //BGM���V�[���ɍ��킹�ĕς���
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

        //HP���O�ɂȂ�����Q�[���I�[�o�[
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
        
            //�|�[�Y��ʏ���
            //Q�������ꂽ��|�[�Y���
            //�|�[�Y����Q�������ꂽ��|�[�Y����
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

            //�|�[�Y���͎��Ԓ�~
            //1�������ꂽ��ĊJ�A�Q�������ꂽ��Z���N�g��ʂցA�R�������ꂽ��Q�[���I��
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
                    Application.Quit();//�Q�[���v���C�I��
                }

            }

            //�S�[������
            if(isGoal == true)
            {
                if(issmallgoal == 1)
            {
                SEm.SEPlay(6);
                issmallgoal = 2;
            }

            //�X�R�A�𒲂ׂăx�X�g�X�R�A��������L�^
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

            //Space�ŃZ���N�g�V�[���ɖ߂�
            if (Input.GetKeyDown(KeyCode.Space))
                {
                    SEm.SEPlay(0);
                    MainSceneManager("SelectScene");
                }
            }
            //�S�[�����������ŏI���


    }

        private void FixedUpdate()
        {
            //Time�𐔂���
            presentGameTime += Time.deltaTime;


            //HP�̌v�Z
            if (isForcused == true)
            {
                charaHP -= damageScale * Time.deltaTime;
                
            }
            else
            {
                if(charaHP < charaHP_MAX)
                charaHP += healScale;
            }

        //transport���g���Ƃ��A�U��ꂽ�ԍ��ɉ������X�e�[�W�ɔ�΂�
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


