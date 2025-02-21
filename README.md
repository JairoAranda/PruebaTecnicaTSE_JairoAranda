# Prueba Técnica - Desarrollo XR con Unity

## Descripción
Este proyecto es una prueba técnica para evaluar habilidades en desarrollo XR con Unity. Incluye funcionalidades como gestión de permisos, personalización del editor, generación de objetos y optimización del rendimiento.

## Requisitos del Entorno
- **Motor de Juego**: [Unity 2019.4.5f1](https://unity.com/es/releases/editor/whats-new/2019.4.5#installs)
- **Plataforma de Construcción**: Android
- **Lenguaje de Programación**: C#
- **Modo de Compilación**: IL2CPP

## Instalación
1. Clonar el proyecto usando el siguiente comando:
   ```sh
   git clone https://github.com/JairoAranda/PruebaTecnicaTSE_JairoAranda.git
   ```
2. Abrir el proyecto en Unity 2019.4.5f1.
3. Configurar la plataforma de construcción en **Android**.
4. Instalar el SDK de Android, NDK y JDK desde **Preferences > External Tools** si no están configurados.
5. Asegurar que las dependencias necesarias están instaladas mediante el **Package Manager**.

## Uso
1. Ejecutar el juego y verificar la solicitud de permisos de ubicación.
2. Usar la ventana personalizada en el Editor para gestionar las escenas.
3. Utilizar el **Inspector** para generar cubos mediante el botón correspondiente.
4. Revisar los logs en la consola para depuración y validaciones.

## Explicación del Código

### CubeSpawner
El `CubeSpawner` es un script que gestiona la generación y eliminación de cubos en la escena. Sus características principales incluyen:
- Instanciación de cubos en posiciones aleatorias dentro de un área definida.
- Uso de un sistema de pooling para optimizar el rendimiento.
- Asignación de materiales aleatorios a los cubos generados.
- Actualización en tiempo real de un contador de cubos en la UI.
- Métodos para generar y eliminar cubos de manera eficiente.

#### Propiedades principales:
- `cubePrefab`: Prefab del cubo que se generará.
- `spawnAreaSize`: Dimensiones del área donde se generarán los cubos.
- `spawnAmount`: Cantidad de cubos a generar por cada acción.
- `maxCubes`: Límite máximo de cubos en la escena.
- `cubeCounterText`: Texto de la UI que muestra el número de cubos activos.
- `randomMaterials`: Array de materiales que se asignarán aleatoriamente a los cubos.

#### Métodos clave:
- `SpawnCube()`: Genera cubos en posiciones aleatorias y gestiona el pool.
- `ClearCubes()`: Desactiva todos los cubos y los almacena en el pool para reutilización.
- `UpdateCubeCounter()`: Actualiza el contador de cubos en la UI.
- `IsPositionOccupied(Vector3 position)`: Verifica si una posición está ocupada para evitar colisiones.
- `CheckParentCube()`: Gestiona un objeto contenedor para organizar los cubos en la jerarquía.

### Editor Personalizado (CubeSpawnerEditor)
Este script extiende el **Inspector** de Unity para `CubeSpawner`, proporcionando una interfaz mejorada con controles personalizados.

#### Características:
- Visualización estructurada y estilizada de propiedades.
- Deslizadores para ajustar dinámicamente la cantidad y el límite de cubos.
- Listado interactivo para gestionar materiales aleatorios.
- Botones de acción para generar y eliminar cubos desde el Inspector.

#### Métodos clave:
- `OnInspectorGUI()`: Dibuja la interfaz personalizada en el Editor.
- `OnEnable()`: Inicializa las propiedades serializadas del script.

### SceneEditorWindow
El `SceneEditorWindow` es una ventana personalizada para el Editor de Unity que permite gestionar las escenas del proyecto desde una interfaz amigable.

#### Características:
- Listado de escenas incluidas en los **Build Settings**.
- Botón para cargar cualquier escena desde la lista.
- Opción para eliminar escenas de los **Build Settings**.
- Campo de arrastre para agregar nuevas escenas al **Build Settings**.
- Botón para recargar la escena actual sin necesidad de buscarla manualmente.

#### Métodos clave:
- `OpenScene(string scenePath)`: Abre una escena guardando los cambios pendientes.
- `ReloadCurrentScene()`: Recarga la escena activa.
- `AddSceneToBuildSettings(Object sceneAsset)`: Agrega una nueva escena al **Build Settings** si no está incluida.
- `RemoveSceneFromBuildSettings(string scenePath)`: Elimina una escena de los **Build Settings** con confirmación del usuario.

### LocationManager
El `LocationManager` gestiona los permisos de ubicación en dispositivos Android.

#### Características:
- Solicita permisos de ubicación al iniciar la aplicación.
- Muestra en la UI el estado del permiso (Otorgado/Rechazado).
- Reinicia la solicitud de permisos si el usuario cambia de aplicación y vuelve.
- Inicia los servicios de ubicación si el permiso es concedido.

#### Métodos clave:
- `CheckAndRequestLocationPermission()`: Verifica y solicita el permiso de ubicación.
- `OnApplicationFocus(bool focusStatus)`: Vuelve a verificar los permisos cuando la aplicación recupera el primer plano.
- `StartLocationServices()`: Inicia los servicios de ubicación si están habilitados por el usuario.
- `UpdatePermissionStatus(string status)`: Actualiza el texto en la UI con el estado del permiso.

## Video de Demostración
Puedes ver un video de demostración del proyecto en el siguiente enlace:
[Ver Video](https://www.youtube.com/watch?v=aXt-LiG-RVI)

## Descarga APK
Puedes descargar el apk para comprobar la gestion de los permisos: [Descarga](https://drive.google.com/file/d/1zMTYnP6X_EAktjvR9OWt-GUN6KZOCcgR/view?usp=sharing)

## Autor
Desarrollado por **Jairo Aranda**.
