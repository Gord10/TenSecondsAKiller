using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public float shakeTime = 0.2f;
    public float shakeStrength = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.S))
        {
            Shake();
        }*/
    }

    public void Shake()
    {
        transform.DOShakePosition(shakeTime, shakeStrength);
    }
}
