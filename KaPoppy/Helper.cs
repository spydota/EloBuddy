using EloBuddy;
using EloBuddy.SDK;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;
using SharpDX;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using Version = System.Version;
using Color = System.Drawing.Color;

namespace KaPoppy
{
    public class Helper
    {
        public static ColorBGRA Green = new ColorBGRA(Color.Green.R, Color.Green.G, Color.Green.B, Color.Green.A);
        public static ColorBGRA Red = new ColorBGRA(Color.Red.R, Color.Red.G, Color.Red.B, Color.Red.A);
        public static bool CastCheckbox(Menu obj, string value)
        {
            return obj[value].Cast<CheckBox>().CurrentValue;
        }
        public static bool CastKeybind(Menu obj, string value)
        {
            return obj[value].Cast<KeyBind>().CurrentValue;
        }
        public static int CastSlider(Menu obj, string value)
        {
            return obj[value].Cast<Slider>().CurrentValue;
        }

        public static AIHeroClient myHero { get { return ObjectManager.Player; } }
        public static void CheckForUpdates()
        {
            string RawVersion = new WebClient().DownloadString("https://raw.githubusercontent.com/Phandaros/EloBuddy/master/" + Assembly.GetExecutingAssembly().GetName().Name + "/Properties/AssemblyInfo.cs");
            var Try = new Regex(@"\[assembly\: AssemblyVersion\(""(\d{1,})\.(\d{1,})\.(\d{1,})\.(\d{1,})""\)\]").Match(RawVersion);
            if (Try.Success)
            {
                if (new Version(string.Format("{0}.{1}.{2}.{3}", Try.Groups[1], Try.Groups[2], Try.Groups[3], Try.Groups[4])) > Assembly.GetExecutingAssembly().GetName().Version)
                {
                    Chat.Print("You have an older version of " + Assembly.GetExecutingAssembly().GetName().Name + ", please update :)", Color.Red);
                }
                else
                {
                    Chat.Print(Assembly.GetExecutingAssembly().GetName().Name + " loaded, Enjoy free elo Kappa", Color.AliceBlue);
                }
            }
        }
        public static bool IsWall(Vector2 pos)
        {
            if (!pos.IsValid())
                return false;

            var ipos = pos.ToNavMeshCell().CollFlags;
            if (ipos.HasFlag(CollisionFlags.Wall) || ipos.HasFlag(CollisionFlags.Building))
            {
                return true;
            }
            return false;
        }
        public static bool IsWall(Vector3 pos)
        {
            if (!pos.IsValid())
                return false;

            var ipos = pos.ToNavMeshCell().CollFlags;
            if (ipos.HasFlag(CollisionFlags.Wall) || ipos.HasFlag(CollisionFlags.Building))
            {
                return true;
            }
            return false;
        }
        public static float GetDamage(Obj_AI_Base target, SpellSlot slot)
        {
            var spellLevel = Player.Instance.Spellbook.GetSpell(slot).Level;
            float damage = 0;
            if (spellLevel == 0)
            {
                return 0;
            }
            spellLevel--;

            switch (slot)
            {
                case SpellSlot.Q:
                    damage = new float[] { 30, 65, 100, 135, 170 }[spellLevel] + 0.65f * Player.Instance.TotalAttackDamage + (0.06f * target.MaxHealth);
                    break;

                case SpellSlot.W:
                    damage = new float[] { 70, 110, 150, 190, 230 }[spellLevel] + 0.7f * Player.Instance.TotalMagicalDamage;
                    break;

                case SpellSlot.E:
                    damage = new float[] { 50, 70, 90, 110, 130 }[spellLevel] + 0.5f * Player.Instance.TotalAttackDamage;
                    break;

                case SpellSlot.R:
                    damage = new float[] { 200, 300, 400 }[spellLevel] + 0.9f * Player.Instance.TotalAttackDamage;
                    break;
            }
            return myHero.CalculateDamageOnUnit(target, DamageType.Physical, damage) - 30;
        }
    }
}