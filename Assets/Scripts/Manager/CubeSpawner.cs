using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    #pragma warning disable 0649
    [SerializeField] private GameObject cubePrefab; // Prefab del cubo a instanciar\
    [SerializeField] private Vector3 spawnAreaSize = new Vector3(5, 5, 5); // Área de spawn
    [SerializeField, Range(1, 10)] private int spawnAmount = 1;  // Cantidad de cubos a generar
    [SerializeField, Range(10, 100)] private int maxCubes = 50; // Máximo de cubos permitidos
    [SerializeField] private TMP_Text cubeCounterText; // Referencia al contador de UI
    [SerializeField] private Material[] randomMaterials; // Materiales aleatorios
    #pragma warning restore 0649

    private Queue<GameObject> cubePool = new Queue<GameObject>(); // Pool de cubos
    private Transform cubeParent; // Parent para organizar los cubos

    private void Start()
    {
        CheckParentCube();
        UpdateCubeCounter();
    }
    public void SpawnCube()
    {
        if (cubePrefab == null)
        {
            Debug.LogError("No se ha asignado un prefab de cubo en el Inspector.");
            return;
        }

        CheckParentCube();

        for (int i = 0; i < spawnAmount; i++)
        {
            if (cubePool.Count >= maxCubes)
            {
                Debug.LogWarning("Límite de cubos alcanzado.");
                return;
            }

            Vector3 randomPosition = Vector3.zero;

            // Asegurarse de que la posición esté libre
            bool positionFound = false;
            int maxAttempts = 10; // Intentos máximos para encontrar una posición libre

            for (int attempt = 0; attempt < maxAttempts; attempt++)
            {
                randomPosition = new Vector3(
                    Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
                    Random.Range(-spawnAreaSize.y / 2, spawnAreaSize.y / 2),
                    Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
                );

                // Comprobar si hay algún cubo en la posición
                if (!IsPositionOccupied(randomPosition))
                {
                    positionFound = true;
                    break; // Si encontramos una posición libre, salimos del bucle
                }
            }

            if (!positionFound)
            {
                Debug.LogWarning("No se encontró una posición libre después de varios intentos.");
                return; // Si no se encontró una posición libre, no instanciamos nada
            }

            GameObject newCube;
            if (cubePool.Count > 0)
            {
                // Reutilizar un cubo del pool
                newCube = cubePool.Dequeue();
                newCube.transform.position = randomPosition;
                newCube.SetActive(true);
                Debug.Log("Reutilizando cubo del pool.");
            }
            else
            {
                // Crear un nuevo cubo
                newCube = Instantiate(cubePrefab, randomPosition, Quaternion.identity);
                newCube.transform.SetParent(cubeParent); // Agrupar cubos bajo el parent
                Debug.Log("Creando un nuevo cubo.");
            }

            // Asignar un material aleatorio
            if (randomMaterials.Length > 0)
            {
                Renderer cubeRenderer = newCube.GetComponent<Renderer>();
                if (cubeRenderer != null)
                {
                    cubeRenderer.material = randomMaterials[Random.Range(0, randomMaterials.Length)];
                    Debug.Log("Asignando material: " + cubeRenderer.sharedMaterial.name);
                }
            }
        }

        UpdateCubeCounter();
    }

    // Método que verifica si una posición está ocupada por un cubo
    private bool IsPositionOccupied(Vector3 position)
    {
        // Comprobamos todos los cubos activos en el parent para ver si alguna posición está ocupada
        foreach (Transform child in cubeParent)
        {
            if (child.gameObject.activeSelf && Vector3.Distance(child.position, position) < 1) // Distancia mínima para considerar ocupada
            {
                return true; // Si hay un cubo cerca, consideramos la posición ocupada
            }
        }
        return false; // Si no hay cubos cerca, la posición está libre
    }

    private void CheckParentCube()
    {
        if (cubeParent == null)
        {
            GameObject cubesObject = GameObject.FindWithTag("CubesParent");
            if (cubesObject != null)
            {
                Debug.LogWarning("Se ha encontrado un objeto con la etiqueta 'CubesParent'. Usando ese objeto.");
                cubeParent = cubesObject.transform;

                foreach (Transform child in cubeParent)
                {
                    if (child.gameObject.activeSelf) continue;
                    cubePool.Enqueue(child.gameObject); // Guardar en el pool
                }
            }
            else
            {
                Debug.LogWarning("No se ha encontrado un objeto con la etiqueta 'CubesParent'. Creando uno nuevo.");
                cubeParent = new GameObject("Cubes").transform;
                cubeParent.tag = "CubesParent"; // Asignar la etiqueta al nuevo objeto
            }

        }
    }

    public void ClearCubes()
    {
        foreach (Transform child in cubeParent)
        {
            child.gameObject.SetActive(false); // Desactivar en lugar de destruir
            cubePool.Enqueue(child.gameObject); // Guardar en el pool
        }

        UpdateCubeCounter();
    }

    private void UpdateCubeCounter()
    {
        if (cubeCounterText != null)
        {
            int activeCubes = 0;

            // Recorremos todos los hijos del parent y contamos solo los activos
            foreach (Transform child in cubeParent)
            {
                if (child.gameObject.activeSelf) 
                {
                    activeCubes++;
                }
            }

            // Actualizamos el texto de la UI
            cubeCounterText.text = $"Cubos Activos: {activeCubes}";

            Debug.Log("Cubos activos: " + activeCubes);
        }
    }
}
