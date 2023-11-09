using System.Collections;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace MetaAdvancedFeatures.SceneUnderstanding
{
    public class OVRSceneManagerAddons : MonoBehaviour
    {
        protected OVRSceneManager SceneManager { get; private set; }

        private void Awake()
        {
            SceneManager = GetComponent<OVRSceneManager>();
        }

        void Start()
        {
            SceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
        }

        private void OnSceneModelLoadedSuccessfully()
        {
            StartCoroutine(AddCollidersAndFixClassifications());
        }

        private IEnumerator AddCollidersAndFixClassifications()
        {
            yield return new WaitForEndOfFrame();

            MeshRenderer[] allObjects = FindObjectsOfType<MeshRenderer>();

            foreach (var obj in allObjects)
            {
                obj.enabled = false;
                if (obj.GetComponent<Collider>() == null || obj)
                {
                    obj.AddComponent<BoxCollider>();
                }
            }

            // fix to desks - for some reason they are upside down with Meta's default code
            OVRSemanticClassification[] allClassifications = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Table))
                .ToArray(); // fix to desks - for some reason they are upside down with Meta's default code

            OVRSemanticClassification[] allClassificationsFloor = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
                .ToArray();
            OVRSemanticClassification[] allClassificationsCeiling = FindObjectsOfType<OVRSemanticClassification>()
                                                                    .Where(c => c.Contains(OVRSceneManager.Classification.Ceiling))
                .ToArray();

            foreach (var classification in allClassifications)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                    transform.localScale.z * -1);
            }

            foreach (var classification in allClassificationsFloor)
            {
                classification.AddComponent<BoxCollider>();
            }
            
            foreach (var classification in allClassificationsCeiling)
            {
                //find snow named gameobject and set its y position to the ceiling position + box collider y size
                GameObject snow = GameObject.Find("Snow");
                Debug.Log("Snow position: " + snow.transform.position);
                snow.transform.position = new Vector3(snow.transform.position.x, classification.transform.position.y - .5f, snow.transform.position.z);
                Debug.Log("Snow new position: " + snow.transform.position);
            }
        }
    }
}