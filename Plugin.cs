using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wheel_of_Doom.Patches;

namespace Wheel_of_Doom
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class WheelOfDoom : BaseUnityPlugin
    {
        private const string modGUID = "Fadey.WheelOfDoom";
        private const string modName = "Wheel of Doom";
        private const string modVersion = "1.0.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        private static WheelOfDoom instance;

        // Used for logging 
        internal ManualLogSource mls;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }

            // Start logging
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("The wheel of doom is summoned");

            harmony.PatchAll(typeof(WheelOfDoom));
            harmony.PatchAll(typeof(WheelOfDoomPatch));
        }
    }
}
