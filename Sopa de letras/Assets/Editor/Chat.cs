using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ChatGpt))]
public class Chat : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        ChatGpt script = (ChatGpt)target;

       GUILayout.Space(15);

        if (GUILayout.Button("enviar"))
        {
            script.Pregunta();
        }
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Guardar script"))
        {
            script.SaveScript();
        }
        if (GUILayout.Button("clear"))
        {
            script.Clear();
        }
        
        GUILayout.EndHorizontal();
    }
}
