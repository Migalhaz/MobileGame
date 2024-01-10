using Game.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "NewBowData", menuName = "Scriptable Object/Weapons/New Bow")]
    public class BowSO : WeaponSO
    {
        public override void Attack()
        {
            if (!CanAttack()) return;
            Debug.Log($"{m_WeaponName} Attack");
            PlayerManager.Instance.m_PlayerCombat.UseArrow();
        }
        public override bool CanAttack()
        {
            return PlayerManager.Instance.m_PlayerCombat.HasArrows();
        }
    }
}