using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameCamera : MonoBehaviour
{
    public float movementTime = 0.5f;

    public void GetAttachedToScientist(Scientist scientist)
    {
        transform.DOKill();
        transform.SetParent(scientist.transform);

        //Start moving to the scientist
        transform.DOLocalMove(new Vector3(0, 0, -10), movementTime);
    }
}
