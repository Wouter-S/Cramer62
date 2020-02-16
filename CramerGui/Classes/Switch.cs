namespace CramerGui
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Runtime.Serialization;

    public enum SwitchClickType
    {
        right,
        right_long,
        right_double,

        left,
        left_double,
        left_long,
        
        both,
        both_double,
        both_long,
    }

    [Table("Scene")]
    public partial class Scene
    {
        public Scene()
        {
            //this.SwitchActions = new List<SwitchAction>();
        }

        public long SceneId { get; set; }
        public string FriendlyName { get; set; }
        //public bool Manual { get; set; }
        //public bool WhenHome { get; set; }
        //public bool WhenAway { get; set; }
        //public bool WhenDark { get; set; }
        //public bool WhenLight { get; set; }
        //public System.DateTime LastAction { get; set; }
        //public long TimeBetween { get; set; }
        //public long Offset { get; set; }
        //public string FriendlyName { get; set; }
        //public bool IsScene { get; set; }

        [NotMapped]
        public bool Loading { get; set; }
        [NotMapped]
        public bool Switched { get; set; }
        //public virtual List<SwitchAction> SwitchActions { get; set; }
        public string MqttAddress { get; set; }

    }
}
