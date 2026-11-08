using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class GoBackScript : MonoBehaviour
{
    public Button backButton;

    private void Start()
    {
        backButton.Select();
    }

    public void GoBack()
    {
        SceneManager.LoadScene("StartMenu");
    }
}