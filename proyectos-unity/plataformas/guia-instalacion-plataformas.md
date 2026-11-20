# Plataformas: guía de instalación (clase del 19/11/2026)

El profe subió un archivo base, `Plataformas 2026 11 18`, para trabajar en clase. Esta guía recoge cómo lo metimos en Unity en el aula (Linux) y cómo repetirlo en casa (Windows).

## Qué es el archivo

- Se llama `Plataformas 2026 11 18`, no tiene extensión y ocupa 4 MiB. Dolphin lo muestra como «archivador Tar».
- En realidad es un **`.unitypackage`**, que por dentro es un `.tar.gz`. Lo comprobamos con `tar -tf`: aparecen carpetas con nombre de GUID que contienen `asset`, `asset.meta` y `pathname`.
- Un `.unitypackage` **no se abre desde Unity Hub**: se **importa dentro de un proyecto** que ya existe. Un proyecto completo (con `Assets/`, `Packages/` y `ProjectSettings/`) sí se añade a Hub con *Add → Add project from disk*.

### Contenido del paquete

| Archivo | Qué es |
|---|---|
| `Plataformas.inputactions` | Controles del juego (nuevo Input System, ficha 06) |
| `Prefabs/Plataforma.prefab` | Plantilla de plataforma (ficha 09) |
| `Scripts/Jugador.cs` | Script del personaje |
| `Scripts/Tortuga.cs` | Script de la tortuga (seguramente un enemigo) |
| `Scenes/SampleScene.unity` | Escena del nivel. **Sustituye** la SampleScene vacía del proyecto nuevo |
| `UniversalRenderPipelineGlobalSettings.asset` | Ajustes de URP del profe |

## Pasos en el aula (Linux, usuario `alumno`)

1. **Comprobar qué es el archivo:**
   ```bash
   cd ~/Descargas/unity_19_11_26
   tar -tf "Plataformas 2026 11 18" | head -20
   ```
2. **Ponerle la extensión**, para que el selector de Unity lo muestre:
   ```bash
   mv "Plataformas 2026 11 18" "Plataformas-2026-11-18.unitypackage"
   ```
3. **Localizar el editor en Hub.** Hub no lo detectaba. Lo buscamos con:
   ```bash
   ls ~/Unity/Hub/Editor /opt/unity* /opt/Unity* 2>/dev/null
   ```
   Está en `/opt/Unity-6000.0.83f1/Editor/`. En Hub, ve a *Instalaciones → Localizar* y elige el archivo **`Unity`**: es el ejecutable de 96,7 MiB, el del icono de «exclamación». No elijas `Unity_s.debug` ni los `.so`.
4. **Licencia.** Ve a *Gestionar licencias → Añadir licencia → Obtener una licencia personal gratuita* y acepta como uso no profesional.
5. **Crear el proyecto.** Ve a *Nuevo proyecto* con estos ajustes:
   - Plantilla **Universal 2D**. Ojo: por defecto viene marcada Universal 3D.
   - Editor 6000.0.83f1 LTS.
   - Nombre `Plataformas` y ubicación `/home/alumno`.
   - AI Assistant y Unity CLI sin marcar, y sin proveedor de control de código fuente.
6. **Importar el paquete.** Ve a *Assets → Import Package → Custom Package…*, elige `Plataformas-2026-11-18.unitypackage`, deja todo marcado y pulsa **Import**. El ⚠ de `SampleScene.unity` solo avisa de que va a sustituir la escena vacía que está abierta. Si pregunta, pulsa **Reload**.
7. **Abrir la escena.** Abre `Assets/Scenes/SampleScene` y pulsa ▶ Play.

## Cómo hacerlo en casa (Windows)

**Antes de salir de clase, llévate el paquete.** El portátil de casa no tiene el archivo. Opciones:
- Súbelo al repo en `proyectos-unity/plataformas/Plataformas-2026-11-18.unitypackage`, como se hizo con el del caso práctico de las fichas. Pesa 4 MiB y no es un binario generado, así que se puede subir.
- Envíatelo por correo, o cópialo a Drive o a un USB.

En casa:
1. Abre Unity Hub. El editor 6000.0.83f1 ya está instalado y Hub lo detecta solo.
2. Comprueba que tienes la licencia Personal activa (*Ajustes → Licencias*). Si no, haz lo mismo que en el paso 4.
3. Crea el proyecto como en el paso 5: **Universal 2D**, 6000.0.83f1 y nombre `Plataformas`. Guárdalo **fuera del repo**, por ejemplo en `C:\Users\Usuario\UnityProjects\Plataformas`, o, si va dentro del repo, sin subir `Library/`, `Temp/`, `Logs/`, `obj/` ni `UserSettings/`. El `.gitignore` ya los excluye.
4. Importa el paquete como en el paso 6: *Assets → Import Package → Custom Package…*. En Windows también puedes arrastrar el `.unitypackage` al panel Project.
5. Abre `Assets/Scenes/SampleScene` y pulsa Play.

## Si algo falla

- **Hub dice «Localizar o instalar Unity Editor»:** el editor no está registrado. Localízalo antes de instalarlo, porque instalarlo son varios GB.
- **«No hay licencias activas»:** sin licencia no se pueden crear ni abrir proyectos. Activa la Personal gratuita.
- **Errores de `InputSystem`:** instala *Window → Package Manager → Unity Registry → Input System*. La plantilla Universal 2D ya lo trae.
- **Sprites rosas:** el render pipeline no coincide. Crea el proyecto con la plantilla correcta (Universal 2D, que es URP).
- **La escena está vacía:** estás viendo la SampleScene antigua. Ábrela otra vez desde `Assets/Scenes`.

## Lo que hizo el profe en clase (19/11/2026, contado de palabra)

1. **Sprite del personaje:** usó una imagen de un soldado estilo Metal Slug como sprite del jugador.
2. **Ejes y giro:** explicó los ejes (X horizontal, Y vertical; en 2D, Z solo indica profundidad) y cómo girar al personaje cuando cambia de dirección (fichas 04 y 05).
3. **Spawn de monedas:** crear monedas desde un prefab con `Instantiate`, probablemente en posiciones aleatorias o cada cierto tiempo (fichas 08, 09, 12, 13 y 14).
4. **Bala y función atacar/disparar:** crear la bala en la posición del arma, darle velocidad en la dirección a la que mira el jugador y destruirla al chocar o pasado un tiempo (fichas 06, 08, 10 y 11).

Cuando suba el código del profe, comparar con esto y preparar ejercicios para el examen.

## Pendiente

- [ ] Llevar el `.unitypackage` a casa (repo, correo o USB).
- [ ] Revisar los avisos de la cuenta de Unity: reconfigurar la app de autenticación (2FA), o podrían suspender la cuenta.
- [ ] Apuntar aquí lo que pida el profe hacer sobre esta base.
