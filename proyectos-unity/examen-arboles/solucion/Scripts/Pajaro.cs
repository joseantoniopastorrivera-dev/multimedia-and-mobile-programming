using UnityEngine;

// Va en el prefab Pajaro.
// Necesita: Rigidbody2D (Kinematic) + CircleCollider2D. El sprite mira a la derecha.
public class Pajaro : MonoBehaviour
{
    [SerializeField] float velocidad = 5;          // el enunciado pide 5 unidades por segundo al empezar
    float borde;                                   // x del borde de la pantalla

    void Start()
    {
        // Media anchura visible = tamaño de la cámara × proporción de la pantalla (16:9 en Full-HD)
        borde = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        // Avanza hacia su parte delantera (su eje X rojo) cambiando transform.position en función del tiempo
        transform.position += transform.right * velocidad * Time.deltaTime;

        if (transform.position.x > borde)                       // llega al borde derecho
            transform.eulerAngles = new Vector3(0, 180, 0);     // mira a la izquierda: transform.right pasa a ser (-1, 0, 0)
        else if (transform.position.x < -borde)                 // llega al borde izquierdo
            transform.eulerAngles = new Vector3(0, 0, 0);       // vuelve a mirar a la derecha
    }
}
