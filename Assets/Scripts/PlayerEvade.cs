using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerEvade : MonoBehaviour
{
    [Header("Parámetros de Evasión")]
    public KeyCode evadeKey = KeyCode.Space;
    public float evadeDistance = 5f;
    public float evadeDuration = 0.3f;
    public float evadeCooldown = 1f;

    private CharacterController controller;
    private bool isEvading = false;
    private bool canEvade = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (Input.GetKeyDown(evadeKey) && canEvade && !isEvading)
        {
            StartCoroutine(EvadeRoutine());
        }
    }

    private IEnumerator EvadeRoutine()
    {
        canEvade = false;
        isEvading = true;

        float elapsed = 0f;
        Vector3 startPos = transform.position;
        // Dirección hacia atrás relativa a la orientación del jugador
        Vector3 targetDir = -transform.forward;

        // Movimiento suave de evasión
        while (elapsed < evadeDuration)
        {
            float t = elapsed / evadeDuration;
            // Se mueve proporcionalmente a evadeDistance
            Vector3 move = targetDir * (evadeDistance * Time.deltaTime / evadeDuration);
            controller.Move(move);

            elapsed += Time.deltaTime;
            yield return null;
        }

        isEvading = false;
        // Esperar cooldown antes de permitir otra evasión
        yield return new WaitForSeconds(evadeCooldown);
        canEvade = true;
    }
}
