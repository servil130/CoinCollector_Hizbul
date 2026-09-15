using UnityEngine;
using System;

public class BelajarDelegate : MonoBehaviour
{
    public delegate void Delegate();

    void Start()
    {
        Delegate1();
        Delegate2();
        Delegate3();
    }
    void Delegate1()
    {
        Delegate Check = StatusCheck;
        Check();
    }
    void Delegate2()
    {
        Delegate Status = Sis;
        Status += Sis;
        Status();
    }
    void Delegate3()
    {
        Action Status = Main;
        Status += Main;
        Status();
    }

    void StatusCheck()
    {
        Debug.Log("Status: ");
    }
    void Sis()
    {
        Debug.Log("Dunia");
    }
    void Main()
    {
        Debug.Log("Iwak Tempe");
    }
}
