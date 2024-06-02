using System.Collections.Generic;
using Runtime.Enums;
using Runtime.Handler;
using Runtime.Interfaces;
using Runtime.Signals;
using UnityEngine;

namespace Runtime.Managers
{
    public class PillManager : MonoBehaviour
    {
        public static volatile PillManager Instance;
        
        public List<ISkill> Skills = new List<ISkill>();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            AddSkill();
        }
        
        void AddSkill()
        {
            Skills.Add(GetComponent<HealthPill>());
            Skills.Add(GetComponent<SonicPerkPill>());
            Skills.Add(GetComponent<PsychoPill>());
            Skills.Add(GetComponent<ShieldPill>());
            Skills.Add(GetComponent<PulseOfImmortalityPerkPill>());
            Skills.Add(GetComponent<AntiDepressantPill>());
            
            GameDataManager.SaveData(PillTypes.HealthPill.ToString(), 3);
            GameDataManager.SaveData(PillTypes.SonicPerk.ToString(), 3);
            GameDataManager.SaveData(PillTypes.PsychoPill.ToString(), 3);
            GameDataManager.SaveData(PillTypes.AntiDepressantPill.ToString(), 3);
            GameDataManager.SaveData(PillTypes.PulseofImmortalityPerk.ToString(), 1);
            GameDataManager.SaveData(PillTypes.Shield.ToString(), 1);
        }
        
        private void OnEnable()
        {
            PlayerSignals.Instance.onSetPillEffect += OnSetPillEffect;
        }
        
        private void OnDisable()
        {
            PlayerSignals.Instance.onSetPillEffect -= OnSetPillEffect;
        }

        void OnSetPillEffect(PillTypes type)
        {
            switch (type)
            {
                case PillTypes.AntiDepressantPill:
                    break;
                case PillTypes.HealthPill:
                    int healthPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(HealthPill));
                    if (healthPillIndex != -1)
                    {
                        if (!Skills[healthPillIndex].IsActive)
                        { 
                            Skills[healthPillIndex].Activate();
                        }
                    }
                    break;
                case PillTypes.PsychoPill:
                    int psychoPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(PsychoPill));
                    if (psychoPillIndex != -1)
                    {
                        if (!Skills[psychoPillIndex].IsActive)
                        { 
                            Skills[psychoPillIndex].Activate();
                        }
                    }
                    break;
                case PillTypes.PulseofImmortalityPerk:
                    int pulseOfImmortalityPerkPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(PulseOfImmortalityPerkPill));
                    if (pulseOfImmortalityPerkPillIndex != -1)
                    {
                        if (!Skills[pulseOfImmortalityPerkPillIndex].IsActive)
                        { 
                            Skills[pulseOfImmortalityPerkPillIndex].Activate();
                        }
                    }
                    break;
                case PillTypes.SalvageSavior:
                    break;
                case PillTypes.Shield:
                    int shieldPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(ShieldPill));
                    if (shieldPillIndex != -1)
                    {
                        if (!Skills[shieldPillIndex].IsActive)
                        { 
                            Skills[shieldPillIndex].Activate();
                        }
                    }
                    break;
                case PillTypes.SonicPerk:
                    int sonicPerkPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(SonicPerkPill));
                    if (sonicPerkPillIndex != -1)
                    {
                        if (!Skills[sonicPerkPillIndex].IsActive)
                        { 
                            Skills[sonicPerkPillIndex].Activate();
                        }
                    }
                    break;
                case PillTypes.SoulHarvestAmplifier:
                    break;
            }
        }
    }
}
