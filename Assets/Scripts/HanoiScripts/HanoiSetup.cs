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

        // katram blokam pievieno bloka lielumu
        for (int i = 0; i < blocks.Length; i++)
        {
            blocks[i].size = blocks.Length - i;
            blocks[i].currentPole = null;
            startPole.PlaceBlock(blocks[i], true);
        }
    }
}
