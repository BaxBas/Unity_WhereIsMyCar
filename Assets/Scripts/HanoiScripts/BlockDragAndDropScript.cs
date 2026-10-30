using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDragAndDropScript : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("Optional. Leave empty to allow dragging anywhere.")]
    public ScreenBoundariesScript screenBoundariesScript;

    public float returnDuration = 0.2f;
    [HideInInspector] public bool droppedOnPole; // priekð staba skripta OnDrop

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 dragOffset;
    private Vector3 startPosition;
    private int startSiblingIndex;

    [Range(0f, 1f)]
    [Tooltip("Fraction of the block that must overlap a pole to count as dropped on it")]
    public float minOverlap = 0.15f;

    private readonly Vector3[] corners = new Vector3[4];

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (screenBoundariesScript == null)
            screenBoundariesScript = Object.FindFirstObjectByType<ScreenBoundariesScript>();
    }

    // pârvçrs recttranform taisnstûrî, lai strâdâtu uz jebkura ekrâna izmçra
    Rect GetWorldRect()
    {
        rectTransform.GetWorldCorners(corners);
        return new Rect(corners[0].x, corners[0].y,
                        corners[2].x - corners[0].x,
                        corners[2].y - corners[0].y);
    }

    // aprçíina bloka un staba pârklâðanâs zonu
    static float OverlapArea(Rect a, Rect b)
    {
        float w = Mathf.Min(a.xMax, b.xMax) - Mathf.Max(a.xMin, b.xMin);
        float h = Mathf.Min(a.yMax, b.yMax) - Mathf.Max(a.yMin, b.yMin);
        return (w > 0f && h > 0f) ? w * h : 0f;
    }

    // atrod to stabu, kuru pârklâj visvairâk (vajadzîgs situâcijâs, kad bloks pârklâj 2 stabus)
    PoleScript FindOverlappingPole()
    {
        Rect blockRect = GetWorldRect();
        float blockArea = blockRect.width * blockRect.height;

        PoleScript best = null;
        float bestArea = 0f;

        foreach (PoleScript pole in PoleScript.All)
        {
            float area = OverlapArea(blockRect, pole.GetWorldRect());
            if (area > bestArea && area >= blockArea * minOverlap)
            {
                bestArea = area;
                best = pole;
            }
        }
        return best;
    }

    private bool PointerToWorld(PointerEventData eventData, out Vector3 world)
    {
        RectTransform parentRect = rectTransform.parent as RectTransform;
        return RectTransformUtility.ScreenPointToWorldPointInRectangle(
            parentRect, eventData.position, eventData.pressEventCamera, out world); // pârvçrs kursora pozîciju (pret kameras ekrânu) uz world space pozîciju
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        StopAllCoroutines(); // atceï atgrieðanâs animâciju, ja tâda ðobrîd notiek

        startPosition = transform.position;
        startSiblingIndex = transform.GetSiblingIndex();
        droppedOnPole = false;

        BlockObjectsScript.isDragging = true;
        BlockObjectsScript.lastDragged = gameObject;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; // padara bloku "neredzamu" event sistemai, kas ïauj stabam zem bloka saòemt OnDrop
        transform.SetAsLastSibling();

        Vector3 pointerWorld;
        dragOffset = PointerToWorld(eventData, out pointerWorld) ? transform.position - pointerWorld : Vector3.zero; // bez ðîs atstarpes bloks tiktu pabîdîts, lai viòa centrs bûtu uz kursora
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 pointerWorld;
        if (!PointerToWorld(eventData, out pointerWorld))
            return;

        // kur blokam vajadzçtu bût, saglabâjot atstarpi starp bloka centru un kursoru
        Vector3 desired = pointerWorld + dragOffset; 
        desired.z = transform.position.z;

        // ja ir scripts, tad getclampedposition piespieþ desired bût iekð atïautâs zonas
        if (screenBoundariesScript != null)
        {
            Vector2 clamped = screenBoundariesScript.GetClampedPosition(desired);
            desired = new Vector3(clamped.x, clamped.y, desired.z);
        }

        transform.position = desired;
    }

    // padara bloku atkal "normâlu" (ieslçdz atpakaï blocksraycasts, lai to varçtu atkal paòemt utt.)
    public void OnEndDrag(PointerEventData eventData)
    {
        BlockObjectsScript.isDragging = false;
        BlockObjectsScript.lastDragged = null;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        PoleScript pole = FindOverlappingPole();
        droppedOnPole = pole != null;

        if (pole != null)
            pole.OnBlockDropped(this);
        else
            StartCoroutine(ReturnToStart());
    }

    IEnumerator ReturnToStart()
    {
        Vector3 from = transform.position;
        float t = 0f;

        while (t < returnDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(from, startPosition, t / returnDuration);
            yield return null;
        }

        transform.position = startPosition;
        transform.SetSiblingIndex(startSiblingIndex);
    }
}