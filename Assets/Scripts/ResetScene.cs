using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetScene : Singleton<ResetScene>
{
    Vector3 storePos = Vector3.zero;
    GameObject playerTransform;
    public bool AllowInput = false;
    [SerializeField] float witTimeToResetPos = 5;
    [SerializeField] int maxTemp = 12;
    int curTemp = 0;
    [SerializeField] GameObject gameOverObj;
    [SerializeField] TextMeshProUGUI tempTxt;
    [SerializeField] GameObject quizPanel;
    public bool AllowToMove = true;
    public float arcHeight = 2.0f; // Ketinggian arc saat kamera bergerak
    public Vector3 StorePos
    {
        get { return storePos; }
        set { storePos = value; }
    }
    private void Start()
    {
        AllowToMove = true;
        AllowInput = true;
        gameOverObj.SetActive(false);
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
    public void StorePlayer(GameObject obj, Transform t)
    {
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

        AllowInput = false;
        ShowQuizPanel();
        curTemp--;
        if (curTemp > 0)
            StartCoroutine(WaitToResetPos(callback));
        else
            gameOverObj.SetActive(true);
        ShowTempTxt(true);
    }
    IEnumerator WaitToResetPos(Action callback)
    {
        yield return new WaitForSeconds(witTimeToResetPos);
        Debug.LogError("Reset Player Pos");
        ShowTempTxt();
        ShowQuizPanel();
        if (PlayerManager.Instance != null) PlayerManager.Instance.transform.position = storePos;
        else Debug.LogError("Player Manager is NULL");
        AllowInput = true;
        if (callback != null)
            callback.Invoke();
        PlayerManager.Instance.curIndex = 0;
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
