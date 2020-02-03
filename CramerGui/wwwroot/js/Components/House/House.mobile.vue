<template>
    <div>
        <div v-for="room in rooms"
             :data-room-id="room.id"
             :data-room-name="room.codeName" class="row">

            <div class="lightButtonWrapper"
                 v-bind:class="{'col-12': (light.isDimmable || room.roomLights.length == 1), 'col-6': !light.isDimmable && room.roomLights.length != 1}"
                 v-for="light in room.roomLights"
                 v-on:click="!light.isDimmable && switchLight(light)">
                <div class="lightButtonWrap">


                    <div style="width:96%; padding-top:15px;" v-show="light.isDimmable">
                        <input class="light0slider" type="range" value="85" v-model="light.percentage" v-on:change="dimLight(light)" min="0" max="100" step="5" />
                    </div>

                    <span class="lightButton"
                          v-bind:class="{dimmable: light.isDimmable}">
                        <b>{{light.name}}</b>
                        <span v-show="light.isDimmable" class="lightId0value">{{light.percentage}}%</span>
                    </span>

                    <div class="lightSwitch"
                         v-bind:class="{dimmable: light.isDimmable, 'on': light.mode == 1, 'off': light.mode == 0}"

                         v-on:click="light.isDimmable && switchLight(light)"></div>
                    <div class="spacer"></div>
                </div>
            </div>
        </div>
    </div>
</template>
<script src="./house.js"></script>