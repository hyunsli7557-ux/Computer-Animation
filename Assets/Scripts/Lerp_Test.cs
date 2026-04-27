using UnityEngine;
using UnityEngine.Apple;

public class Lerp_Test : MonoBehaviour
{
    public Vector2 destiantion;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.Lerp(transform.position, destiantion, Time.deltaTime);
    }
}
