using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SelecionarPalabras : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler, IPointerUpHandler
{
    public Sopadeletras sopadeletras; // Referencia al script de la sopa de letras
    public Color highlightColor = Color.yellow; // Color para resaltar las celdas
    public Color defaultColor = Color.white; // Color por defecto de las celdas

    private List<TextMeshProUGUI> selectedCells = new List<TextMeshProUGUI>();
    private string selectedWord = "";
    private bool isSelecting = false;

    void Update()
    {
        if (isSelecting && Input.GetMouseButtonUp(0))
        {
            CheckWord();
            ClearSelection();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelecting)
        {
            TextMeshProUGUI cellText = eventData.pointerEnter.GetComponent<TextMeshProUGUI>();
            if (cellText != null && !selectedCells.Contains(cellText))
            {
                selectedCells.Add(cellText);
                selectedWord += cellText.text;
                HighlightCell(cellText, true);
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isSelecting = true;
        ClearSelection(); // Limpiar selección previa al comenzar una nueva
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isSelecting = false;
    }

    void HighlightCell(TextMeshProUGUI cellText, bool highlight)
    {
        cellText.color = highlight ? highlightColor : defaultColor;
    }

    void ClearSelection()
    {
        foreach (var cell in selectedCells)
        {
            HighlightCell(cell, false);
        }
        selectedCells.Clear();
        selectedWord = "";
    }

    void CheckWord()
    {
        if (sopadeletras.words.Contains(selectedWord))
        {
            Debug.Log("Palabra correcta: " + selectedWord);
            // Mantener el resaltado para las palabras correctas
        }
        else
        {
            Debug.Log("Palabra incorrecta: " + selectedWord);
            ClearSelection(); // Limpiar el resaltado para palabras incorrectas
        }
    }
}