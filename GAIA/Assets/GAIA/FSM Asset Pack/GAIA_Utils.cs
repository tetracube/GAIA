using System;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine.Assertions;

namespace GAIA {
	public static class Utils
	{
        private static readonly string LOG_PATH = "GAIALogs";

        // <summary>
        // Get a string that has the name of a given enumeration and returns the type of enumerated value associated
        // </summary>
        // <returns>Generic enumerated value</returns>
        // <remarks> Generic lexical analyzer. Converts a lexeme into a tag with meaning </remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TEnum name2Tag<TEnum>(string s)
        where TEnum : struct
        {
            Enum.TryParse(s, true, out TEnum resultInputType);
            return resultInputType;
        }

        public static float ChangeRange(float Value, float OldMin, float OldMax, float NewMin, float NewMax)
        {
            Assert.IsTrue(OldMax > OldMin);
            Assert.IsTrue(NewMax > NewMin);
            Assert.IsTrue(Value >= OldMin && Value <= OldMax);

            float Percentage = (Value - OldMin) / (OldMax - OldMin);
            float Temp = Percentage * (NewMax - NewMin);
            return Temp + NewMin;
        }

        public static int ShiftRange(int Value, int Min)
        {
            return Value + Min;
        }

        public static float StrToFloat(string Str)
        {
            try
            {
                return float.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            } catch {
                UnityEngine.Debug.LogError("Couldn't convert to float. Invalid format of input string.");
            }

            return 0f;
        }

        public static int StrToInt(string Str)
        {
            try
            {
                return int.Parse(Str, System.Globalization.CultureInfo.InvariantCulture);
            }
            catch
            {
                UnityEngine.Debug.LogError("Couldn't convert to int. Invalid format of input string.");
            }

            return 0;
        }

        public static void Log(string Str, string LogFile)
        {
            string LogContent = "[" + DateTime.Now.ToString() + "] - " + Str;
            UnityEngine.Debug.Log(LogContent);
            CreateDirectoryIfNotExists(LOG_PATH);
            StreamWriter SW = new(LOG_PATH + "/" + LogFile, true);
            SW.WriteLine(LogContent);
            SW.Close();
        }

        private static void CreateDirectoryIfNotExists(string Path)
        {
            if (!Directory.Exists(Path))
            {
                Directory.CreateDirectory(Path);
            }
        }
    }
}