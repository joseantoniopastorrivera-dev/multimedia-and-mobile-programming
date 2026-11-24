# Guía para la recuperación: examen «Árboles» resuelto paso a paso

Unity 6 (6000.0.83f1), plantilla **Universal 2D**, C#. Resuelve el examen de la 1ª evaluación del 24/11/2026 (enunciado en [enunciado-examen-arboles.md](enunciado-examen-arboles.md)). Los scripts finales están en [solucion/Scripts/](solucion/Scripts/) y **compilan contra las DLL de Unity 6** (comprobado con `dotnet build`). No se han probado dentro del editor: el primer ensayo completo es tuyo.

## 0. Cómo usar esta guía

- **Imprímela** junto con la chuleta (`../apuntes-examen-unity.pdf`) y la guía paso a paso (`../guia-examen-unity-paso-a-paso.pdf`). Esta es la específica de este examen; las otras explican la interfaz con más detalle.
- Cada requisito tiene: **qué pide**, **pasos en el editor**, **código**, **explicación línea a línea**, **cómo comprobarlo** y **errores típicos**.
- La recuperación puede ser este mismo examen o uno parecido con otras imágenes. Por eso cada bloque termina con **🔧 variantes**: el cambio que podrían pedir y cómo hacerlo.
- Para ensayar, crea un proyecto **nuevo** fuera del repo (por ejemplo `C:\unity\ensayo-arboles`). No abras ni toques `practica-examen-1`.

## 1. El examen de un vistazo

| Pts | Requisito | Dónde se resuelve |
|---|---|---|
| — | Game en Full-HD + importar paquete | §3 Fase A |
| 0,5 | Escena como la imagen, profundidad con **z** | §4 |
| 2 | Cesta con flechas, `Rigidbody2D` a 10 u/s, sin girar ni subir | §5 · `Jugador.cs` |
| 0,5 | 2 límites sólidos invisibles | §6 |
| 1 | Pájaro con `transform.position` a 5 u/s + prefab ×3 | §7 · `Pajaro.cs` |
| 1,5 | Pájaro se da la vuelta en el borde | §8 · `Pajaro.cs` |
| 1 | Pájaro toca manzana → la destruye y +50 % velocidad | §9 · `Pajaro.cs` |
| 1 | Puntos aleatorios 1–3 al coger manzana, en consola | §10 · `Jugador.cs` |
| 1,5 | Spawner vacío: manzana cada 2 s en x [-10,5, 10,5], y [2, 5] | §11 · `Spawner.cs` |
| — | Entrega: **.unitypackage** con la escena guardada | §12 |

**Ojo:** en clase se dijo «entregar la carpeta Assets», pero el enunciado pide **exportar un package**. Haz lo que diga el enunciado del día.

### Visión general: qué objeto hace qué

```
Main Camera (Size 6)          → encuadre: se ven x de -10,67 a 10,67
Fondo (Square azul, z = 10)   → cielo, lo más lejano
Suelo (Square verde, z = 5)   → hierba
Arbol ×N (z = 5)              → decorado, sin collider
Jugador (cesta, z = 0)        → Rigidbody2D Dynamic sin gravedad · Jugador.cs
LimiteIzq / LimiteDer         → BoxCollider2D sin sprite (sólidos invisibles)
Pajaro ×3 (prefab, z = 0)     → Rigidbody2D Kinematic · Pajaro.cs
Spawner (vacío)               → Spawner.cs → Instantiate(prefab Manzana)
Manzana (prefab)              → Rigidbody2D Dynamic (cae) · Collider Is Trigger · tag Manzana
```

**Decisiones que debes saber justificar:**
- **Las manzanas caen con gravedad.** El enunciado no lo dice, pero salen arriba (y entre 2 y 5) y la cesta está abajo: si no cayeran, la cesta nunca podría cogerlas.
- **La manzana es trigger.** Así no empuja a nadie: la cesta y los pájaros solo «notan» que la tocan (`OnTriggerEnter2D`) y la destruyen.
- **La cesta usa `linearVelocity` y el pájaro `transform.position`.** Lo pide así el enunciado: uno por física y otro moviendo el Transform a mano con `Time.deltaTime`.
- **El pájaro lleva Rigidbody2D Kinematic.** Para que salten los triggers hace falta un Rigidbody2D en al menos uno de los dos objetos (la manzana ya lo tiene), pero el consejo del profe es «si se mueve, métele Rigidbody». Kinematic no tiene gravedad y no lo empujan las colisiones.

## 2. Plan de las 3 horas

