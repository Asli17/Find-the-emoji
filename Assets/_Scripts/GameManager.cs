using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Globalization;



/**/


public class GameManager : MonoBehaviour, IDataPersistence
{
    public int gameCount = 0;
    [SerializeField] public TMPro.TextMeshProUGUI m_Object;
    public int coinNum;
    public int score = 0;
    public int lastPosition; // loaded pos
    public int lastPositionCurrent; // get currentPos
    public string positionStringLoad = "";
    public string positionStringSave = "";

    public int win;
    public int lose;
    public int score1;
    public int score2;
    public int score3;


    public bool adNoCoinScreenClicked = false;


    public bool firstTime = false;
    public bool firstFindScreen = false;
    public bool firstFeatureTile = false;
    public bool firstFeatureTileAlreadyShown = false;
    

    public bool firstBoardTile = false;
    public bool firstWin = false;


    public DateTime toSaveDatetime;
    public string toSaveDatetimeString;
    public string savedate;


    public bool tablet = false;
    public bool phone = false;

    public bool minimizedApp = false;

    public DateTime toLoadDatetime;
    public string toLoadDatetimeString;
    public string loadDateStr;

    int prevlose;
    int nextlose;

    public bool soundActive = true;
    public bool musicActive = true;



    public int bestStreak;
    public int bestStreakStats;
    public int currentStreak;




    public int secondsLeft = 0;
    public int minutesLeft = 0;
    public bool activeCountDown;

    public GameObject findScreen;

    public static GameManager Instance;
    public GameState GameState;

    [SerializeField] public GameObject coinNotEnoughScreen;
    public bool coinNotEnough = false;

