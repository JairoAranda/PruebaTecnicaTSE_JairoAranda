using TMPro;
using UnityEngine.Android;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    [Header("UI")]
    #pragma warning disable 0649
    [SerializeField] private TMP_Text permissionStatusText;
    #pragma warning restore 0649
    private void Start() 
    {
        CheckAndRequestLocationPermission();
    }

    private void CheckAndRequestLocationPermission()
    {
        string permission = Permission.FineLocation;
        if (Permission.HasUserAuthorizedPermission(permission))
        {
            // Permiso otorgado
            UpdatePermissionStatus("Permiso de ubicación: Otorgado");
            Debug.Log("Permiso de ubicación: Otorgado");
            StartLocationServices();
        }
        else
        {
            // Permiso no otorgado
            UpdatePermissionStatus("Permiso de ubicación: Rechazado");
            Debug.Log("Permiso de ubicación: Rechazado");
            Permission.RequestUserPermission(permission);
        }
    }

    private void OnApplicationFocus(bool focusStatus) 
    {
        if (focusStatus)
        {
            // Verificar el estado del permiso cuando la aplicación vuelve a estar en primer plano
            CheckAndRequestLocationPermission();
        }
    }

    private void StartLocationServices()
    {
        //Inicia los servicios de ubicación
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("El servicio de ubicación está desactivado en el dispositivo.");
            return;
        }
        
        Input.location.Start();
    }

    void UpdatePermissionStatus(string status)
    {
        if (permissionStatusText != null)
        {
            permissionStatusText.text = status;
        }
    }
}
