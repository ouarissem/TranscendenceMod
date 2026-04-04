using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using TranscendenceMod.Miscannellous;
using TranscendenceMod.Buffs.Items;
using Terraria.ID;
using Terraria.GameContent.Drawing;
using TranscendenceMod.Miscannellous.GlobalStuff;
using TranscendenceMod.Projectiles.Modifiers;
using TranscendenceMod.Projectiles.Weapons.Magic;
using TranscendenceMod.Projectiles;
using System;
using TranscendenceMod.Dusts;
using Terraria.Audio;

namespace TranscendenceMod
{
    public class Dashes : ModPlayer
    {
        /*Dash*/
        public bool HasDash => dashType == DashType.None;

        public float DeathFade;

        public enum DashType
        {
            None,
            Starting,
            Molten,
            Palladium,
            Sharkscale,
            SeraphAegis,
            Corrupt
        }

        public DashType dashType = DashType.None;
        public DashType visualDash = DashType.None;

        public readonly int dashDirLeft = 1;
        public readonly int dashDirRight = -1;
        public int dashCD = 45;
        public int dashRefillTimer;
        public int dashTime = 25;
        public int dashTimer;
        public float dashSpeed = 9;
        public int dashIFrames = 25;

        //0 is no iframes, 1 is bounce, 2 is ram
        public int dashBounce;
        public int dashDir;
        public int DashLeniency = 20;

        public int ramTimer;

        public override void ResetEffects()
        {
            if (dashTimer > dashTime)
                dashTimer = 0;

            if (ramTimer > 0)
                ramTimer--;


            switch (dashType)
            {
                case DashType.Starting:
                    dashSpeed = 8;
                    dashCD = 60;
                    dashTime = 10;
                    dashBounce = 0;
                    break;

                case DashType.Molten:
                    dashSpeed = 18f;
                    dashCD = 15;
                    dashTime = 10;
                    dashBounce = 2;
                    break;

                case DashType.Palladium:
                    dashSpeed = 20f;
                    dashCD = 45;
                    dashTime = 10;
                    dashBounce = 1;
                    break;

                case DashType.Sharkscale:
                    dashSpeed = 32f;
                    dashCD = 45;
                    dashTime = 15;
                    dashBounce = 2;
                    break;

                case DashType.SeraphAegis:
                    dashSpeed = 26f;
                    dashCD = 30;
                    dashTime = 15;
                    dashBounce = 2;
                    break;
                case DashType.Corrupt:
                    dashSpeed = 32f;
                    dashCD = 30;
                    dashTime = 12;
                    dashBounce = 0;
                    break;
            }

            Player.TryGetModPlayer(out TranscendencePlayer mp);

            if (mp.FishTrans == 0)
            {
                bool doubleLeft = Player.controlLeft && Player.releaseLeft && Player.doubleTapCardinalTimer[3] < 15 && Player.doubleTapCardinalTimer[2] == 0;
                bool doubleRight = Player.controlRight && Player.releaseRight && Player.doubleTapCardinalTimer[2] < 15 && Player.doubleTapCardinalTimer[3] == 0;
                if (mp.OverloadedCore && mp.HyperDashKeybind)
                {
                    Vector2 vel = Main.rand.NextVector2Circular(4f, 4f);
                    int d = Dust.NewDust(Player.Center - new Vector2(64), 128, 128, mp.OCoreChargeTimer < 60 ? DustID.CrimsonTorch : DustID.TheDestroyer, vel.X, vel.Y, 0, default, 2f);
                    Main.dust[d].noGravity = true;

                    if (mp.OCoreChargeTimer == 60 && !Main.dedServ)
                        SoundEngine.PlaySound(SoundID.MaxMana, Player.Center);

                    if (++mp.OCoreChargeTimer < 60)
                    {
                        dashDir = Player.direction == 1 ? 1 : -1;
                        mp.OCoreTimer = 0;

                        return;
                    }
                    else if (doubleLeft || doubleRight || mp.OCoreTimer > 0)
                    {
                        if (mp.OCoreTimer == 0)
                        {
                            if (doubleLeft)
                                dashDir = -1;
                            if (doubleRight)
                                dashDir = 1;
                        }

                        HyperDash();
                    }
                }
                else
                {
                    if (doubleRight && dashType != DashType.None && Player.timeSinceLastDashStarted > dashCD && mp.Focus >= 10f || dashTimer > 0 && dashDir == 1)
                    {
                        dashDir = 1;
                        DoDash();
                    }
                    else if (doubleLeft && dashType != DashType.None && Player.timeSinceLastDashStarted > dashCD && mp.Focus >= 10f || dashTimer > 0 && dashDir == -1)
                    {
                        dashDir = -1;
                        DoDash();
                    }
                }

            }


            if (dashRefillTimer > 0)
                dashRefillTimer--;
            else
            {
                dashType = DashType.None;
                visualDash = DashType.None;
            }

            base.ResetEffects();
        }
        public override void Load()
        {
            On_Player.DashMovement += On_Player_DashMovement;
        }

