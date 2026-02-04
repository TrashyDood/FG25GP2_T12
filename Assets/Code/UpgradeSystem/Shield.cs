using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Shield Stats")]
    public int maxHp = 100;
    public int currentHp;
    public float regenDelay = 5f;
    public float blockAngle = 90f;

    [Header("Behaviour")]
    public bool oneHitPerContact = true;

    [Header("Knockback")]
    public float pushSpeed = 8f;

    private Renderer[] renderers;
    private Collider[] colliders;

    private bool isDown;
    private Coroutine respawnRoutine;

    private readonly HashSet<Enemy> hitThisContact = new();

    private void Awake()
    {
        currentHp = maxHp;

        renderers = GetComponentsInChildren<Renderer>(true);
        colliders = GetComponentsInChildren<Collider>(true);

        SetShieldEnabled(true);
        isDown = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        TryProcessEnemyHit(other);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!oneHitPerContact)
            return;

        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null)
            return;

        hitThisContact.Remove(enemy);
    }

    private void TryProcessEnemyHit(Collider other)
    {
        if (isDown)
            return;
        
        var enemy = other.GetComponentInParent<Enemy>();
        if (enemy == null)
            return;

        if (oneHitPerContact && hitThisContact.Contains(enemy))
            return;

        Vector3 hitDirection = (enemy.transform.position - transform.position).normalized;
        if (!IsBlocking(hitDirection))
            return;

        int damage = Mathf.CeilToInt(enemy.Data.damage);
        if (damage <= 0)
            return;

        int hpBefore = currentHp;

        currentHp -= damage;
        if (currentHp <= 0)
        {
            currentHp = 0;
            BringShieldDown();
        }

        if (currentHp < hpBefore)
        {
            ApplyKnockback(enemy);

            if (oneHitPerContact)
                hitThisContact.Add(enemy);
        }
    }

    private bool IsBlocking(Vector3 hitDirection)
    {
        float angle = Vector3.Angle(transform.forward, hitDirection);
        return angle <= blockAngle / 2f;
    }

    private void ApplyKnockback(Enemy enemy)
    {
        if (enemy.Movement == null)
            return;

        Vector3 pushDir = enemy.transform.position - transform.position;
        pushDir.y = 0f;

        if (pushDir.sqrMagnitude < 0.001f)
            return;

        enemy.Movement.ApplyKnockback(pushDir, pushSpeed);
    }

    private void BringShieldDown()
    {
        isDown = true;
        SetShieldEnabled(false);

        hitThisContact.Clear();

        if (respawnRoutine != null)
            StopCoroutine(respawnRoutine);

        respawnRoutine = StartCoroutine(RespawnAfterDelay());
    }

    private IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(regenDelay);

        currentHp = maxHp;
        isDown = false;
        SetShieldEnabled(true);

        respawnRoutine = null;
    }

    private void SetShieldEnabled(bool enabled)
    {
        if (colliders != null)
        {
            foreach (var c in colliders)
                if (c != null) c.enabled = enabled;
        }

        // Could instead change the look of the shield instead of turning invisible?
        if (renderers != null)
        {
            foreach (var r in renderers)
                if (r != null) r.enabled = enabled;
        }
    }
}