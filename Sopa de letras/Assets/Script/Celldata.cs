using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class CellData : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    public TMP_Text letraText;
    public Image highlightImage;
    public int x, y;

    public void OnPointerDown(PointerEventData eventData)
    {
        WordSelector.Instance.IniciarSeleccion(this);
        ActualizarSeleccionDuranteArrastre(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        ActualizarSeleccionDuranteArrastre(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        WordSelector.Instance.TerminarSeleccion();
    }

    private void ActualizarSeleccionDuranteArrastre(PointerEventData eventData)
    {
        List<RaycastResult> resultados = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, resultados);

        foreach (RaycastResult resultado in resultados)
        {
            CellData celda = resultado.gameObject.GetComponent<CellData>();
            if (celda != null && !WordSelector.Instance.celdasSeleccionadas.Contains(celda))
            {
                WordSelector.Instance.AgregarCelda(celda);
                break;
            }
        }
        WordSelector.Instance.ActualizarUI();
    }

    public void MarcarComoCorrecta()
    {
        highlightImage.color = Color.green;
        letraText.color = Color.white;
    }

    public void ResetearSeleccion()
    {
        highlightImage.color = Color.clear;
        letraText.color = Color.black;
    }

    public void Configurar(int x, int y, string letra)
    {
        this.x = x;
        this.y = y;
        letraText.text = letra;
    }
}