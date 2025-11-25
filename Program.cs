using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace NuclearX
{
    class Program
    {
        static ThreatEngine engine = new ThreatEngine();
        static SensorSystem sensors = new SensorSystem();
        static IncidentLogger logger = new IncidentLogger("incident_report.txt");
        static AttackSimulator attacker = new AttackSimulator();

        static List<string> scenarios = new List<string>
        {
            "meltdown", "coolantfailure", "radiation",
            "cyberattack", "sensorhack", "overpressure",
            "blackout", "sabotage", "explosion"
        };

        static void Main(string[] args)
        {
            Console.Title = "NuclearX";
            NuclearXHeader();

            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("Commands:");
            Console.WriteLine("start | stop | status | simulate <scenario> | attack | exit");
            Console.ResetColor();
            Console.WriteLine();

            bool running = false;

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
                Console.Write("NuclearX> ");
                Console.ResetColor();
                string input = Console.ReadLine().ToLower();

                if (input == "start")
                {
                    running = true;
                    Console.WriteLine("[SYSTEM] Monitoring Started.");
                }
                else if (input == "stop")
                {
                    running = false;
                    Console.WriteLine("[SYSTEM] Monitoring Stopped.");
                }
                else if (input == "status")
                {
                    DisplayStatus();
                }
                else if (input.StartsWith("simulate"))
                {
                    string[] parts = input.Split(' ');
                    if (parts.Length == 1)
                    {
                        Console.WriteLine("Available scenarios:");
                        foreach (var s in scenarios)
                            Console.WriteLine(" - " + s);
                    }
                    else if (parts.Length == 2)
                    {
                        SimulateScenario(parts[1]);
                    }
                    else
                    {
                        Console.WriteLine("Usage: simulate <scenario>");
                    }
                }
                else if (input == "attack")
                {
                    attacker.TriggerCyberAttack();
                }
                else if (input == "exit")
                {
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine("Shutting down NuclearX...");
                    Console.ResetColor();
                    break;
                }

                if (running)
                {
                    sensors.UpdateReadings();
                    engine.Analyze(sensors);
                    DisplayLivePanel();
                    logger.LogState(sensors, engine);
                }

                Thread.Sleep(500);
            }
        }

        // ========================= Banner with Typewriter =========================
        static void NuclearXHeader()
        {
            string[] xBanner = new string[]
            {
                "██   ██",
                " ██ ██ ",
                "  ███  ",
                " ██ ██ ",
                "██   ██"
            };

            // X حرف حرف
            Console.ForegroundColor = ConsoleColor.DarkRed;
            foreach (var line in xBanner)
            {
                foreach (char c in line)
                {
                    Console.Write(c);
                    Thread.Sleep(30);
                }
                Console.WriteLine();
            }

            string name = "NuclearX";
            string poweredBy = "Powered by Eng. Ezz Eldeen";

            // النص تحت حرف حرف
            Console.ForegroundColor = ConsoleColor.Red;
            foreach (char c in name)
            {
                Console.Write(c);
                Thread.Sleep(50);
            }
            Console.WriteLine();

            foreach (char c in poweredBy)
            {
                Console.Write(c);
                Thread.Sleep(20);
            }
            Console.WriteLine("\n");

            Console.ResetColor();
        }

        static void DisplayStatus()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n=== System Readings ===");
            Console.WriteLine($"Temp: {sensors.Temp:F2} °C");
            Console.WriteLine($"Pressure: {sensors.Pressure:F2} atm");
            Console.WriteLine($"Radiation: {sensors.Radiation:F2} mSv");
            Console.WriteLine($"Neutron Flux: {sensors.Flux:F2}");
            Console.WriteLine($"Cooling Pump: {sensors.CoolingStatus}");
            Console.WriteLine("=========================\n");
            Console.ResetColor();
        }

        static void DisplayLivePanel()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine("\n────────── Live Reactor Panel ──────────");

            Graph("Temperature", sensors.Temp, 200, 900);
            Graph("Pressure", sensors.Pressure, 50, 300);
            Graph("Radiation", sensors.Radiation, 1, 500);
            Graph("Flux", sensors.Flux, 100, 1000);

            Console.ResetColor();

            if (engine.ThreatScore > 70)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"HIGH RISK DETECTED — SCORE: {engine.ThreatScore:F0}");
            }

            if (engine.ThreatScore > 90)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("CRITICAL RISK — AUTOMATED SHUTDOWN INITIATED");
            }

            Console.ResetColor();
        }

        static void Graph(string name, double value, double min, double max)
        {
            int width = 40;
            int bars = (int)(((value - min) / (max - min)) * width);
            bars = Math.Max(0, Math.Min(width, bars));

            ConsoleColor c = ConsoleColor.Green;

            switch (name)
            {
                case "Temperature":
                    if (value >= 700) c = ConsoleColor.Red;
                    else if (value >= 500) c = ConsoleColor.Yellow;
                    break;
                case "Pressure":
                    if (value > 200) c = ConsoleColor.Red;
                    else if (value >= 150) c = ConsoleColor.Yellow;
                    break;
                case "Radiation":
                    if (value > 200) c = ConsoleColor.Red;
                    else if (value >= 100) c = ConsoleColor.Yellow;
                    break;
                case "Flux":
                    if (value > 800) c = ConsoleColor.Red;
                    else if (value >= 500) c = ConsoleColor.Yellow;
                    break;
            }

            Console.ForegroundColor = c;
            Console.Write($"{name.PadRight(12)}: ");
            Console.WriteLine(new string('|', bars));
            Console.ResetColor();
        }

        static void SimulateScenario(string s)
        {
            switch (s)
            {
                case "meltdown":
                    Console.WriteLine("Meltdown scenario triggered!");
                    sensors.Temp += 300;
                    sensors.Pressure += 80;
                    sensors.Radiation += 200;
                    break;
                case "coolantfailure":
                    Console.WriteLine("Cooling system failure triggered!");
                    sensors.CoolingStatus = "OFFLINE";
                    sensors.Temp += 150;
                    break;
                case "radiation":
                    Console.WriteLine("Radiation spike simulated!");
                    sensors.Radiation += 500;
                    break;
                case "cyberattack":
                    Console.WriteLine("Cyberattack simulated!");
                    sensors.Temp += 50;
                    sensors.Pressure += 20;
                    break;
                case "sensorhack":
                    Console.WriteLine("Sensor hack simulated!");
                    sensors.Temp += 30;
                    sensors.Radiation += 30;
                    break;
                case "overpressure":
                    Console.WriteLine("Overpressure simulated!");
                    sensors.Pressure += 100;
                    break;
                case "blackout":
                    Console.WriteLine("Blackout simulated!");
                    sensors.Temp += 100;
                    sensors.CoolingStatus = "OFFLINE";
                    break;
                case "sabotage":
                    Console.WriteLine("Sabotage simulated!");
                    sensors.Temp += 150;
                    sensors.Pressure += 50;
                    break;
                case "explosion":
                    Console.WriteLine("Explosion simulated!");
                    sensors.Temp += 400;
                    sensors.Pressure += 200;
                    sensors.Radiation += 300;
                    break;
                default:
                    Console.WriteLine("Unknown scenario. Type 'simulate' to see all scenarios.");
                    break;
            }
        }
    }

    class SensorSystem
    {
        Random r = new Random();
        public double Temp = 400;
        public double Pressure = 120;
        public double Radiation = 10;
        public double Flux = 300;
        public string CoolingStatus = "ONLINE";

        public void UpdateReadings()
        {
            Temp += r.NextDouble() * 4 - 2;
            Pressure += r.NextDouble() * 2 - 1;
            Radiation += r.NextDouble() * 1 - 0.5;
            Flux += r.NextDouble() * 10 - 5;

            if (CoolingStatus == "OFFLINE")
                Temp += 3;
        }
    }

    class ThreatEngine
    {
        public double ThreatScore = 0;
        public void Analyze(SensorSystem s)
        {
            double score = 0;
            if (s.Temp > 700) score += 40;
            if (s.Pressure > 200) score += 25;
            if (s.Radiation > 200) score += 30;
            if (s.CoolingStatus == "OFFLINE") score += 20;
            if (s.Flux > 800) score += 15;
            ThreatScore = score;
        }
    }

    class IncidentLogger
    {
        private string path;
        public IncidentLogger(string p) => path = p;

        public void LogState(SensorSystem s, ThreatEngine t)
        {
            if (t.ThreatScore < 70) return;
            string log =
$@"[{DateTime.Now}]
Threat Score: {t.ThreatScore}
Temp={s.Temp}, Pressure={s.Pressure}, Radiation={s.Radiation}, Flux={s.Flux}
Cooling={s.CoolingStatus}
---------------------------";
            File.AppendAllText(path, log + "\n");
        }
    }

    class AttackSimulator
    {
        public void TriggerCyberAttack()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("CYBER THREAT DETECTED: Unauthorized command injection attempt!");
            Console.ResetColor();
        }
    }
}