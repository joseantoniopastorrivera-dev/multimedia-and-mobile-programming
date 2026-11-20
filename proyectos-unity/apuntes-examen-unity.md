# Chuleta para el examen de Unity (PMDM, 1ª evaluación)

> **Cómo usarla:** está ordenada en el orden en que harás las cosas en el examen. Cada bloque tiene los **pasos de interfaz** (🖱️), el **código** (con comentarios línea a línea) y los **fallos típicos** (⚠️). El código sigue el estilo del profe: el proyecto Plataformas, con `InputActionReference`, `eulerAngles`, `linearVelocity`, Raycast al suelo y corrutinas.
>
> Unity 6 (6000.0.83f1) · plantilla **Universal 2D** · 3 horas · el profe da solo las imágenes · se entrega `Assets/` con `Images/`, `Prefabs/`, `Scenes/` y `Scripts/`.

## Índice
1. [Arranque: los 10 primeros minutos](#1-arranque-los-10-primeros-minutos)
2. [Montar la escena](#2-montar-la-escena)
3. [Componentes: Rigidbody2D y Collider2D](#3-componentes-rigidbody2d-y-collider2d)
4. [Control por teclado (Input System)](#4-control-por-teclado-input-system)
5. [Movimiento](#5-movimiento)
6. [Rotación y ángulos de Euler](#6-rotación-y-ángulos-de-euler)
7. [Colisiones y triggers](#7-colisiones-y-triggers)
8. [Instantiate y prefabs](#8-instantiate-y-prefabs)
9. [Destroy](#9-destroy)
10. [Tiempo: deltaTime, contadores, Invoke y corrutinas](#10-tiempo-deltatime-contadores-invoke-y-corrutinas)
11. [Plantillas completas](#11-plantillas-completas)
12. [«No funciona»: síntoma → causa → arreglo](#12-no-funciona-síntoma--causa--arreglo)
13. [Antes de entregar](#13-antes-de-entregar)
14. [Ejercicios tipo examen (las mejoras de Plataformas) y soluciones](#14-ejercicios-tipo-examen-las-mejoras-de-plataformas-y-soluciones)

---

## 1. Arranque: los 10 primeros minutos

🖱️ **Crear el proyecto**
1. Unity Hub → **Nuevo proyecto** → plantilla **Universal 2D**. Ojo: por defecto viene marcada Universal 3D.
2. Editor **6000.0.83f1**, ponle nombre y pulsa **Crear proyecto**.
3. En el aula (Linux), si Hub no encuentra el editor, ve a *Instalaciones → Localizar* → `/opt/Unity-6000.0.83f1/Editor/Unity`. Si no tienes licencia: *Gestionar licencias → Añadir → licencia personal gratuita*.

🖱️ **Crear las carpetas que pide la entrega**
- En la ventana **Project**, clic derecho en `Assets` → **Create → Folder**. Crea `Images`, `Prefabs` y `Scripts`. `Scenes` ya existe.
- Arrastra las imágenes del profe desde el explorador de archivos a `Assets/Images`.

🖱️ **Configurar las imágenes** (selecciona la imagen y mira el **Inspector**)
| Campo | Valor | Por qué |
|---|---|---|
| Texture Type | **Sprite (2D and UI)** | Para poder arrastrarla a la escena |
| Sprite Mode | **Single** si es un dibujo; **Multiple** si es una hoja con varios fotogramas | — |
| Pixels Per Unit | El tamaño en píxeles de un «tile». Por ejemplo, 16 o 32 | Controla lo grande que se ve. Si sale enorme, súbelo |
| Filter Mode | **Point (no filter)** si es pixel art | Si no, se ve borroso |
| Compression | **None** | Mantiene los colores nítidos |

Al terminar, pulsa **Apply**. Para cortar una hoja *Multiple*, abre **Sprite Editor → Slice → Type: Grid By Cell Size o Automatic → Slice → Apply**.

---

## 2. Montar la escena

🖱️ **Pasos**
- **Arrastrar un sprite** desde `Images` a la **Hierarchy** o a la **Scene** crea un GameObject con `SpriteRenderer`.
- **Renombrar:** selecciona el objeto y pulsa F2 (o cambia el nombre arriba en el Inspector).
- **Qué se dibuja delante:** `SpriteRenderer → Order in Layer`. Cuanto mayor el número, más delante. El fondo, por ejemplo, en -10.
- **Tag:** Inspector → **Tag → Add Tag… → +**, escribe el nombre y guarda. Después vuelve al objeto y elige el tag.
- **Layer**, para el suelo: Inspector → **Layer → Add Layer…**, escribe `Suelo` en un hueco libre y asígnala a las plataformas.
- **Objeto vacío**, para un punto de disparo, un spawner o un tope: clic derecho en la Hierarchy → **Create Empty**. Para hacerlo **hijo**, arrástralo encima de otro objeto.
- **Guardar la escena:** **Ctrl+S**. Guarda a menudo.

⚠️ Si el sprite no se ve, revisa tres cosas: que la **Z** esté entre la cámara (z = -10) y 0, que el *Order in Layer* no lo tape otro objeto y que la cámara lo esté enfocando (mira la ventana **Game**).

---

## 3. Componentes: Rigidbody2D y Collider2D

> **Regla del profe:** si el objeto se mueve, ponle `Rigidbody2D`.

🖱️ Inspector → **Add Component** → escribe `Rigidbody2D` o `Box Collider 2D` / `Circle Collider 2D` / `Capsule Collider 2D`.

### Body Type del Rigidbody2D
| Tipo | Le afecta la gravedad y lo empujan | Se mueve con | Ejemplo en Plataformas |
|---|---|---|---|
| **Dynamic** | Sí | `linearVelocity`, `AddForce` | Jugador (Humano), Tortuga |
| **Kinematic** | No (solo se mueve por código) | `linearVelocity`, `MovePosition` | Bala |
| **Static** | No se mueve | — | Plataformas (aunque basta con poner solo el Collider) |

### Ajustes que casi siempre hay que tocar
- **Constraints → Freeze Rotation Z ✔** en el jugador y los enemigos. Si no lo marcas, al chocar con una esquina se vuelcan.
- **Gravity Scale = 0** en los objetos que no deben caer, como balas o naves.
- **Collision Detection = Continuous** en las balas rápidas. Así no atraviesan las paredes.

### Qué collider poner a cada cosa
| Objeto | Collider | Is Trigger |
|---|---|---|
| Plataforma o suelo | Box Collider 2D | ❌ (choque físico) |
| Jugador | Capsule o Box Collider 2D | ❌ |
| Enemigo sólido (tortuga) | Box o Capsule Collider 2D | ❌ |
| Moneda | Circle Collider 2D | ✅ |
| Bala | Capsule o Circle Collider 2D | ✅ |
| Tope invisible o zona | Box Collider 2D | ✅ |

🖱️ Para ajustar el tamaño del collider usa **Edit Collider** (el icono del cuadradito) y arrastra los puntos verdes en la Scene.

---

## 4. Control por teclado (Input System)

### Forma A: la rápida, sin crear acciones (útil si vas justo de tiempo)
```csharp
using UnityEngine;
using UnityEngine.InputSystem;              // necesario para Keyboard

public class MovimientoRapido : MonoBehaviour
{
    [SerializeField] float velocidad = 5f;

    void Update()
    {
        float dir = 0f;
        if (Keyboard.current.aKey.isPressed) dir = -1f;           // isPressed: mientras se mantiene pulsada
        if (Keyboard.current.dKey.isPressed) dir = 1f;
        transform.position += new Vector3(dir, 0, 0) * velocidad * Time.deltaTime;

        if (Keyboard.current.spaceKey.wasPressedThisFrame)        // solo en el frame en que se pulsa
            Debug.Log("Disparo");
    }
}
```
- `isPressed` sirve para el movimiento continuo. `wasPressedThisFrame` sirve para acciones de una vez: saltar o disparar.

### Forma B: la del profe, con Input Actions e `InputActionReference`

🖱️ **Crear las acciones** (en Plataformas ya vienen en `Plataformas.inputactions`):
1. En Project: clic derecho → **Create → Input Actions** y llámalo, por ejemplo, `Controles`. Doble clic para abrirlo.
2. **Action Maps → +** → `Teclado`.
3. **Actions → +** → `Mover`. En las *Action Properties*: **Action Type = Value** (o Pass Through) y **Control Type = Axis**.
4. Pulsa el **+** de `Mover` → **Add Positive/Negative Binding**. En *negative*, pulsa **Listen** y luego la tecla **A**; en *positive*, la tecla **D**.
5. **Actions → +** → `Saltar`: **Action Type = Button**. Añade un binding con **W** y otro con **Space**.
6. **Actions → +** → `Disparar`: **Action Type = Button**, con la tecla que quieras.
7. Pulsa **Save Asset** (o marca *Auto-Save*). Si no guardas, no existe.

🖱️ **Conectar las acciones al script:**
1. Pon el script `Jugador` en el objeto del jugador.
2. En Project, despliega la flechita del `.inputactions`: aparecen las acciones, como `Teclado/Mover`.
3. Arrastra cada acción a su hueco del Inspector: **Mover**, **Saltar** y **Atacar**.

```csharp
using UnityEngine;
using UnityEngine.InputSystem;

public class Jugador : MonoBehaviour
{
    public InputActionReference mover, atacar, saltar;    // se rellenan arrastrando en el Inspector

    void OnEnable()                                       // se ejecuta al activarse el objeto
    {
        mover.action.Enable();                            // ⚠️ ver nota: asegura que las acciones escuchan
        atacar.action.Enable();
        saltar.action.Enable();
        atacar.action.started += Atacar;                  // "cuando empiece Atacar, llama a mi método Atacar"
        saltar.action.started += Saltar;
    }

    void OnDisable()                                      // al desactivarse o destruirse
    {
        atacar.action.started -= Atacar;                  // desuscribirse: si no, quedan llamadas duplicadas o a objetos destruidos
        saltar.action.started -= Saltar;
    }

    void Update()
    {
        float direccion = mover.action.ReadValue<float>(); // -1 (A), 0 (nada) o 1 (D). Es float porque Control Type = Axis
    }

    private void Atacar(InputAction.CallbackContext ctx) { /* disparar */ }
    private void Saltar(InputAction.CallbackContext ctx) { /* saltar */ }
}
```
- **Eventos:** `started` se lanza al empezar a pulsar, `performed` cuando la acción se completa y `canceled` al soltar. Para saltar y disparar se usa `started`.
- **`ReadValue<T>()`:** el tipo depende del *Control Type*: `Axis` → `float`; `Vector 2` → `Vector2` (para moverse en 4 direcciones, por ejemplo una nave).
- ⚠️ **Nota del `Enable()`:** si las teclas no hacen nada, es que la acción no está activada. Funciona sin `Enable()` solo si el asset está puesto como *Project-wide Actions* en **Edit → Project Settings → Input System Package**. Con `Enable()` en `OnEnable` funciona siempre.
- ⚠️ **Error típico:** escribir `Atacar()` sin el parámetro `InputAction.CallbackContext`. Entonces el `+=` no compila.

---

## 5. Movimiento

| Forma | Código | Cuándo |
|---|---|---|
| Por Transform | `transform.position += new Vector3(dir, 0, 0) * velocidad * Time.deltaTime;` | Objeto **sin** Rigidbody (decorado, nave sencilla) |
| Translate | `transform.Translate(Vector3.right * velocidad * Time.deltaTime);` | Igual, pero se mueve en sus **ejes locales** (si está girado, va hacia donde mira) |
| Velocidad física | `rb.linearVelocityX = dir * velocidad;` | Objeto **con** Rigidbody2D. Es lo que usa el profe |
| Velocidad completa | `rb.linearVelocity = new Vector2(dir * velocidad, rb.linearVelocity.y);` | Igual, sin tocar la Y (así respeta la gravedad o el salto) |
| Impulso | `rb.AddForce(Vector2.up * fuerza, ForceMode2D.Impulse);` | Empujones o saltos con «física real» |

**Por qué `* Time.deltaTime`:** `Update` se ejecuta más o menos veces por segundo según el ordenador. `deltaTime` son los segundos que pasaron desde el frame anterior, así que «velocidad × deltaTime» da **unidades por segundo** en cualquier PC. Con `linearVelocity` **no** se multiplica: la velocidad física ya va por segundo.

⚠️ **Unity 6:** `rb.velocity` ahora se llama **`rb.linearVelocity`**. También existen `linearVelocityX` y `linearVelocityY`. Y `drag` ahora es `linearDamping`.
⚠️ Si el objeto tiene Rigidbody, **no** lo muevas con `transform.position`: se teletransporta, atraviesa las paredes y la física falla.

### Salto con detección de suelo (Raycast, como el profe)
```csharp
[SerializeField] float fuerzaSalto = 8;
[SerializeField] bool enSuelo = false;
[SerializeField] LayerMask mascaraSuelo;          // en el Inspector, elige la layer "Suelo"

private void DetectarSuelo()                       // se llama en Update
{
    Vector3 origen = transform.position + new Vector3(0, -1, 0);   // un poco por debajo del centro: los pies
    RaycastHit2D contacto = Physics2D.Raycast(origen, Vector3.down, 0.1f, mascaraSuelo); // rayo corto hacia abajo, solo contra el Suelo
    Debug.DrawRay(origen, Vector3.down * 0.1f, Color.red);         // lo dibuja en la Scene para ver si llega
    enSuelo = contacto;                            // RaycastHit2D se convierte en bool: true si ha tocado algo
}

private void Saltar(InputAction.CallbackContext ctx)
{
    if (enSuelo)
        rb.linearVelocityY = fuerzaSalto;          // fija la velocidad vertical: el salto siempre mide lo mismo
}
```
- ⚠️ El `-1` depende de lo alto que sea el sprite: el rayo tiene que salir justo de los pies. Mira la línea roja en la Scene, con Play activo.
- ⚠️ Si no saltas nunca, comprueba tres cosas: que las plataformas tienen la **layer** `Suelo`, que `mascaraSuelo` tiene `Suelo` marcado y que el rayo llega al suelo.

---

## 6. Rotación y ángulos de Euler

- Unity guarda la rotación como un `Quaternion`. Las personas la leemos en **ángulos de Euler**: grados en X, Y y Z.
- En 2D, **girar en el plano = eje Z**. **Dar la vuelta** al personaje (mirar a izquierda o derecha) = 180° en **Y**.

| Quiero… | Código |
|---|---|
| Fijar una rotación exacta | `transform.eulerAngles = new Vector3(0, 0, 45);` |
| Lo mismo, con Quaternion | `transform.rotation = Quaternion.Euler(0, 0, 45);` |
| Girar sin parar (90° por segundo) | `transform.Rotate(0, 0, 90 * Time.deltaTime);` |
| Sin rotación | `Quaternion.identity` |
| Mirar a la derecha o a la izquierda | `transform.eulerAngles = new Vector3(0, dir > 0 ? 0 : 180, 0);` |

Código del profe para girar al jugador:
```csharp
if (direccion > 0)
    transform.eulerAngles = new Vector3(0, 0, 0);     // mira a la derecha
else if (direccion < 0)
    transform.eulerAngles = new Vector3(0, 180, 0);   // da la vuelta en Y: mira a la izquierda
// si direccion == 0 no se toca: se queda mirando hacia donde estaba
```
**Por qué rotar y no usar `flipX`:** al rotar el objeto también giran sus **hijos**, como el punto de disparo (`Arma`), y sus ejes locales (`transform.right`, `transform.up`). Con `spriteRenderer.flipX = true` solo se da la vuelta al dibujo, y la bala seguiría saliendo hacia el lado de antes.

⚠️ `eulerAngles` devuelve valores entre 0 y 360. Si pones -90, al leerlo obtienes 270. No compares con negativos.
⚠️ La **bala del profe** se mueve con `transform.up` (su eje Y local). Por eso el objeto `Arma` está **rotado** para que su «arriba» apunte hacia delante. Si la bala sale hacia arriba, rota el `Arma` en Z (-90°) o cambia `transform.up` por `transform.right` en `Bala.cs`.

---

## 7. Colisiones y triggers

| | Colisión | Trigger |
|---|---|---|
| Is Trigger | ❌ en los dos colliders | ✅ en al menos uno |
| ¿Se atraviesan? | No: chocan, se paran o empujan | Sí: solo avisa |
| Método | `OnCollisionEnter2D(Collision2D collision)` | `OnTriggerEnter2D(Collider2D collision)` |
| Acceso al otro objeto | `collision.gameObject` | `collision.gameObject` (o el propio `collision`) |
| Uso | Suelo, paredes, enemigo que empuja | Monedas, balas, zonas, topes |

**Requisitos (si falta uno, no salta nada):** los **dos** objetos tienen **Collider2D** y **al menos uno** tiene **Rigidbody2D**.

Variantes: `Enter` (al entrar), `Stay` (mientras está dentro) y `Exit` (al salir). Por ejemplo, `OnTriggerExit2D` y `OnCollisionExit2D`.

```csharp
// Jugador recoge moneda (moneda con Is Trigger y Tag "Moneda")
private void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Moneda"))       // ¿lo que he tocado tiene el tag Moneda?
    {
        puntos += 25;
        Destroy(collision.gameObject);        // destruye la MONEDA (el otro), no a mí
    }
}

// Jugador choca con tortuga (colisión física, Tag "Tortuga")
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Tortuga"))   // en Collision2D hay que pasar por .gameObject
    {
        vida--;
        StartCoroutine(Parpadeo());
    }
}
```
⚠️ Fallos típicos:
- `OnTriggerEnter` sin **2D**: no salta nunca y no da error.
- Parámetro equivocado: en trigger es `Collider2D` y en colisión es `Collision2D`.
- El tag está mal escrito. Distingue mayúsculas.
- Al objeto se le olvidó el tag, aunque el tag exista.
- Usar `CompareTag("X")` mejor que `tag == "X"`: es más rápido y avisa si el tag no existe.

---

## 8. Instantiate y prefabs

🖱️ **Crear un prefab:** monta el objeto en la escena (sprite, componentes, script, tag) y **arrástralo desde la Hierarchy a `Assets/Prefabs`**. Se vuelve azul.
- Para editarlo: doble clic en el prefab, que abre el *Prefab Mode*. O cambia la copia de la escena y pulsa **Overrides → Apply All**.
- Después puedes borrarlo de la escena si solo se va a crear por código.

🖱️ **Pasarlo al script:** declara `[SerializeField] GameObject prefabBala;` y **arrastra el prefab desde Project** al hueco del Inspector.
⚠️ Arrastra el prefab desde **Project**, no la copia de la Hierarchy. Si arrastras la de la escena y luego se destruye, fallará con `MissingReferenceException`.

```csharp
[SerializeField] GameObject prefabBala;      // privado, pero visible en el Inspector (ficha 07)
[SerializeField] Transform puntoDisparo;     // un objeto hijo vacío en la boca del arma

private void Atacar(InputAction.CallbackContext ctx)
{
    Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
    // Instantiate(qué, dónde, con qué rotación). Sale con la rotación del arma: si el jugador está girado, la bala también
}
```
- `Instantiate(prefab, posicion, Quaternion.identity)` crea el objeto sin rotación. Es lo habitual para monedas.
- Devuelve la copia, por si quieres tocarla: `GameObject b = Instantiate(...); b.name = "Bala";`.
- Para crear en una posición aleatoria: `new Vector3(Random.Range(-8f, 8f), 4f, 0)`.
- ⚠️ Con `Random.Range` y números `float`, el máximo puede salir. Con `int`, el máximo **no sale nunca**: `Random.Range(0, 3)` da 0, 1 o 2.

---

## 9. Destroy

| Código | Qué hace |
|---|---|
| `Destroy(gameObject);` | Me destruyo yo (el objeto que tiene este script) |
| `Destroy(gameObject, 3f);` | Me destruyo dentro de 3 segundos. Ideal en el `Start` de la bala |
| `Destroy(collision.gameObject);` | Destruye el **otro** objeto del choque |
| `Destroy(this);` | ⚠️ Solo borra el **script**, no el objeto. Error típico |
| `Destroy(GetComponent<Rigidbody2D>());` | Quita un componente |

- Destruye siempre las balas, ya sea por tiempo o al chocar. Si no, se acumulan para siempre: en Plataformas, `Bala.cs` no se destruye nunca, y en la Hierarchy se ven las copias acumulándose.
- Para que algo desaparezca al salir de la pantalla: `void OnBecameInvisible() { Destroy(gameObject); }`. Necesita un `SpriteRenderer`. En el editor, la vista Scene también cuenta como «visible».

---

## 10. Tiempo: deltaTime, contadores, Invoke y corrutinas

| Herramienta | Para qué |
|---|---|
| `Time.deltaTime` | Segundos desde el frame anterior. Hace el movimiento independiente de los FPS |
| `Time.time` | Segundos desde que empezó el juego |
| `FixedUpdate()` | Se ejecuta a ritmo fijo (50 veces por segundo). Pensado para la física (`AddForce`) |
| Contador `float` | Llevar la cuenta tú mismo |
| `Invoke("Metodo", 2f)` / `InvokeRepeating("Metodo", 0f, 2f)` | Llamar a un método después de un tiempo, o cada cierto tiempo |
| **Corrutina** | Secuencias con esperas: «haz, espera, haz…» |

El profe acepta cualquier método. Usa el que te salga más rápido.

### Contador en Update
```csharp
float temporizador = 0f;
void Update()
{
    temporizador += Time.deltaTime;            // suma los segundos reales
    if (temporizador >= 2f)                    // ¿han pasado 2 s?
    {
        Debug.Log("Han pasado 2 segundos");
        temporizador = 0f;                     // reinicia para volver a contar
    }
}
```

### Corrutina
Es un método que **se pausa sin congelar el juego** y continúa más tarde.
```csharp
using System.Collections;                       // ⚠️ sin esto, IEnumerator no compila

void Start()
{
    StartCoroutine(CrearMonedas());             // ⚠️ escribir solo CrearMonedas(); no hace NADA
}

IEnumerator CrearMonedas()                       // las corrutinas devuelven IEnumerator
{
    while (true)                                 // bucle infinito seguro: dentro hay un yield
    {
        yield return new WaitForSeconds(Random.Range(1f, 5f));   // espera entre 1 y 5 segundos
        Instantiate(prefabMoneda, new Vector3(Random.Range(0, 4f), Random.Range(-4f, 0), 0), Quaternion.identity);
    }
}
```
- `yield return new WaitForSeconds(x)` espera x segundos. `yield return null` espera **un frame**: útil para animar poco a poco.
- Para pararla: `StopAllCoroutines();` o `StopCoroutine(miCorrutina)`.
- ⚠️ Un `while(true)` **sin** `yield` dentro cuelga Unity y tendrás que cerrarlo a la fuerza.
- ⚠️ Si el objeto se destruye o se desactiva, sus corrutinas se paran.

### Corrutinas del profe para efectos visuales (las tienes en `Jugador.cs`)
```csharp
IEnumerator Parpadeo()                               // parpadea 3 veces
{
    for (int i = 0; i < 3; i++)
    {
        sr.color = new Color(1, 1, 1, 0);            // alfa 0 = invisible
        yield return new WaitForSeconds(0.2f);
        sr.color = new Color(1, 1, 1, 1);            // alfa 1 = visible
        yield return new WaitForSeconds(0.2f);
    }
}

IEnumerator GolpeRojo()                              // destello rojo al recibir daño
{
    sr.color = new Color(1, 0, 0, 0.5f);             // rojo semitransparente
    yield return new WaitForSeconds(0.2f);
    sr.color = Color.white;                          // blanco = color original del sprite
}

IEnumerator Desaparecer()                            // se desvanece y se destruye
{
    while (sr.color.a > 0)
    {
        sr.color = new Color(1, 1, 1, sr.color.a - Time.deltaTime);  // baja el alfa poco a poco (1 segundo en total)
        yield return null;                           // espera un frame y repite
    }
    Destroy(gameObject);
}
```
`sr` es el `SpriteRenderer`, que se obtiene en `Start` con `sr = GetComponent<SpriteRenderer>();`.

---

## 11. Plantillas completas

> Copia la que te haga falta, cambia los nombres de tags y teclas y **arrastra las referencias en el Inspector**.

### Jugador de plataformas (versión del profe, con el disparo arreglado)
```csharp
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Jugador : MonoBehaviour
{
    [SerializeField] int vida = 10, puntos = 0;
    [SerializeField] float velocidad = 5, fuerzaSalto = 8;
    [SerializeField] bool enSuelo = false;
    [SerializeField] LayerMask mascaraSuelo;
    public InputActionReference mover, atacar, saltar;
    [SerializeField] GameObject prefabBala;
    [SerializeField] Transform puntoDisparo;
    SpriteRenderer sr;
    Rigidbody2D rb;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();   // coge componentes del MISMO objeto
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        mover.action.Enable(); atacar.action.Enable(); saltar.action.Enable();
        atacar.action.started += Atacar;
        saltar.action.started += Saltar;
    }

    void OnDisable()
    {
        atacar.action.started -= Atacar;
        saltar.action.started -= Saltar;
    }

    void Update()
    {
        float direccion = mover.action.ReadValue<float>();
        if (direccion > 0) transform.eulerAngles = new Vector3(0, 0, 0);
        else if (direccion < 0) transform.eulerAngles = new Vector3(0, 180, 0);
        rb.linearVelocityX = direccion * velocidad;     // solo X: la Y la controla la gravedad o el salto
        DetectarSuelo();
    }

    private void DetectarSuelo()
    {
        Vector3 origen = transform.position + new Vector3(0, -1, 0);
        enSuelo = Physics2D.Raycast(origen, Vector3.down, 0.1f, mascaraSuelo);
        Debug.DrawRay(origen, Vector3.down * 0.1f, Color.red);
    }

    private void Saltar(InputAction.CallbackContext ctx)
    {
        if (enSuelo) rb.linearVelocityY = fuerzaSalto;
    }

    private void Atacar(InputAction.CallbackContext ctx)
    {
        Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Moneda"))
        {
            puntos += 25;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Tortuga"))
        {
            vida--;
            StartCoroutine(Parpadeo());
            if (vida <= 0) StartCoroutine(Desaparecer());   // reutiliza la corrutina del bloque 10
        }
    }

    // + Parpadeo(), GolpeRojo() y Desaparecer() del bloque 10
}
```

### Bala (Rigidbody2D **Kinematic**, Collider **Is Trigger**)
```csharp
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] float velocidad = 6;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = transform.up * velocidad;   // basta con darla una vez: en Kinematic nada la frena
        Destroy(gameObject, 3f);                         // AÑADIDO: no se acumulan balas
    }

    private void OnTriggerEnter2D(Collider2D collision)  // AÑADIDO: impacto
    {
        if (collision.CompareTag("Tortuga"))
        {
            Destroy(collision.gameObject);   // destruye el enemigo
            Destroy(gameObject);             // y la bala
        }
    }
}
```
**Por qué está en `Start` y no en `Update` como lo hace el profe:** el resultado es el mismo, porque un cuerpo Kinematic mantiene su velocidad. Si te lo preguntan, las dos formas valen.

### Spawner (objeto vacío en la escena con este script)
```csharp
using System.Collections;
using UnityEngine;

public class SpawnerMonedas : MonoBehaviour
{
    [SerializeField] GameObject prefabMoneda;

    void Start() { StartCoroutine(CrearMonedas()); }

    IEnumerator CrearMonedas()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(1f, 5f));
            float x = Random.Range(0, 4f);
            float y = Random.Range(-4f, 0);
            Instantiate(prefabMoneda, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}
```

### Enemigo que patrulla (la Tortuga del profe)
```csharp
using UnityEngine;

public class Tortuga : MonoBehaviour
{
    [SerializeField] bool derecha = true;
    [SerializeField] float velocidad = 3;
    Rigidbody2D rb;

    void Start() { rb = GetComponent<Rigidbody2D>(); }

    void Update()
    {
        float vx = derecha ? velocidad : -velocidad;          // operador ternario: condición ? siTrue : siFalse
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }
}
```
La versión con giro en los topes está en el bloque 14, mejora 5.

---

## 12. «No funciona»: síntoma → causa → arreglo

| Síntoma | Causa | Arreglo |
|---|---|---|
| Las teclas no hacen nada | Acción no activada o referencia vacía | `action.Enable()` en `OnEnable`; arrastra las acciones al Inspector; **Save Asset** en el `.inputactions` |
| `NullReferenceException` al pulsar Play | Hueco del Inspector vacío (prefab, Transform…) o falta un `GetComponent` | Mira la línea del error en la **Console** y rellena ese hueco |
| No salta nunca | Plataformas sin la layer `Suelo`, `mascaraSuelo` vacía o el rayo no llega | Asigna la layer y la máscara; mira el rayo rojo en la Scene |
| Salta en el aire (salto infinito) | La máscara incluye al propio jugador | Que el jugador **no** esté en la layer `Suelo` |
| El trigger no salta | Falta Rigidbody2D en los dos, falta *Is Trigger*, el método está mal escrito o el tag no coincide | Repasa el bloque 7 |
| El jugador se vuelca o rueda | Falta **Freeze Rotation Z** | Rigidbody2D → Constraints |
| El objeto cae al vacío | No hay collider en el suelo, o el objeto que no debería caer es Dynamic | Pon un collider al suelo; Gravity Scale 0 o Kinematic |
| La bala sale hacia arriba o hacia atrás | El eje usado (`up` o `right`) no coincide con la rotación del `Arma` | Rota el `Arma` o cambia `transform.up` por `transform.right` |
| La bala atraviesa los objetos | Demasiado rápida | Collision Detection **Continuous** |
| Error `'velocity' is obsolete` | Código antiguo | `linearVelocity` |
| La corrutina no hace nada | Se llamó sin `StartCoroutine` | `StartCoroutine(Metodo());` |
| Unity se congela | `while(true)` sin `yield` | Pon un `yield return` dentro del bucle |
| `IEnumerator` en rojo | Falta un `using` | `using System.Collections;` |
| Se mueve más rápido en un PC que en otro | Falta `Time.deltaTime` | Multiplica por `Time.deltaTime` (salvo con `linearVelocity`) |
| El script no se puede añadir al objeto | El nombre de la clase no coincide con el del archivo, o hay errores de compilación | Mismo nombre exacto; arregla los errores rojos de la Console |
| Los cambios hechos en Play desaparecen | Los cambios en Play no se guardan | Para el Play y repite el cambio |
| `MissingReferenceException` | Arrastraste un objeto de la escena (que luego se destruye) en lugar del prefab | Arrastra el prefab desde **Project** |

---

## 13. Antes de entregar

- [ ] **Ctrl+S**: escena guardada (en el título de la pestaña no debe salir `*`).
- [ ] La escena está en `Assets/Scenes`, los prefabs en `Assets/Prefabs`, los scripts en `Assets/Scripts` y las imágenes en `Assets/Images`.
- [ ] La **Console** no tiene errores rojos. Pulsa Play una última vez para probarlo.
- [ ] Los huecos del Inspector están todos rellenos: prefabs, puntos de disparo, acciones y máscara.
- [ ] 🖱️ Clic derecho en `Assets` → **Show in Explorer** / **Open Containing Folder** y copia la carpeta `Assets` **entera, con los archivos `.meta`**. Los `.meta` guardan las referencias entre prefabs, scripts y sprites: sin ellos, al abrir el proyecto todo sale «Missing».
- [ ] No entregues `Library/`, `Temp/` ni `Logs/`. Los Settings no son obligatorios.

---

## 14. Ejercicios tipo examen (las mejoras de Plataformas) y soluciones

Enunciado del profe (foro de Moodle, 18 y 19/11/2026). Intenta cada uno **sin mirar** la solución y después compara.

1. **Salto en los bordes:** que el salto funcione aunque el jugador esté medio fuera de la plataforma.
2. **Efecto coyote:** que se pueda saltar durante un instante después de salir de la plataforma.
3. **Doble salto.**
4. **Pisar la tortuga:** el jugador recibe un salto involuntario con el doble de fuerza y la tortuga muere. Si la toca de lado, el jugador pierde vida.
5. **Patrulla por trigger:** la tortuga se da la vuelta, visualmente y de dirección, al tocar un objeto **Tope** invisible con collider trigger.
6. **Moneda:** a los 5 segundos parpadea 3 veces y desaparece.

<details>
<summary><b>Solución 1: salto en los bordes</b></summary>

**Causa:** un solo rayo desde el centro deja de tocar el suelo en cuanto el centro sale de la plataforma, aunque los pies sigan encima.
**Arreglo:** detectar con una **caja** del ancho de los pies en lugar de un rayo.
```csharp
[SerializeField] Vector2 tamañoPies = new Vector2(0.8f, 0.1f);   // ancho casi igual al del jugador, muy fina

private void DetectarSuelo()
{
    Vector2 origen = transform.position + new Vector3(0, -1, 0);   // a la altura de los pies
    enSuelo = Physics2D.OverlapBox(origen, tamañoPies, 0f, mascaraSuelo);
    // OverlapBox(centro, tamaño, ángulo, máscara): devuelve el collider que haya dentro de la caja, o null.
    // Si devuelve algo, cuenta como true
}
```
Alternativa: lanzar **dos** rayos, uno en cada pie (`origen ± new Vector3(0.4f, 0, 0)`), y `enSuelo = rayoIzq || rayoDer;`.
</details>

<details>
<summary><b>Solución 2: efecto coyote</b></summary>

**Idea:** guardar «hace cuánto que pisé suelo» y dejar saltar si fue hace poco.
```csharp
[SerializeField] float tiempoCoyote = 0.15f;   // margen en segundos
float contadorCoyote;

void Update()
{
    // ... movimiento ...
    DetectarSuelo();
    if (enSuelo)
        contadorCoyote = tiempoCoyote;          // en el suelo: margen lleno
    else
        contadorCoyote -= Time.deltaTime;       // en el aire: se va gastando
}

private void Saltar(InputAction.CallbackContext ctx)
{
    if (contadorCoyote > 0)                     // en vez de if (enSuelo)
    {
        rb.linearVelocityY = fuerzaSalto;
        contadorCoyote = 0;                     // gastado: no se puede volver a saltar en el aire con él
    }
}
```
Es un **contador de tiempo con `Time.deltaTime`**, justo lo que entra en el examen.
</details>

<details>
<summary><b>Solución 3: doble salto</b></summary>

```csharp
[SerializeField] int saltosExtra = 1;          // 1 = doble salto
int saltosRestantes;

void Update()
{
    // ...
    DetectarSuelo();
    if (enSuelo && rb.linearVelocityY <= 0.01f)  // en el suelo Y sin estar subiendo
        saltosRestantes = saltosExtra;           // recarga los saltos extra
}

private void Saltar(InputAction.CallbackContext ctx)
{
    if (enSuelo)                                 // (o contadorCoyote > 0 si hiciste la mejora 2)
        rb.linearVelocityY = fuerzaSalto;        // salto normal
    else if (saltosRestantes > 0)
    {
        rb.linearVelocityY = fuerzaSalto;        // salto en el aire
        saltosRestantes--;
    }
}
```
**Por qué `rb.linearVelocityY <= 0.01f`:** justo después de saltar, durante un frame, el rayo aún toca el suelo. Sin esta comprobación se recargarían los saltos al despegar y tendrías un salto de más.
</details>

<details>
<summary><b>Solución 4: pisar la tortuga</b></summary>

En `Jugador.cs`, sustituye el `OnCollisionEnter2D`:
```csharp
private void OnCollisionEnter2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Tortuga"))
    {
        Vector2 normal = collision.GetContact(0).normal;   // dirección del choque en el punto de contacto
        if (normal.y > 0.5f)                               // apunta hacia arriba = he caído ENCIMA
        {
            rb.linearVelocityY = fuerzaSalto * 2;          // salto involuntario del doble de fuerza
            Destroy(collision.gameObject);                 // la tortuga muere
        }
        else                                               // choque de lado o desde abajo
        {
            vida--;
            StartCoroutine(GolpeRojo());
            if (vida <= 0) StartCoroutine(Desaparecer());
        }
    }
}
```
**Si la normal te da problemas,** compara alturas: `if (transform.position.y > collision.transform.position.y + 0.5f)`, es decir, «estoy claramente por encima».
Para que la tortuga muera de forma más visual, en lugar de `Destroy` llama a un método suyo que lance una corrutina de desvanecerse.
</details>

<details>
<summary><b>Solución 5: patrulla con Topes</b></summary>

🖱️ **Montaje:**
1. **Create Empty** → `Tope` → **Box Collider 2D** con **Is Trigger ✔**. No le pongas sprite, o quítale el SpriteRenderer, para que sea invisible. Tag nuevo: `Tope`.
2. Hazlo prefab y coloca uno a cada extremo del recorrido de la tortuga.
3. La tortuga tiene **Rigidbody2D Dynamic** con **Freeze Rotation Z**, un collider **sin** trigger y el tag `Tortuga`.

```csharp
using UnityEngine;

public class Tortuga : MonoBehaviour
{
    [SerializeField] bool derecha = true;
    [SerializeField] float velocidad = 3;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Orientar();                                   // que empiece mirando bien
    }

    void Update()
    {
        float vx = derecha ? velocidad : -velocidad;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tope"))
        {
            derecha = !derecha;                        // invierte la dirección
            Orientar();                                // y el dibujo
        }
    }

    private void Orientar()
    {
        transform.eulerAngles = new Vector3(0, derecha ? 0 : 180, 0);  // mismo truco que el jugador
    }
}
```
⚠️ Si el sprite original de la tortuga mira a la izquierda, intercambia el 0 y el 180.
⚠️ El trigger salta porque la tortuga tiene Rigidbody2D: el Tope no lo necesita.
</details>

<details>
<summary><b>Solución 6: moneda que parpadea y desaparece</b></summary>

Script nuevo `Moneda.cs`. Añádelo al **prefab** `Moneda`, que es trigger y tiene el tag `Moneda`:
```csharp
using System.Collections;
using UnityEngine;

public class Moneda : MonoBehaviour
{
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(CicloDeVida());                // arranca al aparecer la moneda
    }

    IEnumerator CicloDeVida()
    {
        yield return new WaitForSeconds(5f);          // 1) espera 5 segundos
        for (int i = 0; i < 3; i++)                   // 2) parpadea 3 veces
        {
            sr.enabled = false;                       // ocultar (más simple que tocar el alfa)
            yield return new WaitForSeconds(0.2f);
            sr.enabled = true;                        // mostrar
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);                          // 3) desaparece
    }
}
```
**Versión sin corrutina**, por si te piden hacerlo con contador: en `Update`, `t += Time.deltaTime;`. Si `t >= 5`, calcula el parpadeo con `sr.enabled = ((int)((t - 5) / 0.2f)) % 2 == 1;`, y si `t >= 6.2f`, `Destroy(gameObject)`. Es más difícil de leer, y por eso aquí se usa la corrutina.
Si el jugador la coge antes, la destruye `Jugador.OnTriggerEnter2D` y la corrutina se para sola.
</details>

### Ejercicios extra para practicar
7. Haz que la bala destruya la tortuga y sume 50 puntos al jugador. Pista: la bala no tiene referencia al jugador. Haz que la tortuga avise, o busca al jugador con `FindFirstObjectByType<Jugador>()`.
8. Haz que una plataforma se mueva de izquierda a derecha sin parar. Pista: Rigidbody2D **Kinematic** y `linearVelocity`, que cambie de sentido con Topes como la tortuga, o con un contador de tiempo.
9. Haz que un objeto gire sin parar sobre sí mismo (90° por segundo) y que, al tocarlo, dé el doble de puntos.
10. Cuenta atrás de 60 segundos y, al llegar a 0, el jugador desaparece con `Desaparecer()`. Muéstrala con `Debug.Log`, porque la UI no entra.
