using UnityEngine;
using System.Collections.Generic;

public class CrearListaDePalabras : MonoBehaviour
{
    public enum Difficulty { Easy, Medium, Hard }
    public Difficulty difficulty = Difficulty.Easy;

    public enum Language { Spanish, English }
    public Language language = Language.Spanish; // Variable para escoger el idioma

    private List<string> englishWords = new List<string>
    {
        "APPLE", "BEACH", "BRAIN", "BREAD", "BRICK", "CHAIR", "CLOCK", "CLOUD", "GHOST", "GRAPE",
        "HONEY", "HOUSE", "LEMON", "MONEY", "PAPER", "PEARL", "PHONE", "PIZZA", "PLANE", "PLANT"
    };

    private List<string> spanishWords = new List<string>
    {
        "MANZANA", "PLAYA", "CEREBRO", "PAN", "LADRILLO", "SILLA", "RELOJ", "NUBE", "FANTASMA", "UVA",
        "MIEL", "CASA", "LIMÓN", "DINERO", "PAPEL", "PERLA", "TELÉFONO", "PIZZA", "AVIÓN", "PLANTA"
    };

    public List<string> GenerateWords(Difficulty difficulty, Language language, int gridSize)
    {
        int wordCount = 0;
        switch (difficulty)
        {
            case Difficulty.Easy:
                wordCount = 5;
                break;
            case Difficulty.Medium:
                wordCount = 7;
                break;
            case Difficulty.Hard:
                wordCount = 10;
                break;
        }

        List<string> selectedWords = new List<string>();
        List<string> wordList = (language == Language.English) ? englishWords : spanishWords;

        // Filtra las palabras que no excedan el tamaño de la cuadrícula
        wordList = wordList.FindAll(word => word.Length <= gridSize);

        for (int i = 0; i < wordCount; i++)
        {
            if (wordList.Count == 0) break; // Si no hay palabras válidas, salir
            selectedWords.Add(wordList[Random.Range(0, wordList.Count)]);
        }

        // Debug para verificar las palabras generadas
        Debug.Log("Palabras generadas: " + string.Join(", ", selectedWords));

        return selectedWords;
    }

    // Método para pasar la lista de palabras a Sopadeletras
    public void SendWordsToSopaDeLetras(Sopadeletras sopadeletras, int gridSize)
    {
        if (sopadeletras != null)
        {
            // Generar las palabras
            List<string> words = GenerateWords(difficulty, language, gridSize);

            // Pasar las palabras a Sopadeletras
            sopadeletras.SetWords(words);
        }
        else
        {
            Debug.LogError("Sopadeletras no está asignado en el Inspector.");
        }
    }

    internal List<string> GenerateWords()
    {
        throw new System.NotImplementedException();
    }
}