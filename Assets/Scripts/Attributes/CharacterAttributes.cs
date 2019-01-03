﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Attributes
{
    public class CharacterAttributes : MonoBehaviour, IStatModifiersProvider
    {
        public enum ModifierType
        {
            Multiplicative,
            Additive
        }

        [System.Serializable]
        public struct Modifier
        {
        }

        [Header("Stat Points")]
        [SerializeField] int strengthPoints;
        [SerializeField] int dexterityPoints;
        [SerializeField] int charismaPoints;
        [SerializeField] int intelligencePoints;
        [SerializeField] int constitutionPoints;

        [Header("Stat effects on Attributes")]
        [SerializeField] float damageBonusPerStrengthPoint = 0.5f;
        [SerializeField] float criticalHitBonusPerStrengthPoint = 1.5f;
        [SerializeField] float criticalHitChancePerDexterityPoint = 1.0f;
        [SerializeField] float armourBonusPerConstitutionPoints = 0.5f;

        public IEnumerable<StatModifier> modifiers
        {
            get
            {
                return new StatModifier[]
                {
                    new StatModifier(FinalStat.DamageBonus, damageBonusPerStrengthPoint * strengthPoints),
                    new StatModifier(FinalStat.CriticalHitBonus, criticalHitBonusPerStrengthPoint * strengthPoints),
                    new StatModifier(FinalStat.CriticalHitChance, criticalHitChancePerDexterityPoint * dexterityPoints),
                    new StatModifier(FinalStat.ArmourBonus, armourBonusPerConstitutionPoints * constitutionPoints)
                };
            }
        }
    }
}