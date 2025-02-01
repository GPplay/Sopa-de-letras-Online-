using UnityEngine;
using System.Collections.Generic;

public class Selecionarpalabras : MonoBehaviour
{
    public GameObject columnaPrefab; // Prefab del sprite de la columna  
    private GameObject columnaInstancia; // Instancia de la columna  

    private string palabraSeleccionada = "";
    private List<GameObject> columnasActivas = new List<GameObject>(); // Para mantener las columnas que permanecen en la pantalla  

    private Vector2Int inicioSeleccion;
    private Vector2Int finSeleccion;
    private bool seleccionando = false;

    void Update()
    {
        // Comprobar si hay toques en la pantalla  
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    inicioSeleccion = GetTouchPosition(touch.position);
                    seleccionando = true;

                    // Crea la columna visual en la posición de inicio  
                    columnaInstancia = Instantiate(columnaPrefab);
                    UpdateColumnaPosition(inicioSeleccion); // Posiciona la columna  
                    columnaInstancia.transform.SetParent(transform); // Asegúrate de que sea hijo del Canvas  
                    break;

                case TouchPhase.Moved:
                    if (seleccionando)
                    {
                        finSeleccion = GetTouchPosition(touch.position);
                        UpdateColumnaPosition(finSeleccion);
                        ActualizarPalabraSeleccionada();
                    }
                    break;

                case TouchPhase.Ended:
                    seleccionando = false;

                    // Verifica la palabra  
                    if (VerificarPalabra())
                    {
                        // Si la palabra es correcta, mantiene la columna  
                        columnasActivas.Add(columnaInstancia); // Añadimos a la lista de columnas activas  
                    }
                    else
                    {
                        // Si la palabra es incorrecta, destruye la columna  
                        Destroy(columnaInstancia);
                    }
                    break;
            }
        }
    }

    private Vector2Int GetTouchPosition(Vector2 touchPosition)
    {
        // Convertir la posición del toque en coordenadas locales de la cuadrícula de letras  
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, touchPosition, null, out Vector2 localPoint);

        int xPos = Mathf.FloorToInt((localPoint.x + (transform as RectTransform).rect.width / 2) / 50f); // Cambia 50f por el tamaño real de tus celdas.  
        int yPos = Mathf.FloorToInt((localPoint.y + (transform as RectTransform).rect.height / 2) / 50f); // Cambia 50f por el tamaño real de tus celdas.  

        // Limitar las posiciones para evitar errores de índice  
        xPos = Mathf.Clamp(xPos, 0, 9); // Cambia 9 por gridSize-1 según el tamaño de tu cuadrícula.  
        yPos = Mathf.Clamp(yPos, 0, 9); // Cambia 9 por gridSize-1 según el tamaño de tu cuadrícula.  

        return new Vector2Int(xPos, yPos);
    }

    private void UpdateColumnaPosition(Vector2Int posicion)
    {
        // Calcula la posición de la columna basada en la cuadrícula  
        Vector3 nuevaPosicion = ConvertirAPosicionMundo(posicion);
        if (columnaInstancia != null)
        {
            columnaInstancia.transform.localPosition = nuevaPosicion; // Usando localPosition en el Canvas  
        }
    }

    private Vector3 ConvertirAPosicionMundo(Vector2Int posicion)
    {
        // Devuelve la posición local en el Canvas según la cuadrícula  
        float cellSize = 50f; // Cambia esto si el tamaño de tu celda es diferente  
        return new Vector3(posicion.x * cellSize - (transform as RectTransform).rect.width / 2 + cellSize / 2,
                           posicion.y * cellSize - (transform as RectTransform).rect.height / 2 + cellSize / 2,
                           0);
    }

    private void ActualizarPalabraSeleccionada()
    {
        // Implementa la lógica para construir la palabra seleccionada  
        palabraSeleccionada = "SOMESELECTEDWORD"; // Placeholder, necesitas construirla dinámicamente  
    }

    private bool VerificarPalabra()
    {
        List<string> words = SopaDeLetrasGenerator.Instance.words; // Referencia a la lista de palabras  

        string palabraInvertida = InvertirPalabra(palabraSeleccionada);

        if (words.Contains(palabraSeleccionada) || words.Contains(palabraInvertida))
        {
            Debug.Log($"¡Palabra encontrada: {palabraSeleccionada}!");
            return true; // Retorna verdadero si se ha encontrado la palabra  
        }
        else
        {
            Debug.Log("Palabra no encontrada.");
            return false; // Retorna falso si no se encuentra la palabra  
        }
    }

    private string InvertirPalabra(string palabra)
    {
        char[] charArray = palabra.ToCharArray();
        System.Array.Reverse(charArray);
        return new string(charArray);
    }
}