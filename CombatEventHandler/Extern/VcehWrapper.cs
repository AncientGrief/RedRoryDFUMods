using System;
using DaggerfallConnect;
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
    public class VcehWrapper
    {
        private static Mod vcehMod;
        private static string modName;
        private Func<CalculateAttackDamageContext, ResultCalculateAttackDamage> onCalculateAttackDamage;
        private Func<SavingThrowContext, ResultSavingThrow> onSavingThrow;

        public bool Init(string nameOfMod)
        {
            modName = nameOfMod;
            vcehMod = ModManager.Instance.GetModFromGUID("fb086c76-38e7-4d83-91dc-f29e6f1bb17e");
            return vcehMod != null;
        }

        #region OnCalculateAttackDamage
        public void RegisterOnCalculateAttackDamage(Func<CalculateAttackDamageContext, ResultCalculateAttackDamage> func)
        {
            if (vcehMod == null)
                return;

            onCalculateAttackDamage = func;
            ModManager.Instance.SendModMessage(vcehMod.Title, "onCalculateAttackDamage",
                new Tuple<string,Func<object[], object[]>>(modName, OnCalculateAttackDamageInternal));
        }

        private object[] OnCalculateAttackDamageInternal(object[] p)
        {
            if (onCalculateAttackDamage == null)
                return null;

            //Call the registered function
            ResultCalculateAttackDamage result = onCalculateAttackDamage(
                new CalculateAttackDamageContext()
                {
                    Attacker = (DaggerfallEntity)p[0],
                    Target = (DaggerfallEntity)p[1],
                    IsEnemyFacingAwayFromPlayer = (bool)p[2],
                    WeaponAnimTime = (int)p[3],
                    Weapon = (DaggerfallUnityItem)p[4],
                    CalculatedDamage = (int)p[5]
                });

            return new object[] { result.CalculatedDamage };

        }
        #endregion

        #region OnSavingThrow
        public void RegisterOnSavingThrow(Func<SavingThrowContext, ResultSavingThrow> func)
        {
            if (vcehMod == null)
                return;

            onSavingThrow = func;
            ModManager.Instance.SendModMessage(vcehMod.Title, "onSavingThrow",
                new Tuple<string,Func<object[], object[]>>(modName, OnSavingThrowInternal));
        }

        private object[] OnSavingThrowInternal(object[] p)
        {
            if (onSavingThrow == null)
                return null;

            //Call the registered function
            ResultSavingThrow result = onSavingThrow(
                new SavingThrowContext()
                {
                    ElementType = (DFCareer.Elements)p[0],
                    EffectFlags = (DFCareer.EffectFlags)p[1],
                    Target = (DaggerfallEntity)p[2],
                    Modifier = (int)p[3],
                    CalculatedPercentDamageOrDuration = (int)p[4],
                });

            return new object[] { result.CalculatedPercentDamageOrDuration };

        }
        #endregion
    }

    #region CalculateAttackDamage Context & Result

    public class CalculateAttackDamageContext
    {
        public DaggerfallEntity Attacker { get; set; }
        public DaggerfallEntity Target { get; set; }
        public bool IsEnemyFacingAwayFromPlayer { get; set; }
        public int WeaponAnimTime { get; set; }
        public DaggerfallUnityItem Weapon { get; set; }

        public int CalculatedDamage { get; set; }
    }

    public class ResultCalculateAttackDamage
    {
        public int CalculatedDamage { get; set; }
    }
    #endregion

    #region Savingthrow Context & Result

    public class SavingThrowContext
    {
        public DFCareer.Elements ElementType { get; set; }
        public DFCareer.EffectFlags EffectFlags { get; set; }
        public DaggerfallEntity Target { get; set; }
        public int Modifier { get; set; }

        public int CalculatedPercentDamageOrDuration { get; set; }
    }

    public class ResultSavingThrow
    {
        public int CalculatedPercentDamageOrDuration { get; set; }
    }
    #endregion

}