using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CubeSpawner))]
public class CubeSpawnerEditor : Editor
{
    private SerializedProperty cubePrefab;
    private SerializedProperty spawnAreaSize;
    private SerializedProperty spawnAmount;
    private SerializedProperty maxCubes;
    private SerializedProperty cubeCounterText;
    private SerializedProperty randomMaterials;

    private void OnEnable()
    {
        cubePrefab = serializedObject.FindProperty("cubePrefab");
        spawnAreaSize = serializedObject.FindProperty("spawnAreaSize");
        spawnAmount = serializedObject.FindProperty("spawnAmount");
        maxCubes = serializedObject.FindProperty("maxCubes");
        cubeCounterText = serializedObject.FindProperty("cubeCounterText");
        randomMaterials = serializedObject.FindProperty("randomMaterials");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update(); // Actualizar valores serializados

        // Título
        GUIStyle titleStyle = new GUIStyle(EditorStyles.boldLabel)
        {
            fontSize = 14,
            alignment = TextAnchor.MiddleCenter
        };
        GUILayout.Label("Cube Spawner ", titleStyle);

        GUILayout.Space(10);

        // Campos editables
        EditorGUILayout.PropertyField(cubePrefab, new GUIContent("Prefab del Cubo"));
        EditorGUILayout.PropertyField(spawnAreaSize, new GUIContent("Tamaño del Área"));
        EditorGUILayout.IntSlider(spawnAmount, 1, 10, new GUIContent("Cantidad de Cubos"));
        EditorGUILayout.IntSlider(maxCubes, 10, 100, new GUIContent("Máximo de Cubos"));
        EditorGUILayout.PropertyField(cubeCounterText, new GUIContent("Contador de UI"));
        EditorGUILayout.LabelField("Materiales Aleatorios", EditorStyles.boldLabel);

        GUILayout.Space(10);

        for (int i = 0; i < randomMaterials.arraySize; i++)
        {
            EditorGUILayout.BeginHorizontal();

            SerializedProperty materialProp = randomMaterials.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(materialProp, new GUIContent($"Material {i + 1}"));

            // Botón para eliminar un material específico
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("X", GUILayout.Width(25)))
            {
                randomMaterials.DeleteArrayElementAtIndex(i);
            }
            GUI.backgroundColor = Color.white;

            EditorGUILayout.EndHorizontal();
        }

        // Botón para añadir un nuevo material al array
        GUI.backgroundColor = Color.yellow;
        if (GUILayout.Button("Añadir Nuevo Material", GUILayout.Height(25)))
        {
            randomMaterials.arraySize++;
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(30);

        // Botón verde para generar cubos
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Generar Cubos", GUILayout.Height(30)))
        {
            ((CubeSpawner)target).SpawnCube();
        }
        GUI.backgroundColor = Color.white;

        GUILayout.Space(5);

        // Botón rojo para eliminar cubos
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("Eliminar Todos los Cubos", GUILayout.Height(30)))
        {
            ((CubeSpawner)target).ClearCubes();
        }
        GUI.backgroundColor = Color.white;

        serializedObject.ApplyModifiedProperties(); // Aplicar cambios
    }
}
