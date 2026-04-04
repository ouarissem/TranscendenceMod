using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using TranscendenceMod.Buffs.Items.InfectionAccessories;
using TranscendenceMod.Dusts;

namespace TranscendenceMod.Items.Mounts
{
    public class LacewingTransformationMount : ModMount
    {
        public override void SetMount(Player player, ref bool skipDust)
        {

        }
        public override void UpdateEffects(Player player)
        {
            MountData.spawnDust = ModContent.DustType<Rainbow>();
            MountData.spawnDustNoGravity = false;
            MountData.buff = ModContent.BuffType<LacewingTransBuff>();
            MountData.heightBoost = 0;

            MountData.runSpeed = 8f;
            MountData.dashSpeed = 8f;
            MountData.jumpHeight = 3;
            MountData.jumpSpeed = 3.15f;
            MountData.acceleration = 0.5f;

            MountData.constantJump = true;
            MountData.usesHover = true;
            MountData.flightTimeMax = int.MaxValue;
            MountData.fatigueMax = int.MaxValue;
            MountData.fallDamage = 0f;
            MountData.blockExtraJumps = true;

            MountData.totalFrames = 1;

            MountData.idleFrameCount = MountData.totalFrames;
            MountData.idleFrameDelay = 12;
            MountData.idleFrameStart = 0;
            MountData.idleFrameLoop = true;

            MountData.runningFrameCount = MountData.totalFrames;
            MountData.runningFrameStart = 0;
            MountData.runningFrameDelay = 12;

            MountData.flyingFrameCount = MountData.totalFrames;
            MountData.flyingFrameStart = 0;
            MountData.flyingFrameDelay = 6;

            MountData.standingFrameCount = MountData.totalFrames;
            MountData.standingFrameStart = 0;
            MountData.standingFrameDelay = 16;

            MountData.inAirFrameCount = MountData.totalFrames;
            MountData.inAirFrameStart = 0;
            MountData.inAirFrameDelay = 6;

            MountData.swimFrameCount = MountData.totalFrames;
            MountData.swimFrameStart = 0;
            MountData.swimFrameDelay = 8;

            MountData.xOffset = 0;
            MountData.yOffset = 12;
            MountData.playerHeadOffset = 5;

            //MountData.bodyFrame = 3;
            MountData.playerXOffset = 0;

            int[] verticalOffsets = new int[MountData.totalFrames];
            for (int l = 0; l < verticalOffsets.Length; l++)
                verticalOffsets[l] = 0;

            MountData.playerYOffsets = verticalOffsets;

            if (!Main.dedServ)
            {
                MountData.textureWidth = MountData.backTexture.Width();
                MountData.textureHeight = MountData.backTexture.Height();
            }
        }
    }
}
