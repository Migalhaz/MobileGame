using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Weapons
{
    [CreateAssetMenu(fileName = "NewSwordData", menuName = "Scriptable Object/Weapons/New Sword")]
    public class SwordSO : WeaponSO
    {
        public override void Attack()
        {
            Debug.Log($"{m_WeaponName} Attack");
        }
    }
}