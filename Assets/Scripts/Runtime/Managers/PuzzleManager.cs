using System;
using System.Collections.Generic;
using DG.Tweening;
using Runtime.Data.UnityObject;
using Runtime.Enums.Playable;
using Runtime.Enums.Puzzle;
using Runtime.Enums.UI;
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
        

        #endregion


        #region Private Variables
        
       private PuzzleParams puzzleParams;
       private GameObject lanternPuzzleHolder;

        #endregion

        #endregion

        


        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PuzzleSignals.Instance.onChangePuzzleColor += OnChangePuzzleColor;
            PuzzleSignals.Instance.onInteractWithPuzzlePieces += OnInteractWithPuzzlePieces;
            PuzzleSignals.Instance.onGetPuzzleCatEyeValues += OnGetPuzzleCatEye;
            PuzzleSignals.Instance.onGetPuzzleEnum += OnGetPuzzleEnum;
            EnemySignals.Instance.onDeathOfHakan += OnDeathOfHakan;
        }

        private void OnDeathOfHakan() {
            puzzleEnum = PuzzleEnum.PictureTable;
        }

        private PuzzleEnum OnGetPuzzleEnum()
        {
            return puzzleEnum;
        }

        private int OnGetPuzzleCatEye()
        {
            return puzzleParams.tablePictureCount;
        }

        private void OnInteractWithPuzzlePieces(GameObject intereact, GameObject puzzlePieces)
        {
          
            if (puzzlePieces is null) return;
            switch (puzzleEnum)
            {
                case PuzzleEnum.PictureTable:
                   Debug.LogWarning("Puzzle is Picture Table");
                    if (intereact.CompareTag(puzzlePieces.tag))
                    {
                        puzzlePieces.transform.parent = null;
                        puzzlePieces.transform.position = intereact.transform.position;
                        puzzlePieces.transform.rotation = intereact.transform.rotation;
                        puzzlePieces.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                        puzzlePieces.layer = 0;
                        intereact.layer = 0;
                        puzzleParams.tablePictureCount++;
                        CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.EnteredHouse);
                        
                    }
                    if (puzzleParams.tablePictureCount == 2)
                    {
                        CameraSignals.Instance.onSetCameraPositionForCutScene?.Invoke(PlayableEnum.SecretWall);
                        PlayableSignals.Instance.onSetUpCutScene?.Invoke(PlayableEnum.SecretWall);
                        puzzleEnum = PuzzleEnum.SecretBookShelf;
                        CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.SecretWall);
                    }
                    break;
                
                case PuzzleEnum.SecretBookShelf:
                    Debug.LogWarning("Puzzle is SecretBookShelf");
                    if (intereact.CompareTag(puzzlePieces.tag))
                    {
                        puzzlePieces.transform.parent = intereact.transform;
                        puzzlePieces.transform.position = intereact.transform.position;
                        puzzlePieces.transform.rotation = intereact.transform.rotation;
                        puzzlePieces.layer = 0;
                        intereact.layer = 0;
                        intereact.GetComponent<MeshRenderer>().material.DOFade(0f, "_BaseColor", 1f);
                        GameObject.FindWithTag("BookShelf").GetComponent<Animator>().SetBool("Open",true);
                        puzzleEnum = PuzzleEnum.Lantern;
                        CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.DetectiveBoard);
                        UITextSignals.Instance.onChangeMissionText?.Invoke();
                    }
                    break;
                
                case PuzzleEnum.Lantern:
                    lanternPuzzleHolder ??= GameObject.FindWithTag("LanternPuzzle").transform.GetChild(0).gameObject;
                    

                    if (intereact.CompareTag(puzzlePieces.tag))
                    {
                        intereact.GetComponent<MeshRenderer>().material.DOFade(0f, "_BaseColor", 1f);
                        
                        var rb = puzzlePieces.GetComponent<Rigidbody>();
                        rb.useGravity = false;
                        rb.isKinematic = true;
                        puzzlePieces.transform.parent = lanternPuzzleHolder.transform;
                        puzzlePieces.transform.position = intereact.transform.position;
                        puzzlePieces.transform.rotation = intereact.transform.rotation;
                    }
                    else
                    {
                        for (int i = 0; i < lanternPuzzleHolder.transform.childCount; i++)
                        {
                            
                            var puzzleRb = lanternPuzzleHolder.transform.GetChild(i).gameObject;
                            puzzleRb.transform.parent = null;
                            var puzzleRbRb = puzzleRb.GetComponent<Rigidbody>();
                            puzzleRbRb.useGravity = true;
                            puzzleRbRb.isKinematic = false;
                            puzzleRbRb.AddForce(new Vector3(0,0,1) * 500f);
                            

                        }
                    }

                    if (lanternPuzzleHolder.transform.childCount == 4)
                    {
                        var newObj = lanternPuzzleHolder.transform.parent.gameObject;
                        for (int i = 0; i < lanternPuzzleHolder.transform.childCount; i++)
                        {
                            var puzzleObj = lanternPuzzleHolder.transform.GetChild(i).gameObject;
                            puzzleObj.layer = 0;
                            
                        }

                        for (int i = 1; i < newObj.transform.childCount; i++)
                        {
                            var puzzleObj = newObj.transform.GetChild(i).gameObject;
                            puzzleObj.layer = 0;
                        }
                        GameObject.FindWithTag("BackDoor").GetComponent<Animator>().SetTrigger("Open");
                        puzzleEnum = PuzzleEnum.HandPuzzle;
                        CoreGameSignals.Instance.onGameManagerGetCurrentGameState?.Invoke(PlayableEnum.HandPuzzle);
                    }
                    break;
                
                case PuzzleEnum.HandPuzzle:
                    if (intereact.CompareTag(puzzlePieces.tag))
                    {
                        intereact.layer = 0;
                        intereact.GetComponent<MeshRenderer>().material.DOFade(0f, "_BaseColor", 0.5f);
                        puzzlePieces.layer = 0;
                        puzzlePieces.transform.parent = null;
                        var newPos = new Vector3(intereact.transform.position.x, intereact.transform.position.y , intereact.transform.position.z + 0.002f);
                        puzzlePieces.transform.position = newPos;
                        puzzlePieces.transform.rotation = intereact.transform.rotation;
                        puzzlePieces.transform.localScale = intereact.transform.localScale;
                        puzzleParams.handPuzzleCount++;
                 


                    }
                    if (puzzleParams.handPuzzleCount == 6)
                    {
                        var door = GameObject.FindWithTag("HakanRoom");
                        door.layer = 9;

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
            PuzzleSignals.Instance.onInteractWithPuzzlePieces -= OnInteractWithPuzzlePieces;
            PuzzleSignals.Instance.onGetPuzzleCatEyeValues -= OnGetPuzzleCatEye;
            PuzzleSignals.Instance.onGetPuzzleEnum -= OnGetPuzzleEnum;
             EnemySignals.Instance.onDeathOfHakan -= OnDeathOfHakan;
        }

        private void OnDisable()
        {
            UnSubscribeEvents();
        }
    }
}