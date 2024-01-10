using Game.Player;
using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    public abstract class WeaponSO : ScriptableObject
    {
        [SerializeField] string m_weaponName;
        [SerializeField, TextArea(5, 10)] string m_weaponDescription;
        [SerializeField] Sprite m_weaponIcon;
        [SerializeField] DamageSettings m_damageSettings;

        public string m_WeaponName => m_weaponName;
        public string m_WeaponDescription => m_weaponDescription;
        public Sprite m_WeaponIcon => m_weaponIcon;
        public DamageSettings m_DamageSettings => m_damageSettings;
        public abstract void Attack();

        public virtual bool CanAttack()
        {
            return true;
        }
    }
}
