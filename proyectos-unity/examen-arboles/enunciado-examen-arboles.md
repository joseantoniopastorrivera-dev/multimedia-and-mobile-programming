# PMDM — Examen 1ª evaluación: «Árboles» (Unity 2D)

Examen del 24/11/2026 (IES Isidra de Guzmán, 2º DAM). Resultados de aprendizaje: RA3, RA4, RA5. El PDF original está en esta misma carpeta (`enunciado-examen-arboles.pdf`). El paquete con los sprites (`examen-arboles.unitypackage`: `arbol`, `cesta`, `manzana`, `pajaro`) se guarda solo en local.

## Enunciado

Realizar un proyecto 2D en Unity que incluya los siguientes requisitos.

- Trabajar en la ventana **Game** con una resolución **Full-HD**.
- Importar el paquete Unity que se adjunta con el enunciado para obtener los assets iniciales necesarios.

| Puntos | Requisito |
|---|---|
| 0,5 | Con los sprites importados y sprites básicos creados con Unity, preparar una escena de juego organizada como la de la imagen. Usar la **z** de los objetos para posicionar los elementos en profundidad. |
| 2 | Crear un objeto **Jugador** con la imagen `cesta.png`. Mientras se mantengan pulsadas las **flechas izquierda o derecha**, la cesta se mueve marcando la velocidad al **Rigidbody2D** con **10 unidades por segundo** en horizontal. Si no se pulsa ninguna, la cesta se para. No debe poder girarse ni moverse en vertical. |
| 0,5 | La cesta no debe salir de la pantalla por los laterales: hay **2 objetos sólidos** que hacen de límite y no se ven. |
| 1 | Crear un objeto **Pajaro** con `pajaro.png`. Se mueve en horizontal **hacia su parte delantera** cambiando el **`transform.position` en función del tiempo**. Velocidad inicial: **5 unidades por segundo**. Crear un **Prefab** y situar **3 pájaros** iniciales como en la imagen. |
| 1,5 | Cuando el pájaro llega al **borde de la pantalla**, **se da la vuelta visualmente** y pasa a moverse en dirección contraria. |
| 1 | Cuando un pájaro toca una **manzana**, la manzana es destruida y el pájaro **aumenta su velocidad un 50 %**. |
| 1 | Llevar un **conteo de puntos** del jugador, inicialmente 0. Si la cesta toca una manzana, la manzana es destruida y el jugador gana una cantidad **aleatoria entre 1 y 3** (aleatoria en cada impacto). En ese momento se escriben en **consola** los puntos. |
| 1,5 | Crear un **objeto vacío Spawner**. **Cada 2 segundos** crea una manzana en un punto aleatorio de la parte superior: **x entre -10,5 y 10,5**, **y entre 2 y 5**. |

> Los puntos de la tabla suman 9. El punto que falta no aparece en el texto del PDF: probablemente corresponda a la importación y la resolución, o a la entrega. Compruébalo en el PDF original.

## Material a entregar

Entregar en el Aula Virtual un **archivo package de Unity** (`.unitypackage`) que contenga todos los assets del juego, **incluida la escena**. Recordatorio: **guardar la escena antes** de exportar el package.
