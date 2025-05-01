using System;
using System.Collections.Generic;
using DaggerfallConnect;
using DaggerfallWorkshop.Game.Entity;
using DaggerfallWorkshop.Game.Formulas;
using DaggerfallWorkshop.Game.Utility.ModSupport;
using UnityEngine;

namespace Game.Mods.CombatEventHandler.Scripts.Events
{
    public class OnSavingThrow : IVcehEvent<Func<DFCareer.Elements, DFCareer.EffectFlags, DaggerfallEntity, int, int>>
    {
        private static readonly List<Func<object[], object[]>> OnSavingThrowCallbacks = new List<Func<object[], object[]>>();

        public string Message => "onSavingThrow";

        public void RegisterFormula(Mod mod, Func<DFCareer.Elements, DFCareer.EffectFlags, DaggerfallEntity, int, int> func)
        {
            FormulaHelper.RegisterOverride(mod, "SavingThrow", func);
        }

        public void OnRegister(object data)
        {
            if (!(data is Tuple<string, Func<object[], object[]>> regData))
            {
                Debug.LogError($"[VCEH] Can't register {Message} event handler; data is null or wrong format.");
                return;
            }

            Debug.Log($"[VCEH] Mod {regData.Item1} successfully registered '{Message}'.");
            OnSavingThrowCallbacks.Add(regData.Item2);
        }

        public int ExecutePipeline(DFCareer.Elements elementType, DFCareer.EffectFlags effectFlags,
            DaggerfallEntity target, int modifier, int calculatedPercentDamageOrDuration)
        {
            object[] ctx =
            {
                elementType,
                effectFlags,
                target,
                modifier,
                calculatedPercentDamageOrDuration
            };

            object[] lastResult = { calculatedPercentDamageOrDuration };
            foreach (var func in OnSavingThrowCallbacks)
            {
                lastResult = func(ctx);
                ctx[5] = lastResult[0]; //Add newly calculated value to the context for the next function

                Debug.Log($"[VCEH] OnSavingThrow new percent damage/duration={lastResult[0]}.");
            }

            return (int)lastResult[0];
        }
    }
}