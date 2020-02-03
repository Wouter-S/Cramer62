import Axios from "axios";
import EventBus from './../../event-bus';
var _ = require('lodash');


export default {

    template: "<Lightlog/>",
    data() {
        return {
            lightLogs: []
        }
    },
    mounted() {
        this.GetLightLogs();
        EventBus.$on("LightsChanged", () => {
            this.GetLightLogs();
        });
    },

    methods: {
        GetLightLogs: function () {
            Axios.get('/api/lights/logs').then(result => {
                this.lightLogs = result.data;
            });
        }
    },
    computed: {
        orderedLogs: function () { 
            return _.orderBy(this.lightLogs, 'unixTimeStamp', 'desc')
        }
    }
};