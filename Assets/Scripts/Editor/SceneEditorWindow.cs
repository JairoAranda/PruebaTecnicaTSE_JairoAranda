using UnityEditor;
using UnityEngine;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Collections.Generic;

public class SceneEditorWindow : EditorWindow
{
    private Vector2 scrollPosition;
    private Object sceneToAdd;

    [MenuItem("Tools/Scene Manager Window")]
    public static void ShowWindow()
    {
        GetWindow<SceneEditorWindow>("Scene Manager");
    }

    private void OnGUI() 
    {
        GUILayout.Space(10);
        // Título
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("Gestor de Escenas ", titleStyle);

        GUILayout.Space(10);

        // Obtener todas las escenas en Build Settings
        EditorBuildSettingsScene[] buildScenes = EditorBuildSettings.scenes;
        List<string> scenePaths = buildScenes.Select(scene => scene.path).ToList();

        // Mostrar la lista de escenas
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));
        foreach (string scenePath in scenePaths)
        {
            if (!string.IsNullOrEmpty(scenePath))
            {
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();

                // Botón para cargar la escena
                if (GUILayout.Button(sceneName, GUILayout.Width(200), GUILayout.Height(30)))
                {
                    OpenScene(scenePath);
                }

                //Botón para eliminar la escena
                GUI.backgroundColor = Color.red;
                if (GUILayout.Button("X", GUILayout.Width(30), GUILayout.Height(30)))
                {
                    RemoveSceneFromBuildSettings(scenePath);
                }
                GUI.backgroundColor = Color.white;

                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(5);
            }
        }
        EditorGUILayout.EndScrollView();

        GUILayout.Space(10);

        // Botón para recargar la escena actual
        if (GUILayout.Button("Recargar Escena Actual"))
        {
            ReloadCurrentScene();
        }

        GUILayout.Space(20);
        GUILayout.Label("Agregar Escena a Build Settings", EditorStyles.boldLabel);

        // Espacio de arrastre para añadir escenas a Build Settings
        sceneToAdd = EditorGUILayout.ObjectField("Arrastra una escena aquí", sceneToAdd, typeof(SceneAsset), false);

        if (sceneToAdd != null)
        {
            AddSceneToBuildSettings(sceneToAdd);
            sceneToAdd = null; // Resetear el campo después de agregar la escena
        }
    }

    private void OpenScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
        {
            EditorSceneManager.OpenScene(scenePath);
        }
    }

    private void ReloadCurrentScene()
    {
        string activeScenePath = SceneManager.GetActiveScene().path;
        if (!string.IsNullOrEmpty(activeScenePath))
        {
            OpenScene(activeScenePath);
        }
        else
        {
            Debug.LogWarning("No hay una escena activa para recargar.");
        }
    }

    private void AddSceneToBuildSettings(Object sceneAsset)
    {
        string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
        if (string.IsNullOrEmpty(scenePath))
        {
            Debug.LogWarning("El objeto arrastrado no es una escena válida.");
            return;
        }

        // Verificar si la escena ya está en Build Settings
        var scenes = EditorBuildSettings.scenes.ToList();
        if (scenes.Any(scene => scene.path == scenePath))
        {
            Debug.LogWarning("La escena ya está en Build Settings.");
            return;
        }

        // Agregar la escena a Build Settings
        scenes.Add(new EditorBuildSettingsScene(scenePath, true));
        EditorBuildSettings.scenes = scenes.ToArray();
        Debug.Log("Escena añadida a Build Settings: " + scenePath);
    }

    private void RemoveSceneFromBuildSettings(string scenePath)
    {
        // Confirmación antes de eliminar la escena
        if (EditorUtility.DisplayDialog("Eliminar Escena", $"¿Seguro que deseas eliminar {scenePath} del Build Settings?", "Sí", "No"))
        {
            List<EditorBuildSettingsScene> scenes = EditorBuildSettings.scenes.ToList();
            scenes.RemoveAll(scene => scene.path == scenePath);
            EditorBuildSettings.scenes = scenes.ToArray();
            Debug.Log($"Escena eliminada del Build Settings: {scenePath}");
        }   
    }
}