| Tiempo | Qué | Puntos que aseguras |
|---|---|---|
| 0:00–0:20 | Fase A: proyecto, Game Full-HD, importar, carpetas, cámara, tags | base |
| 0:20–0:45 | Escena con z (§4) | 0,5 |
| 0:45–1:15 | Cesta + Input Actions + límites (§5–6) | 2,5 |
| 1:15–1:45 | Pájaro, prefab ×3, giro en borde (§7–8) | 2,5 |
| 1:45–2:10 | Prefab Manzana + Spawner (§11) | 1,5 |
| 2:10–2:30 | Pájaro come manzana + puntos (§9–10) | 2 |
| 2:30–2:45 | Probar todo con la tabla de errores (§13) | — |
| 2:45–3:00 | **Guardar escena + exportar package + subir** (§12) | **todo** |

Regla: **a las 2:40 dejas de programar** y exportas, aunque falte algo. Un package entregado con 7 puntos vale más que uno perfecto sin subir. Si te atascas más de 10 minutos en un requisito, sáltalo y vuelve al final.

## 3. Fase A: preparar el proyecto (0:00–0:20)

1. **Unity Hub → New project → Universal 2D** → nombre `ExamenArboles` → Create.
2. **Game a Full-HD:** pestaña **Game** → desplegable de resolución (pone *Free Aspect*) → **Full HD (1920x1080)**. Si no aparece: **+** → Width 1920, Height 1080 → OK.
3. **Importar el paquete:** **Assets → Import Package → Custom Package…** → elige el `.unitypackage` del examen → **Import** (todo marcado). Aparece `Assets/Sprites` con `arbol`, `cesta`, `manzana` y `pajaro`.
4. **Carpetas:** en Project, clic derecho en `Assets` → **Create → Folder**: `Prefabs`, `Scripts` (`Scenes` ya existe; los sprites ya vienen en `Sprites`).
5. **Cámara:** selecciona **Main Camera** → Inspector → **Size = 6**. Por qué: con 16:9 se ve de `-6 × 16/9 = -10,67` a `10,67` en x, así que las manzanas del spawner (x entre -10,5 y 10,5) salen **siempre dentro de la pantalla**. Con el Size 5 por defecto solo se ve hasta 8,89 y algunas manzanas nacerían fuera.
6. **Tags:** selecciona cualquier objeto → **Tag → Add Tag… → +** → `Manzana` → **Save**. Hazlo **ya**, antes de escribir código: si el script usa `CompareTag("Manzana")` y el tag no existe, sale el error `Tag: Manzana is not defined`.
7. **Ctrl+S** para guardar la escena (`Assets/Scenes/SampleScene`).

### Tamaño de los sprites

Todos vienen a **100 Pixels Per Unit**, así que salen con tamaños muy distintos: la cesta mide 7,2 unidades, el árbol 12,8, la manzana 5,12 y el pájaro solo 0,28. No hace falta tocar la importación: ajusta la **Scale** del Transform de cada objeto.

| Objeto | Scale (X = Y) | Tamaño final aprox. |
|---|---|---|
| Árbol | 0,5 | 6,4 × 3,6 |
| Cesta | 0,25 | 1,8 × 1,8 |
| Manzana | 0,12 | 0,6 × 0,6 |
| Pájaro | 3 | 0,85 × 0,85 |

El pájaro es pixel art (28 × 28 píxeles): selecciónalo en Project → **Filter Mode = Point (no filter)** → **Apply** para que no salga borroso al escalarlo.

## 4. Escena con profundidad en z (0,5 pts)

**Qué pide:** montar la escena como la imagen con los sprites del paquete **y** sprites básicos de Unity, usando la **z** para colocar unos detrás de otros.

**Pasos:**
1. **Sprites básicos:** en Project, clic derecho en `Sprites` → **Create → 2D → Sprites → Square**. Se crea un cuadrado blanco.
2. **Cielo:** arrastra el `Square` a la Hierarchy → F2 → `Fondo` → **Color** (en Sprite Renderer) azul claro → Scale X = 22, Y = 12 → **Position Z = 10**.
3. **Suelo:** otro `Square` → `Suelo` → verde → Scale X = 22, Y = 2 → Position (0, -5, **5**).
4. **Árboles:** arrastra `arbol` a la escena → Scale 0,5 → Position (-6, -1, **5**). Duplica con **Ctrl+D** y colócalos como en la imagen.
5. Todo lo que se mueve (cesta, pájaros, manzanas) queda en **z = 0**.

**Por qué funciona:** la cámara está en z = -10 mirando hacia z positivo. Con los objetos en la misma *Sorting Layer* y el mismo *Order in Layer*, URP 2D dibuja primero lo más lejano (z mayor) y encima lo más cercano (z menor). Fondo z 10 → suelo y árboles z 5 → juego z 0.

**Cómo demostrarlo:** en la Scene, pulsa el botón **2D** para pasar a 3D y gira la vista: se ven las capas separadas en profundidad.

