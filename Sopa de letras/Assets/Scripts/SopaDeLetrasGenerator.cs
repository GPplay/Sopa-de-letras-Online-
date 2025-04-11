using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Linq;

public class SopaDeLetrasGenerator : MonoBehaviour
{
    public static SopaDeLetrasGenerator Instance;


    [Header("Configuración")]
    public GameObject cellPrefab;
    public Transform gridParent;

    [Header("Idiomas")]
    public Idioma idiomaActual = Idioma.Español;
    public enum Idioma { Español, Inglés }

    [SerializeField]
    private List<string> palabrasEspanol = new List<string>() {
    "PAN", "CASA", "FLOR", "LUZ", "MESA", "SOL", "MAR", "CIELO", "TIERRA", "FUEGO",
    "AGUA", "VIENTO", "ARBOL", "PERRO", "GATO", "LIBRO", "MANO", "PIE", "OJOS", "BOCA",
    "NARIZ", "COCHE", "CALLE", "PLAZA", "CAMINO", "PUENTE", "BANCO", "PLATO", "VASO", "TENEDOR",
    "CUCHARA", "LLAVE", "PUERTA", "VENTANA", "TECHO", "SUELO", "PISO", "SILLA", "SOFA",
    "CAMA", "ALMOHADA", "MANTEL", "TOALLA", "JABON", "PELO", "UNA", "PIEL", "HUESO", "SANGRE",
    "CARNE", "HIELO", "NIEVE", "LLUVIA", "TRUENO", "RAYO", "NUBE", "ESTRELLA", "LUNA",
    "PLANETA", "BOSQUE", "RIO", "LAGO", "MONTURA", "VALLE", "PLAYA", "ARENA", "PIEDRA", "BARRO",
    "METAL", "ORO", "PLATA", "BRONCE", "HIERRO", "CARBON", "MADERA", "PAPEL", "TELA", "HILO",
    "AGUJA", "BOTON", "ZAPATO", "SOMBRERO", "GUANTE", "BUFANDA", "ABRIGO", "FALDA", "PANTALON", "CAMISA",
    "VESTIDO", "CHAQUETA", "RELOJ", "CADENA", "ANILLO", "PULSERA", "COLLAR", "BOLSO", "MALETA", "MOCHILA",
    "BOLIGRAFO", "LAPIZ", "GOMA", "REGLA", "LIBRETA", "CUADERNO", "MESA", "SILLA", "TABLERO", "TIZA",
    "CRAYON", "CERA", "PINCEL", "LENTE", "FOCO", "BOMBILLA", "RADIO", "TELEFONO", "CELULAR", "CARGADOR",
    "CABLE", "ENCHUFE", "BATERIA", "PANTALLA", "MOUSE", "TECLADO", "MONITOR", "ALTAVOZ", "CAMARA", "VIDEO",
    "FOTO", "MUSICA", "SONIDO", "MICROFONO", "PARLANTE", "CANTANTE", "ACTOR", "PELICULA", "TEATRO", "ESCENA",
    "CORTINA", "VENTILADOR", "AIRE", "CALOR", "FRIO", "ESTUFA", "LAVADORA", "SECADORA", "NEVERA", "HORNO",
    "FUEGOS", "SARTEN", "OLLAS", "TAPA", "RECIPIENTE", "TENEDOR", "CUCHARON", "CUCHILLO", "SABANA", "FRAZADA",
    "EDREDON", "ALFOMBRA", "ESTANTE", "MUEBLE", "ESPEJO", "CUBO", "CAJA", "BOTE", "BARRIL", "SACO",
    "PALETA", "CARAMELO", "GALLETA", "DULCE", "HELADO", "CHOCOLATE", "AZUCAR", "SAL", "PIMIENTA", "CANELA",
    "MIEL", "JUGO", "LECHE", "CAFE", "TE", "BEBIDA", "AGUACATE", "CEREZA", "UVA", "FRESA",
    "LIMON", "MANGO", "PLATANO", "COCO", "PERA", "NARANJA", "SANDIA", "MELON", "DURAZNO", "HIGO", "VALENTINA", "GYBRAM",
    "BRAYAN"
    };

