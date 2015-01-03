using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;


namespace HuyNKSeries.Champ
{
    internal class Jinx : Champion
    {
        public Jinx()
        {
            SetSpells();
            LoadMenu();
        }

        private void SetSpells()
        {
            Q = new Spell(SpellSlot.Q,float.MaxValue);
            //Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);

            W = new Spell(SpellSlot.W, 1500f);
            W.SetSkillshot(0.6f, 60f, 3300f, true, SkillshotType.SkillshotLine);

            E = new Spell(SpellSlot.E, 900f);
            E.SetSkillshot(0.7f, 120f, 1750f, false, SkillshotType.SkillshotCircle);

            R = new Spell(SpellSlot.R, 20000);
            R.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);

            _r2 = new Spell(SpellSlot.R, 20000);
            _r2.SetSkillshot(0.6f, 140f, 1700f, false, SkillshotType.SkillshotLine);
        }

        private void LoadMenu()
        {

            Menu key = new Menu("Phím đánh nhau", "Key");
            key.AddItem(new MenuItem("ComboActive", "Đánh nhau").SetValue(new KeyBind(32, KeyBindType.Press)));
            key.AddItem(
                new MenuItem("HarassActive", "Rỉa máu").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            key.AddItem(
                new MenuItem("HarassActiveT", "Luôn luôn rỉa máu").SetValue(new KeyBind("O".ToCharArray()[0],
                    KeyBindType.Toggle)));
            key.AddItem(
                new MenuItem("LaneClearActive", "Farm Lính").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));
            key.AddItem(
                new MenuItem("R_Nearest_Killable", "Ulti nếu gần chết !").SetValue(new KeyBind("R".ToCharArray()[0],
                    KeyBindType.Press)));
            key.AddItem(
                new MenuItem("Force_R", "Sử dụng R khi hồi chiu").SetValue(new KeyBind("I".ToCharArray()[0], KeyBindType.Press)));
            //add to menu
            Menus.menu.AddSubMenu(key);

            //Spell
            Menu spellMenu = new Menu("Danh Sách Skill", "SpellMenu");

            Menu qMenu = new Menu("Danh sách chiêu Q", "QMenu");

            qMenu.AddItem(new MenuItem("Q_Max_Range", "Q khi cách tầm").SetValue(new Slider(1050, 500, 1200)));
            qMenu.AddItem(new MenuItem("Auto_Q_Slow", "Tự động Q chậm").SetValue(true));
            qMenu.AddItem(new MenuItem("Auto_Q_Immobile", "Tự Q khi đứng im").SetValue(true));
            spellMenu.AddSubMenu(qMenu);

            //W
            Menu wMenu = new Menu("Danh Sách Chiêu W", "WMenu");

            wMenu.AddItem(
                new MenuItem("W_Max_Range", "W khi cách tầm").SetValue(new Slider(900, 500, 1050)));
            spellMenu.AddSubMenu(wMenu);
            //E 

            Menu eMenu = new Menu("Danh Sách Chiêu E", "EMenu");

            eMenu.AddItem(new MenuItem("E_On_Killable", "Tự dùng E nếu giết được").SetValue(true));
            eMenu.AddItem(new MenuItem("E_On_Safe", "Tự E để né chiêu").SetValue(true));
            spellMenu.AddSubMenu(eMenu);

            //Ultimate 
            Menu rMenu = new Menu("Danh Sách Chiêu R", "RMenu");

            rMenu.AddItem(new MenuItem("R_Min_Range", "R Tầm đánh nhỏ nhất").SetValue(new Slider(300, 0, 1000)));
            rMenu.AddItem(new MenuItem("R_Max_Range", "R Tầm đánh lớn nhất").SetValue(new Slider(2000, 0, 20000)));
            rMenu.AddItem(new MenuItem("R_Mec", "R nếu trúng mục tiêu >=").SetValue(new Slider(3, 1, 5)));
            rMenu.AddItem(new MenuItem("R_Overkill_Check", "Kiểm tra để dùng R KS").SetValue(true));

            rMenu.AddSubMenu(new Menu("Không dùng R cho tướng", "Dont_R"));
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.Team != Player.Team)
                )
                rMenu.SubMenu("Dont_R")
                    .AddItem(new MenuItem("Dont_R" + enemy.BaseSkinName, enemy.BaseSkinName).SetValue(false));

            spellMenu.AddSubMenu(rMenu);


            Menus.menu.AddSubMenu(spellMenu);

            //combo
            Menu combo = new Menu("Đánh nhau", "Combo");

            combo.AddItem(new MenuItem("UseQCombo", "Dùng Q").SetValue(true));
            combo.AddItem(new MenuItem("UseWCombo", "Dùng W").SetValue(true));
            combo.AddItem(new MenuItem("UseECombo", "Dùng E").SetValue(true));
            combo.AddItem(new MenuItem("UseRCombo", "Dùng R").SetValue(true));
            combo.AddItem(new MenuItem("Ignite", "Dùng thiêu đốt").SetValue(true));
            combo.AddItem(new MenuItem("Botrk", "Gươm vô danh").SetValue(true));
            Menus.menu.AddSubMenu(combo);

            //harass
            Menu harass = new Menu("Rỉa máu", "Harass");

            harass.AddItem(new MenuItem("UseQHarass", "Dùng Q").SetValue(true));
            harass.AddItem(new MenuItem("UseWHarass", "Dùng W").SetValue(true));
            // AddManaManagertoMenu(harass, "Harass", 30);
            //add to menu
            Menus.menu.AddSubMenu(harass);
            //farm
            Menu farm = new Menu("Dọn lính", "LaneClear");

            farm.AddItem(new MenuItem("UseQFarm", "Dùng Q").SetValue(true));
            //AddManaManagertoMenu(farm, "LaneClear", 30);
            //add to menu
            Menus.menu.AddSubMenu(farm);

            //Misc
            Menu miscMenu = new Menu("Linh Tinh", "Misc");

            miscMenu.AddItem(
                new MenuItem("Misc_Use_WE", "Dùng E + W").SetValue(new KeyBind("E".ToCharArray()[0],
                    KeyBindType.Press)));
            //add to menu
            Menus.menu.AddSubMenu(miscMenu);

            //Drawing
            Menu drawMenu = new Menu("Hiện Tầm Đánh", "Drawing");
            drawMenu.AddItem(new MenuItem("Draw_Disabled", "Tắt tất cả").SetValue(false));
            drawMenu.AddItem(new MenuItem("Draw_Q", "Vòng Q").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_W", "Vòng W").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_E", "Vòng E").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_R", "Vòng R").SetValue(true));
            drawMenu.AddItem(new MenuItem("Draw_R_Killable", "Đánh dấu khi Ulti chết").SetValue(true));

            MenuItem drawComboDamageMenu = new MenuItem("Draw_ComboDamage", "Thanh máu sau khi Combo ").SetValue(true);
            drawMenu.AddItem(drawComboDamageMenu);
            Utility.HpBarDamageIndicator.DamageToUnit = GetComboDamage;
            Utility.HpBarDamageIndicator.Enabled = drawComboDamageMenu.GetValue<bool>();
            drawComboDamageMenu.ValueChanged +=
                delegate(object sender, OnValueChangeEventArgs eventArgs)
                {
                    Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
                };

            Menus.menu.AddSubMenu(drawMenu);

        }


        private float GetComboDamage(Obj_AI_Base target)
        {
            double comboDamage = 0;

            if (Q.IsReady())
                comboDamage += Player.GetSpellDamage(target, SpellSlot.Q);

            if (W.IsReady())
                comboDamage += Player.GetSpellDamage(target, SpellSlot.W);

            if (E.IsReady())
                comboDamage += Player.GetSpellDamage(target, SpellSlot.E);

            if (R.IsReady())
                comboDamage += Player.GetSpellDamage(target, SpellSlot.R);

            if (Items.CanUseItem(HuyNkItems.Bilge.Id))
                comboDamage += Player.GetItemDamage(target, Damage.DamageItems.Bilgewater);

            if (Items.CanUseItem(HuyNkItems.Botrk.Id))
                comboDamage += Player.GetItemDamage(target, Damage.DamageItems.Botrk);

            if (HuyNkItems.IgniteSlot != SpellSlot.Unknown &&
                Player.Spellbook.CanUseSpell(HuyNkItems.IgniteSlot) == SpellState.Ready)
                comboDamage += Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            return (float)(comboDamage + Player.GetAutoAttackDamage(target) * 1);
        }

        private void Combo()
        {
            UseSpells(Menus.menu.Item("UseQCombo").GetValue<bool>(), Menus.menu.Item("UseWCombo").GetValue<bool>(),
                Menus.menu.Item("UseECombo").GetValue<bool>(), Menus.menu.Item("UseRCombo").GetValue<bool>(), "Combo");
        }

        private void Harass()
        {
            UseSpells(Menus.menu.Item("UseQHarass").GetValue<bool>(), Menus.menu.Item("UseWHarass").GetValue<bool>(),
                false, false, "Harass");
        }

        private void UseSpells(bool useQ, bool useW, bool useE, bool useR, string source)
        {

            if (source == "Harass")
            {
                var etarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                E.CastIfWillHit(etarget);
                AutoQ();
                if (useQ)
                    return;
                    HuyNkItems.CastBasicSkillShot(Q, Q.Range, TargetSelector.DamageType.Physical, HitChance.Collision);
                if (useW)
                    HuyNkItems.CastBasicSkillShot(W, W.Range, TargetSelector.DamageType.Magical, HitChance.Collision);
            }
            if (source == "Combo")
            {
                var etarget = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (useE)
                    E.Cast(etarget,true,true);
                    Cast_E();
                var wtarget = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                var qTarget = TargetSelector.GetTarget(Q.Range,TargetSelector.DamageType.Physical);
                if (qTarget != null)
                {
                    if (GetComboDamage(qTarget) >= qTarget.Health && HuyNkItems.Ignite_Ready() &&
                        Menus.menu.Item("Ignite").GetValue<bool>())
                        HuyNkItems.Use_Ignite(qTarget);

                    if (Menus.menu.Item("Botrk").GetValue<bool>())
                    {
                        if (GetComboDamage(qTarget) < qTarget.Health && !qTarget.HasBuffOfType(BuffType.Slow))
                            HuyNkItems.Use_Bilge(qTarget);

                        if (GetComboDamage(qTarget) < qTarget.Health && !qTarget.HasBuffOfType(BuffType.Slow))
                            HuyNkItems.Use_Botrk(qTarget);
                        HuyNkItems.Use_DFG(wtarget);
                    }
                }
                if (useQ)
                    
                    HuyNkItems.CastBasicSkillShot(Q, Q.Range, TargetSelector.DamageType.Physical, HitChance.VeryHigh);
                if (useW)
                    HuyNkItems.CastBasicSkillShot(W, W.Range, TargetSelector.DamageType.Magical, HitChance.Collision);
            }
            
            if (useR)
                Cast_R();
        }

        private void Farm()
        {
            //if (!har("LaneClear"))
            //  return;

            List<Obj_AI_Base> allMinionsQ = MinionManager.GetMinions(ObjectManager.Player.ServerPosition, Q.Range,
                MinionTypes.All, MinionTeam.NotAlly);

            var useQ = Menus.menu.Item("UseQFarm").GetValue<bool>();

            if (useQ && allMinionsQ.Count > 0)
                
                Q.Cast(allMinionsQ[0], HuyNkItems.packets());
            
        }

        private void Cast_E()
        {
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

            if (E.IsReady() && target != null && Menus.menu.Item("E_On_Killable").GetValue<bool>())
            {
                if (Player.GetSpellDamage(target, SpellSlot.E) > target.Health + 25)
                {
                    if (Menus.menu.Item("E_On_Safe").GetValue<bool>())
                    {
                        var ePos = E.GetPrediction(target);
                        if (ePos.UnitPosition.CountEnemysInRange(500) < 2)
                            E.Cast(ePos.UnitPosition, HuyNkItems.packets());
                    }
                    else
                    {
                        E.Cast(target, HuyNkItems.packets());
                    }
                }
            }
        }

        private void Cast_R()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);

            if (R.IsReady() && target != null)
            {
                if (Menus.menu.Item("Dont_R" + target.BaseSkinName) != null)
                {
                    if (!Menus.menu.Item("Dont_R" + target.BaseSkinName).GetValue<bool>())
                    {
                        var minRange = Menus.menu.Item("R_Min_Range").GetValue<Slider>().Value;
                        var minHit = Menus.menu.Item("R_Mec").GetValue<Slider>().Value;

                        if (Get_R_Dmg(target) > target.Health && Player.Distance(target) > minRange)
                        {
                            R.Cast(target, HuyNkItems.packets());
                            return;
                        }

                        foreach (
                            var unit in
                                ObjectManager.Get<Obj_AI_Hero>()
                                    .Where(x => x.IsValidTarget(R.Range) && x.IsVisible)
                                    .OrderBy(x => x.Health))
                        {
                            var pred = R.GetPrediction(unit, true);
                            if (Player.Distance(unit) > minRange && pred.AoeTargetsHitCount >= minHit)
                            {
                                R.Cast(unit, HuyNkItems.packets());
                                //Game.PrintChat("casting");
                                return;
                            }
                        }
                    }
                }
            }
        }

        private void Cast_R_Killable()
        {
            foreach (
                var unit in
                    ObjectManager.Get<Obj_AI_Hero>()
                        .Where(x => x.IsValidTarget(20000) && !x.IsDead && x.IsEnemy)
                        .OrderBy(x => x.Health))
            {
                if (Menus.menu.Item("Dont_R" + unit.BaseSkinName) != null)
                {
                    if (!Menus.menu.Item("Dont_R" + unit.BaseSkinName).GetValue<bool>())
                    {
                        var health = unit.Health + unit.HPRegenRate * 3 + 25;
                        if (Get_R_Dmg(unit) > health)
                        {
                            R.Cast(unit, HuyNkItems.packets());
                            return;
                        }
                    }
                }
            }
        }

        private float Get_R_Dmg(Obj_AI_Hero target)
        {
            double dmg = 0;

            dmg += Player.GetSpellDamage(target, SpellSlot.R);

            var rPred = _r2.GetPrediction(target);
            var collisionCount = rPred.CollisionObjects.Count;

            if (collisionCount >= 7)
                dmg = dmg * .3;
            else if (collisionCount != 0)
                dmg = dmg * ((10 - collisionCount) / 10);

            //Game.PrintChat("collision: " + collisionCount);
            return (float)dmg;
        }

        public void Cast_WE()
        {
            if (W.IsReady() && E.IsReady())
            {
                var vec = Player.ServerPosition + Vector3.Normalize(Game.CursorPos - Player.ServerPosition) * E.Range;

                W.Cast(vec);
                E.Cast(vec);
            }
        }

        public void AutoQ()
        {
            var target = LeagueSharp.Common.TargetSelector.GetTarget(Q.Range, LeagueSharp.Common.TargetSelector.DamageType.Physical);

            if (target != null)
            {
                if (Q.GetPrediction(target).Hitchance >= HitChance.High &&
                    (target.HasBuffOfType(BuffType.Stun) || target.HasBuffOfType(BuffType.Snare)) &&
                    Menus.menu.Item("Auto_Q_Slow").GetValue<bool>())
                    Q.Cast(target, HuyNkItems.packets());
                if (target.HasBuffOfType(BuffType.Slow) && Menus.menu.Item("Auto_Q_Immobile").GetValue<bool>())
                    Q.Cast(target, HuyNkItems.packets());
            }
        }

        public void ForceR()
        {
            var target = LeagueSharp.Common.TargetSelector.GetTarget(R.Range, LeagueSharp.Common.TargetSelector.DamageType.Magical);
            if (target != null && R.GetPrediction(target).Hitchance >= HitChance.High)
                R.Cast(target, HuyNkItems.packets());
        }

        public override void Game_OnGameUpdate(EventArgs args)
        {
            //check if player is dead
            if (Player.IsDead) return;

            //adjust range
            if (Q.IsReady())
                Q.Range = Menus.menu.Item("Q_Max_Range").GetValue<Slider>().Value;
            if (W.IsReady())
                W.Range = Menus.menu.Item("W_Max_Range").GetValue<Slider>().Value;
            if (R.IsReady())
                R.Range = Menus.menu.Item("R_Max_Range").GetValue<Slider>().Value;

            if (Menus.menu.Item("R_Nearest_Killable").GetValue<KeyBind>().Active)
                Cast_R_Killable();

            if (Menus.menu.Item("Force_R").GetValue<KeyBind>().Active)
                ForceR();

            if (Menus.menu.Item("Misc_Use_WE").GetValue<KeyBind>().Active)
            {
                Cast_WE();
            }

            AutoQ();

            if (Menus.menu.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            else
            {
                if (Menus.menu.Item("LaneClearActive").GetValue<KeyBind>().Active)
                    Farm();

                if (Menus.menu.Item("HarassActiveT").GetValue<KeyBind>().Active)
                    Harass();

                if (Menus.menu.Item("HarassActive").GetValue<KeyBind>().Active)
                    Harass();
            }
        }

        public override void Drawing_OnDraw(EventArgs args)
        {
            if (Menus.menu.Item("Draw_Disabled").GetValue<bool>())
                return;

            if (Menus.menu.Item("Draw_Q").GetValue<bool>())
                if (Q.Level > 0)
                    Utility.DrawCircle(Player.Position, Q.Range, Q.IsReady() ? Color.Green : Color.Red);
            if (Menus.menu.Item("Draw_W").GetValue<bool>())
                if (W.Level > 0)
                    Utility.DrawCircle(Player.Position, W.Range, W.IsReady() ? Color.Green : Color.Red);

            if (Menus.menu.Item("Draw_E").GetValue<bool>())
                if (E.Level > 0)
                    Utility.DrawCircle(Player.Position, E.Range, E.IsReady() ? Color.Green : Color.Red);

            if (Menus.menu.Item("Draw_R").GetValue<bool>())
                if (R.Level > 0)
                    Utility.DrawCircle(Player.Position, R.Range, R.IsReady() ? Color.Green : Color.Red);

            if (Menus.menu.Item("Draw_R_Killable").GetValue<bool>() && R.IsReady())
            {
                foreach (
                    var unit in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(x => x.IsValidTarget(20000) && !x.IsDead && x.IsEnemy)
                            .OrderBy(x => x.Health))
                {
                    var health = unit.Health + unit.HPRegenRate * 3 + 25;
                    if (Get_R_Dmg(unit) > health)
                    {
                        Vector2 wts = Drawing.WorldToScreen(unit.Position);
                        Drawing.DrawText(wts[0] - 20, wts[1], Color.Red, "KILL HIM NOWWWWWWWWWWWW!!!");


                    }
                }
            }
        }

    }


}