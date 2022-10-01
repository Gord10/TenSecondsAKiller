using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Scientist possessedScientist;
    public Transform exitDoor;

    private void Awake()
    {
        exitDoor = GameObject.FindGameObjectWithTag("Exit").transform;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
