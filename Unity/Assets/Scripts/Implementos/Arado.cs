using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arado : MonoBehaviour
{

    public KeyCode activarArado = KeyCode.F; // Tecla para activar el arado
    [SerializeField] private Transform puntoRaycast;
    public float distanciaDeteccion = 5f; // Distancia de detección del arado
    public LayerMask capaSuelo; // Capa del suelo para detectar colisiones
    public int materialAradoIndex = 1; // Índice del material del arado en el array de materiales

    bool aradoActivo = false; // Estado del arado

    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(activarArado))
        {
            aradoActivo = !aradoActivo; // Cambiar el estado del arado
            //GetComponent<Renderer>().material = aradoActivo ? materialArado : null; // Cambiar el material del arado
            Debug.Log("Arado " + (aradoActivo ? "activado" : "desactivado"));
        }

        if(!aradoActivo) return; // Si el arado no está activo, salir del método

        if (Physics.Raycast(puntoRaycast.position, Vector3.down, out RaycastHit hit, distanciaDeteccion, capaSuelo))
        {
            Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                // Si el rayo detecta un terreno, activar el arado
                Debug.Log("Arando el terreno en: " + hit.point);
                // Aquí puedes agregar la lógica para arar el terreno, como cambiar su textura o estado
                Vector3 terrainPos = hit.point - terrain.transform.position;
                TerrainData data = terrain.terrainData;

                int mapX = (int)((terrainPos.x / data.size.x) * data.alphamapWidth);
                int mapZ = (int)((terrainPos.z / data.size.z) * data.alphamapHeight);

                int size = 5; // tamaño del área pintada
                int startX = Mathf.Clamp(mapX - size / 2, 0, data.alphamapWidth - 1);
                int startZ = Mathf.Clamp(mapZ - size / 2, 0, data.alphamapHeight - 1);

                float[,,] splatmap = data.GetAlphamaps(startX, startZ, size, size);

                for (int x = 0; x < size; x++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        for (int i = 0; i < data.alphamapLayers; i++)
                            splatmap[x, z, i] = 0;

                        splatmap[x, z, materialAradoIndex] = 1;
                    }
                }

                //splatmap[0, 0, materialAradoIndex] = 1;

                data.SetAlphamaps(startX, startZ, splatmap);
            }
            else
            {
                Debug.LogWarning("El objeto no tiene un Renderer para cambiar el material.");
            }

        }
        else
        {
            // Si no se detecta el suelo, desactivar el arado
            Debug.Log("No hay suelo debajo del arado");
        }

    }
}
