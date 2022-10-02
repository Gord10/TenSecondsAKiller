using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUi : MonoBehaviour
{
    public GameObject inGameScreen;
    public GameObject endGameScreen;

    public TextMeshProUGUI timeCounter;
    public TextMeshProUGUI killedAndRescuedScientistCounter;
    public TextMeshProUGUI endGameText;

    public void UpdateTimeCounter(float time)
    {
        timeCounter.text = Mathf.Ceil(time).ToString();
    }

    public void UpdateKilledAndRescuedScientist(int killedAmount, int rescuedAmount)
    {
        killedAndRescuedScientistCounter.text = "Killed: " + killedAmount + "\nEscaped: " + rescuedAmount;
    }

    public void SetScreen(GameManager.State state)
    {
        inGameScreen.SetActive(state == GameManager.State.IN_GAME);
        endGameScreen.SetActive(state == GameManager.State.END);
    }

    public void UpdateEndGameText(int killAmount, string grade)
    {
        endGameText.text = killAmount + " kills\nGrade: " + grade;
    }
}
