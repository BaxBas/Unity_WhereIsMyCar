using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PoleScript : MonoBehaviour
{
    public static readonly List<PoleScript> All = new List<PoleScript>();

    // bloki, kuri ir uz staba no lejas uz augðu
    public List<BlockDragAndDropScript> stack = new List<BlockDragAndDropScript>();

    private RectTransform rectTransform;
    private readonly Vector3[] corners = new Vector3[4];

    void Awake() { rectTransform = GetComponent<RectTransform>(); }
    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }

    public Rect GetWorldRect()
    {
        rectTransform.GetWorldCorners(corners);   // 0 = bottom-left, 2 = top-right
        return new Rect(corners[0].x, corners[0].y,
                        corners[2].x - corners[0].x,
                        corners[2].y - corners[0].y);
    }

    public void OnBlockDropped(BlockDragAndDropScript block)
    {
        // izòem bloku no iepriekðçjâ staba saraksta
        if (block.currentPole != null)
            block.currentPole.stack.Remove(block);
        stack.Remove(block);

        // aprçíina toròa augstumu = staba apakða + esoðo bloku augstumi
        Rect poleRect = GetWorldRect();
        float stackTop = poleRect.yMin;
        foreach (BlockDragAndDropScript b in stack)
            stackTop += b.GetWorldRect().height;

        // Move by the difference, so it works with any pivot
        Rect blockRect = block.GetWorldRect();
        Vector3 target = block.transform.position + new Vector3(
            poleRect.center.x - blockRect.center.x,   // stâv staba centrâ
            stackTop - blockRect.yMin,                // apakða virs toròa paðas augðas (apakðçjâ bloka vai staba apakðas)
            0f);

        stack.Add(block);
        block.currentPole = this;
        block.MoveTo(target);
    }
}
