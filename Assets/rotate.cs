using UnityEngine;
using UnityEngine.UI;

public class rotate : MonoBehaviour
{
    // Referensi button
    public Button rotateButton;

    // Status rotasi
    private int orientationState = 0; // 0 = LandscapeRight, 1 = LandscapeLeft, 2 = Portrait

    private void Start()
    {
        // Tambahkan listener untuk tombol
        if (rotateButton != null)
        {
            rotateButton.onClick.AddListener(ToggleScreenOrientation);
        }
    }

    private void ToggleScreenOrientation()
    {
        switch (orientationState)
        {
            case 0:
                // Ubah ke Landscape Left
                Screen.orientation = ScreenOrientation.LandscapeLeft;
                orientationState = 1;
                break;

            case 1:
                // Ubah ke Portrait
                Screen.orientation = ScreenOrientation.Portrait;
                orientationState = 2;
                break;

            case 2:
                // Ubah ke Landscape Right
                Screen.orientation = ScreenOrientation.LandscapeRight;
                orientationState = 0;
                break;
        }
    }
}
