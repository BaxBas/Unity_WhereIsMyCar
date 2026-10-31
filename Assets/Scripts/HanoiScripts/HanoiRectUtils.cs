using UnityEngine;

public static class HanoiRectUtils
{
    static readonly Vector3[] corners = new Vector3[4];

    public static Rect GetWorldRect(RectTransform rt)
    {
        rt.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y,
                        corners[2].x - corners[0].x,
                        corners[2].y - corners[0].y);
    }

    // aprçíina bloka un staba pârklâðanos 
    public static float OverlapArea(Rect a, Rect b)
    {
        float w = Mathf.Min(a.xMax, b.xMax) - Mathf.Max(a.xMin, b.xMin);
        float h = Mathf.Min(a.yMax, b.yMax) - Mathf.Max(a.yMin, b.yMin);
        return (w > 0f && h > 0f) ? w * h : 0f;
    }
}
