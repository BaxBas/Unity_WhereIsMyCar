using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class TimerScript : MonoBehaviour
{
    public Text finalTimeText;
    private Stopwatch m_timer;
    private void Start() => m_timer = Stopwatch.StartNew();
    private void Update()
    {
        finalTimeText.text = string.Format("{0:hh\\:mm\\:ss\\.ff}", m_timer.Elapsed);
    }
}
