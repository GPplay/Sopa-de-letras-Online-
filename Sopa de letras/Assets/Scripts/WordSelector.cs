using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System;

public class WordSelector : MonoBehaviour
{
    public static WordSelector Instance;
    public TMP_Text txtPalabraSeleccionada;

    public List<CellData> celdasSeleccionadas = new List<CellData>();
    private Vector2Int direccionActual;
    [SerializeField]private GameManager instance;

    //selecionar
    //private int indiceColorActual = 0;
    private Color colorSeleccionActual;

    private void Awake() {
        Instance = this;

    }

    public void IniciarSeleccion(CellData celda)
    {
        if (celda.esCorrecta) return;

        LimpiarSeleccionAnterior();

        // Siempre amarillo con 50% de opacidad
        colorSeleccionActual = new Color(1f, 1f, 0.5f, 0.5f);

        celda.highlightImage.color = colorSeleccionActual;
        celdasSeleccionadas.Add(celda);
        direccionActual = Vector2Int.zero;
        ActualizarUI();
    }

    public void AgregarCelda(CellData nuevaCelda)
    {
        if (celdasSeleccionadas.Contains(nuevaCelda)) return;

        if (celdasSeleccionadas.Count > 0)
        {
            CellData ultimaCelda = celdasSeleccionadas[^1];
            Vector2Int delta = new Vector2Int(
                nuevaCelda.x - ultimaCelda.x,
                nuevaCelda.y - ultimaCelda.y
            );

            if (delta.x != 0 && delta.y != 0 && Mathf.Abs(delta.x) != Mathf.Abs(delta.y)) return;

            if (direccionActual == Vector2Int.zero)
            {
                direccionActual = new Vector2Int(
                    Mathf.Clamp(delta.x, -1, 1),
                    Mathf.Clamp(delta.y, -1, 1)
                );
            }
            else
            {
                Vector2Int dirNormalized = new Vector2Int(
                    Mathf.Clamp(delta.x, -1, 1),
                    Mathf.Clamp(delta.y, -1, 1)
                );
                if (dirNormalized != direccionActual) return;
            }
        }

        // Si la celda ya era correcta, no cambiar su color
        if (!nuevaCelda.esCorrecta)
        {
            nuevaCelda.highlightImage.color = colorSeleccionActual;
        }
        celdasSeleccionadas.Add(nuevaCelda);
        ActualizarUI();
    }

    public void TerminarSeleccion()
    {
        bool esValida = ValidarPalabra(ObtenerPalabra());
        if (esValida)
        {
            foreach (CellData celda in celdasSeleccionadas)
            {
                // Solo cambiar a morado si no estaba correcta antes
                if (!celda.esCorrecta)
                {
                    celda.highlightImage.color = new Color(0.5f, 0f, 0.5f, 0.8f); // Morado con 80% opacidad
                    celda.MarcarComoCorrecta();
                }
            }
        }
        else
        {
            // Limpiar solo las celdas que no eran correctas antes
            foreach (CellData celda in celdasSeleccionadas)
            {
                if (!celda.esCorrecta)
                {
                    celda.ResetearSeleccion();
                }
            }
        }
        celdasSeleccionadas.Clear();
        ActualizarUI();
    }

    private bool ValidarPalabra(string palabra)
    {

        // Comprobar nulidad del generador
        if (SopaDeLetrasGenerator.Instance == null)
        {
            Debug.LogError("¡Generador no inicializado!");
            return false;
        }

        // Comprobar nulidad de palabrasActuales
        if (SopaDeLetrasGenerator.Instance.palabrasActuales == null)
        {
            Debug.LogError("La lista de palabras está vacía.");
            return false;
        } 

        string palabraInvertida = InvertirPalabra(palabra);

        foreach (string palabraBuscada in SopaDeLetrasGenerator.Instance.palabrasActuales.ToArray())
        {
            bool coincideDirecta = palabra.Equals(palabraBuscada, StringComparison.OrdinalIgnoreCase);
            bool coincideInversa = palabraInvertida.Equals(palabraBuscada, StringComparison.OrdinalIgnoreCase);

            if (coincideDirecta)
            {
                //SopaDeLetrasGenerator.Instance.palabrasActuales.Remove(palabraBuscada);
                instance.MarcarPalabraEncontrada(palabra);
                return true;
            } else if (coincideInversa) {
                instance.MarcarPalabraEncontrada(palabraInvertida);
                return true;
            }
        }
        return false;
    }

    private string InvertirPalabra(string palabra)
    {
        char[] chars = palabra.ToCharArray();
        Array.Reverse(chars);
        return new string(chars);
    }

    private string ObtenerPalabra()
    {
        string palabra = "";
        foreach (CellData celda in celdasSeleccionadas) palabra += celda.letraText.text;
        return palabra;
    }

    private void LimpiarSeleccionAnterior()
    {
        // Limpiar solo las celdas que NO son correctas
        foreach (CellData celda in celdasSeleccionadas)
        {
            if (!celda.esCorrecta)
            {
                celda.ResetearSeleccion();
            }
        }
    }

    public void ActualizarUI()
    {
        if (txtPalabraSeleccionada != null)
            txtPalabraSeleccionada.text = "Selección: " + ObtenerPalabra();
    }
}