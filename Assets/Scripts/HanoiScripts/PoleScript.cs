using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PoleScript : MonoBehaviour
{
    public static readonly List<PoleScript> All = new List<PoleScript>();

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

    public void OnBlockDropped(BlockDragAndDropScript block) { }
}
