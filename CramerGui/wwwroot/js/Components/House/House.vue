<template>
    <div class="row">
        <div class="house col-6" >
            <div v-for="room in rooms" :id="room.codeName" data-group-lights="True"
                 :data-room-id="room.id"
                 :data-room-name="room.codeName"
                 v-on:click="roomClicked(room)"
                 class="roomLayout">

                <span class="indicator" v-bind:class="{'on': room.roomLights.length && room.roomLights[0].mode == 1, 'off': room.roomLights.length && room.roomLights[0].mode == 0}">
                </span>
            </div>
        </div>

        <div class="lightButtons col-6">
            <div class="lightButtonWrapper row"
                 v-bind:class="{dimmable: roomLight.isDimmable}"
                 v-for="roomLight in selectedRoom.roomLights"
                 v-on:click="!roomLight.isDimmable && switchLight(roomLight)">

                <div class="col-12" v-show="roomLight.isDimmable">
                    <input class="lightslider" type="range" value="85" v-model="roomLight.percentage"
                           v-on:change="dimLight(roomLight)" min="0" max="100" step="5" />
                </div>

                <span class="lightButton col-8"
                      v-bind:class="{dimmable: roomLight.isDimmable}">
                    <b >{{roomLight.name}}</b>
                    <span v-show="roomLight.isDimmable">{{roomLight.percentage}}%</span>
                </span>

                <div class="lightSwitch col-4"
                      
                     v-bind:class="{dimmable: roomLight.isDimmable, 'on': roomLight.mode == 1, 'off': roomLight.mode == 0}"
                     v-on:click="roomLight.isDimmable && switchLight(roomLight)"></div>
            </div>
            <div class="lightButtonWrapper row" v-on:click="killLights()">
                <span class="lightButton col-md-8 killEmAll" style="color:red">
                    <b>Kill em all <i v-show="killing" class="fas fa-spinner fa-spin"></i></b>
                </span>
            </div>
        </div>
    </div>
</template>
<script src="./house.js"></script>