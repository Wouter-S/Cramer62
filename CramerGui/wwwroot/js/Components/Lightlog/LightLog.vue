<template>
    <div class="row">
        <div class="col-12 row" v-for="lightLog in orderedLogs">
            <div class="col-3 text-right">
                <span class="off logIcon" v-show="lightLog.value == 0" />
                <span  class="on logIcon" v-show="lightLog.value != 0" />
                {{lightLog.dateTime}}
            </div>

            <div class="col-2 text-right">{{lightLog.lightName}}</div>
            <div class="col-2 text-right">{{lightLog.value}}</div>
        </div>
    </div>
</template>
<script src="./LightLog.js"></script>