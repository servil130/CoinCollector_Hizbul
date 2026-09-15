using UnityEngine;

public class Ghost : Enemy
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public bool isGhost = true;

    public override void Serang()
    {
        Debug.Log("Ghost menyerang!");
    }
}
