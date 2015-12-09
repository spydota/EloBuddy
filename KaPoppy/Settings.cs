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
            Combo.AddSeparator(0);
            Combo.Add("W", new CheckBox("Use W", false));
            Combo.AddSeparator(0);
            Combo.Add("Ws", new CheckBox("Use W only if less than 40% health", false));
            Combo.AddSeparator(0);
            Combo.Add("E", new CheckBox("Use E"));
            Combo.AddSeparator(0);
            Combo.Add("Es", new CheckBox("Use E only stun", false));
            Combo.Add("FEs", new KeyBind("Use Flash E to stun", false, KeyBind.BindTypes.PressToggle, 'J'));
            Combo.Add("R", new CheckBox("Use R to knockup enemies"));
            Combo.Add("Rm", new Slider("Min enemies to knockup: {0}", 3, 1, 5));
            HarassMenu();
        }
        private static void HarassMenu()
        {
            Harass = Menu.AddSubMenu("Harass");
            Harass.Add("Q", new CheckBox("Use Q"));
            Harass.Add("Qm", new Slider("Q min mana %", 40, 0, 100));
            Harass.Add("W", new CheckBox("Use W", false));
            Harass.Add("Ws", new CheckBox("Use W only if less than 40% health", false));
            Harass.Add("Wm", new Slider("W min mana %", 40, 0, 100));
            Harass.Add("E", new CheckBox("Use E"));
            Harass.Add("Es", new CheckBox("Use E only stun", false));
            LaneclearMenu();
        }
        private static void LaneclearMenu()
        {
            Laneclear = Menu.AddSubMenu("Laneclear");
            Laneclear.Add("Q", new CheckBox("Use Q"));
            Laneclear.Add("Qs", new Slider("Use Q if will hit {0} minions", 3, 1, 10));
            Laneclear.Add("Qm", new Slider("Q min mana %", 40, 0, 100));
            JungleclearMenu();
        }
        private static void JungleclearMenu()
        {
            Jungleclear = Menu.AddSubMenu("Jungleclear");
            Jungleclear.Add("Q", new CheckBox("Use Q"));
            Jungleclear.Add("Qm", new Slider("Use Q min mana", 20, 0, 100));
            Jungleclear.Add("W", new CheckBox("Use W", false));
            Jungleclear.Add("Ws", new CheckBox("Use W only if less than 40% health", false));
            Jungleclear.Add("Wm", new Slider("Use W min mana", 40, 0, 100));
            Jungleclear.Add("E", new CheckBox("Use E"));

            FleeMenu();
        }
        private static void FleeMenu()
        {
            Flee = Menu.AddSubMenu("Flee");
            Flee.Add("E", new CheckBox("Use E to nearest in minion mouse pos"));
            Flee.Add("W", new CheckBox("Use W", false));
            Flee.Add("Ws", new CheckBox("Use W only if less than 40% health", false));
            MiscMenu();
        }
        private static void MiscMenu()
        {
            Misc = Menu.AddSubMenu("Misc");
            Misc.AddGroupLabel("Misc");
            Misc.Add("percent", new Slider("Stun chance %", 40, 1, 100));
            Misc.Add("stun", new KeyBind("Force stun selected target", false, KeyBind.BindTypes.HoldActive, 'Z'));
            Misc.AddLabel("How it works: Orbwalk to nearest position where you");
            Misc.AddLabel("can stun selected target");
            Misc.AddLabel("(If flash E is enabled, it will Flash E)");
            Misc.AddSeparator();
            Misc.AddGroupLabel("Drawings");
            Misc.Add("dQ", new CheckBox("Draw Q"));
            Misc.Add("dE", new CheckBox("Draw E"));
            Misc.Add("dR", new CheckBox("Draw R"));
            Misc.AddGroupLabel("Killsteal");
            Misc.Add("Q", new CheckBox("Q Killsteal"));
            Misc.Add("E", new CheckBox("E Killsteal"));
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
                get { return CastCheckbox(Combo, "W"); }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Combo, "Ws"); }
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
            public static bool UseR
            {
                get { return CastCheckbox(Combo, "R"); }
            }
            public static int RMin
            {
                get { return CastSlider(Combo, "Rm"); }
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
                get { return CastCheckbox(Harass, "W")  &&
                        CastSlider(Harass, "Wm") <= myHero.ManaPercent; }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Harass, "Ws"); }
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
                get { return CastCheckbox(Laneclear, "Q") && 
                        CastSlider(Laneclear, "Qm") <= myHero.ManaPercent; }
            }
            public static int Qmin
            {
                get { return CastSlider(Laneclear, "Qs"); }
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
                    return CastCheckbox(Jungleclear, "W") &&
                      CastSlider(Jungleclear, "Wm") <= myHero.ManaPercent;
                }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Jungleclear, "Ws"); }
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
                get { return CastCheckbox(Flee, "W"); }
            }
            public static bool Whealth
            {
                get { return CastCheckbox(Flee, "Ws"); }
            }
        }
        public static class MiscSettings
        {
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
            public static bool AntiGapcloser
            {
                get { return CastCheckbox(WSettings, "W"); }
            }
            public static int StunPercent
            {
                get { return CastSlider(Misc, "percent"); }
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
            public static bool StunTarget
            {
                get { return CastKeybind(Misc, "stun"); }
            }
        }
    }
}
