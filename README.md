# RawDeal Card Game

## Descripción
**RawDeal** es una implementación del clásico juego de cartas coleccionables *Raw Deal*, que simula combates épicos entre superestrellas del wrestling. El objetivo principal del proyecto fue revivir este icónico juego aplicando principios de diseño detallado de software, siguiendo buenas prácticas y principios de *Clean Code*.

El proyecto fue desarrollado en el contexto del curso **IIC2113 - Diseño Detallado de Software** en la **Pontificia Universidad Católica de Chile**.

---

## Características del Juego
- Simula peleas entre dos superestrellas del wrestling usando cartas coleccionables.
- Cada jugador tiene un mazo de 60 cartas más una carta de superestrella.
- Las cartas se clasifican en cuatro tipos principales:
  - **Maneuver** (amarillo): Causan daño al oponente.
  - **Action** (celeste): Generan efectos estratégicos sin causar daño.
  - **Reversal** (rojo): Permiten revertir acciones del oponente.
  - **Hybrid** (dos colores): Pueden actuar como *Maneuver* o *Action* según la elección del jugador.
- Implementación de habilidades especiales para cada superestrella.
- Sistema de validación de mazos para garantizar la correcta configuración de los mismos.
- Manejo de efectos y habilidades según las reglas del juego.
- Soporte para cartas híbridas y reversals con efectos específicos.
- Interfaz de usuario en consola para facilitar la interacción.

---

## Instalación y Ejecución
Para ejecutar el juego en tu máquina:

1. Clona el repositorio:
```
git clone https://github.com/tu_usuario/RawDeal.git
cd RawDeal
```

2. Asegúrate de tener instalado .NET SDK:

```
dotnet --version
```

3. Compila el proyecto:

```
dotnet build
```

4. Ejecuta el juego desde la carpeta RawDeal:

```
cd RawDeal
dotnet run
```


## Pruebas
El proyecto incluye un conjunto de pruebas automatizadas para verificar la validez de los mazos y el correcto funcionamiento de la lógica del juego, para ejecutarlo situarse en carpeta raiz y ejecutar:

```
dotnet test
```