using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class noCoinScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] public Button coinButtonCover;
    [SerializeField] public Button coinButton;

    [SerializeField] public Button AdButton;





    [SerializeField] private GameObject ParentBoard;
    [SerializeField] private GameObject ParentFeatureTile;
    [SerializeField] private GameObject PauseButton;
    [SerializeField] private GameObject RestartButton;
    [SerializeField] private GameObject ParentBoardBackground;
    [SerializeField] private GameObject featureTileBackground;
    [SerializeField] private GameObject rectBackGround;
 
    public GameObject mainBlock;
    bool coinButtonActive = false;

    void OnEnable()
    {
        Featured.Instance.screenActive = false;
        //destroy feature tile && all nodes in board!!
        if(GameOver.Instance.lose == true)
        {
          
            ParentFeatureTile.SetActive(false);
            ParentBoard.SetActive(false);
     

        }
        ParentBoardBackground.SetActive(false);
        featureTileBackground.SetActive(false);
        //DEACTIVATE PAUSE AND RESTART BUTTOnS
        PauseButton.SetActive(false);
        RestartButton.SetActive(false);
        rectBackGround.SetActive(false);

        coinButtonCover.enabled = true;
        coinButtonCover.interactable = false;

        coinButton.interactable = false;

     

        if (GameManager.Instance.tablet == true)
        {
            LeanTween.scale(mainBlock, new Vector3(0.7f, 0.7f, 1), 2.3f).setEase(LeanTweenType.easeOutElastic);

        }
        else
        {

            LeanTween.scale(mainBlock, new Vector3(1, 1, 1), 2.3f).setEase(LeanTweenType.easeOutElastic);
        }
     //   LeanTween.scale(mainBlock, new Vector3(1f, 1f, 1f), 2.3f).setEase(LeanTweenType.easeOutElastic);//.setOnComplete(animateStars);


        // disable when coin is one

    }

    // Update is called once per frame

    void PlayCoinSound()
    {
        FindObjectOfType<AudioManager>().Play("coin");

    }



    public void StartGame()
    {
        if(GameManager.Instance.coinNum > 0)
        {
            FindObjectOfType<AudioManager>().Play("coin");

            LeanTween.scale(mainBlock, new Vector3(0f, 0f, 0f), .4f).setEase(LeanTweenType.easeOutElastic);//.setOnComplete(animateStars);
            PlayCoinSound();


            Invoke("ChangeToPlay", 0.4f);


        }
    }

    private void ChangeToPlay()
    
    {
        rectBackGround.SetActive(true);
        Featured.Instance.LoseCoinFromNoCoinScreen();
        /*
        PauseButton.SetActive(true);
        RestartButton.SetActive(true);
        ParentBoardBackground.SetActive(true);
        featureTileBackground.SetActive(true);
        GameManager.Instance.ChangeState(GameState.FeatureTile);*/

    }



    public void ChangeToAdButtonClicked()
    {
        GameManager.Instance.adNoCoinScreenClicked = true;
    }

    private void Update()
    {
        if (coinButton.interactable == true && coinButtonActive ==false)
        {
            // animate 
            coinButtonActive = true;
            LeanTween.scale(coinButton.gameObject, new Vector3(1.9f, 3.2f, 1), 1f).setEaseInElastic().setOnComplete(scaleBackUp);
        }
    }

    void scaleBackUp()
    {
        LeanTween.scale(coinButton.gameObject, new Vector3(2.3f, 3.2f, 1), 1f).setEaseOutElastic().setOnComplete(ActivateCoinButton);

    }
    void ActivateCoinButton()
    {
        if (coinButton.interactable == true)
        {
            coinButtonActive = false;
        }

    }


    public void ChangeButtonToAlreadyClicked()
    {
        if(GameManager.Instance.coinNum > 0)
        {
            AdButton.interactable = false;
            coinButton.interactable = true;
            coinButtonCover.gameObject.SetActive( false);
            GameManager.Instance.adNoCoinScreenClicked = false;

            //darken text of adbutton

        }

    }

    /*   public void RestartButtonNoCoinScreen()
       {

       }
   */

}
