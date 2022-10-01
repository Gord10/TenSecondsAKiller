using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scientist possessedScientist;
    public Transform exitDoor;

    private Scientist[] scientists;

    private void Awake()
    {
        exitDoor = GameObject.FindGameObjectWithTag("Exit").transform;
        scientists = FindObjectsOfType<Scientist>();
        
    }

    // Start is called before the first frame update
    void Start()
    {
        PossessRandomScientist();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            PossessRandomScientist();
        }
    }

    private void PossessRandomScientist()
    {
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
            Camera.main.transform.SetParent(scientists[index].transform);
            Camera.main.transform.localPosition = Vector3.back * 10;
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
}
