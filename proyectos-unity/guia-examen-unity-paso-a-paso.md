# Guía paso a paso del examen de Unity (PMDM)

> **Para qué sirve:** llevarla impresa al examen y seguirla **en orden**, del enunciado a la entrega. Todo lo de la interfaz va clic a clic, y todo el código está completo para copiar.
> **Se complementa con** `apuntes-examen-unity.pdf` (la chuleta), que tiene las tablas de teoría y las 6 mejoras de Plataformas resueltas.
>
> Unity 6 (6000.0.83f1) · plantilla **Universal 2D** · 3 horas · solo dan imágenes · entrega: `Assets/` con `Images`, `Prefabs`, `Scenes` y `Scripts`.

## Índice
- [0. Mapa de ventanas y atajos](#0-mapa-de-ventanas-y-atajos)
- [1. Plan de las 3 horas](#1-plan-de-las-3-horas)
- [2. Fase A: preparar el proyecto (0:00-0:20)](#2-fase-a-preparar-el-proyecto-000-020)
- [3. Fase B: montar la escena (0:20-0:50)](#3-fase-b-montar-la-escena-020-050)
- [4. Fase C: controles con Input Actions (0:50-1:00)](#4-fase-c-controles-con-input-actions-050-100)
- [5. Fase D: scripts y conectarlos (1:00-2:30)](#5-fase-d-scripts-y-conectarlos-100-230)
  - [5.1 Crear un script y abrirlo](#51-crear-un-script-y-abrirlo)
  - [5.2 Receta JUGADOR](#52-receta-jugador)
  - [5.3 Receta BALA](#53-receta-bala)
  - [5.4 Receta MONEDA](#54-receta-moneda)
  - [5.5 Receta SPAWNER](#55-receta-spawner)
  - [5.6 Receta ENEMIGO QUE PATRULLA](#56-receta-enemigo-que-patrulla)
  - [5.7 Receta OBJETO QUE GIRA o SE MUEVE SOLO](#57-receta-objeto-que-gira-o-se-mueve-solo)
  - [5.8 Receta TIEMPO: contador y cuenta atrás](#58-receta-tiempo-contador-y-cuenta-atrás)
- [6. Fase E: probar y entregar (2:30-3:00)](#6-fase-e-probar-y-entregar-230-300)
- [7. Si algo falla](#7-si-algo-falla)
- [8. Plantilla vacía de script](#8-plantilla-vacía-de-script)

---

## 0. Mapa de ventanas y atajos

```
┌───────────┬─────────────────────────────┬────────────┐
│ HIERARCHY │ SCENE (editar) / GAME (ver) │ INSPECTOR  │
│ objetos   │                             │ componentes│
│ de la     │                             │ del objeto │
│ escena    │                             │ elegido    │
├───────────┴─────────────────────────────┤            │
│ PROJECT (archivos)  │  CONSOLE (errores) │            │
└─────────────────────────────────────────┴────────────┘
```

| Atajo | Qué hace |
|---|---|
| **W** / **E** / **R** | Herramienta mover / rotar / escalar (con el ratón en la Scene) |
| **Ctrl+S** | Guardar la escena. Hazlo cada 5 minutos |
| **Ctrl+D** | Duplicar el objeto seleccionado |
| **F2** | Renombrar |
| **F** | Centrar la vista en el objeto seleccionado (ratón sobre la Scene) |
| **Ctrl+P** | Play / Stop |
| **Ctrl+Shift+C** | Abrir la Console |
| Rueda del ratón | Zoom en la Scene |
| Botón central (o clic derecho) + arrastrar | Moverse por la Scene |

### Cómo añadir un componente (Add Component)
Cada vez que la guía dice **«Add Component → X»**:
1. Selecciona el objeto **en la Hierarchy** (columna izquierda). El Inspector, a la derecha, muestra sus componentes: Transform, Sprite Renderer…
2. **Baja del todo en el Inspector** con la rueda del ratón. Al final, debajo del último componente (a veces debajo de un bloque gris de *Material*), está el botón ancho **Add Component**.
3. Haz clic → se abre un buscador → **escribe el nombre** (por ejemplo, `Rigidbody 2D`, `Box Collider 2D` o el nombre de tu script) → haz clic en el resultado.
4. El componente aparece en el Inspector, **encima** del botón. Despliégalo con la flechita ▶ para ver sus opciones.
5. Para quitar un componente: los **tres puntos ⋮** a su derecha → **Remove Component**.

**Para comprobar que un objeto está completo,** mira los títulos de sus componentes en el Inspector. Una bala, por ejemplo, tiene que mostrar: *Transform*, *Sprite Renderer*, *Rigidbody 2D*, *Circle Collider 2D* y *Bala (Script)*.

### Arrastrar
- **Arrastrar** algo de **Project** a la **Hierarchy** o a la **Scene** lo pone en la escena.
- **Arrastrar** algo de la **Hierarchy** a la carpeta `Prefabs` lo convierte en **prefab** (se pone azul).
- **Arrastrar** algo de **Project** a un **hueco del Inspector** rellena una variable de script.

⚠️ **Nunca cambies nada en Play** (el botón ▶ en azul): al parar, se pierde todo.

---

## 1. Plan de las 3 horas

```
 0:00 ├─ A. Proyecto, carpetas e imágenes ┤ 0:20
 0:20 ├─ B. Escena: suelo, jugador y prefabs ┤ 0:50
 0:50 ├─ C. Input Actions ┤ 1:00
 1:00 ├─ D. Scripts: lo que pida el enunciado ─────┤ 2:30
 2:30 ├─ E. Probar todo, Ctrl+S y entregar ┤ 3:00
```

**Orden dentro de D:** primero lo que da más nota con menos riesgo. Mover al jugador → saltar → recoger monedas (trigger) → disparar (Instantiate) → destruir (Destroy) → efectos de tiempo (corrutinas). **Prueba con ▶ cada cosa nada más hacerla.** Si algo se atasca más de 10 minutos, sáltalo y vuelve al final.

**Antes de empezar:** lee el enunciado entero y apunta en un papel qué objetos hay (jugador, enemigo, moneda…), qué hace cada uno y qué tecla usa. Cada objeto es una receta de la sección 5.

---

## 2. Fase A: preparar el proyecto (0:00-0:20)

### A1. Crear el proyecto
1. **Unity Hub** → **Nuevo proyecto**.
2. Arriba, comprueba que la **Versión del Editor** es **6000.0.83f1**.
3. Plantilla **Universal 2D**. ⚠️ Por defecto viene marcada *Universal 3D*: haz clic en la 2D.
4. **Nombre del proyecto:** tu nombre y el del examen. **Ubicación:** tu carpeta personal.
5. Desmarca *AI Assistant* y *Unity CLI*, y no elijas proveedor de control de código fuente.
6. **Crear proyecto**. Tarda 1 o 2 minutos.

> En el aula (Linux): si Hub dice «Localizar o instalar Unity Editor», ve a **Localizar** → `/opt/Unity-6000.0.83f1/Editor/` → el archivo **`Unity`** de 96 MB, el del icono de exclamación (no el `.debug`). Si dice «No hay licencias activas»: **Gestionar licencias → Añadir → Obtener una licencia personal gratuita**.

### A2. Carpetas
1. En **Project**, haz clic en `Assets`.
2. Clic derecho en un hueco vacío → **Create → Folder** → `Images`. Repite con `Prefabs` y `Scripts`. `Scenes` ya existe.

### A3. Importar las imágenes
1. Arrastra las imágenes del profe desde el explorador de archivos a la carpeta **`Images`** de Project.
2. Selecciona **todas** (clic en la primera y **Shift+clic** en la última) y, en el Inspector:
   - **Texture Type:** Sprite (2D and UI)
   - **Filter Mode:** Point (no filter), si es pixel art. Si son dibujos lisos, déjalo en Bilinear.
   - **Compression:** None
   - Pulsa **Apply**.

### A4. ¿Imagen suelta u hoja de sprites?
- **Un solo dibujo:** **Sprite Mode = Single**. Ya está.
- **Varios dibujos en una imagen** (hoja): **Sprite Mode = Multiple** → **Apply** → botón **Open Sprite Editor**. Si no aparece: **Window → 2D → Sprite Editor**. Después:
  1. Desplegable **Slice**, arriba a la izquierda → **Type: Automatic**, **Pivot: Center**, **Method: Delete Existing** → botón **Slice**.
  2. **Apply**, arriba a la derecha, y cierra.
  3. En Project, pulsa la **flechita ▶** de la imagen: salen los recortes `_0`, `_1`… Usa esos.
  4. Si un recorte sale con varios dibujos juntos (porque están unidos por una línea), usa otro fotograma o dibuja tú el recuadro arrastrando con el ratón en el Sprite Editor, y después **Apply**.
  5. **Si Automatic no corta nada:** la imagen tiene **fondo opaco** (negro, blanco…). Automatic solo separa por transparencia. Opciones:
     - **Slice → Type: Grid By Cell Size** (o *Grid By Cell Count*) → el tamaño de cada celda (por ejemplo, 16 × 16) → **Slice**. Corta en cuadrados iguales aunque haya fondo.
     - **A mano:** borra el recorte que ocupa toda la imagen (selecciónalo y pulsa Supr) → zoom con la rueda → **arrastra en una zona vacía** para dibujar el recuadro → ajusta con los cuadraditos o con X/Y/W/H → **Name** y **Pivot Center** → **Apply**.
     - ⚠️ El fondo opaco **viaja con el recorte** (se ve un cuadro negro). Unity no lo quita. Usa un PNG con transparencia, o una forma de Unity (Create → 2D → Sprites → Circle) como sustituto.

### A5. Tamaño (Pixels Per Unit)
- **Más PPU = más pequeño.** Si al ponerlo en la escena sale **gigante**, **sube** Pixels Per Unit. Si sale **diminuto**, **bájalo** (por ejemplo, de 100 a 32 o a 16). Pulsa **Apply**.
- Referencia: un personaje debería medir de 1 a 2 cuadros de la rejilla.

### A6. Tags y layers (hazlo ya, así no se te olvida)
1. Selecciona cualquier objeto → en el Inspector, **Tag → Add Tag…**
2. Bajo **Tags**, pulsa **+** → escribe el nombre → **Save**. Crea los que vayas a usar: `Suelo`, `Moneda`, `Enemigo`, `Tope`… `Player` ya existe.
3. En el mismo panel, despliega **Layers** y en **User Layer 6** escribe `Suelo`.
   - El **tag** sirve para `CompareTag` en colisiones y triggers.
   - La **layer** sirve para el `LayerMask` del salto (Raycast).

---

## 3. Fase B: montar la escena (0:20-0:50)

### B1. Formas simples si no hay imagen para algo
Project → `Images` → clic derecho → **Create → 2D → Sprites → Square** (o **Circle**, **Triangle**…). Se colorea con **Sprite Renderer → Color**.

### B2. Suelo o plataforma
1. Arrastra la imagen o el `Square` a la Scene → **F2** → `Plataforma`.
2. **Transform → Scale:** por ejemplo, X = 6 y Y = 0.5.
3. **Add Component → Box Collider 2D**. Deja *Is Trigger* sin marcar.
4. **Tag: Suelo** y **Layer: Suelo**. Si pregunta por los hijos: *This object only*.
5. Conviértelo en **prefab**: arrástralo de la Hierarchy a `Prefabs`.
6. Más plataformas: selecciónala, **Ctrl+D** y muévela con **W** o escribiendo **Position** en el Inspector.
7. Para cambiar **todas** a la vez: selecciona el **prefab en Project** y cambia ahí lo que quieras.

### B3. Jugador
1. Arrastra el sprite → **F2** → `Jugador`.
2. Transform → **⋮ → Reset** (lo deja en 0, 0, 0).
3. **Tag: Player.**
4. **Add Component → Rigidbody 2D:** Body Type **Dynamic** → despliega **Constraints** → **Freeze Rotation Z ✔**.
5. **Add Component → Capsule Collider 2D:** ajusta **Size**, más estrecho que alto (por ejemplo, 0.8 × 1.2).
6. **Punto de disparo** (si hay que disparar):
   1. **Para el Play** si está activo.
   2. En la **Hierarchy**, **clic derecho encima de `Jugador`** (no en un hueco vacío) → **Create Empty**.
   3. Aparece `GameObject` **dentro** de Jugador, un poco a la derecha y debajo de él, con una flechita ▶ en Jugador. Si sale suelto, al mismo nivel que Jugador, **arrástralo encima de `Jugador`** para meterlo dentro.
   4. **F2** → `PuntoDisparo` → Enter.
   5. En su Inspector, **Transform → Position**. Al ser hijo, es la posición **relativa al jugador**: pon **X = 0.5, Y = 0.1**, a la derecha y a la altura del arma. Ajusta a ojo con **W** en la Scene hasta que el punto quede en la boca del arma.
   6. **Rotation Z = -90**. Así su eje Y verde apunta hacia delante, porque la bala usará `transform.up`. Con la herramienta **W** y el modo **Local** de la barra de la Scene, la flecha verde debe apuntar a la derecha.
   7. Selecciona **Jugador** → **Jugador (Script)** → **arrastra `PuntoDisparo` desde la Hierarchy** al hueco **Punto Disparo**.
   - **Por qué hijo:** al girar el jugador (`eulerAngles` Y = 180), los hijos giran con él, y el punto de disparo pasa al otro lado y apunta hacia el otro lado.
7. **Prueba:** ▶ Play. Tiene que caer y quedarse de pie sobre la plataforma sin volcar.

### B4. Objetos que se recogen (moneda)
1. Sprite → `Moneda` → **Circle Collider 2D** con **Is Trigger ✔** → **Tag: Moneda**.
2. No necesita Rigidbody: el jugador ya lo tiene.
3. Arrástrala a `Prefabs`. Si se crea por código (spawner), **bórrala de la escena** después.

### B5. Bala
1. Sprite pequeño → `Bala`.
2. **Rigidbody 2D:** Body Type **Kinematic**. **Collision Detection: Continuous**.
3. **Circle o Capsule Collider 2D** con **Is Trigger ✔**.
4. Arrástrala a `Prefabs` y **bórrala de la escena**.

### B6. Enemigo
1. Sprite → `Enemigo` (o `Tortuga`) → **Tag: Enemigo**.
2. **Rigidbody 2D Dynamic** con **Freeze Rotation Z ✔**.
3. **Box o Capsule Collider 2D**, sin trigger.
4. Si patrulla: crea **Topes**. Ve a **Create Empty** → `Tope` → **Box Collider 2D** con **Is Trigger ✔** → **Tag: Tope**. Pon uno a cada lado del recorrido. Sin sprite, así es invisible.

### B7. Spawner (generador)
**Create Empty** → `Spawner`. Ahí irá el script que crea objetos (receta 5.5).

### B8. Cámara
Selecciona **Main Camera** → **Size** (Projection Orthographic): más grande = ves más mapa. Colócala para que se vea todo el nivel en la ventana **Game**.

**Ctrl+S.**

---

## 4. Fase C: controles con Input Actions (0:50-1:00)

1. Project → `Assets` → clic derecho → **Create → Input Actions** → nombre `Controles` → doble clic para abrirlo.
2. **Action Maps → +** → `Teclado`.
3. **Actions → +** → `Mover`:
   - Panel derecho: **Action Type = Value**, **Control Type = Axis**.
   - El **+** junto a `Mover` → **Add Positive/Negative Binding**.
   - Clic en **Negative** → **Path** → **Listen** → pulsa **A** (o la flecha izquierda).
   - Clic en **Positive** → **Path** → **Listen** → pulsa **D**.
4. **Actions → +** → `Saltar`: **Action Type = Button** → clic en `<No Binding>` → **Path** → **Listen** → **Espacio**.
5. **Actions → +** → `Disparar`: **Button** → la tecla del enunciado.
6. Si el enunciado pide moverse en **4 direcciones** (nave): `Mover` con **Control Type = Vector 2** → **+ → Add Up/Down/Left/Right Composite** → W, S, A, D. En el script se lee con `ReadValue<Vector2>()`.
7. **Save Asset**, arriba. O marca **Auto-Save**.

⚠️ **Si Listen graba una tecla rara** (a menudo «-» en teclado español): selecciona el binding → en **Path**, pulsa el botón **T** (modo texto) → escribe el path y pulsa Enter.

| Tecla | Path |
|---|---|
| Espacio | `<Keyboard>/space` |
| Letras | `<Keyboard>/a`, `<Keyboard>/w`, `<Keyboard>/j`… |
| Flechas | `<Keyboard>/leftArrow`, `<Keyboard>/rightArrow`, `<Keyboard>/upArrow`, `<Keyboard>/downArrow` |
| Enter | `<Keyboard>/enter` |
| Ctrl / Shift izquierdo | `<Keyboard>/leftCtrl`, `<Keyboard>/leftShift` |
| Clic izquierdo | `<Mouse>/leftButton` |

⚠️ **Saltar y Disparar tienen que ser Action Type = Button.** Solo `Mover` es Value (Axis o Vector 2).

**Cómo se ve en Project:** pulsa la flechita ▶ del archivo `Controles` y salen `Teclado/Mover`, `Teclado/Saltar`… Esas son las que se arrastran a los huecos del script.

---

## 5. Fase D: scripts y conectarlos (1:00-2:30)

### 5.1 Crear un script y abrirlo
1. Project → `Scripts` → clic derecho → **Create → Scripting → MonoBehaviour Script**. En otras versiones: **Create → C# Script**.
2. **Pon el nombre en el momento**, por ejemplo `Jugador`, y pulsa Enter. ⚠️ El nombre del archivo y el de la `class` tienen que ser **idénticos**. Si lo renombras después, cambia también la `class`.
3. Doble clic → se abre el editor de código. Pega el código, guarda con **Ctrl+S** y **vuelve a Unity**, que compila solo (abajo a la derecha sale una ruedita).
4. Si hay errores, la **Console** los muestra en rojo. Doble clic en el error te lleva a la línea.
5. **Añadirlo a un objeto:** arrastra el script desde Project al objeto en la Hierarchy, o **Add Component** → escribe su nombre.
6. **Rellenar los huecos** que aparecen en el Inspector, arrastrando desde Project o desde la Hierarchy.

### 5.2 Receta JUGADOR
**Necesita:** Rigidbody2D Dynamic + Freeze Rotation Z, Capsule Collider 2D, tag Player, hijo `PuntoDisparo`. **Teclas:** Input Actions de la fase C.

```csharp
using UnityEngine;
using UnityEngine.InputSystem;   // InputActionReference, InputAction
using System.Collections;        // IEnumerator (corrutinas)

public class Jugador : MonoBehaviour
{
    [SerializeField] int vida = 3, puntos = 0;
    [SerializeField] float velocidad = 5, fuerzaSalto = 8;
    [SerializeField] bool enSuelo = false;          // visible para ver en Play si detecta el suelo
    [SerializeField] LayerMask mascaraSuelo;        // en el Inspector: elegir "Suelo"
    [SerializeField] InputActionReference mover, saltar, atacar;   // arrastrar desde Controles
    [SerializeField] GameObject prefabBala;         // arrastrar el PREFAB desde Project
    [SerializeField] Transform puntoDisparo;        // arrastrar el hijo PuntoDisparo desde la Hierarchy
    Rigidbody2D rb;
    SpriteRenderer sr;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();           // componentes de ESTE objeto
        sr = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        mover.action.Enable();                      // activa las acciones (si no, las teclas no responden)
        saltar.action.Enable();
        atacar.action.Enable();
        saltar.action.started += Saltar;            // al pulsar Saltar → llama a Saltar()
        atacar.action.started += Atacar;
    }

    void OnDisable()
    {
        saltar.action.started -= Saltar;            // quitar la suscripción al desactivarse
        atacar.action.started -= Atacar;
    }

    void Update()
    {
        float direccion = mover.action.ReadValue<float>();   // -1 izquierda, 0 nada, 1 derecha
        rb.linearVelocityX = direccion * velocidad;           // mueve en X; la Y la lleva la gravedad

        if (direccion > 0)                                    // girar según hacia dónde va
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (direccion < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);   // da la vuelta: también gira PuntoDisparo

        DetectarSuelo();
    }

    void DetectarSuelo()
    {
        Vector3 origen = transform.position + new Vector3(0, -0.7f, 0);  // en los PIES: ajusta el -0.7
        enSuelo = Physics2D.Raycast(origen, Vector2.down, 0.1f, mascaraSuelo);
        Debug.DrawRay(origen, Vector2.down * 0.1f, Color.red);           // línea roja en la Scene (con Play)
    }

    void Saltar(InputAction.CallbackContext ctx)
    {
        if (enSuelo)
            rb.linearVelocityY = fuerzaSalto;      // impulso vertical
    }

    void Atacar(InputAction.CallbackContext ctx)
    {
        Instantiate(prefabBala, puntoDisparo.position, puntoDisparo.rotation);  // la bala sale girada como el arma
    }

    void OnTriggerEnter2D(Collider2D collision)    // al tocar un trigger (moneda, zona…)
    {
        if (collision.CompareTag("Moneda"))
        {
            puntos++;
            Debug.Log("Puntos: " + puntos);
            Destroy(collision.gameObject);         // destruye la moneda, no a mí
        }
    }

    void OnCollisionEnter2D(Collision2D collision) // al chocar físicamente
    {
        if (collision.gameObject.CompareTag("Enemigo"))
        {
            vida--;
            StartCoroutine(GolpeRojo());
            if (vida <= 0)
                Destroy(gameObject);               // muere el jugador
        }
    }

    IEnumerator GolpeRojo()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.2f);     // espera sin congelar el juego
        sr.color = Color.white;
    }
}
```

**Conectar (Inspector del Jugador):**
1. **Mover / Saltar / Atacar:** en Project, despliega `Controles` (flechita ▶) y arrastra `Teclado/Mover` al hueco *Mover*, y lo mismo con las otras dos.
2. **Mascara Suelo:** desplegable → marca **Suelo**.
3. **Prefab Bala:** arrastra el **prefab** `Bala` desde `Prefabs`.
4. **Punto Disparo:** arrastra el hijo `PuntoDisparo` desde la **Hierarchy**.

**Probar:** ▶ → A/D mueve y gira; Espacio salta (mira el **✔ En Suelo** del Inspector en Play y la línea roja en la Scene); la tecla de disparar crea balas.
**Si no salta:** el rayo no llega al suelo. Cambia `-0.7f` hasta que la línea roja asome por debajo de los pies. Comprueba también la layer `Suelo` y la máscara.
**Si no hay que disparar:** borra `atacar`, `prefabBala`, `puntoDisparo` y el método `Atacar`, o simplemente no lo uses. ⚠️ Si dejas los huecos vacíos y pulsas la tecla, sale un error `NullReference`.

**Variante nave (4 direcciones, sin gravedad):** Rigidbody2D con **Gravity Scale = 0** y en `Update`:
```csharp
Vector2 dir = mover.action.ReadValue<Vector2>();   // Mover con Control Type = Vector 2
rb.linearVelocity = dir * velocidad;               // se mueve en X e Y
```

### 5.3 Receta BALA
**Necesita:** Rigidbody2D **Kinematic** + Continuous, collider **Is Trigger**, prefab.

```csharp
using UnityEngine;

public class Bala : MonoBehaviour
{
    [SerializeField] float velocidad = 10;
    [SerializeField] float vidaMaxima = 3;          // segundos antes de autodestruirse

    void Start()
    {
        GetComponent<Rigidbody2D>().linearVelocity = transform.up * velocidad;  // hacia su "arriba" (el del PuntoDisparo)
        Destroy(gameObject, vidaMaxima);            // se borra sola: no se acumulan
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemigo"))
        {
            Destroy(collision.gameObject);          // destruye al enemigo
            Destroy(gameObject);                    // y a sí misma
        }
    }
}
```
**Si sale hacia arriba:** el `PuntoDisparo` no tiene **Rotation Z = -90**. O cambia `transform.up` por `transform.right`.
**Si sale hacia atrás al girar:** el jugador se gira con `flipX` en lugar de `eulerAngles`. Usa `eulerAngles`.

### 5.4 Receta MONEDA
**Necesita:** collider **Is Trigger**, tag `Moneda`. La recogida la hace el Jugador (5.2). Si además tiene que **caducar**, ponle este script:

```csharp
using UnityEngine;
using System.Collections;

public class Moneda : MonoBehaviour
{
    [SerializeField] float tiempoVida = 5;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(Caducar());                 // ⚠️ siempre con StartCoroutine
    }

    IEnumerator Caducar()
    {
        yield return new WaitForSeconds(tiempoVida);   // espera
        for (int i = 0; i < 3; i++)                    // parpadea 3 veces
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.2f);
            sr.enabled = true;
            yield return new WaitForSeconds(0.2f);
        }
        Destroy(gameObject);                           // desaparece
    }
}
```
**Variantes rápidas:**
- Que **gire**: en `Update`, `transform.Rotate(0, 0, 90 * Time.deltaTime);`.
- Que se **desvanezca**: baja `sr.color.a` en un bucle con `yield return null` (bloque 10 de la chuleta).

### 5.5 Receta SPAWNER
**Necesita:** objeto vacío `Spawner` + este script + arrastrar el **prefab** al hueco.

```csharp
using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    [SerializeField] GameObject prefab;                   // qué crear (moneda, enemigo…)
    [SerializeField] float esperaMin = 1, esperaMax = 3;  // cada cuánto
    [SerializeField] float xMin = -8, xMax = 8;           // dónde (rango horizontal)
    [SerializeField] float y = 4;                         // altura

    void Start()
    {
        StartCoroutine(Generar());
    }

    IEnumerator Generar()
    {
        while (true)                                      // para siempre (seguro porque hay yield)
        {
            yield return new WaitForSeconds(Random.Range(esperaMin, esperaMax));
            Vector3 pos = new Vector3(Random.Range(xMin, xMax), y, 0);
            Instantiate(prefab, pos, Quaternion.identity);   // sin rotación
        }
    }
}
```
**Alternativa sin corrutina:** `void Start() { InvokeRepeating("Crear", 1f, 2f); }` y un método `void Crear() { Instantiate(...); }`. Crea uno cada 2 segundos.
**Que salga en la posición del Spawner:** usa `transform.position` en lugar de `pos`.

### 5.6 Receta ENEMIGO QUE PATRULLA
**Necesita:** Rigidbody2D Dynamic + Freeze Rotation Z, collider sin trigger, tag `Enemigo`, y **Topes** (trigger, tag `Tope`) a los lados.

```csharp
using UnityEngine;

public class Enemigo : MonoBehaviour
{
    [SerializeField] float velocidad = 2;
    [SerializeField] bool derecha = true;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Girar();
    }

    void Update()
    {
        rb.linearVelocityX = derecha ? velocidad : -velocidad;   // ternario: si derecha, + ; si no, -
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Tope"))
        {
            derecha = !derecha;      // cambia de sentido
            Girar();                 // y de dibujo
        }
    }

    void Girar()
    {
        transform.eulerAngles = new Vector3(0, derecha ? 0 : 180, 0);
    }
}
```
**Sin Topes (por tiempo):** en `Update`, con `contador += Time.deltaTime; if (contador >= 2) { derecha = !derecha; Girar(); contador = 0; }`.
**Que muera al pisarlo:** solución 4 de la chuleta (`collision.GetContact(0).normal.y > 0.5f`).

### 5.7 Receta OBJETO QUE GIRA o SE MUEVE SOLO
Sin Rigidbody, si **no** tiene que chocar con nada. Con Rigidbody **Kinematic**, si tiene que detectar choques.

```csharp
using UnityEngine;

public class Movimiento : MonoBehaviour
{
    [SerializeField] float velocidadGiro = 90;     // grados por segundo
    [SerializeField] Vector3 velocidad = new Vector3(2, 0, 0);

    void Update()
    {
        transform.Rotate(0, 0, velocidadGiro * Time.deltaTime);          // gira en Z (plano 2D)
        transform.position += velocidad * Time.deltaTime;                // avanza en coordenadas del mundo
        // transform.Translate(Vector3.right * 2 * Time.deltaTime);      // alternativa: avanza hacia donde MIRA
    }
}
```
**Ángulos de Euler, de un vistazo:**
- `transform.eulerAngles = new Vector3(0, 0, 45);` fija 45°.
- `transform.rotation = Quaternion.Euler(0, 0, 45);` hace lo mismo.
- `transform.Rotate(0, 0, 45);` **suma** 45° a lo que ya tenga.
- ⚠️ Al leer `eulerAngles`, -90 aparece como 270.

### 5.8 Receta TIEMPO: contador y cuenta atrás

```csharp
using UnityEngine;

public class Cronometro : MonoBehaviour
{
    [SerializeField] float tiempoRestante = 60;   // cuenta atrás en segundos
    float tiempoJugado = 0;                       // cuenta hacia arriba

    void Update()
    {
        tiempoJugado += Time.deltaTime;           // suma segundos reales
        tiempoRestante -= Time.deltaTime;         // resta segundos reales

        if (tiempoRestante <= 0)
        {
            tiempoRestante = 0;
            Debug.Log("¡Se acabó el tiempo!");
            Time.timeScale = 0;                   // congela el juego (0 = pausa, 1 = normal)
            enabled = false;                      // desactiva este script para no repetir el mensaje
        }
    }
}
```
- Para **mostrar el tiempo** sin UI (la UI no entra): `Debug.Log` en la Console o una variable `[SerializeField]` para verla en el Inspector durante el Play.
- **Corrutina o contador:** el profe acepta las dos. La corrutina es para secuencias («espera, haz, espera»); el contador, para cosas que se comprueban en cada frame.

---

## 6. Fase E: probar y entregar (2:30-3:00)

1. **Console limpia:** pestaña **Console** → **Clear** → ▶ Play. No puede haber errores rojos. Los avisos amarillos no importan.
2. **Prueba cada punto del enunciado** con Play, en orden, y márcalo en el papel.
3. **Huecos del Inspector:** selecciona cada objeto con script y comprueba que no queda ningún *None* en los huecos.
4. **Ctrl+S.** El nombre de la escena no tiene que tener `*`.
5. **Ordenar:** las imágenes en `Images`, los prefabs en `Prefabs`, los scripts en `Scripts` y la escena en `Scenes`. Mueve los archivos **dentro de Unity** (arrastrando en Project), nunca desde el explorador, para que cada archivo viaje con su `.meta`.
6. **Sacar la carpeta:** clic derecho en `Assets` → **Show in Explorer** / **Open Containing Folder**.
7. Copia **la carpeta `Assets` entera, con los `.meta`**. Los Settings no son obligatorios. No entregues `Library`, `Temp` ni `Logs`.
8. Si se entrega comprimida: clic derecho → comprimir. Comprueba que el zip abre y contiene las 4 carpetas.

---

## 7. Si algo falla

| Síntoma | Causa | Arreglo |
|---|---|---|
| Hub: «Localizar o instalar Editor» | Editor no registrado | Localizar → `/opt/Unity-6000.0.83f1/Editor/Unity` |
| Hub: «No hay licencias activas» | Falta la licencia | Gestionar licencias → licencia personal gratuita |
| Al arrastrar la imagen sale una fila de dibujos | Hoja sin cortar o recorte que une varios | Sprite Editor → Slice, o elige otro recorte |
| El sprite es enorme o diminuto | Pixels Per Unit | Más PPU = más pequeño → Apply |
| Aviso amarillo de `.meta` huérfano | Se borró o movió algo fuera de Unity | Console → Clear. Mueve siempre desde Unity |
| `CS0101 already contains a definition for 'X'` / `CS0111 already defines a member called 'Start'` | Hay dos `class X`: o se pegó el código **debajo** de la plantilla, o hay **dos archivos** `X.cs` (uno en `Assets` y otro en `Scripts`) | Busca en Project el nombre del script: si salen dos, borra el que no use el objeto. Si es uno: **Ctrl+A** → pega solo la receta → Ctrl+S |
| El script no se puede añadir | La `class` no se llama igual que el archivo, o hay errores rojos | Igualar los nombres; arreglar la Console |
| Las teclas no hacen nada | Acciones sin activar, huecos vacíos o asset sin guardar | `Enable()` en `OnEnable`; arrastrar las acciones; **Save Asset** |
| La tecla graba «-» u otra rara | Listen con teclado español | Path → botón **T** → `<Keyboard>/space` |
| Salta solo con una tecla rara | Binding mal grabado | Revisa el binding en el `.inputactions` |
| Error rojo `Tag: X is not defined` | El script usa `CompareTag("X")` y ese tag no existe | Tags & Layers → **+** → crea el tag `X` |
| El «prefab» sale con un icono de imagen, no con el cubo azul | Se creó o renombró una **imagen** dentro de `Prefabs` | La imagen va a `Images`; el prefab se hace arrastrando el **objeto de la Hierarchy** a `Prefabs` |
| Las teclas no responden aunque todo esté bien | El foco está en otra ventana (Hierarchy, Inspector) | Haz **clic dentro de la ventana Game** antes de pulsar |
| No se ve la línea roja del Raycast | `Debug.DrawRay` solo se dibuja en la **Scene** | Pestaña Scene con Play activo, o botón **Gizmos** en Game |
| En la Hierarchy los objetos salen en blanco y no en azul | Ya no están conectados al prefab | Bórralos y vuelve a arrastrar el prefab desde `Prefabs` |
| `UnassignedReferenceException … callbacks of 'Teclado/Disparar'` | Al pulsar la tecla, el método usa una variable `[SerializeField]` que está en *None* (Prefab Bala o Punto Disparo) | Jugador → Jugador (Script) → rellena los dos huecos |
| `NullReferenceException` | Hueco del Inspector vacío | Doble clic en el error → mira qué variable es → rellénala |
| El jugador cae al vacío | Al suelo le falta el collider | Box Collider 2D en las plataformas |
| El jugador vuelca | Falta Freeze Rotation Z | Rigidbody2D → Constraints |
| No salta | Rayo corto o mal colocado, layer o máscara | Línea roja visible; layer `Suelo`; máscara `Suelo` |
| Salta infinitas veces | El jugador también está en la layer `Suelo` | Jugador en la layer Default |
| El trigger no hace nada | Falta Rigidbody2D, falta Is Trigger, tag o `2D` en el método | Repasa la receta |
| La bala sale hacia arriba | `PuntoDisparo` sin Rotation Z = -90 | Rotarlo, o usar `transform.right` |
| La bala atraviesa | Demasiado rápida | Collision Detection = Continuous |
| Las balas se acumulan | Sin Destroy | `Destroy(gameObject, 3f)` en el Start de la bala |
| `velocity is obsolete` | Código antiguo | `linearVelocity` |
| La corrutina no funciona | Llamada sin `StartCoroutine` | `StartCoroutine(Metodo());` |
| Unity se cuelga | `while(true)` sin `yield` | Pon un `yield return` dentro |
| Cambios perdidos | Se hicieron con Play activo | Para y repite el cambio |
| `MissingReferenceException` | Se arrastró un objeto de la escena en vez del prefab | Arrastra el prefab desde Project |

---

## 8. Plantilla vacía de script

```csharp
using UnityEngine;
using UnityEngine.InputSystem;   // si usa teclado
using System.Collections;        // si usa corrutinas

public class NombreIgualAlArchivo : MonoBehaviour
{
    [SerializeField] float velocidad = 5;   // variables que se ven en el Inspector
    Rigidbody2D rb;

    void Start()                            // una vez, al empezar
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()                           // cada frame
    {
    }

    void OnTriggerEnter2D(Collider2D collision)      // tocar un trigger
    {
        if (collision.CompareTag("Tag")) { }
    }

    void OnCollisionEnter2D(Collision2D collision)   // choque físico
    {
        if (collision.gameObject.CompareTag("Tag")) { }
    }

    IEnumerator MiCorrutina()                         // arrancar con StartCoroutine(MiCorrutina());
    {
        yield return new WaitForSeconds(1f);
    }
}
```
