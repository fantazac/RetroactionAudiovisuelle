using System.Collections;
using UnityEngine;

public class PickaxeManager : MonoBehaviour
{
    private float cooldown;
    private float remainingCooldown;

    private float castTime;
    private WaitForSeconds delayCastTime;

    private float minimumDistanceToHitRock;

    private Material material;

    public bool CanHitRock { get; private set; }

    private PickaxeManager()
    {
        cooldown = 0.3f;

        castTime = 0.15f;
        delayCastTime = new WaitForSeconds(castTime);

        minimumDistanceToHitRock = 1;

        CanHitRock = true;
    }

    private void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    public void HitRockInRange(Rock rock)
    {
        if(CanHitRock && Vector3.Distance(rock.transform.position, transform.position) <= minimumDistanceToHitRock)
        {
            CanHitRock = false;
            material.color = Color.red;
            StartCoroutine(CastTime(rock));
        }
    }

    private IEnumerator CastTime(Rock rock)
    {
        yield return delayCastTime;

        DamageRock(rock);
        material.color = Color.white;
    }

    private void DamageRock(Rock rock)
    {
        if(rock.Hit() == 0)
        {
            StaticObjects.GameController.RockDestroyed(rock.Level);
            Destroy(rock.gameObject);
        }
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
