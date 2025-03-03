using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blade : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    private Vector3 startPosition;
    private bool directionX;

    private static int ROTATION_SPEED = 150;
    private static int MOVE_SPEED = 2;
    private void Start()
    {
        directionX = Random.Range(0, 2) == 0 ? true : false;
        startPosition = transform.position;
        StartCoroutine(SpinCoroutine());

        if(directionX)
            StartCoroutine(MoveCoroutineX());
        else
            StartCoroutine(MoveCoroutineY());
    }

    IEnumerator SpinCoroutine()
    {
        while(true)
        {
            transform.Rotate(Vector3.forward, ROTATION_SPEED * Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator MoveCoroutineX()
    {
        while (true)
        {
            float targetPositionX = startPosition.x + 3.0f;
            while (transform.position.x < targetPositionX)
            {
                transform.position += Vector3.right * MOVE_SPEED * Time.deltaTime;
                yield return null;
            }

            targetPositionX = startPosition.x - 3.0f;
            while (transform.position.x > targetPositionX)
            {
                transform.position += Vector3.left * MOVE_SPEED * Time.deltaTime;
                yield return null;
            }
        }
    }
    IEnumerator MoveCoroutineY()
    {
        while (true)
        {
            float targetPositionY = startPosition.y + 3.0f;
            while (transform.position.y < targetPositionY)
            {
                transform.position += Vector3.up * MOVE_SPEED * Time.deltaTime;
                yield return null;
            }

            targetPositionY = startPosition.y - 3.0f;
            while (transform.position.y > targetPositionY)
            {
                transform.position += Vector3.down * MOVE_SPEED * Time.deltaTime;
                yield return null;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        audioSource.Play();
        GameManager.Instance.PlayerGotHit();
    }
}
