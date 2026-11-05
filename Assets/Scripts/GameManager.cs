using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Rules")]
    public float gameLength = 60f;

    [Header("UI")]
    public GameObject startPanel;
    public GameObject resultPanel;
    public TMP_Text p1ScoreText;
    public TMP_Text p2ScoreText;
    public TMP_Text timerText;
    public TMP_Text p1DestinyText;
    public TMP_Text p2DestinyText;

    [Header("Audio")]
    public AudioSource sfxSource;
    public AudioClip gameOverSound;

    public bool IsPlaying { get; private set; }
    public float Progress => 1f - timeLeft / gameLength;   // 0 at start, 1 at the end

    int p1Score, p2Score;
    float timeLeft;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeLeft = gameLength;
        startPanel.SetActive(true);
        resultPanel.SetActive(false);
        UpdateUI();
    }

    void Update()
    {
        if (!IsPlaying) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndGame();
        }
        UpdateUI();
    }

    public void StartGame()      // START button
    {
        startPanel.SetActive(false);
        IsPlaying = true;
    }

    public void Restart()        // PLAY AGAIN button
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void AddScore(int player, int amount)
    {
        if (!IsPlaying) return;
        if (player == 1) p1Score = Mathf.Max(0, p1Score + amount);
        else             p2Score = Mathf.Max(0, p2Score + amount);
        UpdateUI();
    }

    public void PlaySfx(AudioClip clip)
    {
        if (clip != null) sfxSource.PlayOneShot(clip);
    }

    void UpdateUI()
    {
        p1ScoreText.text = "P1: " + p1Score;
        p2ScoreText.text = "P2: " + p2Score;
        timerText.text = Mathf.CeilToInt(timeLeft).ToString();
    }

    void EndGame()
    {
        IsPlaying = false;
        PlaySfx(gameOverSound);
        resultPanel.SetActive(true);

        string win  = "DESTINY: Graduates with honors and becomes a real engineer!";
        string lose = "DESTINY: Retakes CPE102 next semester... the bugs won.";
        string tie  = "DESTINY: Lab partners forever. You co-found a startup together!";

        string p1Destiny = p1Score > p2Score ? win : (p1Score < p2Score ? lose : tie);
        string p2Destiny = p2Score > p1Score ? win : (p2Score < p1Score ? lose : tie);

        p1DestinyText.text = "PLAYER 1  -  " + p1Score + " pts\n\n" + p1Destiny;
        p2DestinyText.text = "PLAYER 2  -  " + p2Score + " pts\n\n" + p2Destiny;
    }
}