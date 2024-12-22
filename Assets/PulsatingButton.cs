using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PulsatingButton : MonoBehaviour
{
    public float pulseSpeed = 2.0f; // Kecepatan detak
    public float scaleAmount = 0.1f; // Amplitudo detak
    public string targetSceneName = "SceneName"; // Nama scene tujuan

    private Vector3 originalScale; // Skala asli tombol
    private Button button; // Referensi ke komponen Button

    void Start()
    {
        // Simpan skala asli tombol
        originalScale = transform.localScale;

        // Ambil referensi ke komponen Button
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Tidak ditemukan komponen Button pada GameObject ini!");
            return;
        }

        // Tambahkan listener untuk klik tombol
        button.onClick.AddListener(OnButtonClick);
    }

    void Update()
    {
        // Animasi detak dengan menggunakan fungsi sinus
        float scaleOffset = Mathf.Sin(Time.time * pulseSpeed) * scaleAmount;
        transform.localScale = originalScale + new Vector3(scaleOffset, scaleOffset, 0);
    }

    void OnButtonClick()
    {
        // Muat scene tujuan
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogError("Nama scene tujuan belum diset!");
        }
    }
}
