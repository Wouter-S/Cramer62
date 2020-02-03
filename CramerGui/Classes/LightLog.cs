namespace CramerAlexa
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LightLog")]
    public partial class LightLog
    {
        [Key]
        public long LogId { get; set; }
        public long LightId { get; set; }
        public long Value { get; set; }

        public System.DateTime Timestamp { get; set; }

        public long UnixTimeStamp
        {
            get
            {
                return ((DateTimeOffset)Timestamp).ToUnixTimeSeconds();
            }
        }

        [NotMapped]
        public string LightName { get; internal set; }

        [NotMapped]
        public string DateTime
        {
            get
            {
                return Timestamp.ToString("dd-MM HH:mm");
            }
        }

    }
}
