using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Reflection;
using System.Collections.Generic;
using static ItemDrop;

namespace BetterTakeAll
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class BetterTakeAllPlugin : BaseUnityPlugin
    {
        private const string pluginGUID = "spboyle7.BetterTakeAll";
        private const string pluginName = "Better Take All";
        private const string pluginVersion = "0.1.0";

        public static ManualLogSource logger = BepInEx.Logging.Logger.CreateLogSource(pluginName);

        public void Awake()
        {
            Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly());
        }

        // MoveAll from source with just one change to calling a different AddItem method
        [HarmonyPatch(typeof(Inventory), "MoveAll")]
        public static class MoveAll_Patch
        {
            private static bool Prefix(Inventory __instance, Inventory fromInventory)
            {
                List<ItemDrop.ItemData> list = new List<ItemDrop.ItemData>(fromInventory.GetAllItems());
                List<ItemDrop.ItemData> list2 = new List<ItemDrop.ItemData>();
                foreach (ItemDrop.ItemData itemData in list)
                {
                    // Original code calls AddItem(itemData, itemData.m_stack, itemData.m_gridPos.x, itemData.m_gridPos.y)
                    // We don't want to do that because that method prefers grid location over stackable inventory
                    // Calling the overloaded AddItem that takes a vector prefers stacking first
                    if (__instance.AddItem(itemData, new Vector2i(itemData.m_gridPos.x, itemData.m_gridPos.y)))
                    {
                        fromInventory.RemoveItem(itemData);
                    }
                    else
                    {
                        list2.Add(itemData);
                    }
                }
                foreach (ItemDrop.ItemData itemData2 in list2)
                {
                    if (__instance.AddItem(itemData2))
                    {
                        fromInventory.RemoveItem(itemData2);
                    }
                }
                __instance.Changed();
                fromInventory.Changed();

                return false;
            }
        }
    }
}
