using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System.Collections;   

public class CameraZoomScript : MonoBehaviour
{
    [Header("Zoom")]
    public float maxZoom = 530f;
    public float minZoom = 150f;
    public float pinchZoomSpeed = 0.9f;
    public float mouseScrollSpeed = 150f;
    
    [Header("Pan")]
    public float mouseFollowSpeed = 1f;
    public float touchPanSpeed = 1f;

    [Header("References")]
    public ScreenBoundariesScript screenBoundariesScript;

    public Camera mainCamera;
    Vector2 lastTouchPos;
    int panFingerId = -1;
    bool isTouchPan = false;

    float lastTapTime = 0f;
    public float doubleTapMaxDelay = 0.4f;
    public float doubleTapMaxDistance = 100f;

    [Header("Desktop Edge Pan")]
    public float edgePanSpeed = 300f;     // world units per second at full push
    [Range(0f, 0.95f)]
    public float deadZone = 0.6f;


    void Awake()
    {
        if(mainCamera == null)
        {
            mainCamera = GetComponent<Camera>();
        }
       
        if(screenBoundariesScript == null)
        {
            screenBoundariesScript = FindFirstObjectByType<ScreenBoundariesScript>();
        }
    }

    private void Start()
    {
        screenBoundariesScript.RecalculateBounds();
        transform.position = screenBoundariesScript.GetClampedCameraPosition(transform.position);
    }

    void Update()
    {
        if(ObjectTransformationScript.isTransforming)
        {
            return;
        }


#if UNITY_EDITOR || UNITY_STANDALONE
        DesktopFollowCursor();
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if ((Mathf.Abs(scroll) > Mathf.Epsilon))
            mainCamera.orthographicSize -= scroll * mouseScrollSpeed;

#else
    HandleTouch();
#endif

        if (Input.touchCount == 2)
            HandlePinch();

        //UpdateMaxZoom();
        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, minZoom, maxZoom);
        screenBoundariesScript.RecalculateBounds();
        transform.position = screenBoundariesScript.GetClampedCameraPosition(transform.position);
    }

    void HandleTouch()
    {
        if (Input.touchCount != 1)
            return;

        Touch touch = Input.GetTouch(0);
        if (IsTouchUIButton(touch.position))
            return;

        if (touch.phase == TouchPhase.Began)
        {
            float dt = Time.time - lastTapTime;
            if (dt <= doubleTapMaxDelay &&
                Vector2.Distance(touch.position, lastTouchPos) <= doubleTapMaxDistance)
            {
                StartCoroutine(ResetZoomSmooth());
                lastTapTime = 0f;
            }
            else
            {
                lastTapTime = Time.time;
            }

            lastTouchPos = touch.position;
            panFingerId = touch.fingerId;
            isTouchPan = true;

        }
        else if (touch.phase == TouchPhase.Moved && isTouchPan &&
            touch.fingerId == panFingerId)
        {
            Vector2 delta = touch.position - lastTouchPos;
            transform.Translate(ScreenDeltaToWorldDelta(delta) * touchPanSpeed, Space.World);
            lastTouchPos = touch.position;

        }
        else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
        {
            isTouchPan = false;
            panFingerId = -1;

        }
    }

    void DesktopFollowCursor()
    {
        Vector2 mouse = Input.mousePosition;
        if (mouse.x < 0 || mouse.x > Screen.width ||
            mouse.y < 0 || mouse.y > Screen.height)
            return;

        // -1..1 from the screen center
        Vector2 norm = new Vector2(
            (mouse.x / Screen.width - 0.5f) * 2f,
            (mouse.y / Screen.height - 0.5f) * 2f);

        Vector2 dir = new Vector2(ApplyDeadZone(norm.x), ApplyDeadZone(norm.y));

        transform.position += (Vector3)(dir * edgePanSpeed * Time.unscaledDeltaTime);
    }

    float ApplyDeadZone(float v)
    {
        float a = Mathf.Abs(v);
        if (a < deadZone) return 0f;
        return Mathf.Sign(v) * (a - deadZone) / (1f - deadZone);
    }

    void HandlePinch()
    {
        Touch touch0 = Input.GetTouch(0), 
            touch1 = Input.GetTouch(1);
        float prevDistance = 
        ((touch0.position - touch0.deltaPosition) - 
        (touch1.position - touch1.deltaPosition)).magnitude;

        float currentDistance = (touch0.position - touch1.position).magnitude;  
        mainCamera.orthographicSize -= (currentDistance - prevDistance) * pinchZoomSpeed;
    }

    bool IsTouchUIButton(Vector2 touchPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current); 
        pointerData.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach(RaycastResult result in results)
        {
            if(result.gameObject.GetComponent<UnityEngine.UI.Button>() != null)
            {
                return true;
            }
        }
        return false; 
    }

    Vector3 ScreenDeltaToWorldDelta(Vector2 delta)
    {
        float worldPerPixel = (2f * mainCamera.orthographicSize) / Screen.height;
        return new Vector3(delta.x * worldPerPixel, delta.y * worldPerPixel, 0f);
    }

    IEnumerator ResetZoomSmooth()
    {
        float duration = 0.25f;
        float elpased = 0f;
        float currentZoom = mainCamera.orthographicSize;
        float targetZoom = maxZoom;

        while (elpased < duration)
        {
            elpased += Time.unscaledDeltaTime;
            mainCamera.orthographicSize = 
                Mathf.Lerp(currentZoom, targetZoom, elpased / duration);
           screenBoundariesScript.RecalculateBounds();
            transform.position = 
                screenBoundariesScript.GetClampedCameraPosition(transform.position);

            yield return null;
        }

        mainCamera.orthographicSize = targetZoom;
        screenBoundariesScript.RecalculateBounds();
        transform.position = 
            screenBoundariesScript.GetClampedCameraPosition(transform.position);
    }

    //void UpdateMaxZoom()
    //{
    //   if(screenBoundariesScript == null || mainCamera == null)
    //        return;

    //    Rect wb = screenBoundariesScript.worldBounds;

    //    float maxZoomByHeight = wb.height / 2f;
    //    float maxZoomByWidth = (wb.width / 2f) / mainCamera.aspect;
    //    maxZoom = Mathf.Min(maxZoomByHeight, maxZoomByWidth);
    //}
}
