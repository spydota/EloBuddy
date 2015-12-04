using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using System.Reflection;
namespace KaPoppy
{
    class Settings : Helper
    {
        public static Menu Menu, Combo, Harass, Laneclear, Jungleclear, Flee, Misc, WSettings;
        public static void Init()
        {
            Menu = MainMenu.AddMenu("KaPoppy", "menu");
            Menu.AddGroupLabel("KaPoppy by Capitão Addon");
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddLabel("Current Version: " + Assembly.GetExecutingAssembly().GetName().Version);

            ComboMenu();
        }
        private static void ComboMenu()
        {
            Combo = Menu.AddSubMenu("Combo");
            Combo.Add("Q", new CheckBox("Use Q"));
            Combo.Add("W", new Slider("Use W if less than {0} stacks", 3, 0, 10));
            Combo.Add("E", new CheckBox("Use E"));
            Combo.Add("Es", new CheckBox("Use E only stun"));
            Combo.Add("FEs", new KeyBind("Use Flash E to stun", false, KeyBind.BindTypes.PressToggle, 'U'));
            HarassMenu();
        }
        private static void HarassMenu()
        {
            Harass = Menu.AddSubMenu("Harass");
            Harass.Add("Q", new CheckBox("Use Q"));
            Harass.Add("Qm", new Slider("Q min mana %", 40, 0, 100));
            Harass.Add("W", new Slider("Use W if less than {0} stacks", 5, 0, 10));
            Harass.Add("Wm", new Slider("W min mana %", 40, 0, 100));
            Harass.Add("E", new CheckBox("Use E"));
            Harass.Add("Es", new CheckBox("Use E only stun", false));
            Harass.Add("Auto", new KeyBind("Auto harass", false, KeyBind.BindTypes.PressToggle, 'Z'));
            LaneclearMenu();
        }
        private static void LaneclearMenu()
        {
            Laneclear = Menu.AddSubMenu("Laneclear");
            Laneclear.Add("Q", new Slider("Use Q if will hit {0} minions", 3, 1, 10));
            Laneclear.Add("Qm", new Slider("Q min mana %", 40, 0, 100));
            JungleclearMenu();
        }
        private static void JungleclearMenu()
        {
            Jungleclear = Menu.AddSubMenu("Jungleclear");
            Jungleclear.Add("Q", new CheckBox("Use Q"));
            Jungleclear.Add("Qm", new Slider("Use Q min mana", 20, 0, 100));
            Jungleclear.Add("W", new Slider("Use W if less than {0} stacks", 5, 0, 10));
            Jungleclear.Add("Wm", new Slider("Use W min mana"));
            Jungleclear.Add("E", new CheckBox("Use E", false));

            FleeMenu();
        }
        private static void FleeMenu()
        {
            Flee = Menu.AddSubMenu("Flee");
            Flee.Add("E", new CheckBox("Use E to nearest in minion mouse pos"));
            Flee.Add("W", new Slider("Use W if less than {0} stacks", 6, 0, 10));
            MiscMenu();
        }
        private static void MiscMenu()
        {
            Misc = Menu.AddSubMenu("Misc");
            Misc.AddGroupLabel("Drawings");
            Misc.Add("dQ", new CheckBox("Draw Q"));
            Misc.Add("dE", new CheckBox("Draw E"));
            Misc.Add("dR", new CheckBox("Draw R"));
            Misc.Add("dEp", new CheckBox("Draw E stun position"));
            Misc.AddGroupLabel("Killsteal");
            Misc.Add("Q", new CheckBox("Q Killsteal"));
            Misc.Add("E", new CheckBox("E Killsteal", false));
            Misc.Add("R", new CheckBox("R Killsteal"));

            WSettingsMenu();
        }
        public static void WSettingsMenu()
        {
            WSettings = Menu.AddSubMenu("AntiGapcloser", "wmenu");

            WSettings.Add("W", new CheckBox("Use W"));
            WSettings.AddGroupLabel("Use W in: ");
            foreach (var dash in DashSpells.DashSpells.Dashes)
            {
                foreach (AIHeroClient enemy in EntityManager.Heroes.Enemies)
                {
                    if (enemy.Hero == dash.champ)
                    {
                        WSettings.Add("w" + dash.champname + dash.spellKey, new CheckBox(dash.champname + " " + dash.spellKey, dash.enabled));
                    }
                    if (enemy.Hero == Champion.Rengar)
                    {
                        WSettings.Add("AntiRengar", new CheckBox("Anti rengar"));
                        Chat.Print("Anti rengo loaded");
                    }
                }
            }

        }
        public static class ComboSettings
        {
            public static bool UseQ
            {
                get { return CastCheckbox(Combo, "Q"); }
            }
            public static bool UseW
            {
                get { return CastSlider(Combo, "W") >= Lib.PassiveCount(); }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Combo, "E"); }
            }
            public static bool UseEs
            {
                get { return CastCheckbox(Combo, "Es"); }
            }
            public static bool UseFlashE
            {
                get { return CastKeybind(Combo, "FEs"); }
            }
        }
        public static class HarassSettings
        {
            public static bool UseQ
            {
                get { return CastCheckbox(Harass, "Q") &&
                        CastSlider(Harass, "Qm") <= myHero.ManaPercent;
                }
            }
            public static bool UseW
            {
                get { return CastSlider(Harass, "W") >= Lib.PassiveCount() &&
                        CastSlider(Harass, "Wm") <= myHero.ManaPercent; }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Harass, "E"); }
            }
            public static bool UseEs
            {
                get { return CastCheckbox(Harass, "Es"); }
            }
            
        }
        public static class LaneclearSettings
        {
            public static bool UseQ
            {
                get { return CastSlider(Laneclear, "Qm") <= myHero.ManaPercent; }
            }
            public static int Qmin
            {
                get { return CastSlider(Laneclear, "Q"); }
            }
        }
        public static class JungleclearSettings
        {
            public static bool UseQ
            {
                get {
                    return CastCheckbox(Jungleclear, "Q") &&
                      CastSlider(Jungleclear, "Qm") <= myHero.ManaPercent;
                }
            }
            public static bool UseW
            {
                get {
                    return CastSlider(Jungleclear, "W") >= Lib.PassiveCount() &&
                      CastSlider(Jungleclear, "Wm") <= myHero.ManaPercent;
                }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Jungleclear, "E"); }
            }
        }
        public static class FleeSettings
        {
            public static bool UseE
            {
                get { return CastCheckbox(Flee, "E"); }
            }
            public static bool UseW
            {
                get { return CastSlider(Flee, "W") >= Lib.PassiveCount(); }
            }
        }
        public static class MiscSettings
        {
            public static bool drawEp
            {
                get { return CastCheckbox(Misc, "dEp"); }
            }
            public static bool drawQ
            {
                get { return CastCheckbox(Misc, "dQ"); }
            }
            public static bool drawE
            {
                get { return CastCheckbox(Misc, "dE"); }
            }
            public static bool drawR
            {
                get { return CastCheckbox(Misc, "dR"); }
            }
            public static bool UseQ
            {
                get { return CastCheckbox(Misc, "Q"); }
            }
            public static bool UseE
            {
                get { return CastCheckbox(Misc, "E"); }
            }
            public static bool UseR
            {
                get { return CastCheckbox(Misc, "R"); }
            }
            public static bool AutoHarass
            {
                get { return CastKeybind(Harass, "Auto"); }
            }
            public static bool AntiGapcloser
            {
                get { return CastCheckbox(WSettings, "W"); }
            }
            public static bool WEnabled(Champion champ, SpellSlot slot)
            {
                foreach (var dash in DashSpells.DashSpells.Dashes)
                {
                    if (dash.champ == champ)
                    {
                        if (dash.spellKey == slot)
                        {
                            return CastCheckbox(WSettings, "w" + dash.champname + dash.spellKey);
                        }
                    }
                }
                return false;
            }
            public static bool AntiRengo
            {
                get { return CastCheckbox(WSettings, "AntiRengar"); }
            }
        }
    }
}