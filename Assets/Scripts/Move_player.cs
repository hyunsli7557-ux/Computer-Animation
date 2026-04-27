using UnityEngine;

public class Move_player : MonoBehaviour
{
    //이동 피라미터
    public float moveSpeed;
    public float jumpForce;
    private Rigidbody2D rd;

    private SpriteRenderer spriteRenderer; //이미지 제어
    private Animator animator; //애니메이션 제어
    private bool isGrounded = true; //땅에 있는지

    //애니메이션 변경 처리
    public RuntimeAnimatorController idleController;
    public RuntimeAnimatorController runController;
    public RuntimeAnimatorController jumpController;

    void SetAnimatorController(RuntimeAnimatorController controller) //애니메이션 변경 함수
    {
        if (animator != null && controller != null)
        {
            animator.runtimeAnimatorController = controller;
        }
    }

    public void OnCollisionEnter2D(Collision2D collision) //충돌감지
    {
        if (collision.gameObject.GetComponent<UnityEngine.Tilemaps.Tilemap>() != null)
        {
            isGrounded = true;

            SetAnimatorController(idleController);
        }
    }

    void MoveMent()
    {
        float moveInput = 0f;

        //wasd 또는 방항키로 이동 처리
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            moveInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            moveInput = 1f;
        }

        //Rigidbody 2D의 속도 옵션을 적용한 자연스러운 움직임 구
        rd.linearVelocity = new Vector2(moveInput * moveSpeed, rd.linearVelocity.y);

        //방향전환
        if (moveInput != 0 && spriteRenderer != null)
        {
            spriteRenderer.flipX = moveInput < 0;
        }

        if (isGrounded)
        {
            if (moveInput != 0)
            {
                SetAnimatorController(runController);
            }
            else
            {
                SetAnimatorController(idleController);
            }
        }
    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rd.linearVelocity = new Vector2(rd.linearVelocity.x, jumpForce);
            isGrounded = false;

            SetAnimatorController(jumpController);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rd = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        SetAnimatorController(idleController);
    }

    // Update is called once per frame
    void Update()
    {
        MoveMent();
        Jump();
    }
}
