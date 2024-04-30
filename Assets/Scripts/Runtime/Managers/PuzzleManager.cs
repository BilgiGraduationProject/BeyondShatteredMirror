using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Enums.Puzzle;
using Runtime.Keys.Input;
using Runtime.Signals;
using Unity.VisualScripting;
using UnityEngine;

namespace Runtime.Managers
{
    public class PuzzleManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PuzzleEnum puzzleEnum;
        private int pieces;
        

        #endregion


        #region Private Variables


        private CD_Puzzle puzzleData;
       [SerializeField] private List<GameObject> puzzlePieces = new List<GameObject>();
       private PuzzleParams puzzleParams;

        #endregion

        #endregion


        private void Awake()
        {
            puzzleData = GetPuzzleData();
        }

       

        private CD_Puzzle GetPuzzleData() => Resources.Load<CD_Puzzle>("Data/CD_Puzzle");


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PuzzleSignals.Instance.onChangePuzzleColor += OnChangePuzzleColor;
            PuzzleSignals.Instance.onInteractWithPuzzlePieces += OnInteractWithPuzzlePieces;
        }

        private void OnInteractWithPuzzlePieces(GameObject intereact, GameObject puzzlePieces)
        {
            if (puzzlePieces is null) return;
            switch (puzzleEnum)
            {
                case PuzzleEnum.PictureTable:
                   
                    if (intereact.CompareTag(puzzlePieces.tag))
                    {
                        puzzlePieces.transform.parent = null;
                        puzzlePieces.transform.position = intereact.transform.position;
                        puzzlePieces.transform.rotation = intereact.transform.rotation;
                        puzzlePieces.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        puzzlePieces.layer = 0;
                        intereact.layer = 0;
                        puzzleParams.tablePictureCount++;
                    }

                    if (puzzleParams.tablePictureCount == 2)
                    {
                        Debug.LogWarning("Completed");
                    }
                    break;
                
            }
        }

       

        private void OnChangePuzzleColor(GameObject obj, bool condition)
        {
            if (condition)
            {
               
                obj.GetComponent<MeshRenderer>().material.DOFade(99f, "_BaseColor", 1f);
            }
            else
            {
                obj.GetComponent<MeshRenderer>().material.DOFade(0f, "_BaseColor", 1f);
            }
        }

        private void UnSubscribeEvents()
        {
            PuzzleSignals.Instance.onChangePuzzleColor -= OnChangePuzzleColor;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}