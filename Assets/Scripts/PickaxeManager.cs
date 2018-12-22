using System.Collections;
using UnityEngine;

public class PickaxeManager : MonoBehaviour
{
    private PickaxeSoundManager pickaxeSoundManager;

    private float cooldown;
    private float remainingCooldown;

    private float timeAfterHit;
    private WaitForSeconds delayAfterHit;

    private float castTime;
    private WaitForSeconds delayCastTime;

    private float minimumDistanceToHitRock;

    private Material material;

    public bool CanHitRock { get; private set; }
    public bool IsHittingRock { get; private set; }

    private PickaxeManager()
    {
        cooldown = 0.2f;

        castTime = 0.15f;
        delayCastTime = new WaitForSeconds(castTime);

        timeAfterHit = 0.1f;
        delayAfterHit = new WaitForSeconds(timeAfterHit);

        minimumDistanceToHitRock = 1;

        CanHitRock = true;
    }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
        pickaxeSoundManager = GetComponent<PickaxeSoundManager>();
    }

    public void HitRockInRange(Rock rock)
    {
        if(CanHitRock && Vector3.Distance(rock.transform.position, transform.position) <= minimumDistanceToHitRock)
        {
            CanHitRock = false;
            IsHittingRock = true;
            material.color = Color.red;
            pickaxeSoundManager.PlaySound(0);
            StartCoroutine(CastTime(rock));
        }
    }

    private IEnumerator CastTime(Rock rock)
    {
        yield return delayCastTime;

        DamageRock(rock);
    }

    private void DamageRock(Rock rock)
    {
        if(rock.Hit() == 0)
        {
            pickaxeSoundManager.PlaySound(1);
            StaticObjects.GameController.RockDestroyed(rock.Level);
            Destroy(rock.gameObject);
        }
        StartCoroutine(AfterHit());
    }

    private IEnumerator AfterHit()
    {
        yield return delayAfterHit;

        IsHittingRock = false;
        material.color = Color.white;
        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        remainingCooldown = cooldown;
        while (remainingCooldown > 0)
        {
            yield return null;

            remainingCooldown -= Time.deltaTime;
        }

        CanHitRock = true;
    }
}
