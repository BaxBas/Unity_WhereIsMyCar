using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class BlockDragAndDropScript : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Tooltip("Optional. Leave empty to allow dragging anywhere.")]
    public ScreenBoundariesScript screenBoundariesScript;

    public float returnDuration = 0.1f;
    public bool droppedOnPole; // priekð staba skripta OnDrop
    public PoleScript currentPole;
    private bool dragAllowed;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    public RectTransform Rect => rectTransform;
    private Vector3 dragOffset;
    private Vector3 startPosition;
    private int startSiblingIndex;

    [Range(0f, 1f)]
    [Tooltip("Fraction of the block that must overlap a pole to count as dropped on it")]
    public float minOverlap = 0.15f;

    [Header("Shake")]
    public float shakeDuration = 0.3f;
    public float shakeStrength = 5f;
    public float shakeSpeed = 40f;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = transform.position;

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        if (screenBoundariesScript == null)
            screenBoundariesScript = Object.FindFirstObjectByType<ScreenBoundariesScript>();
    }

    // atrod to stabu, kuru pârklâj visvairâk (vajadzîgs situâcijâs, kad bloks pârklâj 2 stabus)
    PoleScript FindOverlappingPole()
    {
        Rect blockRect = HanoiRectUtils.GetWorldRect(rectTransform);
        float blockArea = blockRect.width * blockRect.height;

        PoleScript best = null;
        float bestArea = 0f;

        foreach (PoleScript pole in PoleScript.All)
        {
            float area = HanoiRectUtils.OverlapArea(blockRect, pole.GetWorldRect());
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

    bool IsTopOfStack()
    {
        return currentPole == null || currentPole.TopBlock == this;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragAllowed = IsTopOfStack();
        if (!dragAllowed)
        {
            Shake();
            return;
        }
        StopAllCoroutines(); // atceï atgrieðanâs animâciju, ja tâda ðobrîd notiek

        startPosition = transform.position;
        startSiblingIndex = transform.GetSiblingIndex();
        droppedOnPole = false;

        HanoiGameManager.isDragging = true;
        HanoiGameManager.lastDragged = gameObject;

        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false; // padara bloku "neredzamu" event sistemai, kas ïauj stabam zem bloka saòemt OnDrop
        transform.SetAsLastSibling();

        Vector3 pointerWorld;
        dragOffset = PointerToWorld(eventData, out pointerWorld) ? transform.position - pointerWorld : Vector3.zero; // bez ðîs atstarpes bloks tiktu pabîdîts, lai viòa centrs bûtu uz kursora
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragAllowed) return;

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
        if (!dragAllowed) return;

        HanoiGameManager.isDragging = false;
        HanoiGameManager.lastDragged = null;

        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        PoleScript pole = FindOverlappingPole();
        droppedOnPole = pole != null;

        if (pole != null)
            pole.OnBlockDropped(this);
        else
            MoveTo(startPosition, true);
    }

    public void MoveTo(Vector3 target, bool restoreSibling = false)
    {
        StopAllCoroutines();
        StartCoroutine(MoveRoutine(target, restoreSibling));
    }

    IEnumerator MoveRoutine(Vector3 target, bool restoreSibling)
    {
        Vector3 from = transform.position;
        float t = 0f;

        while (t < returnDuration)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(from, target, t / returnDuration);
            yield return null;
        }

        transform.position = target;
        if (restoreSibling)
            transform.SetSiblingIndex(startSiblingIndex);
    }
    public void Shake()
    {
        if (isShaking) return;   // lai nesakrâtos shake animâcijas (ja vairâkas reizes pçc kârtas mçìina pavilkt bloku)
        StartCoroutine(ShakeRoutine());
    }

    private bool isShaking;

    IEnumerator ShakeRoutine()
    {
        isShaking = true;
        Vector3 origin = transform.position;
        float t = 0f;

        while (t < shakeDuration)
        {
            t += Time.deltaTime;

            // ar laiku shake normistas jeb fadeout
            float fade = 1f - (t / shakeDuration); // lçnâm pâriet no 1 uz 0, lai radîtu fade efektu
            float offset = Mathf.Sin(t * shakeSpeed) * shakeStrength * fade; // mainâs starp -1 un 1, lai bloks kustçtots pa labi un kreisi

            transform.position = origin + new Vector3(offset, 0f, 0f);
            yield return null;
        }

        transform.position = origin;
        isShaking = false;
    }
}