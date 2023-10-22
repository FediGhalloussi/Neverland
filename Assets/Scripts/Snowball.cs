using System;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private GameObject snowman;
    void Start()
    {
        if (!GameManager.Instance.GameSnowStarted)
        {
            Instantiate(snowman, new Vector3(0, 0, 5f), Quaternion.identity);
            GameManager.Instance.GameSnowStarted = true;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with " + other.collider.name);
        if (!other.collider.CompareTag("Hand") && !other.collider.CompareTag("Snowball"))
        {
            Debug.Log("Collision detected with other than hand " + other.collider.name);
            Destroy(gameObject);
        }
    }
}