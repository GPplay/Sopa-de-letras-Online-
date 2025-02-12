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

    private void Awake() => Instance = this;

    public void IniciarSeleccion(CellData celda)
    {
        LimpiarSeleccionAnterior();
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

        nuevaCelda.highlightImage.color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        celdasSeleccionadas.Add(nuevaCelda);
        ActualizarUI();
    }

    public void TerminarSeleccion()
    {
        bool esValida = ValidarPalabra(ObtenerPalabra());
        if (esValida) foreach (CellData celda in celdasSeleccionadas) celda.MarcarComoCorrecta();
        else LimpiarSeleccionAnterior();
        celdasSeleccionadas.Clear();
        ActualizarUI();
    }

    private bool ValidarPalabra(string palabra)
    {
        string palabraInvertida = InvertirPalabra(palabra);

        foreach (string palabraBuscada in SopaDeLetrasGenerator.Instance.palabrasActuales.ToArray())
        {
            bool coincideDirecta = palabra.Equals(palabraBuscada, StringComparison.OrdinalIgnoreCase);
            bool coincideInversa = palabraInvertida.Equals(palabraBuscada, StringComparison.OrdinalIgnoreCase);

            if (coincideDirecta || coincideInversa)
            {
                SopaDeLetrasGenerator.Instance.palabrasActuales.Remove(palabraBuscada);
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
        foreach (CellData celda in celdasSeleccionadas) celda.ResetearSeleccion();
    }

    public void ActualizarUI()
    {
        if (txtPalabraSeleccionada != null)
            txtPalabraSeleccionada.text = "Selección: " + ObtenerPalabra();
    }
}