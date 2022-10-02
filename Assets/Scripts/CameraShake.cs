using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public float shakeTime = 0.2f;
    public float shakeStrength = 1;

    public void Shake()
    {
        transform.DOShakePosition(shakeTime, shakeStrength);
    }
}
