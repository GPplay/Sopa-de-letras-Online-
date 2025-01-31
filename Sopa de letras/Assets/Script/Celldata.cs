using UnityEngine;

public class CellData : MonoBehaviour
{
    public int x { get; private set; }
    public int y { get; private set; }

    public void SetCoordinates(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}