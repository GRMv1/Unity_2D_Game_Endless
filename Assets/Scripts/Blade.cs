using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    private void Start()
    {
        StartCoroutine(SpinCoroutine());
    }

    IEnumerator SpinCoroutine()
    {
        while(true)
        {
            transform.Rotate(Vector3.forward, 150 * Time.deltaTime);
            yield return null;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.Play();
        GameManager.Instance.PlayerGotHit();
    }
}
