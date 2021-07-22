<template>
    <v-dialog v-model="modal" ref="dialog" :return-value.sync="value" persistent width="290px">
        <template #activator="{ on, attrs }">
            <v-text-field readonly filled prepend-inner-icon="mdi-calendar" :rules="rules" :label="label" v-model="formattedValue" v-on="on" v-bind="attrs"></v-text-field>
        </template>

        <v-card>
            <v-card-text class="pa-0">
                <v-tabs dark fixed-tabs background-color="primary" v-model="activeTab">
                    <v-tab key="calendar">
                        <v-icon>mdi-calendar</v-icon>
                    </v-tab>

                    <v-tab key="timer">
                        <v-icon>mdi-clock-outline</v-icon>
                    </v-tab>

                    <v-tab-item key="calendar">
                        <v-date-picker :allowed-dates="allowedDate" class="rounded-0" :min="minDateTime" :max="maxDateTime" v-model="date" @input="activeTab = 1" scrollable full-width></v-date-picker>
                    </v-tab-item>

                    <v-tab-item key="timer">
                        <v-time-picker v-model="time" :min="minTiming" :max="maxTiming" class="rounded-0" ref="timer" format="24hr" full-width></v-time-picker>
                    </v-tab-item>
                </v-tabs>
            </v-card-text>

            <v-card-actions>
                <v-btn text @click="cancel">Limpar</v-btn>
                <v-spacer></v-spacer>
                <v-btn text @click="ok" :disabled="!isValid">OK</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>

<script lang="ts">
import Vue from 'vue'

export default Vue.extend({
    name: 'DateTimePicker',
    model: { prop: 'formattedModel', event: 'input' },

    props: ['value', 'minTime', 'minDateTime', 'maxDateTime', 'label', 'rules'],
    methods: {
        allowedDate(value: string): boolean { 
            const weekday = new Date(value + 'T00:00:00').getDay()
            const isSunday = weekday === 0
            return !isSunday;
        },

        reset() {
            this.date = ''
            this.time = ''
            this.$emit('input', this.formattedModel)
            
            const timer: any = this.$refs.timer
            if(timer) timer.selectingHour = true
        },

        cancel() {
            this.modal = false
            this.activeTab = 0
            this.reset()
        },

        ok() {            
            this.$emit('input', this.formattedModel)
            this.modal = false
            this.activeTab = 0
        }
    },

    computed: {
        isValid() {
            const dateOk = !!this.date
            const timeOk = !!this.time

            return dateOk && timeOk
        },

        minTiming() {
            if(this.minTime) return this.minTime
            else if(this.minDateTime) {
                const [date, time] = this.minDateTime.split('T')
                if(this.date == date) return time
                else return null
            }
        },

        maxTiming() {
            if(this.maxDateTime) {
                const [date, time] = this.maxDateTime.split('T')
                if(this.date == date) return time
            }

            return null
        },

        formattedModel() {
            if(!this.date) return ''
            return `${this.date}T${this.time}`
        },
    },

    watch: {
        value(v) {
            this.date = v
        },

        date(v) {
            if(!v) return this.formattedValue =  ''
            const date = new Date(`${v}T${this.time}`)
            this.formattedValue = date.toLocaleDateString() + ' às ' + date.toLocaleTimeString()
        },

        time(v) {
            if(!v) return this.formattedValue =  ''
            const date = new Date(`${this.date}T${v}`)
            this.formattedValue = date.toLocaleDateString() + ' às ' + date.toLocaleTimeString()
        },
    },    

    data: () => ({
        formattedValue: '',
        activeTab: 0,
        modal: false, 
        date: '',
        time: '',
    })
})
</script>

<style lang="sass">

</style>