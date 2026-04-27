using UnityEngine;

public class Move : MonoBehaviour
{
    // 이동 속도
    public float moveSpeed = 5f;

    [Header("Sprites")]
    public Sprite idleSprite;          // 기본 상태 스프라이트
    public Sprite jumpSprite;          // 제자리 점프 스프라이트
    public Sprite jumpForwardSprite;   // 앞으로 점프 스프라이트

    [Header("Jump Settings")]
    public float jumpHeight = 2f;      // 점프 높이
    public float jumpDuration = 0.5f;  // 점프에 걸리는 시간

    private SpriteRenderer spriteRenderer;

    private bool isJumping = false;    // 현재 점프 중인지 여부
    private float jumpTimer = 0f;      // 점프 진행 시간
    private Vector3 startPosition;     // 점프 시작 위치

    private bool isMovingRight = false; // 오른쪽으로 이동 중인지 (점프 방향 판단용)

    void Start()
    {
        // SpriteRenderer 컴포넌트 가져오기
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 시작할 때 기본 스프라이트로 설정
        if (spriteRenderer != null && idleSprite != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    void Update()
    {
        // 이동 방향 벡터 초기화
        Vector2 moveDirection = Vector2.zero;

        // 왼쪽 이동
        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            moveDirection.x -= 1f;
        }

        // 오른쪽 이동
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            moveDirection.x += 1f;
            isMovingRight = true; // 오른쪽 이동 상태 기록
        }

        // 위 이동
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            moveDirection.y += 1f;
        }

        // 아래 이동
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            moveDirection.y -= 1f;
        }

        // 대각선 이동 시 속도 일정하게 유지 (정규화)
        moveDirection = moveDirection.normalized;

        // 실제 이동 적용
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // 스페이스바 눌렀을 때 점프 시작 (점프 중 아닐 때만)
        if (Input.GetKeyDown(KeyCode.Space) && !isJumping)
        {
            StartJump();
        }

        // 점프 중이면 점프 업데이트 실행
        if (isJumping)
        {
            UpdateJump();
        }

        // 매 프레임 끝에서 초기화 (다음 프레임 다시 판단)
        isMovingRight = false;
    }

    void StartJump()
    {
        isJumping = true;      // 점프 시작
        jumpTimer = 0f;        // 시간 초기화
        startPosition = transform.position; // 현재 위치 저장

        // 점프 시 스프라이트 변경
        if (spriteRenderer != null)
        {
            // 오른쪽 이동 중이면 앞으로 점프 이미지
            if (isMovingRight && jumpForwardSprite != null)
            {
                spriteRenderer.sprite = jumpForwardSprite;
            }
            // 아니면 기본 점프 이미지
            else if (jumpSprite != null)
            {
                spriteRenderer.sprite = jumpSprite;
            }
        }
    }

    void UpdateJump()
    {
        // 시간 증가
        jumpTimer += Time.deltaTime;

        // 전체 점프 중 현재 진행 비율 (0 ~ 1)
        float progress = jumpTimer / jumpDuration;

        // 점프 끝났을 때
        if (progress >= 1f)
        {
            // y 위치를 시작 위치로 되돌림 (착지)
            transform.position = new Vector3(
                transform.position.x,
                startPosition.y,
                transform.position.z
            );

            isJumping = false; // 점프 종료

            // 다시 기본 스프라이트로 복귀
            if (spriteRenderer != null && idleSprite != null)
            {
                spriteRenderer.sprite = idleSprite;
            }
        }
        else
        {
            // 사인 함수로 자연스러운 점프 곡선 생성
            // 0 → 1 → 0 형태 (포물선 느낌)
            float height = Mathf.Sin(progress * Mathf.PI) * jumpHeight;

            // y만 변경해서 위아래 점프 구현
            transform.position = new Vector3(
                transform.position.x,
                startPosition.y + height,
                transform.position.z
            );
        }
    }
}