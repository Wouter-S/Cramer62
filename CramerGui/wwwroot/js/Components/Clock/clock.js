export default {

    template: "<Clock/>",
    data() {
        return {
            hours: '',
            minutes: '',
            seconds: '',
            day: '',
            month: ''
        }
    },
    mounted() {
        this.updateDateTime();
    },

    methods: {
        updateDateTime: function updateDateTime() {
            var self = this;
            var now = new Date();

            self.hours = now.getHours();
            self.minutes = self.getZeroPad(now.getMinutes());
            self.seconds = self.getZeroPad(now.getSeconds());
            self.hours = self.hours;
            self.day = self.getZeroPad(now.getDate());
            self.month = self.getZeroPad(now.getMonth()+1);

            setTimeout(self.updateDateTime, 1000);
        },
        getHourTime: function getHourTime(h) {
            return h >= 12 ? 'PM' : 'AM';
        },
        getZeroPad: function getZeroPad(n) {
            return (parseInt(n, 10) >= 10 ? '' : '0') + n;
        }
    }
};