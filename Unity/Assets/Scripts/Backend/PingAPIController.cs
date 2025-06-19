using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Http;
using UnityEngine.Networking;
using TMPro;
using System;

public class PingAPIController : MonoBehaviour
{
    public string apiUrl = "https://localhost:7254/Ping"; // URL de la API a la que se enviará la solicitud
    public TextMeshPro responseText; // Referencia al componente TextMeshProUGUI para mostrar la respuesta

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GetPing());
    }

    private IEnumerator GetPing()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // Enviar la solicitud y esperar la respuesta
            yield return webRequest.SendWebRequest();
            // Comprobar si hubo un error en la solicitud
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error al obtener el ping: " + webRequest.error);
                responseText.text = "Error: " + webRequest.error;
            }
            else
            {
                // Procesar la respuesta de la API
                string response = webRequest.downloadHandler.text;
                Debug.Log("Respuesta de la API: " + response);
                responseText.text = "Ping: " + response; // Mostrar la respuesta en el TextMeshProUGUI
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
