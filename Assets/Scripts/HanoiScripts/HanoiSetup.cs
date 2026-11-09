using UnityEngine;
using UnityEngine.UI;

public class HanoiSetup : MonoBehaviour
{
    public PoleScript startPole;

    [Tooltip("Visi bloki sâkot ar lielâko uz mazâko")]
    public BlockDragAndDropScript[] blocks;
    public float adTimeReduction = 60f;

    public static HanoiSetup Instance { get; private set; }
    public PoleScript goalPole;
    public static bool gameWon;

    [Header("UI")]
    public Text timerText;
    public Text finalTimeText;
    public GameObject winPanel;

    [Header("Star rating")]
    public float threeStarTime = 180f; // sekundēs (3 min un mazâk ir 3 zvaigznes)
    public float twoStarTime = 300f; // 5min un <

    [Tooltip("Indeks 0 = 1 star, 1 = 2 stars, 2 = 3 stars")]
    public Sprite[] winPanels;
    public Image panelImage;

    private float elapsed;
    public static bool timerRunning = false;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // priekš portreta skata
        Screen.orientation = ScreenOrientation.AutoRotation;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        

        SetupGame();
    }

    void Update()
    {
        if (!timerRunning) return;

        elapsed += Time.deltaTime;
        if (timerText != null)
            timerText.text = FormatTime(elapsed);
    }

    // publisks, lai varçtu izveidot restartçðanas pogu vçlâk
    public void SetupGame()
    {
        Canvas.ForceUpdateCanvases(); // lai bloku izmçri bûtu lîdz galam izveidoti

        foreach (PoleScript pole in PoleScript.All)
            pole.stack.Clear();

        // katram blokam pievieno bloka lielumu
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].size = blocks.Length - i;
            blocks[i].currentPole = null;
            startPole.PlaceBlock(blocks[i], true);
        }

        elapsed = 0f;
        gameWon = false;
        if (winPanel != null) winPanel.SetActive(false);
        if (timerText != null) timerText.text = FormatTime(0f);
    }

    public void CheckWin()
    {
        if (gameWon) return;

        if (goalPole.stack.Count == blocks.Length)
        {
            gameWon = true;
            timerRunning = false;

            string finalTime = FormatTime(elapsed);
            if (timerText != null) timerText.text = finalTime;
            if (finalTimeText != null) finalTimeText.text = finalTime;

            int stars = GetStars(elapsed);
            if (panelImage != null && winPanels != null && winPanels.Length >= stars)
                panelImage.sprite = winPanels[stars - 1];

            if (winPanel != null) winPanel.SetActive(true);

            Debug.Log("YOU WIN! Time: " + finalTime);
        }
    }
    int GetStars(float seconds)
    {
        if (seconds <= threeStarTime) return 3;
        if (seconds <= twoStarTime) return 2;
        return 1;
    }

    static string FormatTime(float seconds)
    {
        return System.TimeSpan.FromSeconds(seconds).ToString("hh\\:mm\\:ss\\.ff");
    }
    public void ReduceTime(float seconds)
    {
        if (gameWon) return;
        elapsed = Mathf.Max(0f, elapsed - seconds); // nekad neaiziet zem 0 sekundçm
        if (timerText != null)
            timerText.text = FormatTime(elapsed);
    }

    void OnDestroy()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }
}
