using UnityEngine;

public class VFX : MonoBehaviour
{
    [SerializeField] private VFXType vfxType;
    
    public VFXType Type => this.vfxType;
}
