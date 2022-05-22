using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;
public class Board : MonoBehaviour
{
    public static Board Instance;
    public Featured featureTile;
    public GameObject particleInstance;
    private GameObject particleName;

    private SpriteRenderer featureTileSpriteRenderer;
    // Start is called before the first frame update
    [SerializeField] private int _width = 4;
    [SerializeField] private int _height = 4;
    [SerializeField] private Tiles _tilesPrefab;
    /*[SerializeField] private SpriteRenderer _boardPrefab;*/
    private SpriteRenderer _tileSpriteRenderer;
    private int _minRange = 0;
    private int _maxRange = 80;

    public GameObject PausePanel;
    public bool pausePanelActive = false;


    public float timeSpeed = 0.4f;

    [SerializeField] private GameObject _parentObject;
    private int rnd = 0;
    private bool alreadyAssigned = false;
    private int randomChosenTile = 0;
    public int appearCounter = 0;

    public GameObject button;
    public Sprite buttonSpritePlay;
    public Sprite buttonSpritePause;

    //start board change after 4sec
    public int secondsToStart = 5;
    public IEnumerator toStartTimer;

    public List<Sprite> chosenSpritesArray;


    public int originalSeconds = 2;
    public int secondsLeft; public bool takingAway = false;
    public IEnumerator StartTimer;
    public List<Tiles> _nodes;
    public bool paused = false;
    public bool findScreenFinished = false;
    public bool gridPopulation = false;

    AudioManager audioManager;
    public Sound[] sounds;

    int count = 0;


    GameObject singleNode;

    void Awake()
    {



        if (Instance == null)
        {
            Instance = this;
            _maxRange = _tilesPrefab._gameObjects.Length;
            //_tilesPrefab = GetComponent<Tiles>();
            _tileSpriteRenderer = _tilesPrefab?.GetComponent<SpriteRenderer>();
        }
        else
        {
            _maxRange = _tilesPrefab._gameObjects.Length;
            _tileSpriteRenderer = _tilesPrefab?.GetComponent<SpriteRenderer>();

        }



    }
    private void Start()
    {
        //  Debug.Log("board inside");
        secondsLeft = originalSeconds;
        audioManager = FindObjectOfType<AudioManager>();


    }
    private void Update()
    {
        //Dont call changesingle tile before generategrid is populated completely 
        // maybe while any tile sprite == firstsprite => get tile??
        if (gridPopulation == true)
        {

            if (secondsToStart > 0)
            {
                toStartTimer = StartingTimer();
                StartCoroutine(toStartTimer);
            }
            else if (secondsToStart <= 0)
            {

                StopCoroutine(toStartTimer);        // <-- starting timer
                if (takingAway == false && secondsLeft > 0 && paused == false)
                {
                    StartTimer = Timer();

                    StartCoroutine(StartTimer);
                }
                else if (takingAway == false && secondsLeft == 0 && paused == false)
                {
                    secondsLeft = originalSeconds;
                    changeSingleTile();

                }

            }
        }

    }


