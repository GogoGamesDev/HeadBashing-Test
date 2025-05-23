using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    private int health;

    [Header("Attack Timing")]
    public float minWaitTime = 5f;
    public float maxWaitTime = 10f;

    [Header("Attack Probability")]
    [Range(0f,1f)] public float baseAttackChance = 0.5f;
    [Range(0f,1f)] public float lowHealthBias = 0.5f; //when low health increase the attack chances

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip onDamageClip;
    public AudioClip onDieClip;
    public AudioClip attackClip;

    [Header("Animation")]
    public Animator animator;

    [Header("On Die Parameters")]
    public FirstPersonController player;
    public GameObject panel_UI;

    private Quaternion initialRotation;

    void Start()
    {
        health = maxHealth;
        initialRotation = transform.rotation;
        if (animator != null) animator.applyRootMotion = false;
        StartCoroutine(AttackRoutine());
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        audioSource.PlayOneShot(onDamageClip);
        animator.SetTrigger("onDamage");
        Debug.Log($"{name} recieved {amount} damage. health: {health}/{maxHealth}");
        if (health <= 0)
            Die();
    }

    void Die()
    {
        panel_UI.SetActive(true);
        player.CanMove = false;
        
        Debug.Log($"{name} died");
        audioSource?.PlayOneShot(onDieClip);

        Destroy(gameObject, 1f);
    }

    IEnumerator AttackRoutine()
    {
        while (health > 0)
        {
            float wait = Random.Range(minWaitTime, maxWaitTime);
            yield return new WaitForSeconds(wait);

            float healthRatio = 1f - (float)health / maxHealth;
            float chance = baseAttackChance + healthRatio * lowHealthBias;
            chance = Mathf.Clamp01(chance);

            if (Random.value <= chance)
            {
                PerformAttack();
            }
            else
            {
                Debug.Log("Decidió no atacar esta ronda.");
            }
        }
    }

    void PerformAttack()
    {
        transform.rotation = initialRotation;

        animator?.SetTrigger("bossAttack");
        audioSource?.PlayOneShot(attackClip);
        Debug.Log("Attacking");
    }

    void OnDrawGizmosSelected()
    {
        // visual debug
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
