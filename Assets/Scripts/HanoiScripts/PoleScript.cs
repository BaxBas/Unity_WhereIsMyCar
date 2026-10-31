using System.Collections.Generic;
using UnityEngine;

public class PoleScript : MonoBehaviour
{
    public static readonly List<PoleScript> All = new List<PoleScript>();

    // bloki, kuri ir uz staba no lejas uz augðu
    public List<BlockDragAndDropScript> stack = new List<BlockDragAndDropScript>();
    public BlockDragAndDropScript TopBlock => stack.Count > 0 ? stack[stack.Count - 1] : null;

    private RectTransform rectTransform;
    private readonly Vector3[] corners = new Vector3[4];

    void Awake() { rectTransform = GetComponent<RectTransform>(); }
    void OnEnable() { All.Add(this); }
    void OnDisable() { All.Remove(this); }

    public Rect GetWorldRect()
    {
        return HanoiRectUtils.GetWorldRect(rectTransform);
    }

    public void OnBlockDropped(BlockDragAndDropScript block)
    {
        PlaceBlock(block, false);
    }

    public void PlaceBlock(BlockDragAndDropScript block, bool instant)
    {
        // izòem bloku no iepriekðçjâ staba saraksta
        if (block.currentPole != null)
            block.currentPole.stack.Remove(block);
        stack.Remove(block);

        // aprçíina toròa augstumu = staba apakða + esoðo bloku augstumi
        Rect poleRect = HanoiRectUtils.GetWorldRect(rectTransform);
        float stackTop = poleRect.yMin;
        foreach (BlockDragAndDropScript b in stack)
            stackTop += HanoiRectUtils.GetWorldRect(b.Rect).height;

        // Move by the difference, so it works with any pivot
        Rect blockRect = HanoiRectUtils.GetWorldRect(block.Rect);
        Vector3 target = block.transform.position + new Vector3(
            poleRect.center.x - blockRect.center.x,   // stâv staba centrâ
            stackTop - blockRect.yMin,                // apakða virs toròa paðas augðas (apakðçjâ bloka vai staba apakðas)
            0f);

        stack.Add(block);
        block.currentPole = this;

        if (instant)
        {
            block.transform.position = target;
            block.droppedOnPole = true;
        }
        else
            block.MoveTo(target);
    }
}
