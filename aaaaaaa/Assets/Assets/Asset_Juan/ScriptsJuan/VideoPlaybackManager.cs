using UnityEngine;
using UnityEngine.Video; // Necesario para usar VideoPlayer
using System.Collections.Generic; // Para usar Dictionary


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

        foreach (var item in videoClipsList)
        {
            if (item.clip != null && !videoClipsMap.ContainsKey(item.id))
            {
                videoClipsMap.Add(item.id, item.clip);
            }
        }

        if (videoPlayer != null && renderTexture != null && tvRenderer != null)
        {
            videoPlayer.targetTexture = renderTexture;

            tvRenderer.material.mainTexture = renderTexture;
        }
        else
        {
            Debug.LogError("Faltan referencias en el VideoPlaybackManager (VideoPlayer, RenderTexture o Renderer).");
        }

        videoPlayer.loopPointReached += OnVideoEnd;
    }

    private void Start()
    {
        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
        }

        if (videoPlayer != null) videoPlayer.Stop();
    }

    private void OnVideoPrepared(VideoPlayer vp, int cassetteId)
    {
        vp.prepareCompleted -= (source) => OnVideoPrepared(source, cassetteId);

        Time.timeScale = 0f;


        vp.Play();
        Debug.Log($"Reproduciendo video para el Cassette ID: {cassetteId} en el televisor.");
    }


    public void PlayVideoByID(int id)
    {
        if (videoPlayer == null) return;

        if (videoClipsMap.TryGetValue(id, out VideoClip clip))
        {
            videoPlayer.clip = clip;

            if (videoPanel != null)
            {
                videoPanel.SetActive(true);
            }

            videoPlayer.prepareCompleted += (vp) => OnVideoPrepared(vp, id);

            videoPlayer.Prepare();
        }
        else
        {
            Debug.LogError($"VideoClip no encontrado para el Cassette ID: {id}");
        }
    }

  
    private void OnVideoEnd(VideoPlayer vp)
    {
        if (videoPanel != null)
        {
            videoPanel.SetActive(false);
        }

        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }


        Debug.Log("Reproducción de video en televisor finalizada.");
    }
}