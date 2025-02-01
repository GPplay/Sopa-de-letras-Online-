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

                    // Crea la columna visual en la posici�n de inicio  
                    columnaInstancia = Instantiate(columnaPrefab);
                    UpdateColumnaPosition(inicioSeleccion); // Posiciona la columna  
                    columnaInstancia.transform.SetParent(transform); // Aseg�rate de que sea hijo del Canvas  
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
                        columnasActivas.Add(columnaInstancia); // A�adimos a la lista de columnas activas  
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
        // Convertir la posici�n del toque en coordenadas locales de la cuadr�cula de letras  
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform, touchPosition, null, out Vector2 localPoint);

        int xPos = Mathf.FloorToInt((localPoint.x + (transform as RectTransform).rect.width / 2) / 50f); // Cambia 50f por el tama�o real de tus celdas.  
        int yPos = Mathf.FloorToInt((localPoint.y + (transform as RectTransform).rect.height / 2) / 50f); // Cambia 50f por el tama�o real de tus celdas.  

        // Limitar las posiciones para evitar errores de �ndice  
        xPos = Mathf.Clamp(xPos, 0, 9); // Cambia 9 por gridSize-1 seg�n el tama�o de tu cuadr�cula.  
        yPos = Mathf.Clamp(yPos, 0, 9); // Cambia 9 por gridSize-1 seg�n el tama�o de tu cuadr�cula.  

        return new Vector2Int(xPos, yPos);
    }

    private void UpdateColumnaPosition(Vector2Int posicion)
    {
        // Calcula la posici�n de la columna basada en la cuadr�cula  
        Vector3 nuevaPosicion = ConvertirAPosicionMundo(posicion);
        if (columnaInstancia != null)
        {
            columnaInstancia.transform.localPosition = nuevaPosicion; // Usando localPosition en el Canvas  
        }
    }

    private Vector3 ConvertirAPosicionMundo(Vector2Int posicion)
    {
        // Devuelve la posici�n local en el Canvas seg�n la cuadr�cula  
        float cellSize = 50f; // Cambia esto si el tama�o de tu celda es diferente  
        return new Vector3(posicion.x * cellSize - (transform as RectTransform).rect.width / 2 + cellSize / 2,
                           posicion.y * cellSize - (transform as RectTransform).rect.height / 2 + cellSize / 2,
                           0);
    }

    private void ActualizarPalabraSeleccionada()
    {
        // Implementa la l�gica para construir la palabra seleccionada  
        palabraSeleccionada = "SOMESELECTEDWORD"; // Placeholder, necesitas construirla din�micamente  
    }

    private bool VerificarPalabra()
    {
        List<string> words = SopaDeLetrasGenerator.Instance.words; // Referencia a la lista de palabras  

        string palabraInvertida = InvertirPalabra(palabraSeleccionada);

        if (words.Contains(palabraSeleccionada) || words.Contains(palabraInvertida))
        {
            Debug.Log($"�Palabra encontrada: {palabraSeleccionada}!");
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