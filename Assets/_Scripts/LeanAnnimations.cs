using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LeanAnnimations : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] stars;
    public GameObject mainBlock;
    public Sprite yellowStar;
    public Sprite greyStar;
    int score = 0;

    public GameObject noCoinScreen;
    public GameObject highScoreLabel;



    private void OnEnable()
    {
    

        Board.Instance.pauseBoard();
        Featured.Instance.screenActive = true;
         //   var score = GameManager.Instance.score;
        if(GameOver.Instance.win == true)
        {
            //FindObjectOfType<PlayExtraSound>().Play(); /// play yay sound
            FindObjectOfType<AudioManager>().Play("win", false);
            print("WIIN");
            for (int i = 0; i < stars.Length; i++)
            {
                if (Board.Instance.appearCounter == 0 || Board.Instance.appearCounter == 1)
                {
                     print(Board.Instance.appearCounter);
                    stars[i].GetComponent<SpriteRenderer>().sprite = yellowStar;
                    score = 3;
                //    GameManager.Instance.score3 += 1;
                }
                else if (Board.Instance.appearCounter > 1 && Board.Instance.appearCounter <= 3)
                {
                    stars[0].GetComponent<SpriteRenderer>().sprite = yellowStar;
                    stars[1].GetComponent<SpriteRenderer>().sprite = yellowStar;
                    stars[2].GetComponent<SpriteRenderer>().sprite = greyStar;
                    //       GameManager.Instance.score = 2;
                    score = 2;
                    //    GameManager.Instance.score2 += 1;
                    print(Board.Instance.appearCounter);



                }
                else if (Board.Instance.appearCounter > 3)
                {
                    // Stars[i].sprite = greyStar;
                    stars[0].GetComponent<SpriteRenderer>().sprite = yellowStar;
                    stars[1].GetComponent<SpriteRenderer>().sprite = greyStar;
                    stars[2].GetComponent<SpriteRenderer>().sprite = greyStar;
                    //     GameManager.Instance.score = 1;
                    score = 1;

                    print(Board.Instance.appearCounter);


                }
                // save score

            }

        }
        else if(GameOver.Instance.lose == true)
        {
            print("Looose");
            score = 0;
      //      GameManager.Instance.lose += 1;
        }
        if (score == 1)
        {
            GameManager.Instance.score1 += 1;

        }
        if (score == 2)
        {
            GameManager.Instance.score2 += 1;

        }
        if (score == 3)
        {
            GameManager.Instance.score3 += 1;

        }

        DataPersistenceManager.Instance.SaveGame();
        print("first save on lose");

        if (GameOver.Instance.win)
        {

        LeanTween.scale(mainBlock, new Vector3(0.8f, 0.8f, 0.8f), 2.3f).setDelay(0.4f).setEase(LeanTweenType.easeOutElastic);//.setOnComplete(animateStars);


        //animate stars

        LeanTween.scale(stars[0], new Vector3(8.5f, 8.5f, 1), 2f).setDelay(0.6f).setEaseOutElastic();
        LeanTween.scale(stars[1], new Vector3(10f, 10f, 1), 2f).setDelay(0.7f).setEaseOutElastic();
        LeanTween.scale(stars[2], new Vector3(8.5f, 8.5f, 1), 2f).setDelay(0.8f).setEaseOutElastic();
            if (GameOver.Instance.newHighScore == true)
            {

            Invoke("ActivateHighScore", 1f);

            }
            Invoke("PlayYay", 0.4f);

        }
        else
        {
            LeanTween.scale(mainBlock, new Vector3(0.8f, 0.8f, 0.8f), 2.3f).setEase(LeanTweenType.easeOutElastic);//.setOnComplete(animateStars);


            //animate stars

            LeanTween.scale(stars[0], new Vector3(8.5f, 8.5f, 1), 2f).setDelay(0.4f).setEaseOutElastic();
            LeanTween.scale(stars[1], new Vector3(10f, 10f, 1), 2f).setDelay(0.5f).setEaseOutElastic();
            LeanTween.scale(stars[2], new Vector3(8.5f, 8.5f, 1), 2f).setDelay(0.6f).setEaseOutElastic();
        }
    }
       


  /*  void animateStars()
    {
        LeanTween.scale(stars[0], new Vector3(8.5f,8.5f,1), 2f).setEaseOutElastic();
        LeanTween.scale(stars[1], new Vector3(9f,9f,1), 2f).setDelay(.1f).setEaseOutElastic();
        LeanTween.scale(stars[2], new Vector3(8.5f,8.5f,1), 2f).setDelay(.2f).setEaseOutElastic();

    }*/
    public void Close()
    {
        print("close!!");
        LeanTween.scale(gameObject, Vector3.zero, 0.5f).setOnComplete(OnDisable);
      

        //lose not last coin

        if (GameOver.Instance.lose == true && GameManager.Instance.coinNum>0)
        {
            Featured.Instance.lostGame = true;
        FindObjectOfType<AudioManager>().Play("coin");
            //   Invoke("RestartFromFeature", 0.2f);
         
            DataPersistenceManager.Instance.SaveGame();
            print("save lose leananim before losinf a coin" + GameManager.Instance.coinNum);
            RestartFromFeature();

        }
        //lose and last coin

        else if (GameOver.Instance.lose == true && GameManager.Instance.coinNum == 0)
        {
            FindObjectOfType<AudioManager>().Play("coin");

            Featured.Instance.lostGame = true;
            print("leananim losescreen coin = 0");
           
            /*nocoin activate */
         //   noCoinScreen.SetActive(true);
            GameManager.Instance.coinNotEnoughScreen.SetActive(true);
            FindObjectOfType<AudioManager>().Play("noCoin");

            //  GameManager.Instance.coinNotEnough = true;
            Featured.Instance.screenActive = true;
            DataPersistenceManager.Instance.SaveGame();

        }

        //win

        else if (GameOver.Instance.win==true)
        {
            DataPersistenceManager.Instance.SaveGame();

            Invoke("ActualRestart", 0.2f);
        }




        //DataPersistenceManager.Instance.changeScene();
    }
    void RestartFromFeature()
    {
        Featured.Instance.restartScene();
        Featured.Instance.screenActive = false;

    }
    void ActualRestart()
    {

        Featured.Instance.screenActive = false;
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name);


    }
    /*    void DestroySelf()
        {
            print("destroy");
         //   Destroy(gameObject);
        }*/
    private void OnDisable()
    {
        gameObject.SetActive(false);
        GameOver.Instance.lose = false;
        GameOver.Instance.win = false;
    }


    void ActivateHighScore()
    {
     //   highScoreLabel.SetActive(true);
        LeanTween.scale(highScoreLabel.GetComponent<RectTransform>(), new Vector3(1, 1, 1), .3f).setEase(LeanTweenType.easeOutElastic);
    }

    void PlayYay()
    {
        FindObjectOfType<AudioManager>().Play("yay", false);

    }

}
