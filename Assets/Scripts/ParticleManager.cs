using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    // Mise en cache des systemes de particules
    private ParticleSystem RockDestruction;
    private ParticleSystem RockHit;
    
    // Holders
    [SerializeField] private GameObject RockDestructionHolder;
    [SerializeField] private GameObject RockHitHolder;
    [SerializeField] private GameObject DiamondHolder;
    
    
    private void Start()
    {
        RockDestructionHolder = Instantiate(RockDestructionHolder);
        RockDestruction = RockDestructionHolder.GetComponent<ParticleSystem>();

        RockHitHolder = Instantiate(RockHitHolder);
        RockHit = RockHitHolder.GetComponent<ParticleSystem>();

        Utility.ParticleManager = this;
    }

    public void PlayRockDestruction(Vector3 position)
    {
        RockDestructionHolder.transform.position = position;
        RockDestruction.Play();
    }

    public void PlayRockHit(Vector3 position)
    {
        RockHitHolder.transform.position = position;
        RockHit.Play();
    }

    public void PlaceDiamond(Transform parent)
    {
        Instantiate(DiamondHolder, parent.position, transform.rotation, parent);
    }
}
