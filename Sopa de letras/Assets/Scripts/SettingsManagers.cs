using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
using Unity.Mathematics;
using System;

public class SettingsManager : MonoBehaviour
{
    //volumen
    [SerializeField] private AudioMixer Mixer;
    [SerializeField] private Slider musica;
    [SerializeField] private TextMeshProUGUI contador;

    //idioma
    public TMP_Dropdown dropdownIdioma; // Referencia al Dropdown
    private SopaDeLetrasGenerator generadorSopa; // Referencia al generador

    private void Awake()
    {
        musica.onValueChanged.AddListener(controlMusisca);

        // Buscar el generador de sopa de letras
        generadorSopa = FindObjectOfType<SopaDeLetrasGenerator>();

        // Configurar el dropdown de idioma
        if (dropdownIdioma != null)
        {
            // Asegurarse que el dropdown tenga las opciones correctas
            dropdownIdioma.ClearOptions();
            dropdownIdioma.AddOptions(new System.Collections.Generic.List<string> { "Español", "Inglés" });

            // Añadir listener para el cambio de idioma
            dropdownIdioma.onValueChanged.AddListener(CambiarIdioma);

            // Cargar el idioma guardado (si existe)
            int idiomaGuardado = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);
            dropdownIdioma.value = idiomaGuardado;
        }
    }
 
    private void Start()
    {
        cargar();
    }

    private void cargar()
    {
        musica.value = PlayerPrefs.GetFloat("MusicGroup", 0.75f);
        controlMusisca(musica.value);
    }

    private void controlMusisca(float valor)
    {
        Mixer.SetFloat("MusicGroup", math.log10(valor)*20);
        PlayerPrefs.SetFloat("MusicGroup", musica.value);

        // Actualizar el texto con el porcentaje
        contador.text = Mathf.RoundToInt(valor * 100) + "%";
    }

    public void CambiarIdioma(int idiomaIndex)
    {
        // Lo más importante: Guardar la selección para que la otra escena pueda acceder
        PlayerPrefs.SetInt("IdiomaSeleccionado", idiomaIndex);
        PlayerPrefs.Save();

        Debug.Log("Idioma guardado: " + (idiomaIndex == 0 ? "Español" : "Inglés"));
    }
}