import axios from 'axios'
import Api from './../../api.js'

export default {
    template: '<Weather/>',

    data() {
        return {
            temperature: "",
            description: "",

        }
    },
    components: {

    },
    mounted() {
        let api = new Api();
        api.GetWeather().then(data => {
            this.temperature = data.main.temp;
            this.description = data.weather[0].main;
        })
    }
}