    public void PopSprite()
    {
        count = 0;

        for (int i = 3; i < _nodes.Count; i += 4)
        {

            singleNode = _nodes[i].transform.GetChild(0).gameObject;
          

            LeanTween.scale(singleNode, new Vector3(1.45f, 1.45f, 1.45f), 1.72f).setDelay(count / 10).setEaseOutElastic(); //.setOnComplete(DestroyTileChild);
            DestroyTileChild();
         
        }

        for (int x = 2; x < _nodes.Count - 1; x += 4)
        {
          

          //  print("count" + count);
            singleNode = _nodes[x].transform.GetChild(0).gameObject;
          
            LeanTween.scale(singleNode, new Vector3(1.45f, 1.45f, 1.45f), 1.72f).setDelay(count / 10).setEaseOutElastic();//.setOnComplete(DestroyTileChild);
            //Invoke("HideShowGameobject", count / 10);
            DestroyTileChild();


        }
        
        for (int y = 1; y < _nodes.Count - 2; y += 4)
        {
          
            singleNode = _nodes[y].transform.GetChild(0).gameObject;
        
            LeanTween.scale(singleNode, new Vector3(1.45f, 1.45f, 1.45f), 1.72f).setDelay(count / 10).setEaseOutElastic();//.setOnComplete(DestroyTileChild);
                                                                                                                          //  Invoke("HideShowGameobject", count / 10);
            DestroyTileChild();

        }

        for (int z = 0; z < _nodes.Count - 3; z += 4)
        {
           

            singleNode = _nodes[z].transform.GetChild(0).gameObject;
       
            LeanTween.scale(singleNode, new Vector3(1.45f, 1.45f, 1.45f), 1.72f).setDelay(count / 10).setEaseOutElastic();//.setOnComplete(DestroyTileChild);
                                                                                                                          //       Invoke("HideShowGameobject", count / 10);
            DestroyTileChild();


        }


        if (count == 16)
        {           
            gridPopulation = true;
        }



        Featured.Instance.tile.GetComponent<BoxCollider2D>().enabled = true;
    }



                                    /*PLAY POP AUDIO*/
    void HideShowGameobject()
    {
       if(GameOver.Instance.win == false)
        {

        FindObjectOfType<AudioManager>().Play("pop");
        }

    }


    void DestroyTileChild()
    {
        Invoke("HideShowGameobject", (count + 0.01f) / 10);


        Destroy(singleNode, (count + 1.7f) / 10);
        count += 1;
    }

 /*   void disableLoop()
    {
        // audioManager.GetComponent<AudioSource>().loop = false;
        print("stop the loop");
        FindObjectOfType<AudioManager>().Play("jump", false, true);

    }
*/

    private Tiles getTile()
    {

        alreadyAssigned = false;
        Sprite firstSprite = Tiles.Instance.firstSprite.GetComponent<SpriteRenderer>().sprite;

        // _tileSpriteRenderer.sprite = _tilesPrefab?._gameObjects?[rnd];

        rnd = UnityEngine.Random.Range(_minRange, 80);

        _tileSpriteRenderer.sprite = chosenSpritesArray[rnd];


        if (_nodes != null)
        {
            _nodes.ForEach((t) =>
            {
                //check duplicate
                if (t.GetComponent<SpriteRenderer>().sprite == _tileSpriteRenderer.sprite)
                {
                    alreadyAssigned = true;


                }
                //print(featureTile.tile.GetComponent<SpriteRenderer>().sprite);//.GetComponent<SpriteRenderer>().sprite);

                //check if featureTile sprite appeared
                if (alreadyAssigned == false && t.GetComponent<SpriteRenderer>().sprite == featureTileSpriteRenderer.sprite)
                {
                   // print("appeared");
                    appearCounter = 1;
                  //  print(appearCounter);
                }



            });
        }

        //added 02.05

        //change to  first sprite 



        /*   else if(findScreenFinished == false )// && population is completed)
           {
               _tileSpriteRenderer.sprite = firstSprite;

           }*/



        /* else if(findScreenFinished =true && gridPopulation ==false)
         {
             GenerateGrid();    ____________----->  call NEW generategrid at this point 
             gridPopulation = true;
         }*/
        return _tilesPrefab;



    }

   

    public void GenerateGrid()
    {
        featureTileSpriteRenderer = featureTile._featureTilePrefab.GetComponent<SpriteRenderer>();
     //   print("board" + featureTileSpriteRenderer.sprite);

        //chosenSpritesArray will contain 80 sprites
        for (int i = 0; i < 79; i++)
        {
            rnd = UnityEngine.Random.Range(_minRange, _maxRange);
            chosenSpritesArray.Add(_tilesPrefab?._gameObjects?[rnd]);
        }


        chosenSpritesArray.Add(featureTileSpriteRenderer.sprite);
     //   print(chosenSpritesArray);
      //  print("length of chosenSpritesArray" + chosenSpritesArray.Count);



        _nodes = new List<Tiles>();

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {

                Instance.getTile();
                while (alreadyAssigned)
                {
                    // Debug.Log("already");
                    getTile();
                }

                Tiles node = Instantiate(_tilesPrefab, new Vector2(x, y), Quaternion.identity);

                node.transform.parent = _parentObject.transform;
                _nodes.Add(node);


            }



        }





