# 👁️ Eye of Abyss

Videojuego de plataformas 2D con estética Pixel Art, desarrollado en Unity y C# como Proyecto Fin de Ciclo (DAM2B). Incluye doble salto, salto en pared, dash con invencibilidad, plataformas frágiles y móviles, sistema de reset por eventos sin recarga de escena y tabla de clasificación persistente en JSON con selector de nombre estilo arcade.

---

## 🎮 Sobre el juego

El protagonista se encuentra atrapado en una mazmorra y debe escapar superando una serie de niveles de dificultad progresiva. El objetivo en cada nivel es recoger la llave que desbloquea la salida sin agotar los puntos de vida disponibles.

El diseño de niveles sigue una filosofía de introducción progresiva de mecánicas: cada elemento se presenta en un entorno controlado antes de combinarse con otros, promoviendo una curva de aprendizaje clara y justa.

---

## ⚙️ Mecánicas implementadas

- **Movimiento horizontal** con volteo de sprite
- **Salto simple y doble salto** con contador de saltos y salto corto al soltar la tecla
- **Salto en pared y deslizamiento** con ventana de tiempo tras separarse de la pared
- **Dash horizontal** con anulación de gravedad, invencibilidad y cooldown
- **Sistema de vida** con dos mecanismos de invencibilidad independientes (daño e i-frames del dash)
- **Plataformas móviles** con parenting dinámico del jugador
- **Plataformas frágiles** con temporizador de caída y reset completo
- **Trampas** con rebote físico y periodo de gracia tras el impacto
- **Objetos recolectables** mediante interfaz `Item` (llave y corazones de curación)
- **Cronómetro de nivel** con formato MM:SS.ms y evento de nivel completado
- **Tabla de clasificación** persistente en JSON con top-10 y selector arcade de nombre

---

## 🏗️ Arquitectura destacada

El proyecto aplica un sistema de **reset basado en eventos estáticos de C#** (`Controlador.OnReset`) que permite restaurar todos los sistemas del juego (vida, ítems, plataformas, cronómetro) sin recargar la escena. Cada componente se suscribe al evento en su `Start()` y gestiona su propio estado inicial de forma independiente.

La recolección de objetos se gestiona mediante la **interfaz `Item`**, que define los métodos `Recoger()` y `ResetItem()` como contrato común. Esto permite que `Recoleccion.cs` detecte y procese cualquier objeto recolectable sin conocer su tipo concreto, siguiendo el principio abierto/cerrado.

---

## 🛠️ Tecnologías

| Herramienta | Uso |
|---|---|
| Unity (Personal) | Motor de desarrollo |
| C# | Lenguaje de programación |
| Visual Studio Code | IDE |
| Unity Input System | Gestión de controles |
| TextMeshPro | Interfaz de usuario |
| JsonUtility | Serialización de puntuaciones |
| Git / GitHub | Control de versiones |

---

## 📁 Estructura del proyecto
