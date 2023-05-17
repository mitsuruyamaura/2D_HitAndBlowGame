using UnityEngine;

public class GetInstanceIDExample : MonoBehaviour
{
    void Start()
    {
        Debug.Log("インスタンスID: " + this.GetInstanceID());
    }
}