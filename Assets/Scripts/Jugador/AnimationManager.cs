using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public DashRigidbody dash;
    public PlayerMovement playerMovement;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public IEnumerator DashAnim()
    {
        if (animator != null)
        {
            animator.speed = 10f; // O el valor que desees
            animator.Play("Dash2", 0, 0f);         // Empieza desde el principio sí o sí
        }

        // Espera el tiempo necesario para el dash o la duración de la animación
        yield return new WaitForSeconds(dash.dashDuration / 1.5f);
        animator.speed = 1f;
        if (animator != null)
        {
            animator.SetTrigger("DontDash");      // Transición a otro estado si es necesario
            animator.speed = 3; // Restablece la velocidad normal
        }
    }
    public IEnumerator MoveAnim()
    {
        yield return new WaitForSeconds(0.2f / 2);
        if (animator != null)
        {
            animator.speed = 5f; // O el valor que desees
            animator.Play("MoveFw", 0, 0f);         // Empieza desde el principio sí o sí
        }

        // Espera el tiempo necesario para el dash o la duración de la animación
        yield return new WaitForSeconds(0.3f / 2);
        animator.speed = 1f;
        if (animator != null)
        {
            animator.SetTrigger("DontMove");      // Transición a otro estado si es necesario
            animator.speed = 1f;  // Restablece la velocidad normal
        }
    }

    public IEnumerator ShootAnim()
    {
        // Iniciar la animación de disparo
        if (animator != null)
        {
            animator.SetTrigger("Shoot"); // Asegúrate que este trigger existe en tu Animator Controller
        }
        yield return new WaitForSeconds(0.1f);

        // Iniciar la animación de disparo
        if (animator != null)
        {
            animator.SetTrigger("DontShoot");  // Asegúrate que este trigger existe en tu Animator Controller
        }

    }



}
