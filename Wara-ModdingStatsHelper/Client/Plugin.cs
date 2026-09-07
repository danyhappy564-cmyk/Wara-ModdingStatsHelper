using BepInEx;
using BepInEx.Logging;
using UnityEngine;

namespace ShowMeTheStats
{
    [BepInPlugin("com.moddingstatshelper.spt", "Modding Stats Helper", "1.0")]
    public class ShowMeTheStatsPlugin : BaseUnityPlugin
    {
        /// <summary>
        /// Static handle on BepInEx's logger, so the patches - which are static - can
        /// report a binding that did not resolve instead of failing quietly.
        /// </summary>
        internal static ManualLogSource Log { get; private set; }

        void Awake()
        {
            Log = Logger;

            new ItemShowTooltipPatch().Enable();
            new ShowTooltipPatch().Enable();
            new WeaponUpdatePatch().Enable();
            new DropDownSlotContextPatch().Enable();
            new SlotViewPatch().Enable();
            new ScreenTypePatch().Enable();

            new DropDownSlotContextClosePatch().Enable();
        }



        void Update()
        {
            if (Globals.isWeaponModding)
            {
                bool isKeyDown = Input.GetKey(KeyCode.LeftControl);
                if ((isKeyDown && !Globals.isKeyPressed) || (!isKeyDown && Globals.isKeyPressed))
                {
                    Globals.isKeyPressed = !Globals.isKeyPressed;
                    Globals.simpleTooltip.Show(Globals.lastTooltipText, null, 0.1f, null);
                }
            }
        }
    }
}
