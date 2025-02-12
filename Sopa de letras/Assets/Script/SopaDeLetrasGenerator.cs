using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class SopaDeLetrasGenerator : MonoBehaviour
{
    public static SopaDeLetrasGenerator Instance;

    [Header("Configuración")]
    public GameObject cellPrefab; // Prefab de la celda
    public Transform gridParent;  // Contenedor del grid (debe ser cuadrado)
    public List<string> palabras = new List<string>() {
    "PAN", "CASA", "FLOR", "LUZ", "MESA", "SOL", "MAR", "CIELO", "TIERRA", "FUEGO",
    "AGUA", "VIENTO", "ARBOL", "PERRO", "GATO", "LIBRO", "MANO", "PIE", "OJOS", "BOCA",
    "NARIZ", "COCHE", "CALLE", "PLAZA", "CAMINO", "PUENTE", "BANCO", "PLATO", "VASO", "TENEDOR",
    "CUCHARA", "LLAVE", "PUERTA", "VENTANA", "TECHO", "SUELO", "PISO", "SILLA", "MESA", "SOFA",
    "CAMA", "ALMOHADA", "MANTEL", "TOALLA", "JABON", "PELO", "UÑA", "PIEL", "HUESO", "SANGRE",
    "CARNE", "HUESO", "HIELO", "NIEVE", "LLUVIA", "TRUENO", "RAYO", "NUBE", "ESTRELLA", "LUNA",
    "PLANETA", "BOSQUE", "RIO", "LAGO", "MONTAÑA", "VALLE", "PLAYA", "ARENA", "PIEDRA", "BARRO",
    "METAL", "ORO", "PLATA", "BRONCE", "HIERRO", "CARBON", "MADERA", "PAPEL", "TELA", "HILO",
    "AGUJA", "BOTON", "ZAPATO", "SOMBRERO", "GUANTE", "BUFANDA", "ABRIGO", "FALDA", "PANTALON", "CAMISA",
    "VESTIDO", "CHAQUETA", "RELOJ", "CADENA", "ANILLO", "PULSERA", "COLLAR", "BOLSO", "MALETA", "MOCHILA"};

    private const int GRID_SIZE = 10; // Tamaño fijo 10x10
    private char[,] grid = new char[GRID_SIZE, GRID_SIZE];
    public List<string> palabrasActuales = new List<string>();

    private void Awake() => Instance = this;

    void Start()
    {
        ConfigurarTamañoGridParent();
        GenerarNuevoNivel();
    }

    // Ajusta el tamaño del contenedor para que sea cuadrado
    private void ConfigurarTamañoGridParent()
    {
        GridLayoutGroup gridLayout = gridParent.GetComponent<GridLayoutGroup>();
        float tamañoCelda = gridLayout.cellSize.x;
        float spacing = gridLayout.spacing.x;
        float tamañoTotal = (tamañoCelda + spacing) * GRID_SIZE;
        gridParent.GetComponent<RectTransform>().sizeDelta = new Vector2(tamañoTotal, tamañoTotal);
    }

    public void GenerarNuevoNivel()
    {
        // Limpiar grid anterior
        foreach (Transform child in gridParent) Destroy(child.gameObject);

        // Generar nuevo grid
        InicializarGrid();
        ColocarPalabras();
        LlenarEspaciosVacios();
        DibujarGrid();
    }

    private void InicializarGrid()
    {
        for (int x = 0; x < GRID_SIZE; x++)
            for (int y = 0; y < GRID_SIZE; y++)
                grid[x, y] = '\0';
    }

    private void ColocarPalabras()
    {
        foreach (string palabra in palabras)
        {
            bool colocada = false;
            int intentos = 0;

            while (!colocada && intentos < 100)
            {
                int x = Random.Range(0, GRID_SIZE);
                int y = Random.Range(0, GRID_SIZE);
                Vector2Int direccion = ObtenerDireccionAleatoriaValida(palabra.Length, x, y);

                if (direccion != Vector2Int.zero && PuedeColocarPalabra(palabra, x, y, direccion.x, direccion.y))
                {
                    ColocarPalabra(palabra, x, y, direccion.x, direccion.y);
                    palabrasActuales.Add(palabra);
                    colocada = true;
                }
                intentos++;
            }
        }
    }

    private Vector2Int ObtenerDireccionAleatoriaValida(int longitudPalabra, int x, int y)
    {
        List<Vector2Int> direccionesPosibles = new List<Vector2Int>()
        {
            Vector2Int.right,    // Horizontal →
            Vector2Int.left,     // Horizontal ←
            Vector2Int.up,       // Vertical ↑
            Vector2Int.down,     // Vertical ↓
            new Vector2Int(1, 1),  // Diagonal ↘
            new Vector2Int(-1, -1),// Diagonal ↖
            new Vector2Int(1, -1), // Diagonal ↗
            new Vector2Int(-1, 1)  // Diagonal ↙
        };

        // Filtrar direcciones que no excedan el tamaño del grid
        foreach (Vector2Int dir in direccionesPosibles)
        {
            int newX = x + (longitudPalabra - 1) * dir.x;
            int newY = y + (longitudPalabra - 1) * dir.y;
            if (newX >= 0 && newX < GRID_SIZE && newY >= 0 && newY < GRID_SIZE)
                return dir;
        }

        return Vector2Int.zero;
    }

    private bool PuedeColocarPalabra(string palabra, int x, int y, int dirX, int dirY)
    {
        for (int i = 0; i < palabra.Length; i++)
        {
            int newX = x + i * dirX;
            int newY = y + i * dirY;
            if (grid[newX, newY] != '\0' && grid[newX, newY] != palabra[i])
                return false;
        }
        return true;
    }

    private void ColocarPalabra(string palabra, int x, int y, int dirX, int dirY)
    {
        for (int i = 0; i < palabra.Length; i++)
        {
            int newX = x + i * dirX;
            int newY = y + i * dirY;
            grid[newX, newY] = palabra[i];
        }
    }

    private void LlenarEspaciosVacios()
    {
        for (int x = 0; x < GRID_SIZE; x++)
            for (int y = 0; y < GRID_SIZE; y++)
                if (grid[x, y] == '\0')
                    grid[x, y] = (char)Random.Range('A', 'Z' + 1);
    }

    private void DibujarGrid()
    {
        for (int x = 0; x < GRID_SIZE; x++)
            for (int y = 0; y < GRID_SIZE; y++)
                CrearCelda(x, y, grid[x, y].ToString());
    }

    private void CrearCelda(int x, int y, string letra)
    {
        GameObject celda = Instantiate(cellPrefab, gridParent);
        celda.GetComponent<CellData>().Configurar(x, y, letra);
    }
}