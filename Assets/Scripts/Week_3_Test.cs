using UnityEngine;

public class Test : MonoBehaviour
{
    float jumpForce = 5.0f;
    float gravity = 9.8f;
    float groundY = 0.0f;
    bool isJumping = false;
    float verticalVelocity = 0f;
    bool groundYSet = false;

    // 보이지 않는 벽 (X 범위 제한)
    const float minX = -2f;
    const float maxX = 2f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Position: " + transform.position);
        Debug.Log("Rotation (Quaternion): " + transform.rotation);
        Debug.Log("Euler Angles: " + transform.eulerAngles);
        Debug.Log("Forward: " + transform.forward);
        Debug.Log("Up: " + transform.up);
        Debug.Log("Right: " + transform.right);
    }

    // Update is called once per frame
    void Update()
    {
        basic_move();

        // 점프 시작 위치 기록 (최초 1회)
        if (!groundYSet)
        {
            groundY = transform.position.y;
            groundYSet = true;
        }

        // 스페이스바를 누르면 점프
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            isJumping = true;
            verticalVelocity = jumpForce;
        }

        // 점프 중이면 물리 적용
        if (isJumping)
        {
            verticalVelocity -= gravity * Time.deltaTime;
            Vector3 pos = transform.position;
            pos.y += verticalVelocity * Time.deltaTime;

            // 땅 이하로 내려가면 정지
            if (pos.y <= groundY)
            {
                pos.y = groundY;
                isJumping = false;
                verticalVelocity = 0f;
            }
            transform.position = pos;
        }

        // X -2 ~ 2 범위 밖으로 나가지 못하도록 제한 (if 처리)
        Vector3 pos2 = transform.position;
        if (pos2.x < minX)
            pos2.x = minX;
        if (pos2.x > maxX)
            pos2.x = maxX;
        transform.position = pos2;
    }

    void basic_move()
    {
        float moveSpeed = 5f;
        float rotateSpeed = 100f;

        // 오른쪽 화살표 입력 처리
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * moveSpeed * Time.deltaTime, Space.World);
        }

        // 왼쪽 화살표 입력 처리
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.World);
        }

        // R키 입력 처리
        if (Input.GetKey(KeyCode.R))
        {
            transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime, Space.World);
        }

    }
}
