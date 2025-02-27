using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    private int value;

    private void Start()
    {
        value = UnityEngine.Random.Range(5, 10);
        float scaleValue = value / 5.0f;
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
