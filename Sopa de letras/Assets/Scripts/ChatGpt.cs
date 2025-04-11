using UnityEngine;
using UnityEngine.Networking;
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class ChatGpt : MonoBehaviour
{
    [SerializeField] private string apiKey;

    [TextArea(3, 10)]
    [SerializeField] private string prompt;

    [TextArea(3, 40)]
    [SerializeField] private string result;

    private readonly string chatGptUrlAPI = "https://api.openai.com/v1/completions";
    private readonly string scriptFolder = "Assets/Scripts";
    private readonly string directory = "ChatGPT";

    public void Pregunta()
    {
        Clear(); // Limpia el resultado antes de hacer una nueva consulta
        StartCoroutine(SendPreguntaAPI());
    }

    private IEnumerator SendPreguntaAPI()
    {
        // Construir el JSON siguiendo el formato de curl
        string jsonData = "{\"model\": \"gpt-3.5-turbo-instruct\", \"prompt\": \"" + prompt + "\", \"max_tokens\": 7, \"temperature\": 0}";

        using UnityWebRequest requestChatGPT = new UnityWebRequest(chatGptUrlAPI, "POST");
        requestChatGPT.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(jsonData));
        requestChatGPT.downloadHandler = new DownloadHandlerBuffer();

        requestChatGPT.SetRequestHeader("Content-Type", "application/json");
        requestChatGPT.SetRequestHeader("Authorization", "Bearer " + apiKey);

        result = "Cargando...";
        yield return requestChatGPT.SendWebRequest();

        if (requestChatGPT.result == UnityWebRequest.Result.Success)
        {
            string responseText = requestChatGPT.downloadHandler.text;
            Debug.Log("Respuesta de OpenAI: " + responseText);

            ResponseBodyChatGPT response = JsonUtility.FromJson<ResponseBodyChatGPT>(responseText);
            if (response != null && response.choices.Count > 0)
            {
                result = response.choices[0].text.Trim();
            }
            else
            {
                result = "Respuesta vacía de la API.";
            }
        }
        else
        {
            Debug.LogError("Error: " + requestChatGPT.error + " - " + requestChatGPT.downloadHandler.text);
            result = "Error: " + requestChatGPT.error;
        }

        // Esperar para evitar el error 429 (Too Many Requests)
        yield return new WaitForSeconds(10f);
    }

    public void SaveScript()
    {
        if (!Directory.Exists(scriptFolder + "/" + directory))
        {
            Directory.CreateDirectory(scriptFolder + "/" + directory);
        }

        string className = ParseClassName(result);
        string scriptPath = scriptFolder + "/" + className + ".cs";

        using FileStream fs = new FileStream(scriptPath, FileMode.Create);
        using StreamWriter writer = new StreamWriter(fs);
        writer.Write(result);
    }

    private string ParseClassName(string result)
    {
        int indexClass = result.IndexOf("class", StringComparison.Ordinal);
        int indexDots = result.IndexOf(":", StringComparison.Ordinal);
        if (indexClass == -1 || indexDots == -1 || indexDots <= indexClass) return "GeneratedClass";
        return result.Substring(indexClass + 6, indexDots - indexClass - 6).Trim();
    }

    public void Clear()
    {
        result = string.Empty;
    }
}

[Serializable]
public class ResponseBodyChatGPT
{
    public List<Choice> choices;

    [Serializable]
    public class Choice
    {
        public string text;
    }
}
