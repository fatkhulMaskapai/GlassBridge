using System;
using System.Runtime.InteropServices.ComTypes;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    [SerializeField] private Transform brokenGlass;
    [SerializeField] private float breakMagnitude = 10f;
    [SerializeField] private float radius = 4f;
    [SerializeField] private float explosionPower = 10f;
    [SerializeField] private float upwardForce = 3f;
    [SerializeField] MoveToObject moveToObject;
    MoveToObject mo;
    bool isAllow = true;
    bool isBreak = false;
    BrokenGls sr;
    private void Start()
    {
        mo = GetComponent<MoveToObject>();
        isAllow = true;
        sr = GetComponentInChildren<BrokenGls>();
        ApplyBroken();
    }
    void ApplyBroken(bool v = false)
    {
        if (sr != null)
            sr.gameObject.SetActive(v);
    }
    public void SetBreak(bool v, int index)
    {
        isBreak = v;
        moveToObject.glassIndex = index;
        ApplyBroken();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (!isAllow || isBreak) return;
        if (other.relativeVelocity.magnitude > breakMagnitude)
        {
            isAllow = false;
            if (mo != null)
                mo.allowToMove = false;
            //other.gameObject.GetComponent<PlayerManager>().isInteracting = true;
            //Destroy(gameObject);
            Instantiate(brokenGlass, transform.position, transform.localRotation);
            brokenGlass.transform.localScale = transform.localScale;
            Debug.LogError("<color=yellow>Prepare to reset Pos</color>");
            ResetScene.Instance.PrepareToResetPos(() =>
            {
                ApplyBroken(true);
            });
            //var explosionPos = transform.position;
            //var colliders = Physics.OverlapSphere(explosionPos, radius);
            //foreach (var hit in colliders)
            //{
            //    if (hit.GetComponent<Rigidbody>())
            //    {
            //        hit.GetComponent<Rigidbody>().AddExplosionForce(
            //            explosionPower*other.relativeVelocity.magnitude,
            //            explosionPos,radius,upwardForce);
            //    }
            //}
        }
    }
}
