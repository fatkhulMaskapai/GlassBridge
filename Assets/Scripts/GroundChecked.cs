using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundChecked : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Trigger: {other.gameObject}");
        ResetScene.Instance.PrepareToResetPos();
    }
}
