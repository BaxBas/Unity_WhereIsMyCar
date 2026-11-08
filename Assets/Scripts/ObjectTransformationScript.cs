using UnityEngine;

public class ObjectTransformationScript : MonoBehaviour
{
    public GameObjectsScript gameObjectsScript;
    private bool canFlip = true;
    public float flipCd = .5f;

    void Awake()
    {
        gameObjectsScript = FindFirstObjectByType<GameObjectsScript>();
    }


    void Update()
    {
        if (GameObjectsScript.lastDragged != null)
        {
            if(Input.GetKey(KeyCode.Z))
            {
                GameObjectsScript.lastDragged.GetComponent<RectTransform>().Rotate(
                    0, 0, Time.deltaTime * 12);
            }

            if(Input.GetKey(KeyCode.X))
            {
                GameObjectsScript.lastDragged.GetComponent<RectTransform>().Rotate(
                    0, 0, -Time.deltaTime * 12);
            }

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if(GameObjectsScript.lastDragged.GetComponent<RectTransform>().localScale.y < 0.9f)
                {
                    GameObjectsScript.lastDragged.GetComponent<RectTransform>().localScale =
                    new Vector3(GameObjectsScript.lastDragged.GetComponent<RectTransform>().localScale.x,
                    GameObjectsScript.lastDragged.GetComponent<RectTransform>().localScale.y + 0.001f, 1f);
                }
            }

            RectTransform rt = GameObjectsScript.lastDragged.GetComponent<RectTransform>();
            Vector3 s = rt.localScale;
            float signX = s.x < 0 ? -1f : 1f;
            float absX = Mathf.Abs(s.x);
            float y = s.y;

            if (Input.GetKey(KeyCode.LeftArrow)) absX -= 0.001f;
            if (Input.GetKey(KeyCode.RightArrow)) absX += 0.001f;
            if (Input.GetKey(KeyCode.DownArrow)) y -= 0.001f;
            if (Input.GetKey(KeyCode.UpArrow)) y += 0.001f;

            absX = Mathf.Clamp(absX, 0.3f, 0.9f);
            y = Mathf.Clamp(y, 0.3f, 0.9f);

            rt.localScale = new Vector3(signX * absX, y, 1f);

            if (Input.GetKey(KeyCode.Space) && canFlip)
            {
                GameObjectsScript.lastDragged.GetComponent<RectTransform>().localScale =
                new Vector3(-GameObjectsScript.lastDragged.GetComponent<RectTransform>().localScale.x,
                GameObjectsScript.lastDragged.GetComponent<RectTransform>().localScale.y, 1f);

                // sâkt cooldown taimeri
                StartCoroutine(FlipCooldown());
            }
        }
    }

    //cooldown priekš flippošanas
    private System.Collections.IEnumerator FlipCooldown()
    {
        canFlip = false;
        yield return new WaitForSecondsRealtime(flipCd);
        canFlip = true;
    }
}
