import axios from 'axios'
import Api from './../../api.js'
import EventBus from './../../event-bus';

export default {
    template: '<House/>',

    data() {
        return {
            rooms: "",
            description: "",
            selectedRoom: "",
            killing: false,
            isInitator: false,
        }
    },
    components: {

    },

    methods: {
        loadRooms: function () {
            axios.get('api/room').then(result => {
                this.rooms = result.data.rooms;
                for (var i = 0; i < this.rooms.length; i++) {
                    if (!this.rooms[i].roomLights.length) {
                        continue;
                    }
                    this.rooms[i].lightsOn = this.rooms[i].roomLights[0].mode;
                }
                this.selectedRoom = this.rooms[0];
            });

        },
        roomClicked: function (room) {
            if (room.roomLights.length > 1) {
                this.openRoom(room);
            } else {
                this.switchRoomLights(room);
                this.openRoom(room);
            }
            setTimeout(() => this.selectedRoom = this.rooms[0], 30000); 
        },
        openRoom: function (room) {
            this.selectedRoom = room;
        },

        switchRoomLights: function (room) {
            this.isInitator = true;
            room.roomLights[0].mode = room.roomLights[0].mode == 1 ? 0 : 1;
            room.lightsOn = room.roomLights[0].mode;

            var newState = room.roomLights[0].mode == 1 ? "on" : "off";
            this.$parent.connection.invoke("SwitchRoomLight", room.roomId, newState);

        },
        switchLight: function (light) {
            this.isInitator = true;

            var newMode = light.mode == 0 ? "on" : "off";
            light.mode = light.mode == 1 ? 0 : 1;
            this.$parent.connection.invoke("SwitchLight", light.lightId, newMode);
        },
        dimLight: function (light) {
            this.isInitator = true;
            console.log(light);
            light.mode = 1;
            this.$parent.connection.invoke("DimLight", light.lightId, parseInt(light.percentage));
        },

        killLights: function () {
            this.killing = true;
            this.isInitator = false;
            this.$parent.connection.invoke("KillLights");
        },

    },

    mounted() {
        this.loadRooms();
        EventBus.$on("KillLights", () => {
            this.killLights();
            this.$parent.killingLights = true;
        });
        EventBus.$on("LightsChanged", () => {
            if (!this.isInitator) {
                this.loadRooms();
                this.killing = false;
                this.$parent.killingLights = false;

            }
            this.isInitator = false;
        });
    },
}