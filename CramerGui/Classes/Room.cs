namespace CramerAlexa
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Room")]
    public partial class Room
    {
        public Room()
        {
        }

        [Key]
        public long RoomId { get; set; }
        public string Name { get; set; }
        public string CodeName { get; set; }

        public virtual List<Light> RoomLights { get; set; }
    }
}
