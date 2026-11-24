using UnityEngine;
using System.Collections;        // IEnumerator (corrutinas)

// Va en un objeto vacío llamado Spawner.
public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefabManzana;     // arrastrar el PREFAB Manzana desde la carpeta Prefabs
    [SerializeField] float intervalo = 2;          // cada 2 segundos

    void Start()
    {
        StartCoroutine(CrearManzanas());           // la corrutina se queda funcionando en segundo plano
    }

    IEnumerator CrearManzanas()
    {
        while (true)                               // para siempre (no congela: cada vuelta espera)
        {
            yield return new WaitForSeconds(intervalo);

            float x = Random.Range(-10.5f, 10.5f); // float: los dos extremos pueden salir
            float y = Random.Range(2f, 5f);
            Instantiate(prefabManzana, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}
