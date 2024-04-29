using System.Collections;
using System.IO;
using Runtime.Enums.Camera;
using Runtime.Enums.GameManager;
using Runtime.Enums.UI;
using Runtime.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class PhotoCaptureController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serializable Variables
        
        [Header("Photo Taker")]
        [SerializeField] private RawImage photoDisplayArea;
        [SerializeField] private GameObject photoFrame;
        [SerializeField] private GameObject cameraViewfinder;
        
        #endregion
        
        #region Private Variables
        
        private Texture2D _screenCapture;
        private Sprite _photoSprite;
        private string _fileName;
        private bool _viewingPhoto = true;
      

        #endregion

        #endregion
        
        private void Awake()
        {
            _screenCapture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
            photoFrame.SetActive(false);
        }
        
        private void Update()
        {
            // TODO: Daha sonrası için kamerayı o an açıp açamayacağını da kontrol et.
            if (Input.GetKeyDown(KeyCode.C) && _viewingPhoto)
            {
                if (CoreGameSignals.Instance.onGetGameState() is not GameStateEnum.Game)
                {
                    print("You can't take photo now. You are in the wrong state. \n You are in " + CoreGameSignals.Instance.onGetGameState() + " state.");
                    return;
                }
                StopCoroutine(CaptureShot());
                //StopCoroutine(CaptureScreenshotAndSave());
                RemovePhoto();
                CaptureTime();
            }
            else if (Input.GetKeyDown(KeyCode.C) && !_viewingPhoto || Input.GetKeyDown(KeyCode.Escape) && !_viewingPhoto)
            {
                if(CoreGameSignals.Instance.onGetGameState() is not GameStateEnum.Capture) return;
                print("Capture is stopped.");
                StopCoroutine(CaptureShot());
                RemovePhoto();
                UnCaptureTime();
            }
            
            if (Input.GetMouseButtonDown(0) && !_viewingPhoto)
            {
                StartCoroutine(CaptureShot());
                //StartCoroutine(CaptureScreenshotAndSave());
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Inventory, 1);
            }
        }

        private void CaptureTime()
        {
            cameraViewfinder.SetActive(true);
            CoreUISignals.Instance.onDisableAllPanels?.Invoke();
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Capture);
            //CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StopPlayer);
        }

        private void UnCaptureTime()
        {
            _photoSprite = null;
            _viewingPhoto = true;
            photoFrame.SetActive(false);
            cameraViewfinder.SetActive(false);
            CoreUISignals.Instance.onEnableAllPanels?.Invoke();
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.Game);
        }
        
        IEnumerator CaptureShot()
        {
            _viewingPhoto = true;
            cameraViewfinder.SetActive(false);
            yield return new WaitForEndOfFrame();
            Rect regionToRead = new Rect(0, 0, Screen.width, Screen.height);
            _screenCapture.ReadPixels(regionToRead, 0, 0);
            _screenCapture.Apply();
            ShowPhoto();
            yield return new WaitForSeconds(2f);
            UnCaptureTime();
        }

        void ShowPhoto()
        {
            _photoSprite = Sprite.Create(_screenCapture, new Rect(0, 0, _screenCapture.width, _screenCapture.height), new Vector2(0.5f, 0.5f), 100f);
            ScreenCapture.CaptureScreenshotAsTexture();
            SavePhoto();
            //SavePhotoAsync();
            photoDisplayArea.texture = _photoSprite.texture;
            photoFrame.SetActive(true);
        }
        
        void SavePhoto()
        {
            //_screenCapture = Resize(_screenCapture, _screenCapture.width / 3, _screenCapture.height / 3);
            
            byte[] bytes = _screenCapture.EncodeToPNG(); // Convert Texture2D to PNG byte array
            
            // Create a folder path inside Resources directory
            string folderPath = Path.Combine(Application.dataPath, "Resources/CapturePhotos");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            
            // Generate a unique file name using a timestamp
            string fileName = "/photo_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            _fileName = fileName.Substring(0, fileName.LastIndexOf("."));
            
            // Combine folder path and file name
            string filePath = Path.Combine(Application.dataPath, folderPath + fileName); // Path to save the image
            
            // Save the PNG byte array to the file
            File.WriteAllBytes(filePath, bytes); // Save the PNG byte array to a file

            // Refresh the assets to make sure it appears in the project window
            //UnityEditor.AssetDatabase.Refresh();
            
            print("Capture worked");
        }
        
        async void SavePhotoAsync()
        {
            // Resize the Texture2D before converting it to PNG
            _screenCapture = Resize(_screenCapture, _screenCapture.width / 3, _screenCapture.height / 3);

            byte[] bytes = _screenCapture.EncodeToPNG(); // Convert Texture2D to PNG byte array

            // Create a folder path inside Resources directory
            string folderPath = Path.Combine(Application.dataPath, "Resources/CapturePhotos");
            if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

            // Generate a unique file name using a timestamp
            string fileName = "/photo_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
            _fileName = fileName.Substring(0, fileName.LastIndexOf("."));

            // Combine folder path and file name
            string filePath = Path.Combine(Application.dataPath, folderPath + fileName); // Path to save the image

            // Save the PNG byte array to the file
            await using (FileStream sourceStream = new FileStream(filePath,
                       FileMode.OpenOrCreate, FileAccess.Write, FileShare.None,
                       bufferSize: 4096, useAsync: true))
            {
                await sourceStream.WriteAsync(bytes, 0, bytes.Length);
            };

            print("Capture worked");
        }
        
        Texture2D Resize(Texture2D source, int newWidth, int newHeight)
        {
            source.filterMode = FilterMode.Bilinear;
            RenderTexture rt = RenderTexture.GetTemporary(newWidth, newHeight);
            rt.filterMode = FilterMode.Bilinear;
            RenderTexture.active = rt;
            Graphics.Blit(source, rt);
            Texture2D nTex = new Texture2D(newWidth, newHeight);
            nTex.ReadPixels(new Rect(0, 0, newWidth, newHeight), 0, 0);
            nTex.Apply();
            RenderTexture.active = null;
            return nTex;
        }

        private void RemovePhoto()
        {
            _viewingPhoto = false;
            _photoSprite = null;
            photoFrame.SetActive(false);
        }
    }
}
