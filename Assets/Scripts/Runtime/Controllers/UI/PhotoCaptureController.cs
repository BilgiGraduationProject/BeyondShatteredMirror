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
                StopCoroutine(CaptureShot());
                RemovePhoto();
                CaptureTime();
            }
            
            if (Input.GetMouseButtonDown(0) && !_viewingPhoto)
            {
                StartCoroutine(CaptureShot());
            }

            if (Input.GetKeyDown(KeyCode.I))
            {
                CoreUISignals.Instance.onOpenPanel?.Invoke(UIPanelTypes.Inventory, 1);
            }
        }

        private void CaptureTime()
        {
            CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.Capture);
            cameraViewfinder.SetActive(true);
            //CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StopPlayer);
        }

        private void UnCaptureTime()
        {
            CameraSignals.Instance.onChangeCameraState?.Invoke(CameraStateEnum.Play);
            _photoSprite = null;
            photoFrame.SetActive(false);
            CoreGameSignals.Instance.onGameStatusChanged?.Invoke(GameStateEnum.StartPlayer);
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
            photoDisplayArea.texture = _photoSprite.texture;
            photoFrame.SetActive(true);
        }
    
        void SavePhoto()
        {
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

        private void RemovePhoto()
        {
            _viewingPhoto = false;
            _photoSprite = null;
            photoFrame.SetActive(false);
        }
    }
}
