using System;
using Unity.VisualScripting;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private GameObject snowman;
    private SnowballSpawner snowballSpawner;

    void Start()
    {
        snowballSpawner = FindObjectOfType<SnowballSpawner>();
        if (!GameManager.Instance.GameSnowStarted)
        {
            // make the snowman to have its up vector be the same as the floor's up vector
            
            //Quaternion rotation = Quaternion.Euler(GameManager.Instance.floor.gameObject.transform.rotation.eulerAngles.x - 90, GameManager.Instance.floor.gameObject.transform.rotation.eulerAngles.y, -GameManager.Instance.floor.gameObject.transform.rotation.eulerAngles.z);
            GameObject snowmanInstance = Instantiate(snowman, GameManager.Instance.floor.gameObject.transform);
            snowmanInstance.transform.position = new Vector3(0f, 1f, 5f);
            // Make the snowman's up vector the same as the floor's normal vector
            //snowmanInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, -GameManager.Instance.floorNormal);
            GameManager.Instance.GameSnowStarted = true;
            
            Debug.Log("Snowball spawned !");
        }
        FindObjectOfType<AudioManager>().Play("snowball_appear");
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with " + other.collider.name);
        if (snowballSpawner != null && !other.collider.CompareTag("Hand") && !other.collider.CompareTag("Snowball") && !other.collider.CompareTag("Player") && !other.collider.CompareTag("Fairy") && !snowballSpawner.hasInstantiatedSnowball)
        {
            Debug.Log("Collision detected with other than hand " + other.collider.name);
            Debug.Log("Snowball destroyed !");
            FindObjectOfType<AudioManager>().Play("snowball_miss");
            Destroy(gameObject);
        }
        else if (snowballSpawner == null)
        {
            FindObjectOfType<AudioManager>().Play("snowball_miss");
            Destroy(gameObject);
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger detected with " + other.name);
        if (!other.CompareTag("Hand") && !other.CompareTag("Snowball") && !other.CompareTag("Player") && !FindObjectOfType<SnowballSpawner>().hasInstantiatedSnowball)
        {
            Debug.Log("Trigger detected with other than hand " + other.name);
            // Destroy(gameObject);
        }
    }
}