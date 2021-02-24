using UnityEngine;
public class DestroyOnTime : MonoBehaviour
{
    public float secondsToLive;

    void Start()
    {
        Destroy(gameObject, secondsToLive);
    }
}
