using System;
using UnityEngine;

public class Snowball : MonoBehaviour
{
    [SerializeField] private GameObject snowman;
    void Start()
    {
        if (!GameManager.Instance.GameSnowStarted)
        {
            // make the snowman to have its up vector be the same as the floor's up vector
            
            //Quaternion rotation = Quaternion.Euler(GameManager.Instance.floor.gameObject.transform.rotation.eulerAngles.x - 90, GameManager.Instance.floor.gameObject.transform.rotation.eulerAngles.y, -GameManager.Instance.floor.gameObject.transform.rotation.eulerAngles.z);
            GameObject snowmanInstance = Instantiate(snowman/*, GameManager.Instance.floor.gameObject.transform.rotation*/);
            
            // Make the snowman's up vector the same as the floor's normal vector
            snowmanInstance.transform.rotation = Quaternion.FromToRotation(Vector3.up, -GameManager.Instance.floorNormal);
            GameManager.Instance.GameSnowStarted = true;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("Collision detected with " + other.collider.name);
        if (!other.collider.CompareTag("Hand") && !other.collider.CompareTag("Snowball") && !other.collider.CompareTag("Player") && !FindObjectOfType<SnowballSpawner>().hasInstantiatedSnowball)
        {
            Debug.Log("Collision detected with other than hand " + other.collider.name);
            // Destroy(gameObject);
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