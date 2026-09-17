using System.Linq;
using HarmonyLib;
using UnityEngine;

namespace FreeBuildings
{
    [HarmonyPatch]
    public class Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PieceTable), nameof(PieceTable.UpdateAvailable))]
        public static void UnlockKnownBuildPieces(ref PieceTable __instance)
        {
            if (!Plugin.UnlockAllBuildPiecesEnabled) return;

            foreach(var obj in __instance.m_pieces)
            {
                Piece piece = obj.GetComponent<Piece>();

                if (piece.m_category == Piece.PieceCategory.BuildingWorkbench ||
                    piece.m_category == Piece.PieceCategory.BuildingStonecutter ||
                    (piece.m_category == Piece.PieceCategory.DeepNorth && piece.m_usage.HasFlag(Piece.UsageTagFlags.Building)))
                {
                    SetPrice(piece);
                }
            }
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(PieceTable), nameof(PieceTable.UpdateAvailable))]
        public static void PatchKnownBuildPieces(ref PieceTable __instance)
        {
            if (Plugin.UnlockAllBuildPiecesEnabled) return;

            __instance.m_availablePiecesByCategory[(int)Piece.PieceCategory.BuildingWorkbench].ForEach(p => SetPrice(p));
            __instance.m_availablePiecesByCategory[(int)Piece.PieceCategory.BuildingStonecutter].ForEach(p => SetPrice(p));

            foreach (var item in __instance.m_availablePiecesByCategory[(int)Piece.PieceCategory.DeepNorth])
            {
                if(item.m_usage.HasFlag(Piece.UsageTagFlags.Building))
                {
                    SetPrice(item);
                }
            }
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