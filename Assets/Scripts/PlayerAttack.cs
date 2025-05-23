using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Parameters")]
    public float attackCooldown = 1f;
    public float attackRange = 2f;
    public int attackDamage = 10;
    public LayerMask enemyLayer;
    public Transform attackPoint;
    public float attackRadius = 0.5f;

    private float lastAttackTime;
    private Camera mainCamera;

    [Header("Animation Parameters")]
    public Animator animator;

    void Start()
    {
        mainCamera = Camera.main;
        lastAttackTime = -attackCooldown;
        animator = animator.GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1") && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    void Attack()
    {
        lastAttackTime = Time.time;

        animator.SetTrigger("attack");
        Debug.Log("Ataque ejecutado");

        //Detect Enemy hit by distance
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, attackRadius, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            Debug.Log("Golpeaste: " + enemy.name);
            enemy.GetComponent<Enemy>()?.TakeDamage(attackDamage);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}