    [SerializeField]
    private List<string> palabrasIngles = new List<string>() {
    "HOME", "DOG", "CAT", "BOOK", "HAND", "FOOT", "EYES", "MOUTH", "NOSE", "CAR",
    "STREET", "BRIDGE", "PLATE", "GLASS", "KEY", "DOOR", "WINDOW", "CHAIR", "TABLE", "BED",
    "PILLOW", "TOWEL", "SOAP", "HAIR", "SKIN", "BONE", "BLOOD", "MEAT", "ICE", "SNOW",
    "RAIN", "CLOUD", "STAR", "MOON", "PLANET", "FOREST", "RIVER", "LAKE", "BEACH", "STONE",
    "METAL", "GOLD", "SILVER", "WOOD", "PAPER", "SHOE", "HAT", "SHIRT", "DRESS", "WATCH",
    "BAG", "COIN", "FIRE", "WATER", "WIND", "EARTH", "TREE", "ROAD", "BRONZE", "IRON",
    "CLOTH", "NEEDLE", "BUTTON", "GLASSES", "COMPUTER", "PHONE", "SCREEN", "KEYBOARD", "MOUSE",
    "CAMERA", "MUSIC", "FILM", "LIGHT", "SOUND", "NIGHT", "DAY", "SUN", "MOUNTAIN", "VALLEY",
    "GRASS", "FLOWER", "BREAD", "MILK", "SUGAR", "SALT", "COFFEE", "TEA", "FRUIT", "APPLE",
    "PENCIL", "PEN", "ERASER", "RULER", "NOTEBOOK", "TABLET", "BOARD", "CHALK", "CRAYON", "PAINT",
    "BRUSH", "LENS", "BULB", "LAMP", "RADIO", "PHONE", "CHARGER", "CABLE", "BATTERY", "SCREEN",
    "MONITOR", "SPEAKER", "VIDEO", "PHOTO", "SONG", "MICROPHONE", "SINGER", "ACTOR", "MOVIE", "THEATER",
    "STAGE", "CURTAIN", "FAN", "HEATER", "COOLER", "STOVE", "WASHER", "DRYER", "FRIDGE", "OVEN",
    "FIREPLACE", "PAN", "POT", "LID", "DISH", "FORK", "LADLE", "KNIFE", "SHEET", "BLANKET",
    "DUVET", "RUG", "SHELF", "MIRROR", "BUCKET", "BOX", "CAN", "BAG", "BARREL", "SACK",
    "PALETTE", "CANDY", "COOKIE", "SWEET", "ICECREAM", "CHOCOLATE", "PEPPER", "CINNAMON", "HONEY", "JUICE",
    "AVOCADO", "CHERRY", "GRAPE", "STRAWBERRY", "LEMON", "MANGO", "BANANA", "COCONUT", "PEAR", "ORANGE"
    };

    [SerializeField] public int cantidadPalabrasParaJuego = 5;
    private const int GRID_SIZE = 12;
    private char[,] grid = new char[GRID_SIZE, GRID_SIZE];
    public List<string> palabrasActuales = new List<string>();

    private void Awake()
    {
        SettingsManager selector = FindObjectOfType<SettingsManager>();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);// Evita duplicados
            return;
        }
        Instance = this;

        int idiomaGuardado = PlayerPrefs.GetInt("IdiomaSeleccionado", 0);
        idiomaActual = (Idioma)idiomaGuardado;
    }

    void Start()
    {
        ConfigurarTamañoGridParent();
        GenerarNuevoNivel();
    }

    public void CambiarIdioma(int nuevoIdioma)
    {
        idiomaActual = (Idioma)nuevoIdioma;
        GenerarNuevoNivel();
    }

    private List<string> SeleccionarPalabrasAleatorias()
    {
        List<string> palabrasDisponibles = idiomaActual == Idioma.Español ?
            new List<string>(palabrasEspanol.Where(p => p.Length <= GRID_SIZE)) :
            new List<string>(palabrasIngles.Where(p => p.Length <= GRID_SIZE));

        List<string> palabrasSeleccionadas = new List<string>();
        int cantidad = Mathf.Min(cantidadPalabrasParaJuego, palabrasDisponibles.Count);

        for (int i = 0; i < cantidad; i++)
        {
            int index = Random.Range(0, palabrasDisponibles.Count);
            palabrasSeleccionadas.Add(palabrasDisponibles[index]);
            palabrasDisponibles.RemoveAt(index);
        }

        return palabrasSeleccionadas;
    }

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
        Debug.Log("Generando nuevo nivel...");
        foreach (Transform child in gridParent) Destroy(child.gameObject);
        palabrasActuales.Clear();
        InicializarGrid();

        List<string> palabrasParaNivel = SeleccionarPalabrasAleatorias();

        foreach (string palabra in palabrasParaNivel)
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

        LlenarEspaciosVacios();
        DibujarGrid();

        Debug.Log("Nueva sopa generada.");
    }

    private void InicializarGrid()
    {
        for (int x = 0; x < GRID_SIZE; x++)
            for (int y = 0; y < GRID_SIZE; y++)
                grid[x, y] = '\0';
    }

    private Vector2Int ObtenerDireccionAleatoriaValida(int longitudPalabra, int x, int y)
    {
        List<Vector2Int> direccionesValidas = new List<Vector2Int>();

        // Lista original de direcciones
        List<Vector2Int> direccionesPosibles = new List<Vector2Int>() {
        Vector2Int.right, Vector2Int.left, Vector2Int.up, Vector2Int.down,
        new Vector2Int(1, 1), new Vector2Int(-1, -1), new Vector2Int(1, -1), new Vector2Int(-1, 1)
        };

        // Filtrar direcciones válidas
        foreach (Vector2Int dir in direccionesPosibles)
        {
            int newX = x + (longitudPalabra - 1) * dir.x;
            int newY = y + (longitudPalabra - 1) * dir.y;
            if (newX >= 0 && newX < GRID_SIZE && newY >= 0 && newY < GRID_SIZE)
            {
                direccionesValidas.Add(dir);
            }
        }

        // Elegir aleatoriamente entre las válidas
        return (direccionesValidas.Count > 0)
            ? direccionesValidas[Random.Range(0, direccionesValidas.Count)]
            : Vector2Int.zero;
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