**Errores típicos:**
- El fondo tapa todo → su z es menor que la de los demás (por ejemplo 0 o negativa). Ponle 10.
- Un objeto no se ve → su z es menor que -10 (queda detrás de la cámara) o mayor que el *Far* de la cámara (1000).
- Alguien tocó *Order in Layer*: el orden manda **antes** que la z. Déjalo a 0 en todos.

**Preguntas de la profesora:**
- *¿Por qué usas z y no Order in Layer?* → Porque lo pide el enunciado. Con todo en la misma capa y orden, Unity desempata por la distancia a la cámara.
- *¿La z afecta a la física 2D?* → No. La física 2D solo usa x e y: una manzana en z 0 y un árbol en z 5 chocarían si los dos tuvieran collider.

🔧 **Variante:** «pon las nubes delante de los árboles pero detrás de los pájaros» → nubes con z entre 0 y 5, por ejemplo z = 2.

## 5. Jugador: la cesta con las flechas (2 pts)

**Qué pide:** objeto **Jugador** con `cesta.png`; con flecha izquierda o derecha pulsada, velocidad del **Rigidbody2D** a 10 u/s en horizontal; sin flechas, se para; ni gira ni se mueve en vertical.

### 5.1 Objeto y componentes

1. Arrastra `cesta` a la Hierarchy → F2 → **Jugador** → Scale 0,25 → Position (0, -3.5, 0).
2. **Add Component → Rigidbody 2D**:
   - Body Type **Dynamic** (para que choque con los límites).
   - **Gravity Scale = 0** (no cae).
   - **Constraints → Freeze Position Y** ✔ y **Freeze Rotation Z** ✔ (ni sube ni gira).
3. **Add Component → Box Collider 2D** (se ajusta solo al sprite).

### 5.2 Input Actions (flechas)

1. En Project: clic derecho en `Assets` → **Create → Input Actions** → `Controles`. Doble clic para abrirlo.
2. Action Maps **+** → `Teclado`. Renombra la acción por defecto a **`Mover`**: **Action Type = Value**, **Control Type = Axis**.
3. En `Mover`, **+ → Add Positive/Negative Binding** (o *1D Axis Composite*).
   - **Negative** → **Path** → clic en el botón **T** (modo texto) → escribe `<Keyboard>/leftArrow`.
   - **Positive** → **T** → `<Keyboard>/rightArrow`.
4. Borra el binding vacío que venía. **Save Asset** (o activa *Auto-Save*).

Usa el modo **T** y no *Listen*: con el teclado español *Listen* a veces graba otra tecla (pasó con el espacio, que salió como `-`).

### 5.3 Código: `Scripts/Jugador.cs`

```csharp
using UnityEngine;
using UnityEngine.InputSystem;   // InputActionReference

public class Jugador : MonoBehaviour
{
    [SerializeField] InputActionReference mover;   // acción Mover (flechas)
    [SerializeField] float velocidad = 10;         // 10 unidades por segundo
    [SerializeField] int puntos = 0;               // visible en el Inspector
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        mover.action.Enable();
    }

    void OnDisable()
    {
        mover.action.Disable();
    }

    void Update()
    {
        float direccion = mover.action.ReadValue<float>();       // -1, 0 o 1
        rb.linearVelocity = new Vector2(direccion * velocidad, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Manzana"))
        {
            Destroy(collision.gameObject);
            puntos += Random.Range(1, 4);          // 1, 2 o 3
            Debug.Log("Puntos: " + puntos);
        }
    }
}
```

**Línea a línea** (la parte de los puntos se explica en §10):
- `using UnityEngine.InputSystem;` → da acceso a `InputActionReference`, del sistema de entrada nuevo que trae Unity 6.
- `[SerializeField] InputActionReference mover;` → campo privado pero visible en el Inspector: ahí arrastras la acción `Teclado/Mover` del asset `Controles`. Uso `SerializeField` y no `public` porque ningún otro script necesita tocarlo.
- `float velocidad = 10;` → el valor del enunciado. Al estar serializado se puede cambiar en el Inspector sin tocar el código, pero **el Inspector manda**: si allí pone 5, va a 5.
- `rb = GetComponent<Rigidbody2D>();` → guarda el Rigidbody2D del propio objeto una sola vez en `Start`, en lugar de buscarlo en cada frame.
- `mover.action.Enable();` → las acciones nacen **desactivadas**: sin esta línea `ReadValue` devuelve siempre 0 y la cesta no se mueve. `OnDisable` la apaga cuando el objeto se desactiva.
- `ReadValue<float>()` → el 1D Axis devuelve **-1** con la flecha izquierda, **1** con la derecha y **0** con ninguna (o con las dos a la vez).
- `rb.linearVelocity = new Vector2(direccion * velocidad, 0);` → **marca la velocidad** del Rigidbody2D, como pide el enunciado: -10, 0 o 10 en x. La y siempre es 0 (no se mueve en vertical, junto con *Freeze Position Y*). Como se asigna en cada frame, al soltar las flechas `direccion` vale 0 y la cesta **se para en seco**.

