using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetScene : Singleton<ResetScene>
{
    GameObject playerTransform;
    public bool AllowInput = false;
    [SerializeField] float witTimeToResetPos = 5;
    [SerializeField] int maxTemp = 12;
    int curTemp = 0;
    [SerializeField] TextMeshProUGUI tempTxt;
    [SerializeField] GameObject quizPanel;
    public bool AllowToMove = true;
    public float arcHeight = 2.0f; // Ketinggian arc saat kamera bergerak
    Vector3 storePos = Vector3.zero;
    [HideInInspector] public bool isGameover = false;
    [SerializeField] GameObject gameOverObj;
    [SerializeField] GameObject retryPopUp;
    [SerializeField] Ease ease;
    public Vector3 StorePos
    {
        get { return storePos; }
        set { storePos = value; }
    }
    private Quaternion storedRotation;
    private void Start()
    {
        isGameover = false;
        AllowToMove = true;
        AllowInput = true;
        retryPopUp.transform.localScale = Vector3.zero;
        gameOverObj.transform.localScale = Vector3.zero;

        //gameOverObj.SetActive(false);
        ShowQuizPanel();
        ResetTemp();
        ShowTempTxt();
    }

    void ResetTemp()
    {
        curTemp = maxTemp;
    }
    void ShowTempTxt(bool v = false)
    {
        tempTxt.text = curTemp.ToString();
        //tempTxt.gameObject.SetActive(v);
    }
    public void StorePlayer(GameObject obj, Transform t, Quaternion q)
    {
        storedRotation = q;
        playerTransform = obj;
        StorePos = t.position;
    }

    public void ResetSceneFunction()
	{
		SceneManager.LoadScene(0);
	}
    public void PrepareToResetPos(Action callback = null)
    {
        if (!AllowInput) return;
        Debug.LogError("Prepare to reset Pos");
        AllowInput = false;
        ShowQuizPanel();
        curTemp--;
        if (curTemp > 0)
        {
            retryPopUp.transform.DOScale(Vector3.one, 0.12f).SetEase(ease);
            StartCoroutine(WaitToResetPos(callback));
        }
        else
        {
            TimeManager.Instance.ShowTime(false);
            ShowGameOver(Vector3.one);
        }
        ShowTempTxt(true);
    }
    public void ShowGameOver(Vector3 target)
    {
        isGameover = true;
        gameOverObj.transform.DOScale(target, 0.12f).SetEase(ease);
    }
    IEnumerator WaitToResetPos(Action callback)
    {
        yield return new WaitForSeconds(witTimeToResetPos);
        Debug.LogError("Reset Player Pos");
        ShowTempTxt();
        ShowQuizPanel();
        retryPopUp.transform.DOScale(Vector3.zero, .12f).SetEase(ease);
        if (callback != null)
            callback.Invoke();

        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.transform.rotation = storedRotation;
            PlayerManager.Instance.transform.position = storePos;
        }
        else Debug.LogError("Player Manager is NULL");

        PlayerManager.Instance.curIndex = 0;
        AllowInput = true;
    }
    public void ShowQuizPanel(bool v = false)
    {
        quizPanel.SetActive(v);
    }
    private void OnDestroy()
    {
        System.GC.Collect();
        StopAllCoroutines();
    }
}
