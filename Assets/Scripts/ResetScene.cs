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
    public Vector3 StorePos
    {
        get { return storePos; }
        set { storePos = value; }
    }
    private void Start()
    {
        AllowInput = true;
        tempTxt.gameObject.SetActive(false);
        gameOverObj.SetActive(false);
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
        tempTxt.gameObject.SetActive(v);
    }
    public void StorePlayer(GameObject obj, Transform t)
    {
        playerTransform = obj;
        StorePos = t.position;
    }
    void ResetPlayerPos()
    {
        AllowInput = true;
        playerTransform.transform.position = storePos;
    }

    public void ResetSceneFunction()
	{
		SceneManager.LoadScene(0);
	}
    public void PrepareToResetPos()
    {
        if (!AllowInput) return;

        AllowInput = false;
        curTemp--;
        if (curTemp > 0)
            StartCoroutine(WaitToResetPos());
        else
            gameOverObj.SetActive(true);
        ShowTempTxt(true);
    }
    IEnumerator WaitToResetPos()
    {
        yield return new WaitForSeconds(witTimeToResetPos);
        Debug.LogError("Reset Player Pos");
        ShowTempTxt();
        ResetPlayerPos();

    }
}