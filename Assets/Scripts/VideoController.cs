using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.Events;

public class VideoController : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    [SerializeField] private VideoAspectRatio _aspectRatio;
    [SerializeField] private Camera _mainCamera;

    public UnityEvent VideoStart;
    public UnityEvent VideoEnd;

    public float Distance = 50f;
    public float Height = 50f;
    
    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.targetMaterialProperty = "_BaseMap";
    }
    
    void Start()
    {
        AlignPosition();
        _videoPlayer.aspectRatio = _aspectRatio;
        if (!_videoPlayer)
        {
            _videoPlayer.loopPointReached += OnVideoEnd;
        }
    }

    public void Play()
    {
        StartCoroutine(PrepareVideo());
    }

    //Centers and rotates the video player to face towards the camera.
    public void AlignPosition()
    {
        var direction = _mainCamera.transform.forward;
        direction.y = 0;
        
        transform.position = _mainCamera.transform.position + (direction.normalized * Distance) + (Vector3.up * Height);
        transform.parent.forward = (_mainCamera.transform.position - transform.position).normalized;
    }

    //Called by UI slider to update vertical position.
    public void UpdateVerticalPosition(float value)
    {
        var relative = (Mathf.Clamp(value, 0f, 1f)/1f) * Height;
        var newPos = transform.position;
        newPos.y = _mainCamera.transform.position.y;
        newPos += (Vector3.up * relative);

        transform.position = newPos;
    }

    private IEnumerator PrepareVideo()
    {
        _videoPlayer.Prepare();
        while (!_videoPlayer.isPrepared)
        {
            yield return null;
        }
        VideoStart?.Invoke();
        _videoPlayer.Play();
    }

    private void OnVideoEnd(VideoPlayer videoPlayer)
    {
        VideoEnd?.Invoke();
        videoPlayer.Stop();
    }

    private void OnDestroy()
    {
        if (!_videoPlayer)
        {
            _videoPlayer.loopPointReached -= OnVideoEnd;
        }
    }
}