**Cómo comprobarlo:** ▶ → **clic dentro de la ventana Game** (si no, no tiene el foco del teclado) → flechas. En el Inspector del Rigidbody2D (sección *Info*) se ve *Linear Velocity* = (10, 0) / (-10, 0) / (0, 0).

**Errores típicos:**
- `NullReferenceException` en `mover.action` → no has arrastrado la acción al hueco **Mover** del script.
- No se mueve → falta `Enable()`, no hiciste clic en Game, o el asset no está guardado (*Save Asset*).
- La cesta cae → Gravity Scale no está a 0 o falta *Freeze Position Y*.
- La cesta gira al chocar con el límite → falta *Freeze Rotation Z*.
- Se desliza al soltar → estás usando `AddForce` en vez de asignar `linearVelocity`.
- `InvalidOperationException: You are trying to read Input using the UnityEngine.Input class…` → usaste `Input.GetKey`, del sistema antiguo, y el proyecto está en el nuevo. Usa el Plan B de abajo.

**Plan B sin Input Actions** (si el `.inputactions` te da guerra, cambia solo `Update` y borra `mover`, `OnEnable` y `OnDisable`):

```csharp
void Update()
{
    float direccion = 0;
    if (Keyboard.current.leftArrowKey.isPressed) direccion = -1;
    if (Keyboard.current.rightArrowKey.isPressed) direccion = 1;
    rb.linearVelocity = new Vector2(direccion * velocidad, 0);
}
```

`Keyboard.current` lee el teclado directamente con el sistema nuevo. `isPressed` es verdadero **mientras** la tecla está pulsada, que es justo lo que pide el enunciado.

**Preguntas de la profesora:**
- *¿Por qué Dynamic y no Kinematic?* → Un Kinematic no lo paran los colliders estáticos: atravesaría los límites invisibles. Dynamic sí choca; la gravedad la anulo con Gravity Scale 0.
- *¿Por qué `linearVelocity` y no `transform.position`?* → Lo pide el enunciado, y además así la física detecta los choques con los límites. Moviendo el Transform a mano la cesta se «teletransporta» y puede atravesarlos.
- *¿Qué diferencia hay entre `linearVelocity` y `velocity`?* → Es lo mismo: Unity 6 renombró `velocity` a `linearVelocity`. El nombre antiguo sale como obsoleto.

🔧 **Variantes:**
- «Que se mueva con A/D» → en `Controles` cambia los paths a `<Keyboard>/a` y `<Keyboard>/d` (o añade un segundo 1D Axis con ellas).
- «Que mire hacia donde va» → al final de `Update`: `if (direccion > 0) transform.eulerAngles = new Vector3(0, 0, 0); else if (direccion < 0) transform.eulerAngles = new Vector3(0, 180, 0);`
- «Que acelere poco a poco» → `rb.AddForce(new Vector2(direccion * fuerza, 0));` y pon *Linear Damping* a 2 para que frene.

## 6. Límites invisibles (0,5 pts)

**Qué pide:** dos objetos **sólidos** que impidan salir a la cesta por los laterales **sin verse**.

1. **GameObject → Create Empty** → F2 → `LimiteIzq` → Position (-11.2, -3.5, 0).
2. **Add Component → Box Collider 2D** → Size X = 1, Y = 4. **Sin** Is Trigger (tiene que ser sólido) y **sin** Rigidbody2D (es estático).
3. **Ctrl+D** → `LimiteDer` → Position X = 11.2.

Es invisible porque es un objeto vacío: no tiene Sprite Renderer. Se ve como un rectángulo verde solo en la Scene (gizmo del collider), nunca en Game.

**Por qué x = ±11,2:** el borde de la pantalla está en ±10,67 (§3). El collider mide 1 de ancho, así que su cara interior queda en ±10,7: la cesta se para justo al llegar al borde.

**Errores típicos:**
- La cesta atraviesa el límite → el límite tiene Is Trigger marcado, o la cesta es Kinematic.
- La cesta tiembla contra el límite → normal en Dynamic con mucha velocidad. Pon *Collision Detection = Continuous* en el Rigidbody2D de la cesta.

**Pregunta:** *¿Por qué no le pones Rigidbody2D al límite?* → Un collider sin Rigidbody2D es **estático**: no se mueve nunca y es lo más barato para la física. Solo necesita Rigidbody2D lo que se mueve.

## 7. Pájaro con `transform.position` + prefab ×3 (1 pt)

**Qué pide:** `Pajaro` con `pajaro.png` que avanza hacia **su parte delantera** cambiando `transform.position` con el tiempo, a 5 u/s. Hacer un **prefab** y poner **3** en la escena.

