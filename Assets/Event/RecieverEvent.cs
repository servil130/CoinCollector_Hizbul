using UnityEngine;

public class RecieverEvent : MonoBehaviour
{
    void OnEnable()
    {
        PemancarEvent.ButtonOnclick += Reaksi;
    }

    void OnDisable()
    {
        PemancarEvent.ButtonOnclick -= Reaksi;
    }

    void Reaksi()
    {
        Debug.Log("Lu di terima njir");
    }
}
