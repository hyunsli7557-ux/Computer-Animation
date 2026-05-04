using UnityEngine;

// 배경을 계속 왼쪽으로 이동시키고,
// 화면 밖으로 나가면 다시 오른쪽으로 붙여서 "무한 스크롤" 만드는 스크립트
public class BackgroundController : MonoBehaviour
{
    [SerializeField, Range(1f, 50f)]
    float scrollSpeed = 10f; // 배경 이동 속도

    [SerializeField]
    Transform background1; // 첫 번째 배경

    [SerializeField]
    Transform background2; // 두 번째 배경 (이어붙일 용도)

    [SerializeField]
    UnityEngine.Camera targetCamera; // 기준이 되는 카메라

    private SpriteRenderer background1Renderer;
    private SpriteRenderer background2Renderer;

    void Start()
    {
        // 카메라가 지정 안 되어 있으면 메인 카메라 자동 할당
        if (targetCamera == null)
            targetCamera = UnityEngine.Camera.main;

        // 배경이 없으면 실행 중단
        if (background1 == null || background2 == null)
            return;

        // 배경의 크기/경계값을 얻기 위해 SpriteRenderer 가져오기
        background1Renderer = background1.GetComponent<SpriteRenderer>();
        background2Renderer = background2.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // 필수 요소 없으면 실행 안 함 (에러 방지)
        if (background1 == null || background2 == null || background1Renderer == null || background2Renderer == null)
            return;

        // 두 배경을 계속 왼쪽으로 이동
        MoveBackground(background1);
        MoveBackground(background2);

        // 화면 밖으로 나간 배경을 반대쪽 끝으로 이동시킴
        RepositionIfOutside(background1, background1Renderer, background2Renderer);
        RepositionIfOutside(background2, background2Renderer, background1Renderer);
    }

    void MoveBackground(Transform bg)
    {
        // 왼쪽(Vector3.left) 방향으로 속도 * 시간 만큼 이동
        bg.position += Vector3.left * scrollSpeed * Time.deltaTime;
    }

    void RepositionIfOutside(Transform movingBackground, SpriteRenderer movingRenderer, SpriteRenderer otherRenderer)
    {
        // 아직 화면 안에 있으면 아무것도 안 함
        if (movingRenderer.bounds.max.x > GetCameraLeftX())
            return;

        // 완전히 화면 왼쪽 밖으로 나갔으면
        // 다른 배경 오른쪽 끝 뒤에 붙임
        float newX = otherRenderer.bounds.max.x + movingRenderer.bounds.extents.x;

        // 위치 재설정 (x만 바꾸고 y,z는 유지)
        movingBackground.position = new Vector3(newX, movingBackground.position.y, movingBackground.position.z);
    }

    float GetCameraLeftX()
    {
        // 카메라 없으면 가장 작은 값 반환 (안전 처리)
        if (targetCamera == null)
            return float.NegativeInfinity;

        // 카메라와 현재 오브젝트 사이 거리 (z축 기준)
        float distance = Mathf.Abs(targetCamera.transform.position.z - transform.position.z);

        // Viewport (0,0.5) = 화면의 "왼쪽 중앙"
        // → 이걸 월드 좌표로 변환해서 "화면 왼쪽 끝 x좌표" 구함
        return targetCamera.ViewportToWorldPoint(new Vector3(0f, 0.5f, distance)).x;
    }
}