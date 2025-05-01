using System;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Utility.ModSupport;

/*
 * This script is intended for DFU mod developers who want to make use of Vanilla Combat Event Handler
 * and don't want to fiddle around with DFU's weird mod system.
 *
 * This class is made public for every DFU mod developer to use.
 */

namespace VanillaCombatEventHandler
{
    public static class VcehWrapper
    {
        private static Mod vcehMod;
        private static string modName;
        private static Func<AttackDamageCalculatedContext, ResultAttackDamageCalculated> onOnAttackDamageCalculated;

        public static bool Init(string nameOfMod)
        {
            modName = nameOfMod;
            vcehMod = ModManager.Instance.GetModFromGUID("fb086c76-38e7-4d83-91dc-f29e6f1bb17e");
            return vcehMod != null;
        }

        public static void RegisterOnCalculateAttackDamage(Func<AttackDamageCalculatedContext, ResultAttackDamageCalculated> func)
        {
            if (vcehMod == null)
                return;

            onOnAttackDamageCalculated = func;
            ModManager.Instance.SendModMessage(vcehMod.Title, "onAttackDamageCalculated",
                new Tuple<string,Func<object[], object[]>>(modName, OnAttackDamageInternal));
        }

        private static object[] OnAttackDamageInternal(object[] p)
        {
            if (onOnAttackDamageCalculated == null)
                return null;

            ResultAttackDamageCalculated result = onOnAttackDamageCalculated(
                new AttackDamageCalculatedContext()
                {
                    Attacker = (DaggerfallEntity)p[0],
                    Target = (DaggerfallEntity)p[1],
                    IsEnemyFacingAwayFromPlayer = (bool)p[2],
                    WeaponAnimTime = (int)p[3],
                    Weapon = (DaggerfallUnityItem)p[4],
                    CalculatedDamage = (int)p[5]
                });

            return new object[] { result.AttackDamage };

        }
    }

    public class AttackDamageCalculatedContext
    {
        public DaggerfallEntity Attacker { get; set; }
        public DaggerfallEntity Target { get; set; }
        public bool IsEnemyFacingAwayFromPlayer { get; set; }
        public int WeaponAnimTime { get; set; }
        public DaggerfallUnityItem Weapon { get; set; }

        public int CalculatedDamage { get; set; }
    }

    public class ResultAttackDamageCalculated
    {
        public int AttackDamage { get; set; }
    }
}