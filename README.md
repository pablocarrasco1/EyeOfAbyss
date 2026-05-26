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

```
Assets/
├── Scenes/          # Escenas del juego (MenuInicio, SampleScene)
├── Scripts/         # Todos los scripts C# del proyecto
├── Prefabs/         # Prefabs de objetos, trampas y UI
├── Sprites/         # Assets gráficos Pixel Art
└── Audio/           # Efectos de sonido y música
```

---

## 🎯 Scripts principales

| Script | Responsabilidad |
|---|---|
| `Controlador.cs` | Estado global del juego y evento `OnReset` |
| `ControladorMovimiento.cs` | Movimiento, salto, wall jump y dash del jugador |
| `VidaJugador.cs` | Sistema de vida e invencibilidad |
| `VidaUI.cs` | Representación visual de los corazones en el HUD |
| `CronometroNivel.cs` | Medición y formato del tiempo de partida |
| `SistemaPuntuaciones.cs` | Gestión y persistencia de la tabla de clasificación |
| `PantallaFinalControlador.cs` | Pantalla de resultados y selector de nombre arcade |
| `ObjetoLlave.cs` | Llave de nivel con lógica de reset |
| `ObjetoVida.cs` | Objeto de curación con lógica de reset |
| `Recoleccion.cs` | Detección genérica de objetos `Item` |
| `PlataformaMovil.cs` | Plataforma con movimiento entre dos puntos |
| `PlataformaFragil.cs` | Plataforma con temporizador de caída y reset |
| `Trampa.cs` | Daño y rebote físico al jugador |
| `Item.cs` | Interfaz común para objetos recolectables |

---

## 🕹️ Controles

| Acción | Tecla |
|---|---|
| Moverse | `A` / `D`  o  `←` / `→` |
| Saltar / Doble salto | `Espacio` |
| Dash | `Shift izquierdo` / `E` |
| Salto en pared | `Espacio` (junto a una pared) |

---

## 📊 Sistema de puntuaciones

Las partidas completadas se registran en un archivo `scores.json`.

---

## 👤 Autor

**Pablo Carrasco Paredero**  
DAM2B - IES Venancio Blanco  
Proyecto Fin de Ciclo 2026  
Tutor: Alejandro Pérez-Moneo Nieto

