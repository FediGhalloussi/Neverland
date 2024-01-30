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

            MeshRenderer[] allObjectss = FindObjectsOfType<MeshRenderer>();

            foreach (var obj in allObjectss)
            {
                if (obj.GetComponent<Collider>() == null || obj)
                {
                    obj.AddComponent<BoxCollider>();
                }
            }

            OVRSemanticClassification[] allObjects = FindObjectsOfType<OVRSemanticClassification>();

            foreach (var obj in allObjects)
            {
                if (obj.gameObject.GetComponentInChildren<MeshRenderer>() != null)
                {
                    //obj.GetComponentInChildren<MeshRenderer>().enabled = false;
                }
            }

            // fix to desks - for some reason they are upside down with Meta's default code
            OVRSemanticClassification[] allClassifications = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Table))
                .ToArray(); // fix to desks - for some reason they are upside down with Meta's default code

            OVRSemanticClassification[] allClassificationsFloor = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Floor))
                .ToArray();

            GameManager.Instance.floor = allClassificationsFloor[0];
            // set the floor normal
            {
                // Get the OVRCameraRig object
                OVRCameraRig ovrCameraRig = FindObjectOfType<OVRCameraRig>();

                // Send a raycast from the camera position downwards
                RaycastHit hit;
                Debug.DrawRay(new Vector3(0f,0f,0f), GameManager.Instance.floor.transform.position*1000f, Color.magenta, 10000f);
                if (Physics.Raycast(new Vector3(0f,0f,0f), GameManager.Instance.floor.transform.position * 1000f, out hit))
                {
                    // If the raycast hits the floor, get the normal
                    Vector3 hitNormal = hit.normal;
                    Debug.DrawRay(GameManager.Instance.floor.transform.position, hitNormal*1000f, Color.cyan, 10000f);
                    GameManager.Instance.floorNormal = - hitNormal;
                }
            }

            OVRSemanticClassification[] allClassificationsCeiling = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Ceiling))
                .ToArray();
            GameManager.Instance.ceiling = allClassificationsCeiling[0];

            foreach (var classification in allClassifications)
            {
                transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y,
                    transform.localScale.z * -1);
            }

            /*foreach (var classification in allClassificationsFloor)
            {
                classification.AddComponent<BoxCollider>();
            }*/

            foreach (var classification in allClassificationsCeiling)
            {
                //find snow named gameobject and set its y position to the ceiling position + box collider y size
                GameObject snow = GameObject.Find("Snow");
                snow.transform.position = new Vector3(snow.transform.position.x,
                    classification.transform.position.y - .5f, snow.transform.position.z);
            }
        }
    }
}