using UnityEngine;

public class Background : MonoBehaviour
{
    public Transform mainCamera;
    public float scrollSpeed = 0.5f;
    private Vector3 lastCameraPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastCameraPosition = mainCamera.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaMovement = mainCamera.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * scrollSpeed, 0f, 0f);
        lastCameraPosition = mainCamera.position;
    }
}
