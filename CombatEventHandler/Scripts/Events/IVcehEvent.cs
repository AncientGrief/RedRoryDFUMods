using System;
using System.Collections.Generic;
using DaggerfallWorkshop.Game.Utility.ModSupport;

namespace Game.Mods.CombatEventHandler.Scripts.Events
{
    /// <summary>
    /// Base VCEH event interface
    /// </summary>
    public interface IVcehEventBase
    {
        /// <summary>
        /// DFU Message for registering a callback
        /// </summary>
        string Message { get; }

        /// <summary>
        /// Gets called when the mod registers a callback and handles the data
        /// </summary>
        /// <param name="data"></param>
        void OnRegister(object data);
    }

    /// <summary>
    /// For registering to a DFU formula override
    /// </summary>
    /// <typeparam name="TCallbackFunc">The type of the Callback function needed</typeparam>
    public interface IVcehEvent<TCallbackFunc> : IVcehEventBase
        where TCallbackFunc : Delegate
    {
        /// <summary>
        /// Registers the DFU formula override
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="func"></param>
        void RegisterFormula(Mod mod, TCallbackFunc func);
    }
}