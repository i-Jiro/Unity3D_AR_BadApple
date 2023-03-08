using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    [SerializeField] private VideoAspectRatio _aspectRatio;
    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.targetMaterialProperty = "_BaseMap";
    }
    
    void Start()
    {
        _videoPlayer.aspectRatio = _aspectRatio;
        StartCoroutine(PrepareVideo());
    }

    private IEnumerator PrepareVideo()
    {
        _videoPlayer.Prepare();
        while (!_videoPlayer.isPrepared)
        {
            yield return null;
        }
        _videoPlayer.Play();
    }
}
