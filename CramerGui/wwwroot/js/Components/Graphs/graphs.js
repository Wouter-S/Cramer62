import axios from 'axios'

export default {
    template: '<Graphs/>',

    data() {
        return {
            graphData: null
        }
    },
    components: {

    },
    mounted() {
        axios.get('/graph')
            .then(response => {
                this.graphData = response.data;
            })
    }
}