using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUi : MonoBehaviour
{
    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI killedAndRescuedScientistCounter;

    public void UpdateTimeCounter(float time)
    {
        timeCounter.text = Mathf.Ceil(time).ToString();
    }

    public void UpdateKilledAndRescuedScientist(int killedAmount, int rescuedAmount)
    {
        killedAndRescuedScientistCounter.text = "Killed: " + killedAmount + "\nEscaped: " + rescuedAmount;
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
