namespace CramerGui
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Text.RegularExpressions;

    public enum LightMode
    {
        on = 1,
        off = 0
    }

    [Table("Light")]
    public partial class Light
    {
        [Key]
        public long LightId { get; set; }
        public string Name { get; set; }
        public Nullable<long> Percentage { get; set; }
        public LightMode Mode { get; set; }
        public bool IsDimmable { get; set; }
        public string FriendlyName { get; set; }
        public string FriendlyNameSlug { get { return ToUrlSlug(FriendlyName); } }
        public string MqttAddress { get; set; }
        public Nullable<long> RoomId { get; set; }

        public static string ToUrlSlug(string value)
        {
            value = value.Replace(" ", "");
            //value = value.ToLowerInvariant();
            //var bytes = Encoding.UTF8.GetBytes(value);
            //value = Encoding.ASCII.GetString(bytes);
            //value = Regex.Replace(value, @"\s", "-", RegexOptions.Compiled);
            //value = Regex.Replace(value, @"[^a-z0-9\s-_]", "", RegexOptions.Compiled);
            //value = value.Trim('-', '_');
            //value = Regex.Replace(value, @"([-_]){2,}", "$1", RegexOptions.Compiled);
            return value;
        }
    }
}
