using MigalhaSystem.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Weapons;
using MigalhaSystem.ScriptableEvents;
namespace Game.Player
{
    public class PlayerCombat : PlayerComponent
    {
        [Header("Weapons Settings")]
        [SerializeField] WeaponSO m_weaponInFirstSlot;
        [SerializeField] WeaponSO m_weaponInSecondSlot;
        WeaponSO m_currentWeapon;

        [Header("Bow Settings")]
        [SerializeField, Min(0)] int m_arrows;
        private void Awake()
        {
            m_currentWeapon = m_weaponInFirstSlot;
        }

        public void Attack()
        {
            if (!CanAct()) return;
            m_currentWeapon?.Attack();
            PlayerAction();
        }

        public void SwitchWeapon()
        {
            if (!CanAct()) return;
            if (m_currentWeapon == m_weaponInFirstSlot)
            {
                m_currentWeapon = m_weaponInSecondSlot;
            }
            else
            {
                m_currentWeapon = m_weaponInFirstSlot;
            }
            Debug.Log(m_currentWeapon.m_WeaponName.Color(Color.red));
            PlayerAction();
        }
        
        public bool HasArrows()
        {
            return m_arrows > 0;
        }

        public void UseArrow()
        {
            m_arrows--;
        }
    }
}