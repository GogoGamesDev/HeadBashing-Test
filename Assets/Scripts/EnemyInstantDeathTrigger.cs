// Trigger letal del enemigo: muerte instantánea y knockback compatible con CharacterController
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class EnemyInstantDeathTrigger : MonoBehaviour
{
    [Header("Feedback de muerte")]
    public AudioSource audioSource;
    public AudioClip deathClip;

    [Header("Knockback (CharacterController)")]
    public float knockbackForce = 10f;
    public Vector3 knockbackDirection = new Vector3(0f, 1f, -1f);
    public float knockDuration = 0.2f;

    private void Awake()
    {
        var col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Intentamos detectar CharacterController en lugar de Rigidbody
        var controller = other.GetComponent<CharacterController>();
        if (controller == null) return;

        Debug.Log("Trigger letal activado por: " + other.name);

        // Sonido de muerte
        if (audioSource != null && deathClip != null)
            audioSource.PlayOneShot(deathClip);

        // Iniciar knockback y luego recarga de nivel
        StartCoroutine(DoKnockbackAndReload(controller));
    }

    private IEnumerator DoKnockbackAndReload(CharacterController controller)
    {
        float elapsed = 0f;
        Vector3 worldDir = knockbackDirection.normalized;

        while (elapsed < knockDuration)
        {
            // mover al jugador manualmente
            controller.Move(worldDir * knockbackForce * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }

    }
}