using System.Linq;
using UnityEngine;

public class ParticleSystemShapeFitter : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public GameObject planeObject;

    void Start()
    {
        // find ovr scene manager component in scene
        var sceneManager = FindObjectOfType<OVRSceneManager>();
        sceneManager.SceneModelLoadedSuccessfully += OnSceneModelLoadedSuccessfully;
    }

    private void OnSceneModelLoadedSuccessfully()
        {
            planeObject = FindObjectsOfType<OVRSemanticClassification>()
                .Where(c => c.Contains(OVRSceneManager.Classification.Ceiling))
                .ToArray()[0].gameObject;
            
            if (particleSystem == null || planeObject == null)
            {
                Debug.LogError("Please assign Particle System and Plane Object in the inspector.");
                return;
            }

            // Get the plane's normal vector
            Vector3 planeNormal = planeObject.transform.up;

            // Set the particle system's main module's simulation space to World to ensure accurate rotation
            var mainModule = particleSystem.main;
            mainModule.simulationSpace = ParticleSystemSimulationSpace.World;

            // Align the particle system's rotation with the plane's normal
            particleSystem.transform.rotation = Quaternion.FromToRotation(Vector3.up, planeNormal);

            // Adjust the position of the particle system to match the plane's position
            particleSystem.transform.position = planeObject.transform.position;

            // Optional: You may need to adjust the scale based on your specific requirements
            // particleSystem.transform.localScale = planeObject.transform.localScale;

            // Access the shape module of the particle system
            var shapeModule = particleSystem.shape;

            // Set the shape type to Mesh
            shapeModule.shapeType = ParticleSystemShapeType.MeshRenderer;

            // Assign the plane's mesh to the particle system shape
            shapeModule.meshRenderer = planeObject.GetComponentInChildren<MeshRenderer>();

            // Optionally, adjust other shape parameters like radius or scale based on your requirements
            // shapeModule.radius = someValue;
            // shapeModule.scale = someValue;
        }
    
}