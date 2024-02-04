using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A vfx factory using recycling and pooling
/// </summary>
public class VFXFactory : MonoBehaviour
{
    private readonly Dictionary<VFXType, Queue<GameObject>> availableVFXByType = new Dictionary<VFXType, Queue<GameObject>>();

    [System.Serializable]
    public class VFXPrefab
    {
        public VFXType vfxType;
        public GameObject prefab;
        public int preinstantiateCount;
    }

    [SerializeField] private List<VFXPrefab> vfxPrefabs = new List<VFXPrefab>();

    public static VFXFactory Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("There is multiple instances of singleton VFXFactory");
            return;
        }

        Instance = this;

        foreach (VFXPrefab vfxPrefab in vfxPrefabs)
        {
            Debug.Log("Preinstantiate " + vfxPrefab.preinstantiateCount + " VFX of type " + vfxPrefab.vfxType);
            availableVFXByType.Add(vfxPrefab.vfxType, new Queue<GameObject>());
            PreinstantiateVFX(vfxPrefab.prefab, vfxPrefab.preinstantiateCount);
        }
    }

    public GameObject GetVFX(VFXType vfxType, Vector3 position)
    {
        Queue<GameObject> availableVFXs = Instance.availableVFXByType[vfxType];

        GameObject vfx = null;
        if (availableVFXs.Count > 0)
        {
            vfx = availableVFXs.Dequeue();
        }

        if (vfx == null)
        {
            // Instantiate a new VFX.
            vfx = InstantiateVFX(vfxType);
        }

        vfx.transform.position = position;
        vfx.SetActive(true);
        
        return vfx;
    }

    public static void ReleaseVFX(GameObject vfx, VFXType vfxType)
    {
        Queue<GameObject> availableVFXs = Instance.availableVFXByType[vfxType];
        vfx.SetActive(false);
        availableVFXs.Enqueue(vfx);
    }

    private void PreinstantiateVFX(GameObject vfxPrefab, int numberOfVFXToPreinstantiate)
    {
        VFXType vfxType = vfxPrefab.GetComponent<VFX>().Type;
        Queue<GameObject> vfxs = availableVFXByType[vfxType];

        for (int index = 0; index < numberOfVFXToPreinstantiate; index++)
        {
            GameObject vfx = InstantiateVFX(vfxType);
            vfx.SetActive(false);
            vfxs.Enqueue(vfx);
        }
    }

    private static GameObject InstantiateVFX(VFXType vfxType)
    {
        VFXPrefab vfxPrefab = Instance.vfxPrefabs.Find(v => v.vfxType == vfxType);

        if (vfxPrefab != null)
        {
            GameObject vfx = Instantiate(vfxPrefab.prefab);
            vfx.transform.parent = Instance.transform;
            return vfx;
        }
        else
        {
            Debug.LogError("VFXPrefab not found for type: " + vfxType);
            return null;
        }
    }
}
