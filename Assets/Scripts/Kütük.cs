using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kütük : MonoBehaviour
{
    public float[] hizlar;
    public float[] süreler;
    WheelJoint2D kütükWheel;
    JointMotor2D motorum;
    public int index;   
    void Start()
    {
        kütükWheel = GetComponent<WheelJoint2D>();
        motorum = new JointMotor2D();                     
        StartCoroutine("DönüsIslemi");
    }    
    IEnumerator DönüsIslemi()
    {        
        index = 0;
        while (true)
        {
            kütükWheel.motor = motorum;     //kütükWheel'ın motoru benim motorum oldu
            motorum.maxMotorTorque = 1000;
            motorum.motorSpeed = hizlar[index];
            süreler[0] = 0;
            yield return new WaitForSecondsRealtime(süreler[index]);
            hizlar[index] = Random.Range(hizlar[index] - 20, hizlar[index] + 20); //Hızlar bu range'te random olacağından şans faktörümüz de oldu            
            index++;
            if (index == hizlar.Length)
                index = 0;
        }
    }
}