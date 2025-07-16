using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target; // El objetivo al que la c�mara seguir�
    public Vector3 offset = new Vector3(0, 5, -10); // Desplazamiento de la c�mara respecto al objetivo
    public float rotationSpeed = 5f; // Velocidad de rotacion de la c�mara
    public float zoomSpeed = 2f; // Velocidad de zoom de la c�mara
    public float minZoom = 5f; // Zoom m�nimo
    public float maxZoom = 20f; // Zoom m�ximo
    public Transform cameraTransform; // Transform de la c�mara

    private float currentYaw = 0f; // �ngulo de rotaci�n en el eje Y
    private float currentPitch = 15f; // �ngulo de rotaci�n en el eje X
    private float minPitch = -10f; // �ngulo m�nimo de inclinaci�n de la c�mara
    private float maxPitch = 60f; // �ngulo m�ximo de inclinaci�n de la c�mara

    private void LateUpdate()
    {
        if (Input.GetMouseButton(1)) // Bot�n derecho del rat�n para rotar
        {
            currentYaw += Input.GetAxis("Mouse X") * rotationSpeed; // Rotaci�n horizontal
            currentPitch -= Input.GetAxis("Mouse Y") * rotationSpeed; // Rotaci�n vertical
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch); // Limitar la inclinaci�n de la c�mara

        }

        // Zoom con la rueda del rat�n
        float scroll = Input.GetAxis("Mouse ScrollWheel"); // Obtener el valor de la rueda del rat�n
        if (scroll != 0f) // Si se est� desplazando la rueda del rat�n
        {
            float zoomAmount = scroll * zoomSpeed; // Calcular el cambio de zoom
            offset.z = Mathf.Clamp(offset.z + zoomAmount, -maxZoom, -minZoom); // Limitar el zoom
        }
        // Posici�n de la c�mara
        Quaternion quaternion = Quaternion.Euler(currentPitch, currentYaw, 0); // Crear una rotaci�n basada en los �ngulos actuales
        Vector3 desiredPosition = target.position + quaternion * offset; // Calcular la posici�n deseada de la c�mara

        // Actualizar la posici�n y rotaci�n de la c�mara
        cameraTransform.position = desiredPosition; // Establecer la posici�n de la c�mara
        cameraTransform.LookAt(target.position); // Hacer que la c�mara mire hacia el objetivo
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
