"use strict";
import axios from "axios";

class Api {

    GetWeather() {
        var weatherUrl = "http://api.openweathermap.org/data/2.5/weather?q=Amsterdam,NL&id=524901&appId=78d1bf2ba99674bfdd0ce722f983af04&units=metric";
        return axios.get(weatherUrl)
            .then(response => {
                return response.data;
            })
    }
}

export default Api