1. Arrastra `pajaro` a la Hierarchy → F2 → **Pajaro** → Scale 3 → Position (-5, 3, 0).
2. **Add Component → Rigidbody 2D** → **Kinematic**.
3. **Add Component → Circle Collider 2D**.
4. **Add Component → Pajaro** (el script, §7.1).
5. **Prefab:** arrastra `Pajaro` **desde la Hierarchy** a la carpeta `Prefabs`. Se vuelve azul en la Hierarchy.
6. Arrastra el prefab de `Prefabs` a la escena dos veces más y colócalos como en la imagen (por ejemplo y = 2, 3 y 4, con x distintas). Así tienes 3 instancias.
7. Si alguno debe empezar yendo a la izquierda: en **esa instancia** pon **Rotation Y = 180**.

### 7.1 Código: `Scripts/Pajaro.cs`

```csharp
using UnityEngine;

public class Pajaro : MonoBehaviour
{
    [SerializeField] float velocidad = 5;
    float borde;

    void Start()
    {
        borde = Camera.main.orthographicSize * Camera.main.aspect;
    }

    void Update()
    {
        transform.position += transform.right * velocidad * Time.deltaTime;

        if (transform.position.x > borde)
            transform.eulerAngles = new Vector3(0, 180, 0);
        else if (transform.position.x < -borde)
            transform.eulerAngles = new Vector3(0, 0, 0);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Manzana"))
        {
            Destroy(collision.gameObject);
            velocidad *= 1.5f;
        }
    }
}
```

**Movimiento, línea a línea:**
- `transform.right` → el eje X rojo **del propio pájaro**: su «parte delantera», porque el sprite mira a la derecha. Si el pájaro está girado 180° en Y, `transform.right` vale (-1, 0, 0) y avanza hacia la izquierda **sin cambiar el código**. Por eso no uso `Vector3.right`, que siempre apunta a la derecha del mundo.
- `* velocidad * Time.deltaTime` → `deltaTime` son los segundos desde el frame anterior (unos 0,016 a 60 FPS). Multiplicar por él convierte «5 por frame» en «**5 por segundo**», igual en un ordenador rápido o lento. **Esto es «en función del tiempo»** del enunciado.
- `transform.position +=` → mueve el Transform directamente, como pide el enunciado, sin usar la física.

**Cómo comprobarlo:** ▶ y mira el Transform de un pájaro: la x crece unas 5 unidades por segundo.

**Errores típicos:**
- Va hacia atrás → el sprite mira a la izquierda: usa `-transform.right`.
- Va rapidísimo o lentísimo según el PC → falta `Time.deltaTime`.
- Los 3 pájaros no cambian al editar el prefab → editaste una instancia. Abre el prefab con doble clic en `Prefabs`, o en la instancia usa **Overrides → Apply All**.

🔧 **Variantes:**
- «Que el pájaro también suba y baje» → `transform.position += new Vector3(0, Mathf.Sin(Time.time * 2) * Time.deltaTime, 0);` debajo del avance.
- «Velocidad distinta en cada pájaro» → cambia `velocidad` en el Inspector de cada instancia (queda en negrita: es un *override*).

## 8. El pájaro se da la vuelta en el borde (1,5 pts)

**Qué pide:** al llegar al borde de la pantalla, el pájaro **se gira visualmente** y vuelve en dirección contraria.

Está en el mismo `Pajaro.cs`:
- `borde = Camera.main.orthographicSize * Camera.main.aspect;` → calcula la x del borde **a partir de la cámara**: *Size* es la media altura visible (6) y *aspect* es ancho/alto (1920/1080 = 1,78). 6 × 1,78 = **10,67**. Si cambias el Size o la resolución, el borde se recalcula solo. Se hace en `Start` porque no cambia durante la partida.
- `if (transform.position.x > borde)` → ha pasado el borde derecho → `eulerAngles = (0, 180, 0)`: gira 180° sobre el eje Y. Visualmente el sprite queda **en espejo** (mira a la izquierda) y, como el avance usa `transform.right`, **la dirección cambia sola**. Un solo cambio da las dos cosas que pide el enunciado.
- `else if (x < -borde)` → borde izquierdo → `eulerAngles = (0, 0, 0)`: vuelve a mirar a la derecha.

**Por qué asigno ángulos fijos y no `transform.Rotate(0, 180, 0)`:** con `Rotate`, en el frame siguiente el pájaro sigue un poco fuera del borde y volvería a girar, quedándose temblando. Asignar «mira a la izquierda» es idempotente: repetirlo no cambia nada.

**Cómo comprobarlo:** ▶ y deja pasar unos segundos: cada pájaro llega al borde, se ve girado y vuelve.

