using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // ===============================
    // 1. 가위바위보 선택 종류 정의
    // ===============================
    public enum Choice { None, Rock, Scissors, Paper }

    [Header("UI Buttons")]
    public Button buttonRock;        // 바위 버튼
    public Button buttonScissors;    // 가위 버튼
    public Button buttonPaper;       // 보 버튼
    public Button buttonRestart;     // 일반 리스타트 버튼

    [Header("Game Over UI")]
    public GameObject gameOverPanel;      // 게임 종료 화면 전체 패널
    public Button buttonGameOverRestart;  // 게임 종료 후 다시하기 버튼
    public Button buttonQuit;             // 게임 종료 버튼

    [Header("Display Images")]
    public Image imagePlayer;    // 플레이어 선택 이미지
    public Image imageComputer;  // 컴퓨터 선택 이미지

    [Header("Sprites")]
    public Sprite spriteRock;        // 바위 이미지
    public Sprite spriteScissors;    // 가위 이미지
    public Sprite spritePaper;       // 보 이미지

    [Header("Score Board")]
    public TextMeshProUGUI textScorePlayer;    // 플레이어 점수 표시
    public TextMeshProUGUI textScoreComputer;  // 컴퓨터 점수 표시

    [Header("Result Text")]
    public TextMeshProUGUI resultText;   // 승패 결과 텍스트
    public TextMeshProUGUI gameOverText;    //게임 결과 텍스트

    const int Winscore = 5;

    // ===============================
    // 2. 게임 상태 변수
    // ===============================
    private int scorePlayer = 0;      // 플레이어 점수
    private int scoreComputer = 0;    // 컴퓨터 점수

    private bool isGameOver = false;  // 게임 종료 여부
    private bool isAnimating = true;  // 이미지 애니메이션 여부

    // ===============================
    // 3. 애니메이션 관련 변수
    // ===============================
    private float animationInterval = 0.1f;  // 이미지 변경 속도
    private float animationTimer = 0f;       // 시간 누적용
    private int currentSpriteIndex = 0;      // 현재 이미지 인덱스
    private Sprite[] sprites;                // 바위/가위/보 배열

    // ===============================
    // 4. 컴퓨터 랜덤 선택
    // ===============================
    Choice GetComputerChoice()
    {
        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0: return Choice.Rock;
            case 1: return Choice.Scissors;
            case 2: return Choice.Paper;
            default: return Choice.Rock;
        }
    }

    // ===============================
    // 5. 승패 판단 + 점수 처리
    // ===============================
    string DetermineWinner(Choice player, Choice computer)
    {
        // 무승부
        if (player == computer)
            return "무승부";

        // 플레이어 승리 조건
        bool playerWins =
            (player == Choice.Rock && computer == Choice.Scissors) ||
            (player == Choice.Scissors && computer == Choice.Paper) ||
            (player == Choice.Paper && computer == Choice.Rock);

        // 점수 반영
        if (playerWins)
            scorePlayer++;
        else
            scoreComputer++;

        return playerWins ? "플레이어 승리!" : "컴퓨터 승리!";
    }

    // ===============================
    // 6. Choice → Sprite 변환
    // ===============================
    Sprite GetSpriteFromChoice(Choice choice)
    {
        switch (choice)
        {
            case Choice.Rock: return spriteRock;
            case Choice.Scissors: return spriteScissors;
            case Choice.Paper: return spritePaper;
            default: return spriteRock;
        }
    }

    // ===============================
    // 7. 플레이어 버튼 클릭 처리
    // ===============================
    void OnPlayerChoice(Choice choice)
    {
        // 게임 끝났으면 입력 무시
        if (isGameOver) return;

        // 애니메이션 멈춤
        isAnimating = false;

        // 컴퓨터 랜덤 선택
        Choice computerChoice = GetComputerChoice();

        // 이미지 적용
        imagePlayer.sprite = GetSpriteFromChoice(choice);
        imageComputer.sprite = GetSpriteFromChoice(computerChoice);

        // 승패 계산
        string result = DetermineWinner(choice, computerChoice);

        // 점수 UI 업데이트
        textScorePlayer.text = scorePlayer.ToString();
        textScoreComputer.text = scoreComputer.ToString();

        // 결과 출력
        resultText.text = result;
        Debug.Log("결과:" + result);

        // 게임 종료 체크
        if (scorePlayer >= Winscore || scoreComputer >= Winscore)
            EndGame();
    }

    void SetRpsButtonInteractable(bool value)
    {
        buttonRock.interactable = value;
        buttonScissors.interactable = value;
        buttonPaper.interactable= value;
    }

    // ===============================
    // 8. 게임 종료 체크 (5점 도달)
    // ===============================
    void EndGame()
    {
        if (isGameOver)
            return;

        isGameOver = true;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(true);

        //최종 승자 표시
        if (resultText != null)
        {
            if (scorePlayer > scoreComputer)
            {
                gameOverText.text = "User Win";
            }
            else if (scoreComputer > scorePlayer)
            {
                gameOverText.text = "Computer WIn";
            }
        }
        SetRpsButtonInteractable(true);
    }

    // ===============================
    // 9. 게임 전체 초기화 (재시작)
    // ===============================
    void OnRestart()
    {
        // 점수 초기화
        scorePlayer = 0;
        scoreComputer = 0;

        // UI 점수 초기화
        textScorePlayer.text = "0";
        textScoreComputer.text = "0";

        // 상태 초기화
        isGameOver = false;
        isAnimating = true;

        // 애니메이션 초기화
        isAnimating = true;
        animationTimer = 0f;
        currentSpriteIndex = 0;

        imagePlayer.sprite = spriteRock;
        imageComputer.sprite = spriteRock;

        // 게임 오버 UI 숨김
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        // 안내 문구
        resultText.text = "가위 바위 보 중 하나를 선택하세요!";
    }

    // ===============================
    // 10. 게임 종료 버튼
    // ===============================
    void OnQuit()
    {
        Application.Quit(); // 빌드에서 종료

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // 에디터 종료
#endif
    }

    // ===============================
    // 11. 시작 시 초기 설정
    // ===============================
    void Start()
    {
        // 버튼 이벤트 연결
        buttonRock.onClick.AddListener(() => OnPlayerChoice(Choice.Rock));
        buttonScissors.onClick.AddListener(() => OnPlayerChoice(Choice.Scissors));
        buttonPaper.onClick.AddListener(() => OnPlayerChoice(Choice.Paper));

        buttonRestart.onClick.AddListener(OnRestart);
        buttonGameOverRestart.onClick.AddListener(OnRestart);
        buttonQuit.onClick.AddListener(OnQuit);

        // 초기 텍스트
        resultText.text = "가위 바위 보 중 하나를 선택하세요!";

        // 애니메이션용 배열
        sprites = new Sprite[] { spriteRock, spriteScissors, spritePaper };

        // 게임오버 UI 초기 숨김
        gameOverPanel.SetActive(false);
    }

    // ===============================
    // 12. 매 프레임 실행 (애니메이션)
    // ===============================
    void Update()
    {
        // 애니메이션 꺼져있으면 실행 안 함
        if (!isAnimating) return;

        // 시간 누적
        animationTimer += Time.deltaTime;

        // 일정 시간마다 이미지 변경
        if (animationTimer >= animationInterval)
        {
            animationTimer = 0f;

            // 다음 이미지로 순환
            currentSpriteIndex = (currentSpriteIndex + 1) % 3;

            imagePlayer.sprite = sprites[currentSpriteIndex];
            imageComputer.sprite = sprites[(currentSpriteIndex + 1) % 3];
        }
    }
}