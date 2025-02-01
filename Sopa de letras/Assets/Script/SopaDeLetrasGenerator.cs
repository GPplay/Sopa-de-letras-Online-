using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SopaDeLetrasGenerator : MonoBehaviour
{
    public static SopaDeLetrasGenerator Instance;
    public enum Language { Spanish, English }
    public Language language = Language.Spanish; // Idioma por defecto: espa�ol

    public int gridSize = 10; // Tama�o de la cuadr�cula (10x10 por defecto)
    public GameObject cellPrefab; // Prefab de la casilla (debe tener un componente Text o TextMeshProUGUI)
    public Transform gridParent; // Objeto padre en el Canvas donde se generar� la cuadr�cula

    private char[,] grid; // Matriz que representa la sopa de letras
    public List<string> words = new List<string>(); // Lista de palabras a incluir en la sopa de letras

    private List<string> spanishWords = new List<string>
    {
        "MANZANA", "PLAYA", "CEREBRO", "PAN", "LADRILLO", "SILLA", "RELOJ", "NUBE", "FANTASMA", "UVA",
        "MIEL", "CASA", "LIM�N", "DINERO", "PAPEL", "PERLA", "TEL�FONO", "PIZZA", "AVI�N", "PLANTA"
    };

    private List<string> englishWords = new List<string>
    {
        "APPLE", "BEACH", "BRAIN", "BREAD", "BRICK", "CHAIR", "CLOCK", "CLOUD", "GHOST", "GRAPE",
        "HONEY", "HOUSE", "LEMON", "MONEY", "PAPER", "PEARL", "PHONE", "PIZZA", "PLANE", "PLANT"
    };

    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        GenerateWords();
        GenerateGrid();
        PlaceWords();
        FillEmptySpaces();
        DisplayGrid();
    }

    // Genera la lista de palabras aleatorias seg�n el idioma seleccionado
    private void GenerateWords()
    {
        List<string> wordList = (language == Language.Spanish) ? spanishWords : englishWords;
        int wordCount = 5; // N�mero de palabras a incluir (puedes ajustarlo)

        for (int i = 0; i < wordCount; i++)
        {
            if (wordList.Count == 0) break;
            string word = wordList[Random.Range(0, wordList.Count)];
            words.Add(word);
            wordList.Remove(word); // Evita duplicados
        }

        Debug.Log("Palabras generadas: " + string.Join(", ", words));
    }

    // Inicializa la cuadr�cula vac�a
    private void GenerateGrid()
    {
        grid = new char[gridSize, gridSize];
    }

    // Coloca las palabras en la cuadr�cula
    private void PlaceWords()
    {
        foreach (string word in words)
        {
            bool placed = false;
            while (!placed)
            {
                int x = Random.Range(0, gridSize);
                int y = Random.Range(0, gridSize);
                int directionX = Random.Range(-1, 2); // Direcci�n horizontal (-1, 0, 1)
                int directionY = Random.Range(-1, 2); // Direcci�n vertical (-1, 0, 1)

                if (directionX == 0 && directionY == 0) continue; // Evita direcciones inv�lidas

                if (CanPlaceWord(word, x, y, directionX, directionY))
                {
                    PlaceWord(word, x, y, directionX, directionY);
                    placed = true;
                }
            }
        }
    }

    // Verifica si una palabra puede colocarse en una posici�n y direcci�n espec�ficas
    private bool CanPlaceWord(string word, int x, int y, int directionX, int directionY)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int newX = x + i * directionX;
            int newY = y + i * directionY;

            // Verifica l�mites de la cuadr�cula
            if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize)
                return false;

            // Verifica si la celda est� ocupada por otra palabra
            if (grid[newX, newY] != '\0' && grid[newX, newY] != word[i])
                return false;
        }
        return true;
    }

    // Coloca la palabra en la cuadr�cula
    private void PlaceWord(string word, int x, int y, int directionX, int directionY)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int newX = x + i * directionX;
            int newY = y + i * directionY;

            grid[newX, newY] = word[i];
        }
    }

    // Rellena los espacios vac�os con letras aleatorias
    private void FillEmptySpaces()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (grid[x, y] == '\0')
                {
                    grid[x, y] = GetRandomLetter();
                }
            }
        }
    }

    // Genera una letra aleatoria (A-Z)
    private char GetRandomLetter()
    {
        return (char)Random.Range('A', 'Z' + 1);
    }

    // Muestra la cuadr�cula en el Canvas utilizando el prefab de casillas
    private void DisplayGrid()
    {
        if (cellPrefab == null || gridParent == null)
        {
            Debug.LogError("�Asigna el prefab de la casilla y el objeto padre en el Inspector!");
            return;
        }

        GridLayoutGroup gridLayout = gridParent.GetComponent<GridLayoutGroup>();
        if (gridLayout == null)
        {
            Debug.LogError("�El objeto padre debe tener un componente GridLayoutGroup!");
            return;
        }

        // Ajusta el tama�o de las celdas  
        float cellSize = 50f; // Aseg�rate de que el tama�o se ajuste a tus necesidades  
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        // Genera las casillas en el Canvas  
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                GameObject cell = Instantiate(cellPrefab, gridParent);
                TextMeshProUGUI cellText = cell.GetComponentInChildren<TextMeshProUGUI>();
                CellData cellData = cell.GetComponent<CellData>(); // Aseg�rate de que tu prefab tenga este componente  

                if (cellText == null || cellData == null)
                {
                    Debug.LogError("�El prefab de la casilla debe tener componentes TextMeshProUGUI y CellData!");
                    return;
                }

                // Asigna letras y coordenadas  
                cellText.text = grid[x, y].ToString();
                cellData.SetCoordinates(x, y);
            }
        }
    }
}