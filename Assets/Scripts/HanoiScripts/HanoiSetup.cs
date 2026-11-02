using UnityEngine;
using UnityEngine.UI;

public class HanoiSetup : MonoBehaviour
{
    public PoleScript startPole;

    [Tooltip("Visi bloki sâkot ar lielâko uz mazâko")]
    public BlockDragAndDropScript[] blocks;

    public static HanoiSetup Instance { get; private set; }
    public PoleScript goalPole;
    public static bool gameWon;

    [Header("UI")]
    public Text timerText;
    public Text finalTimeText;
    public GameObject winPanel;

    private float elapsed;
    private bool timerRunning;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SetupGame();
    }

    void Update()
    {
        if (!timerRunning) return;

        elapsed += Time.unscaledDeltaTime;
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
        timerRunning = true;
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
            timerRunning = false;   // freezes the time

            string finalTime = FormatTime(elapsed);
            if (timerText != null) timerText.text = finalTime;
            if (finalTimeText != null) finalTimeText.text = "Time: " + finalTime;
            if (winPanel != null) winPanel.SetActive(true);

            Debug.Log("YOU WIN! Time: " + finalTime);
        }
    }

    static string FormatTime(float seconds)
    {
        return System.TimeSpan.FromSeconds(seconds).ToString("hh\\:mm\\:ss\\.ff");
    }
}
