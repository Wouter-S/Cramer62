namespace CramerAlexa
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

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
        public string MqttAddress { get; set; }
        public Nullable<long> RoomId { get; set; }
        public string FriendlyNameSlug { get; set; }
    }
}
