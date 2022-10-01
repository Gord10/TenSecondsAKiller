using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scientist possessedScientist;
    public Transform exitDoor;

    private GameUi gameUi;
    private Scientist[] scientists;
    private float possessCounter = 10;
    private int killedScientistAmount = 0;
    private int rescuedScientistAmount = 0;

    private void Awake()
    {
        gameUi = FindObjectOfType<GameUi>();
        exitDoor = GameObject.FindGameObjectWithTag("Exit").transform;
        scientists = FindObjectsOfType<Scientist>();

        gameUi.UpdateKilledAndRescuedScientist(killedScientistAmount, rescuedScientistAmount);
    }

    // Start is called before the first frame update
    void Start()
    {
        //PossessRandomScientist();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PossessRandomScientist();
        }

        possessCounter -= Time.deltaTime;
        if(possessCounter <= 0)
        {
            PossessRandomScientist();
        }

        gameUi.UpdateTimeCounter(possessCounter);
    }

    private void PossessRandomScientist()
    {
        possessCounter = 10;

        if(!IsThereInnocentScientist())
        {
            return;
        }

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
    }

    public void OnScientistKill()
    {
        killedScientistAmount++;
        gameUi.UpdateKilledAndRescuedScientist(killedScientistAmount, rescuedScientistAmount);
    }
}
