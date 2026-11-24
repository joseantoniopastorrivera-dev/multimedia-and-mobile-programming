using UnityEngine;
using UnityEngine.InputSystem;   // InputActionReference

// Va en el objeto Jugador (la cesta).
// Necesita: Rigidbody2D (Dynamic, Gravity Scale 0, Freeze Position Y y Freeze Rotation Z) + BoxCollider2D.
public class Jugador : MonoBehaviour
{
    [SerializeField] InputActionReference mover;   // acción Mover (1D Axis: flecha izquierda / flecha derecha)
    [SerializeField] float velocidad = 10;         // el enunciado pide 10 unidades por segundo
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        mover.action.Enable();                     // sin esto las flechas no responden
    }

    void OnDisable()
    {
        mover.action.Disable();
    }

    void Update()
    {
        float direccion = mover.action.ReadValue<float>();       // -1 izquierda, 0 ninguna, 1 derecha
        rb.linearVelocity = new Vector2(direccion * velocidad, 0); // Y = 0: nunca se mueve en vertical
    }

}
