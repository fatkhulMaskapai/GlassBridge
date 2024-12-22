using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToChangeScene : MonoBehaviour
{
    // Nama scene yang dituju, dapat diatur dari Inspector
    [SerializeField]
    private string targetSceneName;

    void Update()
    {
        // Cek apakah layar disentuh atau mouse di-klik
        if (Input.GetMouseButtonDown(0))
        {
            ChangeScene();
        }
    }

    // Method untuk mengganti scene
    private void ChangeScene()
    {
        // Pastikan nama scene tidak kosong
        if (!string.IsNullOrEmpty(targetSceneName))
        {
            SceneManager.LoadScene(targetSceneName);
        }
        else
        {
            Debug.LogWarning("Target scene name is not set!");
        }
    }
}
