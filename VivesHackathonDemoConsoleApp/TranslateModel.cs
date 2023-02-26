using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VivesHackathonDemoConsoleApp
{
    public class TranslateModel
    {
        [JsonProperty("detectedLanguage")]
        public Language Language { get; set; }
        public List<Translation> Translations { get; set; }
    }

    public class Language
    {
        [JsonProperty("Language")]
        public string Text { get; set; }
        public double Score { get; set; }
    }

    public class Translation
    {
        public string Text { get; set; }
        public string To { get; set; }
    }
}
