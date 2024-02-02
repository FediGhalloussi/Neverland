using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassthroughFader : MonoBehaviour
{
    private Material mat;

    private Mesh mesh;

    [SerializeField] private ParticleSystem particleSystem;
    // Start is called before the first frame update
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void Disappear()
    {
        //particleSystem.Play();
        StartCoroutine(FadeAway(0.05f));
    }

    private IEnumerator FadeAway(float t)
    {
        while (mat.color.a < 1)
        {
            var c = mat.color;
            c.a += 0.02f;
            mat.color = c;
            yield return new WaitForSeconds(t);
        }
        Destroy(gameObject);
    }
}
