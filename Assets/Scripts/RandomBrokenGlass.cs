using UnityEngine;

public class RandomBrokenGlass : MonoBehaviour
{
    public int glassIndex = 0;
    [SerializeField] BreakGlass[] breakGlass;
    void Start()
    {
        RandomBreak();
    }
    public void RandomBreak()
    {
        foreach (BreakGlass c in breakGlass)
            c.SetBreak(false, glassIndex);
        int i = Random.Range(0, breakGlass.Length);
        breakGlass[i].SetBreak(true, glassIndex);
    }
}
