using System;
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
                    var antiDepressantPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(AntiDepressantPill));
                    if (antiDepressantPillIndex != -1)
                    {
                        if (!Skills[antiDepressantPillIndex].IsActive)
                        { 
                            Skills[antiDepressantPillIndex].Activate();
                            CoreUISignals.Instance.onActivatePill?.Invoke(antiDepressantPillIndex, PillTypes.AntiDepressantPill);
                        }
                    }
                    break;
                case PillTypes.HealthPill:
                    var healthPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(HealthPill));
                    if (healthPillIndex != -1)
                    {
                        if (!Skills[healthPillIndex].IsActive)
                        { 
                            Skills[healthPillIndex].Activate();
                            CoreUISignals.Instance.onActivatePill?.Invoke(Skills[healthPillIndex].Duration, PillTypes.HealthPill);
                        }
                    }
                    break;
                case PillTypes.PsychoPill:
                    var psychoPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(PsychoPill));
                    if (psychoPillIndex != -1)
                    {
                        if (!Skills[psychoPillIndex].IsActive)
                        { 
                            Skills[psychoPillIndex].Activate();
                            CoreUISignals.Instance.onActivatePill?.Invoke(Skills[psychoPillIndex].Duration, PillTypes.PsychoPill);
                        }
                    }
                    break;
                case PillTypes.PulseofImmortalityPerk:
                    var pulseOfImmortalityPerkPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(PulseOfImmortalityPerkPill));
                    if (pulseOfImmortalityPerkPillIndex != -1)
                    {
                        if (!Skills[pulseOfImmortalityPerkPillIndex].IsActive)
                        { 
                            Skills[pulseOfImmortalityPerkPillIndex].Activate();
                            CoreUISignals.Instance.onActivatePill?.Invoke(Skills[pulseOfImmortalityPerkPillIndex].Duration, PillTypes.PulseofImmortalityPerk);
                        }
                    }
                    break;
                case PillTypes.SalvageSavior:
                    break;
                case PillTypes.Shield:
                    var shieldPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(ShieldPill));
                    if (shieldPillIndex != -1)
                    {
                        if (!Skills[shieldPillIndex].IsActive)
                        { 
                            Skills[shieldPillIndex].Activate();
                            CoreUISignals.Instance.onActivatePill?.Invoke(Skills[shieldPillIndex].Duration, PillTypes.Shield);
                        }
                    }
                    break;
                case PillTypes.SonicPerk:
                    var sonicPerkPillIndex = Skills.FindIndex(skill => skill.GetType() == typeof(SonicPerkPill));
                    if (sonicPerkPillIndex != -1)
                    {
                        if (!Skills[sonicPerkPillIndex].IsActive)
                        { 
                            Skills[sonicPerkPillIndex].Activate();
                            CoreUISignals.Instance.onActivatePill?.Invoke(Skills[sonicPerkPillIndex].Duration, PillTypes.SonicPerk);
                        }
                    }
                    break;
                case PillTypes.SoulHarvestAmplifier:
                    break;
                default:
                    break;
            }
        }
    }
}
