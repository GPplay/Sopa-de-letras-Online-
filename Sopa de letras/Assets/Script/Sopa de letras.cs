using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Sopadeletras : MonoBehaviour
{
    public int gridSize = 10; // Tamaño de la cuadrícula
    public List<string> words = new List<string> { "UNITY", "ANDROID", "GAME", "CODE", "FUN" }; // Lista de palabras
    private char[,] grid;
    public TextMeshProUGUI gridText; // Referencia al objeto TextMeshPro

    void Start()
    {
        GenerateGrid();
        PlaceWords();
        FillEmptySpaces();
        DisplayGrid();
    }

    void GenerateGrid()
    {
        grid = new char[gridSize, gridSize];
    }

    void PlaceWords()
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

    bool CanPlaceWord(string word, int x, int y, int directionX, int directionY)
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

    void PlaceWord(string word, int x, int y, int directionX, int directionY)
    {
        for (int i = 0; i < word.Length; i++)
        {
            int newX = x + i * directionX;
            int newY = y + i * directionY;

            grid[newX, newY] = word[i];
        }
    }

    void FillEmptySpaces()
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

    void DisplayGrid()
    {
        string gridString = "";
        for (int y = 0; y < gridSize; y++)
        {
            for (int x = 0; x < gridSize; x++)
            {
                gridString += grid[x, y] + " ";
            }
            gridString += "\n";
        }
        gridText.text = gridString; // Asigna el texto al objeto TextMeshPro
    }
}
