using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HanoiStartScript : MonoBehaviour
{
    public Button hanoiButton;
    public Animator fadeAnim;

    private void Start()
    {
        hanoiButton.Select();
    }

    public void FadeOut()
    {
        fadeAnim.SetBool("AlphaFade", true);

        StartCoroutine(WaitForFade());
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Game is exiting");
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator WaitForFade()
    {
        yield return new WaitForSeconds(1f);

        LoadScene("HanoiScene");
    }
}
