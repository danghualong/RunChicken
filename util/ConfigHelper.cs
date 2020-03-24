using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RunChicken.util
{
    public class ConfigHelper
    {
        public const int TEXT_NUM = 12;
        public static List<string> DEFAULT_WORDS = new List<string>() { "一", "二", "三", "四", "五", "六", "七", "八", "九", "十", "百", "千" };
        public static string GetConfig(string key,string defaultValue)
        {
            if (string.IsNullOrEmpty(key))
            {
                return defaultValue;
            }
            try
            {
                var result=ConfigurationManager.AppSettings[key];
                if (result == null)
                {
                    return defaultValue;
                }
                return result;
            }
            catch(Exception ex)
            {
                return defaultValue;
            }
        }

        public static string SaveConfig(string key, string data)
        {
            if (string.IsNullOrEmpty(key))
            {
                return "保存的键值不能为空";
            }
            ConfigurationManager.AppSettings[key] = data;
            return string.Empty;
        }


        public static List<string> GetWordVector(string textPool)
        {
            if (string.IsNullOrEmpty(textPool))
            {
                return null;
            }
            ISet<string> sets = new HashSet<string>();
            for (var i = 0; i < textPool.Length; i++)
            {
                if (textPool[i] != ' ')
                {
                    sets.Add(textPool[i].ToString());
                }
            }
            return new List<string>(sets);
        }
    }
}
