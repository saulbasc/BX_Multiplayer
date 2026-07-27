
# Juego multijugador en Unity 6

Proyecto desarrollado como Trabajo Final del ciclo de **Desarrollo de Aplicaciones Multiplataforma (DAM)** utilizando **Unity 6** para dispositivos Android.

El juego implementa un sistema multijugador **peer-to-peer (host-client)** mediante **Netcode for GameObjects**, permitiendo crear salas privadas, disputar partidas online y registrar estadísticas persistentes mediante Firebase.

<p align="center">
  <img src="https://github.com/user-attachments/assets/321a023e-7721-4204-8af5-0804560dfecc" alt="Gameplay" width="850">
</p>

---

## Características

- Multijugador online en tiempo real.
- Arquitectura peer-to-peer (host-client).
- Registro e inicio de sesión.
- Estadísticas personales persistentes.
- Ranking global de jugadores.
- Salas privadas mediante código.
- Lobby previo a la partida.
- Partidas configurables por equipos.
- Desarrollado para Android.

---

## Tecnologías

| Tecnología | Uso |
|------------|-----|
| **Unity 6** | Motor del juego |
| **C#** | Lógica de la aplicación |
| **Netcode for GameObjects** | Sincronización multijugador mediante arquitectura host-client |
| **Unity Relay** | Conexión entre jugadores |
| **Unity Lobby** | Gestión de salas privadas |
| **Firebase Authentication** | Registro e inicio de sesión |
| **Firebase** | Almacenamiento de estadísticas |

---

## Configuración local

Este repositorio no incluye configuración operativa de Firebase.

Antes de abrir o compilar el proyecto con todas las funcionalidades online, cada entorno debe añadir su propia configuración local de Firebase en rutas no versionadas:

- `Assets/StreamingAssets/google-services-desktop.json`
- `Assets/Plugins/Android/FirebaseApp.androidlib/res/values/google-services.xml`

Como referencia, el repositorio incluye una plantilla sanitizada en `Assets/StreamingAssets/google-services-desktop.example.json`.

Pasos recomendados:

1. Crear o seleccionar un proyecto propio en Firebase.
2. Registrar la aplicación Android con el identificador correspondiente.
3. Descargar o generar la configuración necesaria para Unity/Android.
4. Copiar los archivos de configuración a las rutas anteriores.
5. Verificar que Authentication y Firestore estén habilitados en el proyecto de Firebase.

Estos archivos están excluidos de git para evitar subir identificadores o claves de cliente reales.

---

## Arquitectura

```text
                   Host (Jugador)
                         │
              Netcode for GameObjects
                         │
        ┌────────────────┴────────────────┐
        │                                 │
   Cliente 1                        Cliente 2
        │                                 │
        └──────────── Unity Relay ────────┘

             Unity Lobby → Gestión de salas
             Firebase → Autenticación y estadísticas
```

El juego utiliza una arquitectura **peer-to-peer (host-client)**.

Uno de los jugadores actúa como **Host**, ejecutando la lógica de la partida mientras participa como jugador. El resto de clientes se conectan al Host utilizando **Unity Relay**, mientras que **Unity Lobby** gestiona la creación de salas y **Firebase** almacena la autenticación y las estadísticas.

---

## Funcionalidades

### Registro e inicio de sesión

Permite crear una cuenta o iniciar sesión para asociar las estadísticas al usuario.

---

### Menú principal

Desde el menú principal es posible acceder al resto de funcionalidades del juego.

<p align="center">
  <img src="https://github.com/user-attachments/assets/91b1e7e2-97f4-4ab6-a934-8832e94598e8" alt="Menú principal" width="800">
</p>

---

### Estadísticas personales

Consulta de las estadísticas acumuladas por cada jugador.

<p align="center">
  <img src="https://github.com/user-attachments/assets/9ad84a63-44c4-45a8-a9de-c2845430c743" alt="Estadísticas personales" width="800">
</p>

---

### Ranking global

Clasificación con las estadísticas de todos los jugadores registrados, actuando como Ranking.

<p align="center">
  <img src="https://github.com/user-attachments/assets/83318b1c-8cf4-4172-8848-1d183b52a9ca" alt="Ranking global" width="850">
</p>

---

### Salas privadas

Los jugadores pueden crear una sala privada o unirse mediante un código compartido.

<p align="center">
  <img src="https://github.com/user-attachments/assets/2fdfcca4-e1aa-4193-bb4a-f0cedb47807f" alt="Salas privadas" width="850">
</p>

---

### Lobby

Antes de comenzar la partida, los jugadores pueden seleccionar equipo y el host configurar la duración del encuentro.

<p align="center">
  <img src="https://github.com/user-attachments/assets/3e2e38a3-6a93-4e82-b437-c57c22877e17" alt="Lobby" width="850">
</p>

---

### Partida

Los encuentros enfrentan a dos equipos en tiempo real. El objetivo es marcar más goles que el rival antes de que finalice el tiempo establecido.

<p align="center">
  <img src="https://github.com/user-attachments/assets/321a023e-7721-4204-8af5-0804560dfecc" alt="Partida" width="900">
</p>

---
