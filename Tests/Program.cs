using System;
using System.Globalization;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using ManyHappyReturns;
using RimWorld;
using Verse;

internal static class Program
{
    private static int failures;
    private static int passed;
    private static string output;

    private static int Main(string[] args)
    {
        string managed = args.Length > 0 ? args[0] : @"C:\Program Files (x86)\Steam\steamapps\common\RimWorld\RimWorldWin64_Data\Managed";
        output = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "fixtures"));
        Directory.CreateDirectory(output);
        AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
        {
            string path = Path.Combine(managed, new AssemblyName(e.Name).Name + ".dll");
            return File.Exists(path) ? Assembly.LoadFrom(path) : null;
        };
        CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
        return Run();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static int Run()
    {
        Console.WriteLine("Runtime assembly: " + typeof(Scribe).Assembly.Location);
        Console.WriteLine("Mod assembly: " + typeof(ManyHappyReturnsSettings).Assembly.Location);
        Test("Clean settings defaults", () => CheckDefaults(new ManyHappyReturnsSettings()));
        Test("Reset restores all settings", () =>
        {
            var s = new ManyHappyReturnsSettings { morningLetter = false, forgottenThought = false, moodFactor = 2f };
            s.Reset();
            CheckDefaults(s);
        });
        Test("Finite mood boundaries", () =>
        {
            foreach (var pair in new[] { (-10f, .5f), (.5f, .5f), (1.35f, 1.35f), (2f, 2f), (100f, 2f) })
                Equal(pair.Item2, new ManyHappyReturnsSettings { moodFactor = pair.Item1 }.ClampedMoodFactor);
        });
        Test("Non-finite mood values recover to default", () =>
        {
            foreach (float value in new[] { float.NaN, float.NegativeInfinity, float.PositiveInfinity })
                Equal(1f, new ManyHappyReturnsSettings { moodFactor = value }.ClampedMoodFactor);
        });
        Test("Actual Scribe settings round-trip", () =>
        {
            var saved = new ManyHappyReturnsSettings { morningLetter = false, forgottenThought = false, moodFactor = 1.75f };
            string path = Path.Combine(output, "settings.xml");
            Scribe.saver.InitSaving(path, "settings");
            saved.ExposeData();
            Scribe.saver.FinalizeSaving();
            var loaded = Load(path);
            Equal(false, loaded.morningLetter);
            Equal(false, loaded.forgottenThought);
            Equal(1.75f, loaded.moodFactor);
        });
        Test("Old empty settings use defaults", () => CheckDefaults(LoadXml("<settings />")));
        Test("Old partial settings retain stored toggle and default new fields", () =>
        {
            var s = LoadXml("<settings><morningLetter>False</morningLetter></settings>");
            Equal(false, s.morningLetter);
            Equal(true, s.forgottenThought);
            Equal(1f, s.moodFactor);
        });
        Test("Out-of-range saved factor is normalized after loading", () =>
        {
            Equal(.5f, LoadXml("<settings><moodFactor>-3</moodFactor></settings>").moodFactor);
            Equal(2f, LoadXml("<settings><moodFactor>12</moodFactor></settings>").moodFactor);
        });
        Test("NaN saved factor is normalized after loading", () =>
            Equal(1f, LoadXml("<settings><moodFactor>NaN</moodFactor></settings>").moodFactor));
        Test("Score thresholds produce documented five grades", () =>
        {
            int[] expected = { 0, 0, 1, 1, 2, 2, 3, 3, 3, 4, 4, 4 };
            for (int score = 0; score < expected.Length; score++) Equal(expected[score], BirthdayUtility.StageForScore(score));
        });
        Test("Wish points follow documented count bands", () =>
        {
            int[] expected = { 0, 1, 2, 3, 3, 4, 4, 4 };
            for (int count = 0; count < expected.Length; count++) Equal(expected[count], BirthdayUtility.PointsForWishers(count));
            Equal(4, BirthdayUtility.PointsForWishers(100));
        });
        Test("Null pawns safely decline birthday eligibility", () =>
        {
            Equal(false, BirthdayUtility.IsBirthdayToday(null));
            Equal(false, BirthdayUtility.CanCelebrate(null));
            Equal(false, BirthdayUtility.AlreadyWished(null, null));
            Equal(0, BirthdayUtility.CountWishers(null, out bool close));
            Equal(false, close);
        });
        Test("Invalid interaction pairs have no weight", () =>
            Equal(0f, new InteractionWorker_BirthdayWish().RandomSelectionWeight(null, null)));
        Test("Shortcut leaves native visibility available for customization", () =>
        {
            Equal(typeof(MainButtonWorker), typeof(MainButtonWorker_Settings).GetProperty("Visible").GetMethod.DeclaringType);
            Equal(typeof(MainButtonWorker), typeof(MainButtonWorker_Settings).GetProperty("Disabled").GetMethod.DeclaringType);
        });
        Test("New memories capture mood scale without changing existing memories", () =>
        {
            var def = new ThoughtDef
            {
                defName = "MHR_TestMemory",
                thoughtClass = typeof(Thought_Memory),
                stages = new List<ThoughtStage> { new ThoughtStage { baseMoodEffect = 4f } }
            };
            var make = typeof(GameComponent_Birthdays).GetMethod("MakeMemory", BindingFlags.Static | BindingFlags.NonPublic);
            var settings = ManyHappyReturnsMod.Settings;
            settings.moodFactor = .5f;
            var first = (Thought_Memory)make.Invoke(null, new object[] { def, 0 });
            settings.moodFactor = 2f;
            var second = (Thought_Memory)make.Invoke(null, new object[] { def, 0 });
            Equal(.5f, first.moodPowerFactor);
            Equal(2f, second.moodPowerFactor);
            Equal(4f, def.stages[0].baseMoodEffect);
            settings.Reset();
        });
        Console.WriteLine($"RESULT: {passed} passed, {failures} failed");
        return failures == 0 ? 0 : 1;
    }

    private static ManyHappyReturnsSettings LoadXml(string xml)
    {
        string path = Path.Combine(output, "input.xml");
        File.WriteAllText(path, xml);
        return Load(path);
    }

    private static ManyHappyReturnsSettings Load(string path)
    {
        var loaded = new ManyHappyReturnsSettings();
        Scribe.loader.InitLoading(path);
        loaded.ExposeData();
        // Test this object's real Scribe read/write callbacks without starting Unity's
        // whole-save cross-reference/profiler pipeline. It requires an active game process.
        Scribe.loader.ForceStop();
        typeof(Scribe).GetField("mode", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static)
            .SetValue(null, LoadSaveMode.PostLoadInit);
        loaded.ExposeData();
        Scribe.ForceStop();
        return loaded;
    }

    private static void CheckDefaults(ManyHappyReturnsSettings settings)
    {
        Equal(true, settings.morningLetter);
        Equal(true, settings.forgottenThought);
        Equal(1f, settings.moodFactor);
    }

    private static void Equal<T>(T expected, T actual)
    {
        if (!Equals(expected, actual)) throw new Exception($"Expected {expected}, observed {actual}");
    }

    private static void Test(string name, Action action)
    {
        try { action(); passed++; Console.WriteLine("PASS: " + name); }
        catch (Exception ex) { failures++; Console.WriteLine("FAIL: " + name + "\n" + ex); }
        finally { Scribe.ForceStop(); }
    }
}
