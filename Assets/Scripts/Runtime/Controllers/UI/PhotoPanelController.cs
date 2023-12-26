using System.Collections.Generic;
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
        private List<Texture2D> _photoTextureList = new List<Texture2D>();
        
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
            _photoTextureArray = Resources.LoadAll<Texture2D>("CapturePhotos");

            foreach (var photo in _photoTextureArray)
            {
                _photoTextureList.Add(photo);
            }
        }

        private void CreatePhotoObjects()
        {
            foreach (var photo in _photoTextureList)
            {
                GameObject photoObject = Instantiate(photoObjectPrefab, photoObjectParent.transform);
                //photoObject.GetComponent<Image>().sprite = photo;
                photoObject.transform.GetChild(0).transform.GetChild(0).GetComponent<RawImage>().texture = photo;
            }
        }
    }
}