import Vue from 'vue'
import Graphs from './Components/Graphs/graphs.vue'
import House from './Components/House/House.mobile.vue'
import Scenes from './Components/Scenes/Scenes.vue'
import Currentusage from './Components/CurrentUsage/CurrentUsage.vue'
import { HubConnectionBuilder } from '@aspnet/signalr';
import EventBus from './event-bus';
import Axios from 'axios';

import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.config.productionTip = false;
Vue.use(BootstrapVue);
window.component = new Vue({
    el: '#app',
    components: {
        Graphs,
        House,
        Currentusage,
        Scenes
    },
    methods: {
        showTab: function (tabName) {
            this.activeTab = tabName;

        },
        killLights: function () {
            EventBus.$emit('KillLights');
        },

        initSignalr: function () {
            this.connection = new HubConnectionBuilder()
                .withUrl("/TheHub")
                .build();

            this.connection.on("LightsChanged", () => {
                EventBus.$emit("LightsChanged");
            });

            this.connection.on("PowerChanged", () => {
                EventBus.$emit("PowerChanged");
            });

            this.connection.onclose(() => {
                this.initSignalr();
            })

            this.connection.start().then(() => {

            }).catch(err => {
                this.initSignalr();
                return console.error(err.toString());
            });
        },

    },
    data: {
        activeTab: "house",
        connection: null,
        killingLights: false,
        graphUrls: {},

    },
    mounted() {
        this.initSignalr();

        this.graphUrls = {
            Last7Days: "/graph/image/Last7Days",
            Last30Days: "/graph/image/Last30Days",
            Last24Hrs: "/graph/image/Last24Hrs",
            Temperature: "/graph/image/Temperature",
            ScreenSaverPower: "/graph/image/ScreenSaverPower",
            ScreenSaverTemp: "/graph/image/ScreenSaverTemp"
        };

    }
})
