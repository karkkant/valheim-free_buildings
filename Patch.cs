using HarmonyLib;
using UnityEngine;

namespace FreeBuildings
{
    [HarmonyPatch]
    public class Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ObjectDB), "Awake")]
        public static void PatchBuildingPieceCost(ref ObjectDB __instance)
        {
            UnityEngine.Object[] array = Resources.FindObjectsOfTypeAll(typeof(GameObject));

            for (int i = 0; i < array.Length; i++)
            {
                GameObject val = (GameObject)array[i];

                Piece piece;
                if (val.TryGetComponent<Piece>(out piece))
                {
                    if (piece.m_category == Piece.PieceCategory.BuildingWorkbench || piece.m_category == Piece.PieceCategory.BuildingStonecutter)
                    {
                        for (int j = 0; j < piece.m_resources.Length; j++)
                        {
                            piece.m_resources[j].m_amount = 0;
                        }
                    }
                }
            }
        }
    }
}