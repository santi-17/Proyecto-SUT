using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AradoDisco : MonoBehaviour
{
    public KeyCode activarArado = KeyCode.G; // Tecla para activar el arado
    [SerializeField] private Transform puntoRaycast;
    [SerializeField] private Terrain terrain;

    public float distanciaDeteccion = 5f; // Distancia de detecci�n del arado
    public LayerMask Suelo; // Capa del suelo para detectar colisiones
    public int materialAradoIndex = 1; // �ndice del material del arado en el array de materiales

    public float profundidadSurco = 0.1f; // Profundidad del arado en el terreno
    public int size = 5; // Tama�o del �rea afectada por el arado

    bool aradoActivo = false; // Estado del arado

    // Start is called before the first frame update
    void Start()
    {
        profundidadSurco = 0.1f;
        //Terrain terrain = GetComponent<Terrain>();
        if (terrain != null)
        {
            TerrainData data = terrain.terrainData;

            float[,] alturas = data.GetHeights(0, 0, data.heightmapResolution, data.heightmapResolution);
            for (int x = 0; x < data.heightmapResolution; x++)
            {
                for (int z = 0; z < data.heightmapResolution; z++)
                {
                    alturas[z, x] = 0.5f; // mitad de la altura m�xima
                }
            }

            data.SetHeights(0, 0, alturas);
            terrain.Flush();
            Debug.Log("Terreno elevado a 0.5f");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(activarArado))
        {
            aradoActivo = !aradoActivo; // Cambiar el estado del arado
            //GetComponent<Renderer>().material = aradoActivo ? materialArado : null; // Cambiar el material del arado
            Debug.Log("Arado de disco " + (aradoActivo ? "activado" : "desactivado"));
        }

        if (!aradoActivo) return; // Si el arado no est� activo, salir del m�todo

        if (Physics.Raycast(puntoRaycast.position, Vector3.down, out RaycastHit hit, distanciaDeteccion))
        {
            Debug.DrawRay(puntoRaycast.position, Vector3.down * distanciaDeteccion, Color.red);

            //Terrain terrain = hit.collider.GetComponent<Terrain>();
            if (terrain != null)
            {
                // Si el rayo detecta un terreno, activar el arado
                Debug.Log("Arando de disco el terreno en: " + hit.point);
                // Aqu� puedes agregar la l�gica para arar el terreno, como cambiar su textura o estado
                Vector3 terrainPos = hit.point - terrain.transform.position;
                TerrainData data = terrain.terrainData;
                float profundidadEnMetros = 0.1f; // esto es lo que vos quer�s, por ejemplo 1 metro
                float alturaMaxima = data.size.y;
                float profundidadSurco = profundidadEnMetros / alturaMaxima;

                int heightmapX = (int)((terrainPos.x / data.size.x) * data.heightmapResolution);
                int heightmapZ = (int)((terrainPos.z / data.size.z) * data.heightmapResolution);

                //int size = 5; // tama�o del �rea pintada
                // Asegurarse de que el �rea afectada no se salga de los l�mites del heightmap
                int startX = Mathf.Clamp(heightmapX - size / 2, 0, data.heightmapResolution - 1);
                int startZ = Mathf.Clamp(heightmapZ - size / 2, 0, data.heightmapResolution - 1);

                //pinto la textura del terreno

                int alphamapX = (int)((terrainPos.x / data.size.x) * data.alphamapWidth);
                int alphamapZ = (int)((terrainPos.z / data.size.z) * data.alphamapHeight);

                int startAlphaX = Mathf.Clamp(alphamapX - size / 2, 0, data.alphamapWidth - 1);
                int startAlphaZ = Mathf.Clamp(alphamapZ - size / 2, 0, data.alphamapHeight - 1);

                float[,,] splatmap = data.GetAlphamaps(startAlphaX, startAlphaZ, size, size);

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

                data.SetAlphamaps(startAlphaX, startAlphaZ, splatmap);

                //2. deformar el terreno (heigthmap)
                float[,] heights = data.GetHeights(startX, startZ, size, size);

                for (int x = 0; x < size; x++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        float valorAntes = heights[z, x];

                        float distanciaCentro = Mathf.Abs(x - size / 2);
                        float factor = 1f - (distanciaCentro / (size / 2f));
                        heights[z, x] -= profundidadSurco * factor;
                        //heights[z, x] -= profundidadSurco;
                        Debug.Log($"Altura antes: {valorAntes}, despu�s: {heights[z, x]}");
                        Debug.Log($"Modificando altura en ({z},{x}) de {heights[z, x] + profundidadSurco} a {heights[z, x]}");

                        //heights[z, x] -= profundidadSurco; // Reducir la altura del terreno
                        heights[z, x] = Mathf.Clamp01(heights[z, x]); // Asegurarse de que la altura no se salga de los l�mites


                    }
                }

                data.SetHeights(startX, startZ, heights);

                terrain.Flush();

                Debug.Log("Terreno arado  de disco en: " + hit.point + " con profundidad: " + profundidadSurco);

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
