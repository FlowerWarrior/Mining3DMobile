using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SellParticles : MonoBehaviour
{
    ParticleSystem moneyParticles;

    void Start()
    {
        moneyParticles = GetComponent<ParticleSystem>();
    }

    void OnEnable()
    {
        BackpackMgr.SoldItems += PlayParticles;
    }

    void OnDisable()
    {
        BackpackMgr.SoldItems -= PlayParticles;
    }

    void PlayParticles(int value)
    {
        moneyParticles.Play();
    }
}
