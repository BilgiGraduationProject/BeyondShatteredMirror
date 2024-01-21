using System;
using UnityEngine;
using UnityEngine.Video;

namespace Runtime.Data.ValueObject
{
    [Serializable]
    public struct CutsceneData
    {
        public string videoName;
        public string videoPath;
        
        public void LoadVideoClip(VideoPlayer videoPlayer)
        {
            string fullPath = System.IO.Path.Combine(Application.streamingAssetsPath, videoPath);
            videoPlayer.url = fullPath;
            videoPlayer.Prepare();
        }
    }
}
