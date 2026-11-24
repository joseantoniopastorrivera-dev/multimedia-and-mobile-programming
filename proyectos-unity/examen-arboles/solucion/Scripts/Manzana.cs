using UnityEngine;

// Va en el prefab Manzana. No lo pide el enunciado: evita que las manzanas que nadie coge
// caigan para siempre y se acumulen en la Hierarchy.
// Necesita: Rigidbody2D (Dynamic, cae con la gravedad) + CircleCollider2D con Is Trigger.
public class Manzana : MonoBehaviour
{
    [SerializeField] float tiempoVida = 6;         // segundos hasta borrarse sola

    void Start()
    {
        Destroy(gameObject, tiempoVida);           // Destroy con retardo: no hace falta Update
    }
}
