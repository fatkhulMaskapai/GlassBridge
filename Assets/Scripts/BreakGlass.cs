using System;
using UnityEngine;

public class BreakGlass : MonoBehaviour
{
    [SerializeField] private Transform brokenGlass;
    [SerializeField] private float breakMagnitude = 10f;
    [SerializeField] private float radius = 4f;
    [SerializeField] private float explosionPower = 10f;
    [SerializeField] private float upwardForce = 3f;
    MoveToObject mo;
    private void Start()
    {
        mo = GetComponent<MoveToObject>();
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.relativeVelocity.magnitude > breakMagnitude)
        {
            if (mo != null)
                mo.allowToMove = false;
            //other.gameObject.GetComponent<PlayerManager>().isInteracting = true;
            //Destroy(gameObject);
            Instantiate(brokenGlass, transform.position, transform.localRotation);
            brokenGlass.transform.localScale = transform.localScale;
            ResetScene.Instance.PrepareToResetPos(() =>
            {
                gameObject.SetActive(false);
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
