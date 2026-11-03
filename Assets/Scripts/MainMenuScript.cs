using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    public Button startButton;
    public Button hanoiButton;
    public Animator fadeAnim;

    private void Start()
    {
        startButton.Select();
        hanoiButton.Select();
    }

    public void FadeOutCity()
    {
        fadeAnim.SetBool("AlphaFade", true);

        StartCoroutine(WaitForFadeCity());
    }

    public void FadeOutHanoi()
    {
        fadeAnim.SetBool("AlphaFade", true);

        StartCoroutine(WaitForFadeHanoi());
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

    private IEnumerator WaitForFadeCity()
    {
        yield return new WaitForSeconds(1f);

        LoadScene("CityScene");
    }

    private IEnumerator WaitForFadeHanoi()
    {
        yield return new WaitForSeconds(1f);

        LoadScene("HanoiTower");
    }
}
