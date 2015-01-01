using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeagueSharp;
using LeagueSharp.Common;

using SharpDX;

using Color = System.Drawing.Color;

namespace HuyNKSeries.Champ
{
    class template : Champion
    {
        
        public template()
        {
            SetUpSpells();
            LoadMenu();

          
        }
        public void SetUpSpells()
        {
		

        }

        public void LoadMenu()
        {
            //Create Menu
            Menu key = new Menu("Key", "Key");
           
                key.AddItem(new MenuItem("ComboActive", "Combo!").SetValue(new KeyBind(32, KeyBindType.Press)));
                key.AddItem(new MenuItem("HarassActive", "Harass!").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));
                key.AddItem(new MenuItem("HarassActiveT", "Harass (toggle)!").SetValue(new KeyBind("N".ToCharArray()[0], KeyBindType.Toggle)));
                Menus.menu.AddSubMenu(key);
            //Farm
            Menu farm = new Menu("Farm","Farm");
            farm.AddItem(new MenuItem("UseQF", "Use Q Farm").SetValue(false));
            Menus.menu.AddSubMenu(farm);
            // combo
            Menu combo = new Menu("Combo", "combo");
            combo.AddItem(new MenuItem("UseQC", "Use Q").SetValue(true));
            combo.AddItem(new MenuItem("UseEC", "Use E").SetValue(true));
            Menus.menu.AddSubMenu(combo);
			//Misc
             Menu misc = new Menu("Misc", "misc");
			misc.AddItem(new MenuItem("UseET", "Use E (Toggle)").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            misc.AddItem(new MenuItem("UseEInterrupt", "Use E To Interrupt").SetValue(true));
            misc.AddItem(new MenuItem("PushDistance", "E Push Distance").SetValue(new Slider(425, 475, 300)));
            Menus.menu.AddSubMenu(misc);

        }
         
        

        public override void Game_OnGameUpdate(EventArgs args)
        {
            if (Menus.menu.Item("ComboActive").GetValue<KeyBind>().Active)
            {
                Combo();
            }
            if (Menus.menu.Item("HarassActive").GetValue<KeyBind>().Active)
            {
                Harass();
            }

            if(Menus.menu.Item("UseQF").GetValue<bool>())
            {
            }

            if (Menus.menu.Item("UseEInterrupt").GetValue<KeyBind>().Active)
            {
            }

        }

        public void farm()
        {
           
        }
       
        private void Harass()
        {
           
        }

        private void Combo()
        {
           
        }

        

        
    }
}

