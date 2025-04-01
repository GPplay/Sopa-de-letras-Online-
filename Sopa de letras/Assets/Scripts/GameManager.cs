using Ricimi;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textoContador;
    [SerializeField] private TextMeshProUGUI palabras1;
    [SerializeField] private TextMeshProUGUI palabras2;
    [SerializeField] private GameObject botonPausa;
    [SerializeField] private GameObject popupPrefab;
    [SerializeField] private GameObject popupWin;
    [SerializeField] private TextMeshProUGUI textoTiempoWin;

    private int numeroPalabrasEncontradas = 0;
    private int segundosTotales = 0;
    private float tiempoAcumulado = 0f;
    private List<string> palabrasEncontradas = new List<string>();
    private bool juegoPausado = false;
    private Canvas m_canvas;

    private static GameManager instance;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        popupPrefab.SetActive(false);
        popupWin.SetActive(false);
        m_canvas = FindObjectOfType<Canvas>();
        if (m_canvas == null)
        {
            Debug.LogError("No se encontró un Canvas en la escena.");
        }
        ActualizarTexto();
        ActualizarPalabrasUI();
    }

    void Update()
    {
        if (juegoPausado) return;

        tiempoAcumulado += Time.deltaTime;
        if (tiempoAcumulado >= 1f)
        {
            segundosTotales++;
            ActualizarTexto();
            tiempoAcumulado -= 1f;
        }
    }

    public void ActualizarPalabrasUI()
    {
        if (SopaDeLetrasGenerator.Instance == null)
        {
            Debug.LogError("Generador de sopa de letras no inicializado!");
            return;
        }
        var palabras = SopaDeLetrasGenerator.Instance.palabrasActuales;
        int mitad = Mathf.CeilToInt(palabras.Count / 2f);
        palabras1.text = FormatearLista(palabras.Take(mitad).ToList());
        palabras2.text = FormatearLista(palabras.Skip(mitad).ToList());
    }

    private string FormatearLista(List<string> palabras)
    {
        return string.Join("\n", palabras.Select(p => $"{p}{(palabrasEncontradas.Contains(p) ? " (E)" : "")}"));
    }

    public void MarcarPalabraEncontrada(string palabra)
    {
        if (!palabrasEncontradas.Contains(palabra))
        {
            palabrasEncontradas.Add(palabra);
            numeroPalabrasEncontradas++;
            ActualizarPalabrasUI();
            VerificarVictoria();
        }
    }

    public void ActualizarTexto()
    {
        int minutos = segundosTotales / 60;
        int segundos = segundosTotales % 60;
        textoContador.text = $"Tiempo: {minutos:00}:{segundos:00}";
    }

    public void PausarJuego()
    {
        botonPausa.SetActive(false);
        if (!juegoPausado)
        {
            StartCoroutine(AnimarBotonPausaYPausar());
        }
    }

    private IEnumerator AnimarBotonPausaYPausar()
    {
        Animator botonAnimator = botonPausa.GetComponent<Animator>();
        if (botonAnimator != null)
        {
            botonAnimator.SetTrigger("PausarAnimacion");
            yield return new WaitForSeconds(botonAnimator.GetCurrentAnimatorStateInfo(0).length);
        }
        juegoPausado = true;
        OpenPopup(popupPrefab);
        yield return new WaitForSeconds(1.0f);
    }

    public void OpenPopup(GameObject popup)
    {
        if (popup == null)
        {
            Debug.LogError("popupPrefab no está asignado.");
            return;
        }
        GameObject instanciaPopup = Instantiate(popup, m_canvas.transform, false);
        instanciaPopup.SetActive(true);
        instanciaPopup.transform.localScale = Vector3.zero;
        Popup popupScript = instanciaPopup.GetComponent<Popup>();
        if (popupScript != null)
        {
            popupScript.Open();
        }
        else
        {
            Debug.LogError("El prefab popupPrefab no tiene el script Popup.");
        }
    }

    private IEnumerator AnimarWin()
    {
        // Actualizar el tiempo en el menú de victoria
        ActualizarTiempoWin();

        // Animar el popupWin
        Animator botonAnimator = popupWin.GetComponent<Animator>();
        if (botonAnimator != null)
        {
            botonAnimator.SetTrigger("PausarAnimacion");
            yield return new WaitForSeconds(botonAnimator.GetCurrentAnimatorStateInfo(0).length);
        }

        juegoPausado = true;
        OpenPopup(popupWin);
        yield return new WaitForSeconds(1.0f);
    }

    public void ReanudarJuego()
    {
        juegoPausado = false;
        botonPausa.SetActive(true);
    }

    private void VerificarVictoria()
    {
        if (numeroPalabrasEncontradas >= SopaDeLetrasGenerator.Instance.cantidadPalabrasParaJuego)
        {
            StartCoroutine(AnimarWin());
            juegoPausado = true;
        }
    }

    public void ReiniciarJuego()
    {
        numeroPalabrasEncontradas = 0;
        segundosTotales = 0;
        tiempoAcumulado = 0f;
        palabrasEncontradas.Clear();

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Esperar un frame y regenerar la sopa de letras
        StartCoroutine(EsperarYGenerarSopa());
    }

    private IEnumerator EsperarYGenerarSopa()
    {
        yield return null; // Esperar un frame para que la escena se cargue completamente

        if (SopaDeLetrasGenerator.Instance != null)
        {
            Debug.Log("Generando nueva sopa de letras...");
            SopaDeLetrasGenerator.Instance.GenerarNuevoNivel();
        }
        else
        {
            Debug.LogError("SopaDeLetrasGenerator no encontrado después de recargar la escena.");
        }
    }

    private void ActualizarTiempoWin()
    {
        if (textoTiempoWin != null)
        {
            int minutos = segundosTotales / 60;
            int segundos = segundosTotales % 60;
            textoTiempoWin.text = $"{minutos:00}:{segundos:00}"; // Formato solo con minutos y segundos
        }
        else
        {
            Debug.LogError("textoTiempoWin no está asignado en el Inspector.");
        }
    }
}