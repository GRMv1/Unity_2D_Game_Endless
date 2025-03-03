using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingBlade : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private Transform bladeTransform;

    private static int ROTATION_SPEED = 150;
    private void Start()
    {
        StartCoroutine(SpinCoroutine());
    }

    IEnumerator SpinCoroutine()
    {
        while (true)
        {
            transform.Rotate(Vector3.forward, ROTATION_SPEED * Time.deltaTime);
            bladeTransform.Rotate(Vector3.forward, ROTATION_SPEED * Time.deltaTime);
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.Play();
        GameManager.Instance.PlayerGotHit();
    }
}
