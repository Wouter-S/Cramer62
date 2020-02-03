<template>
    <div>
        <table id="powerConsumptionTable">
            <template v-for="row in graphData">
                <tr>
                    <td>
                        <strong>{{row.Year}}</strong>
                    </td>
                </tr>
                <tr>
                    <td>
                        Month <br />
                        L kWh <br />
                        H kWh <br />
                    </td>


                    <td class="tableCell" v-for="reading in row.Readings">
                        {{reading.Month}} <br />{{reading.PowerLow}}<br />{{reading.PowerHigh}}
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr>
            </template>
        </table>
    </div>

</template>
<script src="./Graphs.js"></script>