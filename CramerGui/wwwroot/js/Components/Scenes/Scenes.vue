<template>
    <div class="row">
        <div class="col-6 col-sm-12 sceneSwitchWrapper" v-for="scene in scenes" v-on:click="setScene(scene)">
            <div class="sceneSwitch text-center">
                {{scene.friendlyName}}
                <i v-show="scene.loading" class="fas fa-spinner fa-spin sceneSwitchIcon"></i>
                <i v-show="scene.switched" class="fas fa-check sceneSwitchIcon" style="color: green;"></i>
            </div>
        </div>
    </div>
</template>
<script src="./Scenes.js"></script>