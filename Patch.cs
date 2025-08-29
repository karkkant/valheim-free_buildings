using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace FreeBuildings
{
    [HarmonyPatch]
    public class Patch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ObjectDB), nameof(ObjectDB.Awake))]
        public static void PatchBuildingPieceCost(ref ObjectDB __instance)
        {
            if (!Plugin.UnlockAllBuildPiecesEnabled)
            {
                return;
            }

            UnityEngine.Object[] resources = Resources.FindObjectsOfTypeAll(typeof(GameObject));

            for (int i = 0; i < resources.Length; i++)
            {
                GameObject val = (GameObject)resources[i];

                Piece piece;
                if (val.TryGetComponent<Piece>(out piece))
                {
                    if (piece.m_category == Piece.PieceCategory.BuildingWorkbench || piece.m_category == Piece.PieceCategory.BuildingStonecutter)
                    {
                        SetPrice(piece);
                    }
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PieceTable), nameof(PieceTable.UpdateAvailable))]
        public static void PatchKnownBuildPieces(ref PieceTable __instance)
        {
            if (Plugin.UnlockAllBuildPiecesEnabled)
            {
                return;
            }

            __instance.m_availablePieces[(int)Piece.PieceCategory.BuildingWorkbench].ForEach(p => SetPrice(p));
            __instance.m_availablePieces[(int)Piece.PieceCategory.BuildingStonecutter].ForEach(p => SetPrice(p));
        }

        private static void SetPrice(Piece piece)
        {
            foreach (var resource in piece.m_resources)
            {
                resource.m_amount = 0;
            }
        }
    }
}