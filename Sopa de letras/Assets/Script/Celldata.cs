using UnityEngine;
using UnityEngine.Events;

public class CellData : MonoBehaviour
{
    public int x { get; private set; }
    public int y { get; private set; }
    public UnityEvent onCellSelected; // Evento para la selección de celdas  

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    public void SelectCell()
    {
        onCellSelected.Invoke(); // Llama al evento al seleccionar la celda  
    }
}