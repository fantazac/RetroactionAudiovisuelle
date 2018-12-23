using System.Collections;
using UnityEngine;

public class PickaxeManager : MonoBehaviour
{
    private float cooldown;
    private float remainingCooldown;

    private float timeAfterHit;
    private WaitForSeconds delayAfterHit;

    private float castTime;
    private WaitForSeconds delayCastTime;

    private float pickaxeRange;

    private Material material;

    public bool CanHitRock { get; private set; }
    public bool CanMine { get; set; }
    public bool IsHittingRock { get; private set; }

    private PickaxeManager()
    {
        cooldown = 0.2f;

        castTime = 0.15f;
        delayCastTime = new WaitForSeconds(castTime);

        timeAfterHit = 0.1f;
        delayAfterHit = new WaitForSeconds(timeAfterHit);

        pickaxeRange = 1.2f;

        CanHitRock = true;
    }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public void HitRockInRange(Rock rock)
    {
        if (CanMine && CanHitRock && Vector3.Distance(rock.transform.position, transform.position) <= pickaxeRange)
        {
            CanHitRock = false;
            IsHittingRock = true;
            StaticObjects.FoleySoundEffectManager.StopSound(0);
            material.color = Color.red;
            StaticObjects.SoundEffectManager.PlaySound(2);
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
        if (rock.Hit() == 0)
        {
            Utility.ParticleManager.PlayRockDestruction(rock.transform.position);
            StaticObjects.SoundEffectManager.PlaySound(3);
            StaticObjects.GameController.RockDestroyed(rock.Value);
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
