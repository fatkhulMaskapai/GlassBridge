using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuizShowUp : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("Show Quiz");
    }
}
