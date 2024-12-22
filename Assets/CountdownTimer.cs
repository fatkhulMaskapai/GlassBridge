using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Untuk restart scene

public class CountdownTimer : MonoBehaviour
{
    public Text timerText; // Assign UI Text object here via Inspector
    public GameObject gameOverPanel; // Panel Game Over dengan tombol restart

    private float countdownTime = 60f; // Waktu hitung mundur dalam detik

    void Start()
    {
        if (timerText == null)
        {
            Debug.LogError("Timer Text belum diatur! Pastikan untuk menghubungkannya di Inspector.");
            enabled = false;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false); // Sembunyikan panel Game Over saat mulai
        }
    }

    void Update()
    {
        if (countdownTime > 0)
        {
            countdownTime -= Time.deltaTime;
            UpdateTimerDisplay();
        }
        else
        {
            countdownTime = 0; // Pastikan tidak negatif
            UpdateTimerDisplay();
            TriggerGameOver();
        }
    }

    void UpdateTimerDisplay()
    {
        int hours = Mathf.FloorToInt(countdownTime / 3600); // Hitung jam
        int minutes = Mathf.FloorToInt((countdownTime % 3600) / 60); // Hitung menit
        int seconds = Mathf.FloorToInt(countdownTime % 60); // Hitung detik

        timerText.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);
    }

    void TriggerGameOver()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true); // Tampilkan panel Game Over
        }
        enabled = false; // Hentikan update timer
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Restart scene aktif
    }
}