    public bool restarted = false;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else Debug.Log("error");

    }



 




    void Start()
    {
/**/
        
/**/

        Screen.sleepTimeout = (int)0f;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        
        //set current scene as the one who gets saved
        // lastPositionCurrent = SceneManager.GetActiveScene().buildIndex;

        if (positionStringLoad == "med")
        {
   
            Board.Instance.timeSpeed = 0.8f;
        }
        else
            if (positionStringLoad == "hard")
            {

                Board.Instance.timeSpeed = 0.5f;
            
        }
        else
            if (positionStringLoad == "easy")
        {

            Board.Instance.timeSpeed = 1.1f;

        }
        // positionStringSave = SceneManager.GetActiveScene().name;


        //   toLoadDatetime = DateTime.ParseExact(loadDateStr, "dd/MM/yyyy HH:mm:ss", CultureInfo.GetCultureInfo("de-DE"));
        if (loadDateStr != "")
        {

            //   toLoadDatetime = DateTime.ParseExact(loadDateStr, "MM/dd/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            toLoadDatetime = DateTime.ParseExact(loadDateStr, "MM/dd/yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture);

        }

        bestStreakStats = bestStreak;
        print(gameCount);

        m_Object.text = coinNum.ToString();
        /*  if (coinNum > 0 || coinNotEnough == false)
          {*/
        ChangeState(

            GameState.CheckScreenSize);

    }

    public void callLoadAgain()
    {
        if (minimizedApp == true)
        {
            toLoadDatetime = DateTime.ParseExact(loadDateStr, "MM/dd/yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture);


            TestTime.Instance.CalculateTime();
        }

    }


    public void LoadData(GameData gameData)
    {
        this.gameCount = gameData.gameNumber;

        this.coinNum = gameData.coinNumber;

        this.win = gameData.win;
        this.lose = gameData.lose;

        this.score1 = gameData.score1;
        this.score2 = gameData.score2;
        this.score3 = gameData.score3;

        this.loadDateStr = gameData.savedTIme;

        this.minutesLeft = gameData.minutesLeft;
        this.secondsLeft = gameData.secondsLeft;


        this.activeCountDown = gameData.timerActive;

        this.soundActive = gameData.sound;
        this.musicActive = gameData.music;

        this.bestStreakStats = gameData.bestStreakStat;
        this.bestStreak = gameData.bestStreak;
        this.currentStreak = gameData.currentStreak;
        /*  this.bestStreakStats =0;
          this.bestStreak =0;
          this.currentStreak = 0;*/


        positionStringLoad = gameData.lastPos;

        this.coinNotEnough = gameData.notEnoughCoins;

        this.restarted = gameData.restarted;

        this.phone = gameData.isPhone;
        this.tablet = gameData.isTablet;

        print(gameData);


        this.firstTime = gameData.firstTime;
    }
    public void SaveData(GameData gameData)
    {
        Debug.Log("gamemanager savedata" + this.lose);

        //  savedate = DateTime.Now.ToString(/*CultureInfo.InvariantCulture*/ CultureInfo.GetCultureInfo("de-DE"));
        savedate = DateTime.UtcNow.ToString("MM/dd/yyyy HH:mm:ss zzz", CultureInfo.InvariantCulture);//DateTimeStyles.AdjustToUniversal); //CultureInfo.GetCultureInfo("de-DE"));



        gameData.lastPos = positionStringLoad;



        gameData.gameNumber = this.gameCount;

      /*  gameData.bestStreakStat = this.bestStreakStats;
       gameData.bestStreak = this.bestStreak;
        gameData.currentStreak = this.currentStreak;*/


        gameData.bestStreakStat = 0;
        gameData.bestStreak = 0;
        gameData.currentStreak = 0;


        gameData.win = this.win;
        gameData.lose = this.lose;

        gameData.score1 = this.score1;
        gameData.score2 = this.score2;
        gameData.score3 = this.score3;

        gameData.sound = this.soundActive;
        gameData.music = this.musicActive;

        gameData.savedTIme = this.savedate;

        //

        gameData.timerActive = this.activeCountDown;
        gameData.secondsLeft = this.secondsLeft;
        gameData.minutesLeft = this.minutesLeft;



        gameData.restarted = this.restarted;



        gameData.notEnoughCoins = this.coinNotEnough;

/*
        gameData.secondsLeft = 00;
        gameData.timerActive = true;
        gameData.minutesLeft = 2;*/

  /*  gameData.coinNumber =5;*/


      gameData.coinNumber = this.coinNum;

        gameData.isTablet = this.tablet;
        gameData.isPhone = this.phone;


       /* gameData.firstTime =true;*/
        gameData.firstTime =this.firstTime;
    }




    public void ChangeState(GameState newState)
    {
        GameState = newState;
        print("GAMEMANAGER minLeft" + minutesLeft);
        print("GAMEMANAGER secLeft" + secondsLeft);
        print(loadDateStr);
        /*   if (coinNum > 0) { 
   */
        switch (newState)
        {
            case GameState.CheckScreenSize:
                UiScaler.Instance.CheckResolution();
                break;
            case GameState.CheckTimer:
                TestTime.Instance.CalculateTime();
                break;

            case GameState.FeatureTile:
                print("gamestate featuretile");
                Featured.Instance?.choseFeatureTile();
                currentStreak += 1;
                FindObjectOfType<CurrentStreakMenu>().ChangeCurrStreak();
                break;
            case GameState.ActivateFindScreen:
                print("gamestate activateFindScreen");

                findScreen.SetActive(true);
                findFeatureScreenAnim.Instance?.startFindScreenAnimation();
                /*if (coinNum==0)
                {
                    coinNotEnough = true;
                }*/
                break;

            case GameState.GenerateGrid:

                Board.Instance?.GenerateGrid();

                break;
            case GameState.ChoseTile:

                Tiles.Instance.checkForClicks();

                break;

            case GameState.Win:
                gameCount += 1;
                FindObjectOfType<AudioManager>().Play("yes");

                win += 1;
                GameOver.Instance.Win();
                print("score" + score);
                //     SaveData();

                break;
            case GameState.Lose:
                print("change state prevlose" + prevlose);

                print("change state lose" + lose);
                //lose = prevlose;
                lose += 1;
                // nextlose = lose;
                gameCount += 1;
                //      this.lose = prevlose;
                print("change state lose" + nextlose);

                GameOver.Instance.Lose();
                //   DataPersistenceManager.Instance.SaveGame();
                print("score" + score);

                //      SaveData();
                break;
            case GameState.Restart:
                coinNum -= 1;

                GameOver.Instance.Restart();

                print("restart" + coinNum);
                //      SaveData();
                break;


            case GameState.changeToSlowScene:

                Menu.Instance.LoadSceneSlow();

                //      SaveData();
                break;

            case GameState.changeToMedScene:


                Menu.Instance.LoadSceneMedium();

                break;
            case GameState.changeToHardScene:

                Menu.Instance.LoadSceneFast();
                break;
               
        }
    }

}



public enum GameState
{
    CheckTimer = 0,
    FeatureTile = 1,
    GenerateGrid = 2,
    ChoseTile = 3,
    Win = 4,
    Lose = 5,
    Restart = 6,
    changeToSlowScene = 7,
    changeToMedScene = 8,
    changeToHardScene = 9,
    ActivateFindScreen = 10,
    NotEnoughCoins = 11,
    CheckScreenSize = 12,

}
