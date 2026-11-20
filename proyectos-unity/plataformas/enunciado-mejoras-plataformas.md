# Mejoras Plataformas (enunciado del profe, foro de Moodle)

Paquetes en `paquetes/`: `plataformas-2026-11-18.unitypackage` (base) y `plataformas-2026-11-19.unitypackage` (incluye moneda, bala, tope y sprite del jugador).

## 18/11/2026

### SALTO
- Que funcione en los bordes de las plataformas. Y efecto Coyote.
- Doble salto.
- Si el jugador pisa la tortuga, recibe un salto inmediato involuntario del doble de fuerza.

### TORTUGA
- Patrulla por trigger. Cuando la tortuga toca un objeto Tope (invisible, con collider trigger), se da la vuelta (visualmente, camina en la otra dirección, etc.).
- Cuando el jugador la pisa, muere.

## 19/11/2026

### MONEDA
- Pasados 5 segundos, la moneda parpadea 3 veces y después desaparece.
