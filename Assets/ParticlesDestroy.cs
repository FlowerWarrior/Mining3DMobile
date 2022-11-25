using UnityEngine;
using System.Collections;

public class ParticlesDestroy : MonoBehaviour
{
    private IEnumerator Start()
    {
        yield return new WaitForSeconds(GetComponent<ParticleSystem>().main.duration);
        Destroy(gameObject);
    }
}