**Errores típicos:**
- Se gira pero sigue en la misma dirección → mueves con `Vector3.right` en lugar de `transform.right`.
- Gira antes del borde o sale de la pantalla → el Size de la cámara no es el que crees, o la ventana Game no está en Full-HD.
- Se queda temblando en el borde → usaste `Rotate` (ver arriba).

**Pregunta:** *¿Qué son los ángulos de Euler?* → La rotación expresada como 3 ángulos en grados, uno por eje (X, Y, Z). Unity la guarda internamente como un *Quaternion*; `eulerAngles` permite leerla y escribirla en grados, que es lo que vemos en el Inspector.

🔧 **Variante:** «que se gire con `localScale` en vez de rotación» → `transform.localScale = new Vector3(-3, 3, 1);`. Pero entonces `transform.right` **no** cambia: tendrías que guardar la dirección en una variable `int direccion = 1;`, moverte con `Vector3.right * direccion` y cambiar el signo en el borde.

## 9. El pájaro toca una manzana: la destruye y acelera (1 pt)

**Prefab Manzana** (lo usan este requisito, el 10 y el 11):
1. Arrastra `manzana` a la Hierarchy → F2 → **Manzana** → Scale 0,12 → **Tag = Manzana**.
2. **Add Component → Rigidbody 2D** → Dynamic, Gravity Scale 1 (cae despacio; súbelo si la quieres más rápida).
3. **Add Component → Circle Collider 2D** → **Is Trigger** ✔.
4. *(Opcional)* **Add Component → Manzana** (el script que la borra a los 6 s).
5. Arrástrala a `Prefabs` y **bórrala de la Hierarchy**: las crea el Spawner.

**Código** (en `Pajaro.cs`):

```csharp
void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Manzana"))
    {
        Destroy(collision.gameObject);
        velocidad *= 1.5f;
    }
}
```

- `OnTriggerEnter2D` → Unity la llama sola el primer frame en que el collider del pájaro **entra** en un trigger. Funciona porque la manzana es trigger y al menos uno de los dos tiene Rigidbody2D.
- `collision.CompareTag("Manzana")` → solo reacciona a manzanas. `CompareTag` es más rápido que `collision.tag == "Manzana"` y avisa si el tag no existe.
- `Destroy(collision.gameObject)` → destruye **la manzana**. Con `Destroy(gameObject)` destruirías al pájaro.
- `velocidad *= 1.5f;` → +50 %: 5 → 7,5 → 11,25… Es acumulativo: cada manzana lo acelera otro 50 % sobre la velocidad actual. La `f` hace falta porque `1.5` sin ella es `double` y `velocidad` es `float`.

**Cómo comprobarlo:** ▶ → selecciona un pájaro y mira su campo *Velocidad* en el Inspector mientras cruza las manzanas que caen.

**Errores típicos:**
- No pasa nada al tocarse → la manzana no tiene el tag `Manzana`, no tiene Is Trigger, o el método está mal escrito (`OnTriggerEnter` sin `2D`, o con `Collision2D` en vez de `Collider2D`).
- Se destruye el pájaro → `Destroy(gameObject)` en lugar de `Destroy(collision.gameObject)`.

🔧 **Variante:** «que acelere como máximo hasta 15» → `velocidad = Mathf.Min(velocidad * 1.5f, 15);`.

## 10. Puntos aleatorios de 1 a 3 (1 pt)

En `Jugador.cs`:

```csharp
void OnTriggerEnter2D(Collider2D collision)
{
    if (collision.CompareTag("Manzana"))
    {
        Destroy(collision.gameObject);
        puntos += Random.Range(1, 4);
        Debug.Log("Puntos: " + puntos);
    }
}
```

- `int puntos = 0;` → empieza en 0, como pide el enunciado. Está serializado para verlo en el Inspector durante el Play.
- `Random.Range(1, 4)` → **con enteros el máximo NO entra**: devuelve 1, 2 o 3. `Random.Range(1, 3)` solo daría 1 o 2: es **el error más típico de este requisito**. Con floats sí entran los dos extremos (por eso el spawner usa `-10.5f, 10.5f`).
- Se llama dentro del `if`, así que sale un número nuevo **en cada impacto**.
- `Debug.Log("Puntos: " + puntos);` → escribe el total acumulado en la **Console** en ese momento.

**Cómo comprobarlo:** ▶ → coge varias manzanas → la Console muestra `Puntos: 2`, `Puntos: 3`, `Puntos: 6`… (sube de 1 en 1, 2 en 2 o 3 en 3).

**Pregunta:** *¿Por qué los puntos en el Jugador y no en la Manzana?* → Porque la manzana se destruye: si guardara los puntos, se perderían con ella. El dato tiene que vivir en un objeto que dure toda la partida.

🔧 **Variante:** «a los 10 puntos, gana» → después del `Debug.Log`: `if (puntos >= 10) { Debug.Log("¡Has ganado!"); Time.timeScale = 0; }` (`timeScale = 0` congela el juego).

