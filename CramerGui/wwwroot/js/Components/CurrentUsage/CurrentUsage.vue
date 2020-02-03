<template>
    <div style="margin-top: 10px;">
        <div class="block currentUsage" v-on:click="showTab('power-current')">
            {{readings.power_current}}w
        </div>
        <div class="block" id="powerConsumption" v-on:click="showTab('power')">

            <div id="powerConsumptionWrapper">
                <span class="powerName powerHighReading">{{Math.round(readings.power_high)}} kWh</span>
                <span class="powerName powerLowReading">{{Math.round(readings.power_low)}} kWh</span>
                <span class="powerName powerGasReading">{{Math.round(readings.gas)}} m&#179;</span>
            </div>
        </div>
        <div class="block currentUsage" v-on:click="showTab('temperature')">
            {{Math.round(readings.living_temperature * 10) / 10}}°C
        </div>
        <div>
            <img :src="graphUrls.Last7Days" width="280" height="250" frameborder="0" style="text-align: center; margin-left: auto;margin-right: auto;" class="showMobile" >
        </div>
    </div>
</template>
<script>
    import Axios from "axios";
    export default {
        template: "<CurrentUsage />",
        data() {
            return {
                readings: [],
                graphUrls: {
                    Last7Days: "/graph/image/Last7Days",
                }

            }
        },
        mounted() {
            Axios.get('api/readings').then(result => {
                this.readings = result.data;

                if (this.readings.powerCurrent == null) {
                    this.readings.powerCurrent = "2200f"
                }
            });
        },

        methods: {
            showTab(tabName) {
                this.$parent.showTab(tabName);
            }
        }
    };
</script>