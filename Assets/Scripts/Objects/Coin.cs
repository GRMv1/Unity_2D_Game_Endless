using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private int value;

    private static int MIN_VALUE = 5;
    private static int MAX_VALUE = 10;

    private void Start()
    {
        value = UnityEngine.Random.Range(MIN_VALUE, MAX_VALUE);
        float scaleValue = value / (float)MIN_VALUE;
        transform.localScale = new Vector3(scaleValue, scaleValue, 1);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        StartCoroutine(PickCoroutine());
    }

    private IEnumerator PickCoroutine()
    {
        GetComponentInChildren<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        GameManager.Instance.PlayerGotCoin(value);
        audioSource.Play();
        yield return new WaitUntil(() => !audioSource.isPlaying);
        Destroy(gameObject);

    }
}
