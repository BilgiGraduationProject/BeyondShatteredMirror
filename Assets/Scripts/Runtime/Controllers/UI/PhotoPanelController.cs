using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Runtime.Controllers.UI
{
    public class PhotoPanelController : MonoBehaviour
    {
        #region Self Variables
        
        #region Serialized Variables

        [SerializeField] private GameObject photoObjectParent;
        [SerializeField] private GameObject photoObjectPrefab;
        
        #endregion
        
        #region Private Variables

        private Texture2D[] _photoTextureArray;
        private Sprite[] _photoSpriteArray;
        private List<Sprite> _photoSpriteList = new List<Sprite>();
        
        #endregion

        #endregion
        
        private void Awake()
        {
            //if(_photoSpriteList.Count > 0) _photoSpriteList.Clear();
            AddPhotosToPanel();
        }

        private void Start()
        {
            CreatePhotoObjects();
        }

        private void AddPhotosToPanel()
        {
            _photoSpriteArray = Resources.LoadAll<Sprite>("CapturePhotos");

            foreach (var photo in _photoSpriteArray)
            {
                _photoSpriteList.Add(photo);
            }
        }

        private void CreatePhotoObjects()
        {
            foreach (var photo in _photoSpriteList)
            {
                GameObject photoObject = Instantiate(photoObjectPrefab, photoObjectParent.transform);
                //photoObject.GetComponent<Image>().sprite = photo;
                photoObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>().sprite = photo;
            }
        }
    }
}