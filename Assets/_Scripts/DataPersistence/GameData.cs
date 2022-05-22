using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class GameData 
{
    public string lastPos;
    public int coinNumber;

    public int gameNumber;
    public int win;
    public int lose;


    public int score3;
    public int score2;
    public int score1;

    public bool sound;
    public bool music;
    public string savedTIme;

   public  int  secondsLeft;
    public int minutesLeft;

  public  bool timerActive;

    public GameData()
    {
        this.lastPos = "slow";
        this.coinNumber  = 5;
        this.gameNumber = 0;

        this.win = 0;
        this.lose = 0;

        this.savedTIme ="";

        this.score3 = 0;
        this.score2 = 0;
        this.score1 = 0;

        this.sound = true;
        this.music = true;

        this.timerActive = false;

        this.secondsLeft = 0;
        this.minutesLeft = 0;

    }
}