using UnityEngine;
using UnityEngine.Video; // Necesario para usar VideoPlayer
using System.Collections.Generic; // Para usar Dictionary

/// <summary>
/// Gestiona la reproducción de videos específicos por ID en un televisor 3D.
/// Se encarga de asignar el VideoClip y controlar el VideoPlayer.
/// </summary>
public class VideoPlaybackManager : MonoBehaviour
{
    // Singleton
    public static VideoPlaybackManager Instance { get; private set; }

    [Header("Componentes del Televisor 3D")]
    [Tooltip("El componente VideoPlayer de Unity (puede estar en este mismo objeto).")]
    public VideoPlayer videoPlayer;

    [Tooltip("El material del televisor donde se proyectará el video.")]
    public Renderer tvRenderer;

    [Tooltip("La Render Texture que el VideoPlayer usará como objetivo y que se asignará al material del televisor.")]
    public RenderTexture renderTexture;

    [Header("Canvas y Paneles")]
    [Tooltip("Panel de Canvas que muestra un mensaje de 'reproduciendo' o un botón de salida (opcional).")]
    public GameObject videoPanel;

    [System.Serializable]
    public struct CassetteVideo
    {
        public int id;
        public VideoClip clip;
    }

    [Header("Clips de Video")]
    [Tooltip("Arrastra aquí los VideoClips y asigna sus IDs (1, 2, 3, 4).")]
    public List<CassetteVideo> videoClipsList = new List<CassetteVideo>();

    // Diccionario para búsqueda rápida de clips por ID
    private Dictionary<int, VideoClip> videoClipsMap = new Dictionary<int, VideoClip>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // 1. Inicializar el diccionario para búsqueda rápida
        foreach (var item in videoClipsList)
        {
            if (item.clip != null && !videoClipsMap.ContainsKey(item.id))
            {
                videoClipsMap.Add(item.id, item.clip);
            }
        }

        // 2. Configurar la salida del VideoPlayer al Renderer
        if (videoPlayer != null && renderTexture != null && tvRenderer != null)
        {
            // La salida del video va a la Render Texture
            videoPlayer.targetTexture = renderTexture;

            // Asignar la Render Texture al material del televisor para mostrar el video
            // Asumiendo que el material tiene una propiedad _MainTex para la textura principal
            tvRenderer.material.mainTexture = renderTexture;
        }
        else
        {
            Debug.LogError("Faltan referencias en el VideoPlaybackManager (VideoPlayer, RenderTexture o Renderer).");
        }

        // 3. Suscribirse al evento de finalización del video
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Start()
    {
        // Asegurarse de que el panel de control (si existe) esté oculto al inicio
        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
        }

        // Opcional: Pausar el VideoPlayer al inicio
        if (videoPlayer != null) videoPlayer.Stop();
    }

    // Método que se suscribe al evento PrepareCompleted
    private void OnVideoPrepared(VideoPlayer vp, int cassetteId)
    {
        // Asegurarse de eliminar el listener inmediatamente después de que se ejecute una vez
        // Esto evita que el método se ejecute para el siguiente video (si no se elimina manualmente)
        vp.prepareCompleted -= (source) => OnVideoPrepared(source, cassetteId);

        // Pausar el tiempo del juego mientras el video se reproduce
        Time.timeScale = 0f;

        //// --- MOSTRAR CURSOR ---
        //if (GameSceneLocalManager.Instance != null)
        //{
        //    GameSceneLocalManager.Instance.SetCursorState(true);
        //}

        vp.Play();
        Debug.Log($"Reproduciendo video para el Cassette ID: {cassetteId} en el televisor.");
    }


    /// <summary>
    /// Intenta reproducir un video dado su ID de cassette.
    /// </summary>
    /// <param name="id">El ID del cassette (1, 2, 3 o 4).</param>
    public void PlayVideoByID(int id)
    {
        if (videoPlayer == null) return;

        if (videoClipsMap.TryGetValue(id, out VideoClip clip))
        {
            // 1. Asignar el nuevo clip
            videoPlayer.clip = clip;

            // 2. Opcional: Mostrar el panel de control o mensaje (Canvas 3)
            if (videoPanel != null)
            {
                videoPanel.SetActive(true);
            }

            // 3. Suscribirse al evento de preparación. 
            // Usamos una sintaxis que permite pasar el ID del cassette para el log y la lógica.
            videoPlayer.prepareCompleted += (vp) => OnVideoPrepared(vp, id);

            // 4. Iniciar la preparación del video
            videoPlayer.Prepare();
        }
        else
        {
            Debug.LogError($"VideoClip no encontrado para el Cassette ID: {id}");
        }
    }

    /// <summary>
    /// Se llama cuando el video termina de reproducirse.
    /// </summary>
    private void OnVideoEnd(VideoPlayer vp)
    {
        // 1. Ocultar el panel de control/mensaje
        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
        }

        // 2. Reanudar el tiempo del juego
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        // --- OCULTAR CURSOR ---
        // Devolver el control al juego (cursor oculto y bloqueado).
        //if (GameSceneLocalManager.Instance != null)
        //{
        //    GameSceneLocalManager.Instance.SetCursorState(false);
        //}

        Debug.Log("Reproducción de video en televisor finalizada.");
    }
}