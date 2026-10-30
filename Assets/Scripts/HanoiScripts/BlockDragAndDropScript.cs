using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDragAndDropScript : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("Optional. Leave empty to allow dragging anywhere.")]
    public ScreenBoundariesScript screenBoundariesScript;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector3 dragOffset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (screenBoundariesScript == null)
            screenBoundariesScript = Object.FindFirstObjectByType<ScreenBoundariesScript>();
    }

    private bool PointerToWorld(PointerEventData eventData, out Vector3 world)
    {
        RectTransform parentRect = rectTransform.parent as RectTransform;
        return RectTransformUtility.ScreenPointToWorldPointInRectangle(
            parentRect, eventData.position, eventData.pressEventCamera, out world); // pârvçrs kursora pozîciju (pret kameras ekrânu) uz world space pozîciju
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
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
    }
}