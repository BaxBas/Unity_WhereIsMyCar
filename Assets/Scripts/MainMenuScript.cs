using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class MainMenuScript : MonoBehaviour
{
    public Button startButton;
    public Animator fadeAnim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        startButton.Select();
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

        LoadScene("CityScene");
    }
}
