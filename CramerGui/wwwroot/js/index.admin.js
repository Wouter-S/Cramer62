import Vue from 'vue'
import Axios from 'axios';
import BootstrapVue from 'bootstrap-vue'
import 'bootstrap/dist/css/bootstrap.css'
import 'bootstrap-vue/dist/bootstrap-vue.css'

Vue.config.productionTip = false;
Vue.use(BootstrapVue);

window.component = new Vue({
    el: '#app',
    components: {
    },
    methods: {
        SaveLights: function () {
            Axios.post('/api/lights', this.lights).then(() => {
                this.loadData();
                alert("Succesfully saved");

            });
        },

        switchTab: function (tab) {
            this.activeTab = tab;
        },
        SaveScenes: function () {
            Axios.post('/api/scenes', this.scenes).then(() => {
                this.loadData();
                alert("Succesfully saved");
            });
        },

        //AddAction: function () {
        //    this.activeScene.switchActions.push({ switchId: this.activeScene.switchId, lightId: 0, defaultAction: 100 });
        //},

        DeleteScene: function (scene) {
            for (var i = 0; i < this.scenes.length; i++) {
                if (this.scenes[i].sceneId == scene.sceneId) {
                    this.scenes.splice(i, 1);
                }
            }
        },

        DeleteSceneAction: function (scene, action) {
            console.log("DeleteSwitchAction")
            for (var i = 0; i < this.scenes.length; i++) {
                if (this.scenes[i].sceneId == scene.sceneId) {
                    //if (typeof this.scenes[i].switchActions == 'undefined') {
                    //    continue;
                    //}
                    //for (var j = 0; j < this.switches[i].switchActions.length; j++) {

                    //    if (this.switches[i].switchActions[j].switchActionId == action.switchActionId) {
                    //        this.switches[i].switchActions.splice(j, 1);
                    //    }
                    //}
                }
            }
        },

        AddScene: function () {
            this.scenes.push({ sceneId: 0 });
        },

        //SaveSchedules: function () {
        //    Axios.post('/api/schedules', this.schedules).then(() => {
        //        this.loadData();
        //        alert("Succesfully saved");

        //    });
        //},

        //AddSchedule: function () {
        //    schedules.push({ scheduleId: 0, scheduleType: 2, value: 0 });
        //},

        loadData: function () {
            Axios.get('/api/lights?admin=true').then((result) => {
                this.lights = result.data;
            });

            Axios.get('/api/scenes').then((result) => {
                this.scenes = result.data;
            });

            //Axios.get('/api/schedules').then((result) => {
            //    this.schedules = result.data;
            //});

            Axios.get('api/room').then((result) => {
                this.rooms = result.data.rooms;
            });
        },
    },
    data: {
        activeTab: 'scenes',
        activeScene: null,
        lights: [],
        scenes: {},
        //schedules: {},
        rooms: {},
        clickTypes: [
            { id: "0", name: "right" },
            { id: "1", name: "right_long" },
            { id: "2", name: "right_double" },

            { id: "3", name: "left" },
            { id: "4", name: "left_double" },
            { id: "5", name: "left_long" },

            { id: "6", name: "both" },
            { id: "7", name: "both_double" },
            { id: "8", name: "both_long" }
        ]
    },
    mounted() {
        this.loadData();
    }
})
