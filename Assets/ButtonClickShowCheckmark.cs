using UnityEngine;
using UnityEngine.UI;

public class ButtonClickShowCheckmark : MonoBehaviour
{
    // Referensi untuk gambar checklist
    [SerializeField] private Image checkmarkImage;

    // Fungsi untuk menangani klik tombol
    public void OnButtonClick()
    {
        if (checkmarkImage != null)
        {
            // Toggle status aktif dari gambar checklist
            bool isActive = checkmarkImage.gameObject.activeSelf;
            checkmarkImage.gameObject.SetActive(!isActive);
        }
        else
        {
            Debug.LogError("Gambar checklist belum diatur!");
        }
    }
}
