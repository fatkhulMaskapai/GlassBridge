using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class TypingText : MonoBehaviour
{
    public Text uiText; // Referensi ke komponen Text
    [TextArea]
    public string fullText; // Teks lengkap yang akan diketik
    public float typingSpeed = 0.05f; // Waktu jeda antar karakter (detik)

    private void Start()
    {
        if (uiText != null)
        {
            StartCoroutine(TypeText());
        }
        else
        {
            Debug.LogError("Komponen Text belum dihubungkan!");
        }
    }

    private IEnumerator TypeText()
    {
        uiText.text = ""; // Kosongkan teks awal
        foreach (char letter in fullText.ToCharArray())
        {
            uiText.text += letter; // Tambahkan karakter berikutnya
            yield return new WaitForSeconds(typingSpeed); // Tunggu sebelum melanjutkan
        }
    }
}
