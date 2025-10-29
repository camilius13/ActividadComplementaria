using UnityEngine;
using UnityEngine.Video;

public class videoFondo : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.isLooping = true;
        videoPlayer.Play();
    }

    void Update()
    {
        if (!videoPlayer.isPlaying)
            videoPlayer.Play();
    }
}
