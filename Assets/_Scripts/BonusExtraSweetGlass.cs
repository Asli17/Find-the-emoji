using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusExtraSweetGlass : MonoBehaviour
{
    public GameObject mainBlock;
    bool clicked = false;

    [SerializeField] private TMPro.TextMeshProUGUI extraAmountNumber;
    public GameObject bonusBackToGame;

    public GameObject BonusExtraHammer; // from Glass;


    bool animateFinish = false;


    private void OnEnable()
    {
        FindObjectOfType<AudioManager>().Play("success");

        // 60 - green +1 hammer
        //110 turkis +1 glass
        //170  darjblue +1 hammer +1 glass
        //240 red +2 hammer + 1glass

        //320 lila +2 glass + 2hammre
        //410 lila
        //500 upgrade other lila 3+hammer 
        //600 lila 
        // 700
        //800





        if (GameManager.Instance.greenPresentBonus || GameManager.Instance.bluePresentBonus || GameManager.Instance.darkBluePresentBonus )
        {

            // 1 hammmer
            extraAmountNumber.text = "+1";
            GameManager.Instance.ExtraSweetBonbon += 1;
        }

        else if (GameManager.Instance.redPresentBonus || GameManager.Instance.lilaPresentBonus)
        {
            // 2 hamer
            extraAmountNumber.text = "+2";
            GameManager.Instance.ExtraSweetBonbon += 2;

        }
        else 
        {
            // 2 hamer
            extraAmountNumber.text = "+3";
            GameManager.Instance.ExtraSweetBonbon += 3;

        }
        /*   else if (GameManager.Instance.rainbowPresentBonus)
           {
               // 3 hammer 
               extraAmountNumber.text = "+3";

           }*/


        LeanTween.scale(mainBlock, new Vector3(0.8f, 0.8f, 1), 0.4f).setEaseOutElastic().setOnComplete(Change);
        ;





    }
    void Change()
    {
        animateFinish = true;

    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {


            if (clicked == false&& animateFinish ==true)
            {

                clicked = true;
                gameObject.SetActive(false);
                animateFinish = false;

                if (GameManager.Instance.greenPresentBonus)
                {

                bonusBackToGame.SetActive(true);
                }
                else
                {
                    BonusExtraHammer.SetActive(true);
                }
                //  FindObjectOfType<CurrentStreakMenu>().ChangeCurrStreak();
                //


                // print("gridpop firstalert" + Board.Instance.gridPopulation);
            }
        }

    }
}