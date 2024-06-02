using UnityEngine;
using System.Collections;
using Runtime.Controllers;
using Runtime.Enums;
using Runtime.Interfaces;
using UnityEditor.Experimental.GraphView;

namespace Runtime.Handler
{
    public abstract class BaseSkill : MonoBehaviour, ISkill
    {
        public PillTypes pillType;
        public float duration;
        private bool _isActive;

        public PillTypes PillType => pillType;
        public float Duration => duration;
        public bool IsActive => _isActive;

        public virtual void Activate()
        {
            _isActive = true;
            foreach (var pillController in Resources.FindObjectsOfTypeAll<PillController>())
            {
                print("Foreach");
                if (pillController.PillType != pillType) continue;
                print("Foreach if");
                pillController.PillBGImage.color =  Color.Lerp(Color.red, Color.yellow, .5f);
                break;
            }
            StartCoroutine(DeactivateAfterDuration());
        }

        public virtual void Deactivate()
        {
            foreach (var pillController in Resources.FindObjectsOfTypeAll<PillController>())
            {
                if (pillController.PillType != pillType) continue;
                pillController.PillBGImage.color = Color.white;
                break;
            }
            _isActive = false;
        }

        private IEnumerator DeactivateAfterDuration()
        {
            yield return new WaitForSeconds(duration);
            Deactivate();
        }
    }
}