        //  var center = new Vector2((float)_width / 2 - 0.5f, _height / 2 + 0.5f);
        var center = new Vector2((float)_width / 2 - 0.5f, _height / 2 + 0.53f);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);


        // _nodes.ForEach((tile) => { tile.GetComponent<BoxCollider2D>().enabled = false; });

        //  Debug.Log("ft inst");
        /* FeatureTile.Instance?.choseFeatureTile();*/
        GameManager.Instance.ChangeState(GameState.ChoseTile);

    }







    /*GENERATE GRID ORIGINAL FUNCTION*/


    /*
    public void GenerateGrid()
    {
        featureTileSpriteRenderer = featureTile._featureTilePrefab.GetComponent<SpriteRenderer>();
        print("board" + featureTileSpriteRenderer.sprite);


        _nodes = new List<Tiles>();
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {

                Instance.getTile();
                while (alreadyAssigned)
                {
                    // Debug.Log("already");
                    getTile();
                }

                Tiles node = Instantiate(_tilesPrefab, new Vector2(x, y), Quaternion.identity);

                node.transform.parent = _parentObject.transform;
                _nodes.Add(node);




            }
        }

        //  var center = new Vector2((float)_width / 2 - 0.5f, _height / 2 + 0.5f);
        var center = new Vector2((float)_width / 2 - 0.5f, _height / 2 + 0.53f);
        Camera.main.transform.position = new Vector3(center.x, center.y, -10);


        // _nodes.ForEach((tile) => { tile.GetComponent<BoxCollider2D>().enabled = false; });

        //  Debug.Log("ft inst");
        *//* FeatureTile.Instance?.choseFeatureTile();*//*
        GameManager.Instance.ChangeState(GameState.ChoseTile);

    }
*/
    public void changeSingleTile()
    {


        randomChosenTile = UnityEngine.Random.Range(_minRange, _width * _width);
        alreadyAssigned = false;
        // rnd = Random.Range(_minRange, _maxRange);
        rnd = UnityEngine.Random.Range(_minRange, 80);

        //    var randomChosenSprite = _tilesPrefab._gameObjects?[rnd];
        var randomChosenSprite = chosenSpritesArray[rnd];

        // get node sprite
        var nodeSprite = _nodes[randomChosenTile].GetComponent<SpriteRenderer>().sprite;
        //chose random tile
        _nodes.ForEach((t) =>
        {
            //random tile check duplicate
            if (t.GetComponent<SpriteRenderer>().sprite == randomChosenSprite)
            {
                alreadyAssigned = true;


            }


            // var freeNodes = _nodes.Where();
        });

        if (alreadyAssigned)
        {
            changeSingleTile();
        }
        else if (alreadyAssigned == false)
        {
            //set new sprite for single tile
            _nodes[randomChosenTile].GetComponent<SpriteRenderer>().sprite = randomChosenSprite;
            //set child sprite active for 1s

            //   _nodes[randomChosenTile].
            /*       particleName=  Instantiate(particleInstance);
                     particleName.transform.parent = _parentObject.transform;*/




            //FindObjectOfType<AudioManager>().Play("click");





            if (_nodes[randomChosenTile].GetComponent<SpriteRenderer>().sprite == featureTileSpriteRenderer.sprite)
            {
              //  print("appeared changed tile");
                appearCounter += 1;
               // print(appearCounter);
            }

        }
    }
    public void pauseBoard()
    {
        //print("here");
        if (gridPopulation == true)
        {
            //normal mode 
            if (paused == false && Featured.Instance.screenActive == false)
            {
            //    print(paused);
                button.GetComponent<Image>().sprite = buttonSpritePlay;
                paused = true;
                if (takingAway == true && secondsLeft > 0)
                {

                    StopCoroutine(StartTimer);
                }



            }
            //test this 
            /* else if (paused == false && Featured.Instance.screenActive == true)
             {
                 StopCoroutine(StartTimer);
             }*/
            else if (paused == true && Featured.Instance.screenActive == false)
            {
                if (takingAway == true && secondsLeft > 0)
                {

                    StopCoroutine(StartTimer);
                }
            }
            // 

            else if (paused == true && Featured.Instance.screenActive == true)
            {
                button.GetComponent<Image>().sprite = buttonSpritePause;
              //  print(paused);

                paused = false;

                StartTimer = Timer();

                StartCoroutine(StartTimer);
                secondsLeft = originalSeconds;

                if (Featured.Instance.openTile == true || Featured.Instance.clicked == true)
                {
                    Featured.Instance.StartCoroutine(Featured.Instance.FeatureTimer);
                    //decrease coin after warning
                }

            }
        }
    }
    public void PauseButton()
    {

        //only after popanimation


/*PAUSE IF NOT ALREADY PAUSED*/
        if (paused == false && pausePanelActive == false && Featured.Instance.screenActive == false && gridPopulation ==true)
        {
            ThemeSound.Instance.audio.volume = 0.04f;

            button.GetComponent<Image>().sprite = buttonSpritePlay;
            paused = true;
            //       print("pause");
            //  StopCoroutine(Timer());
            StopCoroutine(StartTimer);
            pausePanelActive = true;


            //activate panel
            PausePanel.SetActive(true);
        }

/*START IF PAUSED*/

        else if (paused == true && Featured.Instance.screenActive == false &&gridPopulation ==true)// && Featured.Instance.screenActive == false && pausePanelActive == true)
        {
            ThemeSound.Instance.audio.volume = 0.1f;

            button.GetComponent<Image>().sprite = buttonSpritePause;
          //  print(paused);

            paused = false;
            pausePanelActive = false;

            StartTimer = Timer();

            StartCoroutine(StartTimer);
            secondsLeft = originalSeconds;



        }
    }


    public void changeClickedSingleTile(int positionIndex)
    {
        randomChosenTile = UnityEngine.Random.Range(_minRange, _width * _width);
        //var specificTile = "";
        alreadyAssigned = false;
        rnd = UnityEngine.Random.Range(_minRange, _maxRange);
        //define new maxrange -ex. 50
        //make array with array[0] ==ftile + 49 rand. gameobjects

        var randomChosenSprite = _tilesPrefab._gameObjects?[rnd];
        // get node sprite
        //       var nodeSprite = _nodes[randomChosenTile].GetComponent<SpriteRenderer>().sprite;

        _nodes.ForEach((t) =>
        {
            if (t.GetComponent<SpriteRenderer>().sprite == randomChosenSprite)
            {
                alreadyAssigned = true;


            }
            // var freeNodes = _nodes.Where();
        });
        if (alreadyAssigned)
        {
            changeClickedSingleTile(positionIndex);
        }
        else
        {
            _nodes[positionIndex].GetComponent<SpriteRenderer>().sprite = randomChosenSprite;
            if (_nodes[randomChosenTile].GetComponent<SpriteRenderer>().sprite == featureTileSpriteRenderer.sprite)
            {
             //   print("appeared through clicked");
                appearCounter += 1;
             //   print(appearCounter);
            }

        }
    }

    /*   void restart()
       {
           //restarrt
       }
   */
    IEnumerator Timer()
    {
        takingAway = true;
        yield return new WaitForSeconds(timeSpeed);
        secondsLeft--;
        takingAway = false;
        //   print(secondsLeft);
    }

    IEnumerator StartingTimer()
    {

        yield return new WaitForSeconds(1);
        secondsToStart--;

        //   print(secondsLeft);
    }
}