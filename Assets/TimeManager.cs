using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Untuk restart scene

public class TimeManager : Singleton<TimeManager>
{
    public Text timerText; // Assign UI Text object here via Inspector

    [SerializeField] float maxTime = 60f; // Waktu hitung mundur dalam detik
    float curTime = 0f;
    bool enableTime = true;
    void Start()
    {
        ShowTime(true);
        if (timerText == null)
        {
            Debug.LogError("Timer Text belum diatur! Pastikan untuk menghubungkannya di Inspector.");
            ShowTime(false);
        }
    }
    public void ResetTime()
    {
        curTime = 0;
        UpdateTimerDisplay();
    }
    public void ShowTime(bool v)
    {
        enableTime = v;
    }

    void Update()
    {
        switch (enableTime)
        {
            case true:
                if (curTime < maxTime)
                {
                    curTime += Time.deltaTime;
                    UpdateTimerDisplay();
                }
                else
                {
                    //curTime = 0; // Pastikan tidak negatif
                    UpdateTimerDisplay();
                    ResetScene.Instance.ShowGameOver(Vector3.one);
                    ShowTime(false);
                }
                break;
            default: break;
        }
    }

    void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(curTime / 3600); // Hitung jam
        int minutes = Mathf.FloorToInt((curTime % 3600) / 60); // Hitung menit
        int seconds = Mathf.FloorToInt(curTime % 60); // Hitung detik

        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart scene aktif
    }
}
