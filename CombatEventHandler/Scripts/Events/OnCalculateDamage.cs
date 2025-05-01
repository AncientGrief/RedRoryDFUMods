using System;
using System.Collections.Generic;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Formulas;
using DaggerfallWorkshop.Game.Items;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;

namespace Game.Mods.CombatEventHandler.Scripts.Events
{
    public class OnCalculateDamage
        : IVcehEvent<Func<DaggerfallEntity, DaggerfallEntity, bool, int, DaggerfallUnityItem, int>>
    {
        private static readonly List<Func<object[], object[]>> OnCalculateAttackDamage = new List<Func<object[], object[]>>();

        public string Message => "onCalculateAttackDamage";

        public void RegisterFormula(Mod mod, Func<DaggerfallEntity, DaggerfallEntity, bool, int, DaggerfallUnityItem, int> func)
        {
            FormulaHelper.RegisterOverride(mod, "CalculateAttackDamage", func);
        }

        public void OnRegister(object data)
        {
            if (!(data is Tuple<string, Func<object[], object[]>> regData))
            {
                Debug.LogError($"VCEH: Can't register {Message} event handler; data is null or wrong format.");
                return;
            }

            Debug.Log($"VCEH: Mod {regData.Item1} successfully registered '{Message}'.");
            OnCalculateAttackDamage.Add(regData.Item2);
        }

        public int ExecutePipeline(DaggerfallEntity attacker, DaggerfallEntity target,
            bool isEnemyFacingAwayFromPlayer, int weaponAnimTime, DaggerfallUnityItem weapon, int damage)
        {
            //One could also add the vanilla damage for all subscribed mods so they can see the vanilla damage and the pipeline damage
            object[] ctx =
            {
                attacker,
                target,
                isEnemyFacingAwayFromPlayer,
                weaponAnimTime,
                weapon,
                damage
            };

            object[] lastResult = { damage };
            foreach (var func in OnCalculateAttackDamage)
            {
                lastResult = func(ctx);
                ctx[5] = lastResult[0]; //Add newly calculated damage to the context for the next function

                Debug.Log($"VCEH: OnAttackDamageCalculated new damage={lastResult[0]}.");
            }

            return (int)lastResult[0];
        }
    }
}