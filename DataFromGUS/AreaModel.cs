using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace DataFromGUS
{
    public class AreaModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("nazwa")]
        public string Name { get; set; }

        [JsonProperty("id-nadrzedny-element")]
        public int ParentId { get; set; }

        [JsonProperty("id-poziom")]
        public int LevelId { get; set; }

        [JsonProperty("nazwa-poziom")]
        public string LevelName { get; set; }

        [JsonProperty("czy-zmienne")]
        public bool IsVariable { get; set; }

        public SolidColorBrush LevelNameColor { get; set; }


    }
}