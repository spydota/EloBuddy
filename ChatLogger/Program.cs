using System;
using System.Diagnostics;
using System.IO;
using EloBuddy;
using EloBuddy.SDK.Events;
using EloBuddy.Sandbox;
using EloBuddy.SDK.Menu;
using EloBuddy.SDK.Menu.Values;

namespace Chat_Logger
{
    public static class Program
    {
        public static string LogFile;
        static Menu Menu;
        private static string CalloutsFile;
        public static Stopwatch Stopwatch;
        static void Main(string[] args)
        {
            Loading.OnLoadingComplete += Loading_OnLoadingComplete; ;
        }

        private static void Loading_OnLoadingComplete(EventArgs args)
        {
            LogFile = SandboxConfig.DataDirectory + "\\Chat Logs\\" + DateTime.Now.ToString("yy-MM-dd") + " " + DateTime.Now.ToString("HH-mm") + " - " + Player.Instance.ChampionName + ".txt";
            CalloutsFile = SandboxConfig.DataDirectory + "\\Callouts\\" + DateTime.Now.ToString("yy-MM-dd") + " " + DateTime.Now.ToString("HH-mm") + " - " + Player.Instance.ChampionName + " Callout" + ".txt";
            Stopwatch = new Stopwatch();
            Stopwatch.Start();
            if (!Directory.Exists(SandboxConfig.DataDirectory + "\\Chat Logs\\"))
            {
                Directory.CreateDirectory(SandboxConfig.DataDirectory + "\\Chat Logs\\");
            }
            if (!Directory.Exists(SandboxConfig.DataDirectory + "\\Callouts\\"))
            {
                Directory.CreateDirectory(SandboxConfig.DataDirectory + "\\Callouts\\");
            }

            Menu = MainMenu.AddMenu("Chat Logger", "eooqqqqqqq");
            Menu.AddGroupLabel("Chat logger made by Capitao Addon");
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddSeparator();
            Menu.AddLabel("What it does?");
            Menu.AddLabel("It exports your in-game chat to a .txt in your %AppData%, usefull to know when you got");
            Menu.AddLabel("called out while botting");

            var q = Menu.Add("logall", new CheckBox("Log chat"));
            var w = Menu.Add("logcall", new CheckBox("Log callouts"));
            q.OnValueChange += delegate
            (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs changArgs)
            {
                if (changArgs.NewValue == true)
                {
                    if (!File.Exists(LogFile))
                    {
                        File.Create(LogFile);
                    }
                }
            };
            w.OnValueChange += delegate
            (ValueBase<bool> sender, ValueBase<bool>.ValueChangeArgs changArgs)
            {
                if (changArgs.NewValue == true)
                {
                    if (!File.Exists(CalloutsFile))
                    {
                        File.Create(CalloutsFile);
                    }
                }
            };
            if (Menu["logall"].Cast<CheckBox>().CurrentValue)
            {
                if (!File.Exists(LogFile))
                {
                    File.Create(LogFile);
                }
            }
            if (Menu["logcall"].Cast<CheckBox>().CurrentValue)
            {
                if (!File.Exists(CalloutsFile))
                {
                    File.Create(CalloutsFile);
                }
            }
            Chat.Print("The chat log for this game can be found at " + LogFile, System.Drawing.Color.White);
            Chat.OnMessage += OnChat;
        }
        private static void OnChat(AIHeroClient sender, ChatMessageEventArgs args)
        {
            if (Menu["logall"].Cast<CheckBox>().CurrentValue)
            {
                using (var sw = new StreamWriter(LogFile, true))
                {
                    long elapsedTime = Stopwatch.ElapsedMilliseconds;
                    long elapsedMinutes = elapsedTime / 60000;
                    long elapsedSeconds = 0;

                    Math.DivRem(elapsedTime, 60000, out elapsedSeconds);
                    elapsedSeconds /= 1000;

                    sw.WriteLine("[" + elapsedMinutes + ":" + (elapsedSeconds < 10 ? "0" : "") + elapsedSeconds + "] " + args.Message.ToString()
                    .Replace("<font color=\"#ff3333\">", "")
                    .Replace("<font color=\"#40c1ff\">", "")
                    .Replace("<font color=\"#ffffff\">", "")
                    .Replace("</font>", ""));
                    sw.Close();
                }
            }
            if (Menu["logcall"].Cast<CheckBox>().CurrentValue)
            {
                var msg = args.Message.ToLower();
                if (msg.Contains("hack") || msg.Contains("autoplay") || msg.Contains("macro") || msg.Contains("script") || msg.Contains("report") || msg.Contains("ticket") || msg.Contains("bol") ||
                    msg.Contains("leaguesharp") || msg.Contains("elobuddy"))
                {
                    using (var cw = new StreamWriter(CalloutsFile, true))
                    {
                        long elapsedTime = Stopwatch.ElapsedMilliseconds;
                        long elapsedMinutes = elapsedTime / 60000;
                        long elapsedSeconds = 0;

                        Math.DivRem(elapsedTime, 60000, out elapsedSeconds);
                        elapsedSeconds /= 1000;
                        cw.WriteLine("[" + elapsedMinutes + ":" + (elapsedSeconds < 10 ? "0" : "") + elapsedSeconds + "] " + args.Message.ToString()
                        .Replace("<font color=\"#ff3333\">", "")
                        .Replace("<font color=\"#40c1ff\">", "")
                        .Replace("<font color=\"#ffffff\">", "")
                        .Replace("</font>", ""));
                        cw.Close();
                    }
                }
            }
        }
    }
}