## 11. Spawner de manzanas cada 2 s (1,5 pts)

1. **GameObject → Create Empty** → F2 → **Spawner**.
2. **Add Component → Spawner** (el script).
3. Arrastra el **prefab** `Manzana` desde la carpeta `Prefabs` al hueco **Prefab Manzana**. Del Project, no de la Hierarchy.

```csharp
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefabManzana;
    [SerializeField] float intervalo = 2;

    void Start()
    {
        StartCoroutine(CrearManzanas());
    }

    IEnumerator CrearManzanas()
    {
        while (true)
        {
            yield return new WaitForSeconds(intervalo);

            float x = Random.Range(-10.5f, 10.5f);
            float y = Random.Range(2f, 5f);
            Instantiate(prefabManzana, new Vector3(x, y, 0), Quaternion.identity);
        }
    }
}
```

**Línea a línea:**
- `using System.Collections;` → ahí está `IEnumerator`. Sin esta línea: error `CS0246`.
- `StartCoroutine(CrearManzanas());` → arranca la corrutina una vez. A partir de ahí se ejecuta «a trozos», frame a frame.
- `IEnumerator CrearManzanas()` → una **corrutina** es un método que puede **pausarse** (`yield return`) y seguir donde lo dejó, sin bloquear el juego.
- `while (true)` → bucle infinito, pero **no congela Unity** porque en cada vuelta el `yield` devuelve el control al motor durante 2 segundos. Un `while (true)` sin `yield` sí colgaría el editor.
- `yield return new WaitForSeconds(intervalo);` → «vuelve dentro de 2 segundos».
- `Random.Range(-10.5f, 10.5f)` y `Random.Range(2f, 5f)` → floats: incluyen los extremos. La `f` es obligatoria.
- `Instantiate(prefab, posición, rotación)` → crea una copia del prefab. `Quaternion.identity` = sin rotación.

**Alternativas que también valen** (el profe no exige un método concreto):

```csharp
// Con InvokeRepeating: llama a CrearManzana cada 2 s empezando a los 2 s
void Start() { InvokeRepeating(nameof(CrearManzana), 2, 2); }
void CrearManzana()
{
    Instantiate(prefabManzana, new Vector3(Random.Range(-10.5f, 10.5f), Random.Range(2f, 5f), 0), Quaternion.identity);
}
```

```csharp
// Con un contador de tiempo en Update
float contador = 0;
void Update()
{
    contador += Time.deltaTime;          // suma los segundos reales
    if (contador >= intervalo)
    {
        contador = 0;
        Instantiate(prefabManzana, new Vector3(Random.Range(-10.5f, 10.5f), Random.Range(2f, 5f), 0), Quaternion.identity);
    }
}
```

**Cómo comprobarlo:** ▶ → en la Hierarchy aparece una `Manzana(Clone)` cada 2 segundos, cada vez en una x distinta.

**Errores típicos:**
- `UnassignedReferenceException: prefabManzana` → hueco vacío en el Inspector.
- Las manzanas desaparecen de la escena al borrar la original → arrastraste el objeto de la Hierarchy en vez del prefab de `Prefabs`.
- Sale una sola manzana → falta el `while (true)`.
- Salen miles a la vez → falta el `yield return` dentro del `while` (y el editor se cuelga).

**Preguntas de la profesora:**
- *¿Qué es una corrutina y por qué la usamos?* → Un método que puede pausarse con `yield return` y continuar más tarde sin congelar el juego. La usamos para esperas («cada 2 s», «durante 0,2 s ponlo rojo») sin llevar contadores en `Update`.
- *¿Por qué el Spawner es un objeto vacío?* → Solo necesita ejecutar un script: no se ve ni choca, así que no necesita Sprite Renderer ni collider.

🔧 **Variante:** «cada vez más rápido» → al final del `while`: `intervalo = Mathf.Max(0.5f, intervalo - 0.1f);`.

## 12. Entrega: exportar el package

1. **Ctrl+S** (guardar la escena). Si el nombre de la pestaña de la escena tiene `*`, **no** está guardada.
2. En Project, clic derecho en **Assets → Export Package…**
3. Comprueba que salen marcados: `Scenes/SampleScene`, `Scripts/*.cs`, `Prefabs/*`, `Sprites/*`, `Controles.inputactions` y el `Square`.
4. **Include dependencies** ✔ → **Export…** → nombre `ExamenArboles-Apellido.unitypackage` → Guardar.
5. **Prueba de oro (2 min):** crea un proyecto Universal 2D vacío, importa tu package y dale a ▶. Si funciona allí, funcionará en el ordenador del profe.
6. Sube el `.unitypackage` al Aula Virtual y **comprueba que aparece** en la entrega.

