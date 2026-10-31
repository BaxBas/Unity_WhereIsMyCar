using UnityEngine;

public class HanoiSetup : MonoBehaviour
{
    public PoleScript startPole;

    [Tooltip("Visi bloki sâkot ar lielâko uz mazâko")]
    public BlockDragAndDropScript[] blocks;

    void Start()
    {
        SetupGame();
    }

    // publisks, lai varçtu izveidot restartçðanas pogu vçlâk
    public void SetupGame()
    {
        Canvas.ForceUpdateCanvases(); // lai bloku izmçri bûtu lîdz galam izveidoti

        foreach (PoleScript pole in PoleScript.All)
            pole.stack.Clear();

        foreach (BlockDragAndDropScript block in blocks)
        {
            block.currentPole = null;
            startPole.PlaceBlock(block, true);
        }
    }
}
