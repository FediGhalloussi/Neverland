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
}