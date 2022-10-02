using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public Scientist possessedScientist;
    public Transform ExitDoor { get; private set; }
    public int killAmountForGettingA = 10; //Player will get A grade if they kill this amount of scientists

    public enum State
    {
        IN_GAME,
        END
    }

    private State state = State.IN_GAME;

    private new AudioSource audio;
    private GameUi gameUi;
    private Scientist[] scientists;
    private CameraShake cameraShake;

    private float possessCounter = 10;
    private int killedScientistAmount = 0;
    private int rescuedScientistAmount = 0;
    private float timeWhenGameEnded = 0; //Uses Time.realtimeSinceStartup;


    private void Awake()
    {
        gameUi = FindObjectOfType<GameUi>();
        ExitDoor = GameObject.FindGameObjectWithTag("Exit").transform;
        scientists = FindObjectsOfType<Scientist>();
        cameraShake = FindObjectOfType<CameraShake>();

        audio = GetComponent<AudioSource>();

        SetState(State.IN_GAME);
        gameUi.UpdateKilledAndRescuedScientist(killedScientistAmount, rescuedScientistAmount);

        /*
        int i;
        for(i = 0; i < 13; i++)
        {
            print(GetGrade(i, killAmountForGettingA));
        }*/
    }

    private void SetState(State newState)
    {
        state = newState;
        gameUi.SetScreen(state);
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif

        //Possess a random scientist if the possessCounter reaches 0
        possessCounter -= Time.deltaTime;
        if(possessCounter <= 0)
        {
            PossessRandomScientist();
        }

        //Show time on UI
        gameUi.UpdateTimeCounter(possessCounter);

        if(state == State.END)
        {
            float timeThreshold = 1f;
            //Restart the game if the player has pressed any key after timeThreshold seconds
            if (Input.anyKeyDown && Time.realtimeSinceStartup - timeWhenGameEnded > timeThreshold)
            {
                RestartScene();
            }
        }
    }

    private void RestartScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(sceneName);
    }

    private void PossessRandomScientist()
    {
        possessCounter = 10; //Reset the counter

        if(!IsThereInnocentScientist()) //There is no scientist to possess, so return the function
        {
            return;
        }

        audio.Play();

        //Find a random scientist and possess him
        int index = Random.Range(0, scientists.Length); 

        if(scientists[index] != null && scientists[index].state == Scientist.State.INNOCENT) //Check if the scientist exists, he is innocent and alive
        {
            if(possessedScientist)
            {
                possessedScientist.GetUnpossessed();
            }
            
            scientists[index].GetPossessed();
        }
        else //The found scientist is not available for possession, possess another one
        {
            PossessRandomScientist();
        }
    }

    //Check is there is a scientist who is alive and innocent
    private bool IsThereInnocentScientist()
    {
        int i;
        for(i = 0; i < scientists.Length; i++)
        {
            if(scientists[i] != null && scientists[i].state == Scientist.State.INNOCENT)
            {
                return true;
            }
        }

        return false;
    }

    public void OnScientistRescue()
    {
        rescuedScientistAmount++;
        gameUi.UpdateKilledAndRescuedScientist(killedScientistAmount, rescuedScientistAmount);
        CheckIfGameEnded();
    }

    public void OnScientistKill()
    {
        cameraShake.Shake();
        killedScientistAmount++;
        gameUi.UpdateKilledAndRescuedScientist(killedScientistAmount, rescuedScientistAmount);
        CheckIfGameEnded();
    }

    private void CheckIfGameEnded()
    {
        if(killedScientistAmount + rescuedScientistAmount >= scientists.Length -1) //End game, because all scientists are either rescued or killed
        {
            SetState(State.END);
            gameUi.UpdateEndGameText(killedScientistAmount, GetGrade(killedScientistAmount, killAmountForGettingA));
            timeWhenGameEnded = Time.timeSinceLevelLoad;
        }
    }

    public string GetGrade(int killAmount, int killAmountForA)
    {
        if(killAmount > killAmountForA)
        {
            return "A+";
        }

        char c = (char) ('A' + (killAmountForA - killAmount));

        if(c > 'D')
        {
            return "F";
        }

        string text = "";
        text += c;
        return text;
    }
}