## 13. «No funciona»: síntoma → causa → arreglo

| Síntoma | Causa | Arreglo |
|---|---|---|
| La cesta no se mueve | Acción sin `Enable()`, Game sin foco o hueco *Mover* vacío | `Enable()` en `OnEnable`, clic en Game, arrastrar la acción |
| La cesta cae | Gravity Scale 1 | Gravity Scale 0 + Freeze Position Y |
| La cesta sale de la pantalla | Límites con Is Trigger, cesta Kinematic o límites mal colocados | Límites sólidos en x = ±11,2, cesta Dynamic |
| La cesta gira al chocar | Falta Freeze Rotation Z | Constraints → Freeze Rotation Z |
| Pájaro va al revés | Sprite mira a la izquierda | `-transform.right` |
| Pájaro no se gira en el borde | `Vector3.right` en vez de `transform.right` | usar `transform.right` |
| Pájaro tiembla en el borde | `transform.Rotate` | asignar `eulerAngles` fijos |
| No se detecta la manzana | Sin tag, sin Is Trigger o sin Rigidbody2D | Tag `Manzana`, Is Trigger, Rigidbody2D en la manzana |
| `Tag: Manzana is not defined` | Tag no creado | Tag → Add Tag… → `Manzana` |
| Puntos solo 1 o 2 | `Random.Range(1, 3)` | `Random.Range(1, 4)` |
| `UnassignedReferenceException` | Hueco del Inspector vacío | Arrastrar el prefab o la acción |
| `CS0246 IEnumerator` | Falta `using System.Collections;` | Añadirlo |
| `CS0101` (clase duplicada) | Dos ficheros con la misma clase | Borrar el sobrante (con su `.meta`) |
| El script no se puede añadir | Nombre del fichero ≠ nombre de la clase | `Pajaro.cs` ↔ `class Pajaro` |
| Error de `Input.GetKey` | Sistema de entrada antiguo | Input Actions o `Keyboard.current` |
| Objeto invisible | z detrás de la cámara o tapado por el fondo | z entre -9 y 10, fondo en z 10 |

Siempre: **Console → Clear** → ▶ → lee **el primer error rojo** y haz doble clic: te lleva a la línea.

## 14. ¿Qué pasaría si…?

- **…pongo la cesta Kinematic?** Se mueve igual con `linearVelocity`, pero atraviesa los límites, porque un Kinematic no choca con colliders estáticos.
- **…quito `Time.deltaTime` al pájaro?** Iría a 5 unidades **por frame**: unas 300 u/s a 60 FPS, y distinto en cada ordenador.
- **…la manzana no es trigger?** Choca físicamente: empuja la cesta y rebota en los pájaros. Habría que usar `OnCollisionEnter2D(Collision2D collision)` y `collision.gameObject.CompareTag(...)`.
- **…ningún objeto tiene Rigidbody2D?** No salta ni un trigger ni una colisión: Unity necesita al menos un Rigidbody2D en la pareja.
- **…el Spawner instancia el objeto de la escena en lugar del prefab?** Funciona hasta que esa manzana se destruye. Desde entonces: `MissingReferenceException`.
- **…pongo la cámara a Size 5?** El pájaro se gira en ±8,89 (lo calcula solo), pero algunas manzanas nacen fuera de la pantalla, entre 8,89 y 10,5.

## 15. Chuleta final y plan de ensayo

**Componentes por objeto:**

| Objeto | Rigidbody2D | Collider | Script | Tag |
|---|---|---|---|---|
| Jugador (cesta) | Dynamic, Gravity 0, Freeze Pos Y + Rot Z | Box | Jugador | — |
| LimiteIzq/Der | — | Box (sólido) | — | — |
| Pajaro (prefab) | Kinematic | Circle | Pajaro | — |
| Manzana (prefab) | Dynamic | Circle, **Is Trigger** | (Manzana) | **Manzana** |
| Spawner | — | — | Spawner | — |

**Números del enunciado:** cesta 10 · pájaro 5 · +50 % = `*= 1.5f` · puntos `Random.Range(1, 4)` · spawner cada 2 s, x `-10.5f..10.5f`, y `2f..5f` · cámara Size 6 · Game 1920×1080.

**Plan de ensayo** (antes de la recuperación):
1. **Ensayo 1, con la guía delante (≈2 h):** proyecto nuevo, sigue §3–§12 sin saltarte nada y exporta el package.
2. **Ensayo 2, solo con la tabla de §1 y la chuleta (≈1 h 30):** cronométrate con el plan de §2.
3. **Ensayo 3, variantes:** haz 3 🔧 al azar (A/D, puntos para ganar, InvokeRepeating…).
4. **El día antes:** repasa §13 y las preguntas de la profesora. Imprime esta guía, la chuleta y la guía paso a paso.
