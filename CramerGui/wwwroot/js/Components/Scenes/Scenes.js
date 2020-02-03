import Axios from "axios";

export default {

    template: "<Scenes/>",
    data() {
        return {
            scenes: []
        }
    },
    mounted() {
        Axios.get('/api/scenes').then(result => {
            this.scenes = result.data;
        });
    },

    methods: {
        setScene: function (scene) {
            scene.loading = true;
            Axios.get('/api/scene/flip/' + scene.sceneId).then(() => {
                scene.loading = false;
                scene.switched = true;

                setTimeout(function () {
                    scene.switched = false;
                }, 3000);
            });
        }
    }
};