        private void On_Player_DashMovement(On_Player.orig_DashMovement orig, Player self)
        {
            self.TryGetModPlayer(out Dashes mp);
            self.TryGetModPlayer(out TranscendencePlayer mp2);

            if (!mp2.EmpoweringTabletEquipped && mp2.Focus >= 10f && mp.dashType == DashType.None && mp.dashTimer == 0 && mp.dashRefillTimer == 0 && mp2.OCoreTimer <= 0 && mp2.FishTrans == 0)
            {
                if (self.timeSinceLastDashStarted == 1)
                    mp2.ExpendFocus(self, 10f, 60f);

                orig(self);
            }
        }

        public override void PreUpdate()
        {
            base.PreUpdate();

            Player.TryGetModPlayer(out TranscendencePlayer TranscendencePlayer);


            bool belowCA = Player.dashType != 1 && Player.dashType != 3 && Player.dashType != 5;

            if (TranscendencePlayer.MoltenShieldEquipped && belowCA)
            {
                dashType = DashType.Molten;
                visualDash = DashType.Molten;
                dashRefillTimer = 30;
            }
            if (TranscendencePlayer.PalladiumShieldEquipped && belowCA)
            { 
                dashType = DashType.Palladium;
                visualDash = DashType.Palladium;
                dashRefillTimer = 30;
            }
            if (TranscendencePlayer.SharkscaleSetWear)
            {
                dashType = DashType.Sharkscale;
                visualDash = DashType.Sharkscale;
                dashRefillTimer = 30;
            }
            if (TranscendencePlayer.CosmicAegis)
            {
                dashType = DashType.SeraphAegis;
                visualDash = DashType.SeraphAegis;
                dashRefillTimer = 30;
            }
            if (TranscendencePlayer.CorruptWanderingKit)
            {
                dashType = DashType.Corrupt;
                visualDash = DashType.Corrupt;
                dashRefillTimer = 30;
            }


            if (ramTimer > 0)
            {
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];
                    bool shouldBeHit = npc != null && npc.active && !npc.friendly;

                    if (shouldBeHit && npc.Hitbox.Intersects(Player.Hitbox) && dashTimer > 0 && dashBounce == 1)
                    {
                        float speed = 1f;
                        switch (dashType)
                        {
                            case DashType.Palladium: speed = 6f; break;
                        }

                        if (!npc.dontTakeDamage) npc.SimpleStrikeNPC(TranscendencePlayer.ShieldDamage, -dashDir, true, Player.GetTotalKnockback(DamageClass.Generic).Base, DamageClass.Melee, true);

                        dashTimer = 0;

                        Player.velocity = Player.DirectionTo(npc.Center) * (speed * -2f);
                        Player.GiveImmuneTimeForCollisionAttack(45);
                        ramTimer = 15;
                    }
                    if (shouldBeHit && npc.Hitbox.Intersects(Player.Hitbox) && dashBounce == 2)
                    {
                        int dmg = 30;
                        switch (dashType)
                        {
                            case DashType.SeraphAegis: dmg = TranscendencePlayer.AegisRamDamage; break;
                        }
                        if (npc.GetGlobalNPC<TranscendenceNPC>().HitCD == 0 && !npc.dontTakeDamage)
                        {
                            npc.SimpleStrikeNPC(dmg, -dashDir, true, Player.GetTotalKnockback(DamageClass.Generic).Base, DamageClass.Melee, true);
                            Player.GiveImmuneTimeForCollisionAttack(60);
                        }
                        npc.GetGlobalNPC<TranscendenceNPC>().HitCD = 45;
                        ramTimer = 15;
                    }
                }

            }
        }

        public void HyperDash()
        {
            Player.TryGetModPlayer(out TranscendencePlayer TranscendencePlayer);

            TranscendencePlayer.OCoreTimer++;
            Player.gravity *= 0f;
            Player.velocity.Y *= 0f;

            bool boss = TranscendenceUtils.BossAlive();

            //Do Dash Movement
            ramTimer = 15;
            Vector2 dashvel = Player.velocity;

            dashvel.X = dashDir * 27.5f;
            if (dashTimer > (dashTime * 0.5)) dashvel *= 0.7f;

            Player.direction = dashDir;
            Player.velocity.X = dashvel.X;
            Player.shadowArmor = true;


            float size = 30f + (float)Math.Sin(TranscendencePlayer.OCoreChargeTimer * 0.5f) * 10f;
            for (int i = 0; i < 32; i++)
            {
                Vector2 vec = Vector2.One.RotatedBy(MathHelper.TwoPi * i / 32f) * size;
                vec.Y /= 4.5f;
                Vector2 pos = Player.Center + vec.RotatedBy(dashvel.ToRotation() + MathHelper.PiOver2);

                Dust d = Dust.NewDustPerfect(pos, DustID.TheDestroyer,
                    Vector2.Zero, 0, Color.White, 2f);
                d.noGravity = true;
            }



        }
        public void DoDash()
        {
            Player.TryGetModPlayer(out TranscendencePlayer TranscendencePlayer);

            if (dashTimer == 0)
                TranscendencePlayer.ExpendFocus(Player, 10f, 60f);

            Player.timeSinceLastDashStarted = 0;
            TranscendencePlayer.FullRotResetCD = 5;

            ramTimer = 15;
            dashTimer++;
            Vector2 dashvel = Player.velocity;

            dashvel.X = dashDir * dashSpeed;
            if (dashTimer > (dashTime * 0.375)) dashvel *= 0.5f;

            Player.velocity.X = dashvel.X;
            Player.shadowArmor = true;

            DoVisualDash();
        }
        public void DoVisualDash()
        {
            Player.fullRotation = Player.velocity.X * 0.05f;
            Player.fullRotationOrigin = Player.Size * 0.5f;

            switch (visualDash)
            {
                case DashType.SeraphAegis:
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                Dust d = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, (float)(Math.Sin(TranscendenceWorld.Timer / (4 + (i * 0.25f))) * 65)), ModContent.DustType<PlayerCosmicBlood>(),
                                    Vector2.Zero, 0, TranscendenceWorld.CosmicPurple, 1f);
                                d.velocity = new Vector2(dashDir * -5, 0);
                                d.noGravity = true;

                                Dust d2 = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, (float)(Math.Sin(TranscendenceWorld.Timer / (4 + (i * 0.25f))) * -65)), ModContent.DustType<PlayerCosmicBlood>(),
                                    Vector2.Zero, 0, TranscendenceWorld.CosmicPurple, 1f);
                                d2.velocity = new Vector2(dashDir * -5, 0);
                                d2.noGravity = true;
                            }
                        }
                        break;
                    }
                case DashType.Sharkscale:
                    {
                        int type = Main.rand.NextBool(3) ? ModContent.DustType<MuramasaDust>() : ModContent.DustType<BetterBlood>();
                        Dust d = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, (float)Math.Ceiling(Math.Sin(TranscendenceWorld.Timer)) * 25f), type,
                            Player.velocity, 0, Color.White, type == ModContent.DustType<BetterWater>() ? 2f : 1f);
                        d.velocity = new Vector2(dashDir * -5, 0);
                        d.noGravity = true;

                        break;
                    }
                case DashType.Molten:
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            Dust d = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, Main.rand.NextFloat(Player.width / -2f, Player.width / 2f)), ModContent.DustType<Ember>(),
                                Vector2.Zero, 0, Color.White, 1f);
                            d.velocity = new Vector2(dashDir * -5, 0);
                            d.noGravity = true;
                        }
                        break;
                    }
                case DashType.Palladium:
                    {
                        Dust d = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, (float)(Math.Sin(dashTimer / 2f) * 35)), ModContent.DustType<Ember2>(),
                            Vector2.Zero, 0, Color.OrangeRed, 1f);
                        d.velocity = new Vector2(dashDir * -5, 0);

                        Dust d2 = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, (float)(Math.Sin(dashTimer / 2f) * -35)), ModContent.DustType<Ember2>(),
                            Vector2.Zero, 0, Color.OrangeRed, 1f);
                        d2.velocity = new Vector2(dashDir * -5, 0);
                        break;
                    }
                case DashType.Corrupt:
                    {
                        Dust d = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, (float)(Math.Sin(dashTimer * 0.66f) * 45f)), ModContent.DustType<ArenaDustNoCenter>(),
                            Vector2.Zero, 0, Color.Lerp(Color.Teal, Color.Yellow, dashTimer / (float)dashTime), 1f);
                        d.velocity = new Vector2(dashDir * -5, 0);

                        Dust d2 = Dust.NewDustPerfect(Player.Center - new Vector2(Player.width / 2 * dashDir, (float)(Math.Sin(dashTimer * 0.66f) * -45f)), ModContent.DustType<ArenaDustNoCenter>(),
                            Vector2.Zero, 0, Color.Lerp(Color.Teal, Color.Yellow, dashTimer / (float)dashTime), 1f);
                        d2.velocity = new Vector2(dashDir * -5, 0);
                        break;
                    }
            }
        }
    }
}

