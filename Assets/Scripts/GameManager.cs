using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [HideInInspector]
    public Scientist possessedScientist;
    public Transform ExitDoor { get; private set; }
    public int killAmountForGettingA = 10;

    public enum State
    {
        IN_GAME,
        END
    }

    private State state = State.IN_GAME;

    private new AudioSource audio;
    private GameUi gameUi;
    private Scientist[] scientists;
    private float possessCounter = 10;
    private int killedScientistAmount = 0;
    private int rescuedScientistAmount = 0;
    private float timeWhenGameEnded = 0; //Uses Time.realtimeSinceStartup;

    private void Awake()
    {
        gameUi = FindObjectOfType<GameUi>();
        ExitDoor = GameObject.FindGameObjectWithTag("Exit").transform;
        scientists = FindObjectsOfType<Scientist>();

        audio = GetComponent<AudioSource>();

        SetState(State.IN_GAME);
        gameUi.UpdateKilledAndRescuedScientist(killedScientistAmount, rescuedScientistAmount);

        int i;
        for(i = 0; i < 13; i++)
        {
            print(GetGrade(i, killAmountForGettingA));
        }
    }

    private void SetState(State newState)
    {
        state = newState;
        gameUi.SetScreen(state);
    }

    // Start is called before the first frame update
    void Start()
    {
        //PossessRandomScientist();
    }

    // Update is called once per frame
    void Update()
    {
        possessCounter -= Time.deltaTime;
        if(possessCounter <= 0)
        {
            PossessRandomScientist();
        }

        gameUi.UpdateTimeCounter(possessCounter);

        if(state == State.END)
        {
            if(Input.anyKeyDown && Time.realtimeSinceStartup - timeWhenGameEnded > 1)
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
        possessCounter = 10;

        if(!IsThereInnocentScientist())
        {
            return;
        }

        audio.Play();

        int index = Random.Range(0, scientists.Length);

        if(scientists[index] != null && scientists[index].state == Scientist.State.INNOCENT)
        {
            if(possessedScientist)
            {
                possessedScientist.GetUnpossessed();
            }
            
            scientists[index].GetPossessed();
        }
        else
        {
            PossessRandomScientist();
        }
    }

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
        killedScientistAmount++;
        gameUi.UpdateKilledAndRescuedScientist(killedScientistAmount, rescuedScientistAmount);
        CheckIfGameEnded();
    }

    private void CheckIfGameEnded()
    {
        if(killedScientistAmount + rescuedScientistAmount >= scientists.Length -1)
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

        char c = (char) ('A' - (killAmount - killAmountForA));

        if(c > 'D')
        {
            return "F";
        }

        string text = "";
        text += c;
        return text;
    }
}
