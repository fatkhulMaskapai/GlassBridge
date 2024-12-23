using DG.Tweening;
using System.Collections;
using UnityEngine;

public class MoveToObject : MonoBehaviour
{
    [SerializeField] int glassIndex = 0;
    //public Camera player; // Kamera utama yang akan bergerak
    [HideInInspector]
    public PlayerManager player; // Kamera utama yang akan bergerak
    public float moveDuration = 1.0f; // Durasi perpindahan kamera
    public Vector3 offset = new Vector3(0, 2, -5); // Offset dari posisi target
    float arcHeight = 2.0f; // Ketinggian arc saat kamera bergerak
    ResetScene rs;
    [HideInInspector]
    public bool allowToMove = true;

    private void Start()
    {
        allowToMove = true;
        rs = ResetScene.Instance;
        player = PlayerManager.Instance;
        arcHeight = rs.arcHeight;
    }
    private void OnMouseDown()
    {
        if (!rs.AllowInput || !rs.AllowToMove || !allowToMove) return;
        if (player == null)
        {
            Debug.LogError("Main Camera belum diatur!");
            return;
        }
        if (player.curIndex == glassIndex || player.curIndex == glassIndex - 1)
            StartCoroutine(WaitJump());
        else
            Debug.LogError($"Please move to near Glass, playerIndex: {player.curIndex}, glassIdIndex: {glassIndex}");
    }
    IEnumerator WaitJump()
    {
        rs.AllowToMove = false;
        // Hitung posisi target dengan offset
        Vector3 targetPosition = transform.position + offset;
        //InputManager.Instance.ForceJump();
        yield return new WaitForSeconds(0.1f);
        DOTween.Kill(player);

        // Hitung posisi tengah untuk membuat gerakan arc
        Vector3 middlePosition = new Vector3(
            (  player.transform.position.x + targetPosition.x) / 2,
            Mathf.Max(  player.transform.position.y, targetPosition.y) + arcHeight,
            (  player.transform.position.z + targetPosition.z) / 2
        );
        float t = moveDuration / 2;
        player.transform.DOMove(middlePosition, t).SetEase(Ease.OutQuad);
        yield return new WaitForSeconds(t);
        player.transform.DOMove(targetPosition, t).SetEase(Ease.InQuad);
        //Sequence moveSequence = DOTween.Sequence();
        //moveSequence.Append(  player.transform.DOMove(middlePosition, moveDuration / 2).SetEase(Ease.OutQuad));
        //moveSequence.Append(  player.transform.DOMove(targetPosition, moveDuration / 2).SetEase(Ease.InQuad));

        //Vector3 direction = transform.position -   player.transform.position;
        //Quaternion targetRotation = Quaternion.LookRotation(direction);
        //player.transform.DORotateQuaternion(targetRotation, moveDuration).SetEase(Ease.InOutQuad);
        yield return new WaitForSeconds(moveDuration);
        player.curIndex++;
        rs.AllowToMove = true;
    }
}
