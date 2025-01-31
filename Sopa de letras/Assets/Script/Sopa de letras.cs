using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Sopadeletras : MonoBehaviour
{
    public int gridSize = 10; // Tamaño fijo de la cuadrícula (10x10)
    public List<string> words = new List<string>(); // Lista de palabras
    public GameObject cellPrefab; // Prefab de la celda
    public Transform gridParent; // Panel que contiene la cuadrícula
    public CrearListaDePalabras lista;

    private char[,] grid;


    void Start()
    {
        // Verificar si hay palabras asignadas
        if (words.Count == 0)
        {
            Debug.LogError("No se han asignado palabras. Asegúrate de llamar a SetWords antes de Start.");
            return;
        }

        GenerateGrid();
        PlaceWords();
        FillEmptySpaces();
        DisplayGrid();
    }

    // Método para recibir la lista de palabras
    public void SetWords(List<string> newWords)
    {
        words = newWords;
    }

    public void GenerateGrid()
    {
        grid = new char[gridSize, gridSize];
    }

    public void PlaceWords()
    {
        foreach (string word in words)
        {
            bool placed = false;
            while (!placed)
            {
                int x = Random.Range(0, gridSize);
                int y = Random.Range(0, gridSize);
                int directionX = Random.Range(-1, 2);
                int directionY = Random.Range(-1, 2);

                if (directionX == 0 && directionY == 0) continue;

                if (CanPlaceWord(word, x, y, directionX, directionY))
                {
                    PlaceWord(word, x, y, directionX, directionY);
                    placed = true;
                }
            }
        }
    }

    public bool CanPlaceWord(string word, int x, int y, int directionX, int directionY)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int newX = x + i * directionX;
            int newY = y + i * directionY;

            if (newX < 0 || newX >= gridSize || newY < 0 || newY >= gridSize)
                return false;

            if (grid[newX, newY] != '\0' && grid[newX, newY] != word[i])
                return false;
        }
        return true;
    }

    public void PlaceWord(string word, int x, int y, int directionX, int directionY)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int newX = x + i * directionX;
            int newY = y + i * directionY;

            grid[newX, newY] = word[i];
        }
    }

    public void FillEmptySpaces()
    {
        for (int x = 0; x < gridSize; x++)
        {
            for (int y = 0; y < gridSize; y++)
            {
                if (grid[x, y] == '\0')
                {
                    grid[x, y] = (char)Random.Range('A', 'Z' + 1);
                }
            }
        }
    }

    public void DisplayGrid()
    {
        // Tamaño fijo de las celdas (ajusta este valor según sea necesario)
        float cellSize = 100f;

        // Ajustar el tamaño del Grid Layout Group
        GridLayoutGroup gridLayout = gridParent.GetComponent<GridLayoutGroup>();
        gridLayout.cellSize = new Vector2(cellSize, cellSize);

        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                // Instancia la celda y la asigna al gridParent
                GameObject cell = Instantiate(cellPrefab, gridParent);

                // Asignar coordenadas a la celda
                CellData cellData = cell.GetComponent<CellData>();
                if (cellData == null)
                {
                    cellData = cell.AddComponent<CellData>();
                }
                cellData.SetCoordinates(x, y);

                // Configurar el texto de la celda
                TextMeshProUGUI text = cell.GetComponentInChildren<TextMeshProUGUI>();
                text.text = grid[x, y].ToString();

                // Ajustar el tamaño de la fuente para que quepa en la celda
                text.fontSize = (int)(cellSize * 0.5f); // Ajusta este valor según sea necesario
                text.alignment = TextAlignmentOptions.Center; // Centrar el texto
            }
        }
    }
}