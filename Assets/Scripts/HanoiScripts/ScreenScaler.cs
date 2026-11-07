using UnityEngine;

[ExecuteAlways]
public class PlayfieldFitter : MonoBehaviour
{
    [Tooltip("Native pixel size of the background image")]
    public Vector2 designSize = new Vector2(1920, 1080);

    [Tooltip("Off = whole picture always visible (bars at the sides). On = fills the screen, edges get cropped.")]
    public bool cover = false;

    void OnEnable() { Apply(); }
    void Update() { Apply(); }

    void Apply()
    {
        RectTransform rt = transform as RectTransform;
        RectTransform parent = rt.parent as RectTransform;
        if (parent == null) return;

        rt.anchorMin = rt.anchorMax = rt.pivot = new Vector2(0.5f, 0.5f);
        rt.anchoredPosition = Vector2.zero;
        rt.sizeDelta = designSize;

        Vector2 p = parent.rect.size;
        float sx = p.x / designSize.x;
        float sy = p.y / designSize.y;
        float s = cover ? Mathf.Max(sx, sy) : Mathf.Min(sx, sy);
        rt.localScale = new Vector3(s, s, 1f);
    }
}