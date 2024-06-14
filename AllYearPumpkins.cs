using System;
using System.Diagnostics.CodeAnalysis;
using MonoMod.Cil;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ModLoader;

namespace AllYearPumpkins
{
    [SuppressMessage("ReSharper", "UnusedType.Global")]
    public class AllYearPumpkins : Mod
    {
        public override void Load()
        {
            IL_WorldGen.UpdateWorld_GrassGrowth += PatchRemoveHalloweenCheck;
            if (WorldGen.VanillaGenPasses["Weeds"] is PassLegacy legacy) {
                WorldGen.ModifyPass(legacy, PatchRemoveHalloweenCheck);
            }
        }

        private void PatchRemoveHalloweenCheck(ILContext il)
        {
            try {
                new ILCursor(il)
                    .GotoNext(MoveType.Before, instruction => instruction.MatchLdsfld(typeof(Main), "halloween"),
                        instruction => instruction.MatchBrfalse(out _))
                    .RemoveRange(2);
            } catch (Exception e) {
                throw new ILPatchFailureException(ModContent.GetInstance<AllYearPumpkins>(), il, e);
            }
        }
    }
}