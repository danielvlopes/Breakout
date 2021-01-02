using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Text ballsText;
    [SerializeField] private Text levelsText;
    [SerializeField] private Text scoreText;
    [SerializeField] private Text highScoreText;

    [SerializeField] private GameObject panelMenu;
    [SerializeField] private GameObject panelPlay;
    [SerializeField] private GameObject panelLevelCompleted;
    [SerializeField] private GameObject panelGameOver;
    [SerializeField] private GameObject[] levels;

    public static GameManager Instance { get; private set; }

    public enum State { MENU, INIT, PLAY, LEVECOMPLETED, LOADLEVEL, GAMEOVER };
    private State currentState;
    private GameObject currentBall;
    private GameObject currentLevel;
    private GameObject currentPaddle;
    private bool isSwitchingState;

    private int score;
    public int Score
    {
        get { return score; }
        set {
          score = value;
          scoreText.text = "SCORE: " + score;
        }
    }

    private int level;
    public int Level
    {
        get { return level; }
        set {
            level = value;
            levelsText.text = "LEVEL: " + level;
        }
    }

    private int balls;
    public int Balls
    {
        get { return balls; }
        set {
            balls = value;
            ballsText.text = "BALLS: " + balls;
       }
    }

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        SwitchState(State.MENU);
    }

    public void PlayClicked()
    {
        SwitchState(State.INIT);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case State.MENU:
                break;
            case State.INIT:
                break;
            case State.PLAY:
                if (currentBall == null)
                {
                    if (Balls > 0)
                    {
                        currentBall = Instantiate(ballPrefab);
                    }
                    else
                    {
                        SwitchState(State.GAMEOVER);
                    }
                }

                if (currentLevel != null && currentLevel.transform.childCount == 0 && !isSwitchingState)
                {
                    SwitchState(State.LEVECOMPLETED);
                }
                break;
            case State.LEVECOMPLETED:
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                Destroy(currentPaddle);

                if (Input.anyKeyDown)
                {
                    SwitchState(State.MENU);
                }
                break;
        }

    }

    void BeginState(State newState)
    {
        switch (newState)
        {
            case State.MENU:
                panelMenu.SetActive(true);
                highScoreText.text = "High Score: " + PlayerPrefs.GetInt("highscore");
                break;
            case State.INIT:
                Cursor.visible = false;
                panelPlay.SetActive(true);
                Score = 0;
                Level = 0;
                Balls = 3;
                currentPaddle = Instantiate(playerPrefab);
                if ( currentLevel != null )
                {
                    Destroy(currentLevel);
                }
                SwitchState(State.LOADLEVEL);
                break;
            case State.PLAY:
                break;
            case State.LEVECOMPLETED:
                Destroy(currentBall);
                Destroy(currentLevel);
                Level++;
                panelLevelCompleted.SetActive(true);
                SwitchState(State.LOADLEVEL, 2f);
                break;
            case State.LOADLEVEL:
                if (Level >= levels.Length)
                {
                    SwitchState(State.GAMEOVER);
                }
                else
                {
                    currentLevel = Instantiate(levels[Level]);
                    SwitchState(State.PLAY);
                }

                break;
            case State.GAMEOVER:
                panelGameOver.SetActive(true);
                Cursor.visible = true;

                if (Score > PlayerPrefs.GetInt("highscore"))
                {
                    PlayerPrefs.SetInt("highscore", score);
                }
                break;
        }
    }

    void EndState()
    {
        switch (currentState)
        {
            case State.MENU:
                panelMenu.SetActive(false);
                break;
            case State.INIT:
                break;
            case State.PLAY:
                break;
            case State.LEVECOMPLETED:
                panelLevelCompleted.SetActive(false);
                break;
            case State.LOADLEVEL:
                break;
            case State.GAMEOVER:
                panelPlay.SetActive(false);
                panelGameOver.SetActive(false);
                break;
        }
    }

    public void SwitchState(State newState, float delay = 0)
    {
        StartCoroutine(SwitchDelay(newState, delay));
    }

    IEnumerator SwitchDelay(State newState, float delay)
    {
        isSwitchingState = true;
        yield return new WaitForSeconds(delay);
        EndState();
        currentState = newState;
        BeginState(newState);
        isSwitchingState = false;
    }
}
