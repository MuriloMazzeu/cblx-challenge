<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols=10>
        <v-card outlined>
          <v-card-title>Novo Registro de Saída</v-card-title>

          <v-card-text>
            <v-form v-model="isValidForm" ref="frm" lazy-validation>
              <v-select v-model="type" :items="types" :rules="required" label="Classe" prepend-inner-icon="mdi-shape" filled></v-select>
              <date-time-picker v-model="outDateTime" min-time="08:00" :rules="required" label="Data de Saída" ref="outDT" />
            </v-form>
          </v-card-text>

          <v-divider></v-divider>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn text @click="reset">limpar</v-btn>
            <v-btn text :loading="loading" :disabled="!isValidForm" @click="send">cadastrar</v-btn>
          </v-card-actions>
        </v-card>
      </v-col>

      <v-col cols=10>
        <v-card outlined>
          <v-card-title>Registros de Saída Pendentes</v-card-title>
          <v-simple-table>
            <template #default>
              <thead>
                <tr>
                  <th class="text-left">Id</th>
                  <th class="text-left">Classe</th>
                  <th class="text-left">Prioridade</th>
                  <th class="text-left">Data de Saída</th>
                  <th></th>
                </tr>
              </thead>
              <tbody>
                <tr v-if="transports.length == 0">
                  <td colspan="4" class="text-center text-caption grey--text">Sem Registros Cadastrados</td>
                </tr>
                <tr v-else v-for="item in transports" :key="item.id">
                  <td>{{ item.id }}</td>
                  <td>{{ item.Type }}</td>
                  <td>Minério {{ priorityFor(item) }}</td>
                  <td>{{ item.startAt }}</td>
                  <td class="text-right">
                    <v-dialog max-width="290">
                      <template v-slot:activator="{ on, attrs }">
                        <v-btn small outlined v-on="on" v-bind="attrs">retorno</v-btn>
                      </template>
                      <v-card>
                        <v-card-title>Retorno de Cargueiro</v-card-title>
                        <v-card-text>
                          <date-time-picker v-model="item.endDate" :min-date-time="item.startAtISO" label="Data de Retorno" ref="inDT" />
                          <v-select filled :items="mineralsFor(item.Type)" label="Mineral" prepend-inner-icon="mdi-diamond-stone" v-model="item.mineral"></v-select>
                          <v-text-field filled hint="Peso em quilos" type="number" :placeholder="maxWeightFor(item.Type) + ''" inputmode="numeric" min="1" step="1" :max="maxWeightFor(item.Type)" label="Quantidade" prepend-inner-icon="mdi-weight-kilogram" v-model="item.amount"></v-text-field>
                        </v-card-text>
                        <v-card-actions>
                          <v-spacer></v-spacer>
                          <v-btn text @click="save(item)" :loading="loading2">salvar</v-btn>
                        </v-card-actions>
                      </v-card>
                    </v-dialog>  
                  </td>
                </tr>
              </tbody>
            </template>
          </v-simple-table>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
import Vue from 'vue'
import Axios from 'axios'
import Firebase from 'firebase/app'
import 'firebase/firestore'

import DateTimePicker from '../components/DateTimePicker.vue'

export default Vue.extend({
  name: 'Add',
  data: () => ({
    transports: new Array(0),
    isValidForm: true,
    loading: false,
    loading2: false,
    type: '',     
    outDateTime: '',
    required: [
      (v: any) => !!v || 'Esse campo é obrigatorio',
    ]
  }),

  computed: {
    types() {
      if(this.transports.length == 0) return ['I', 'II', 'III', 'IV']
      const useds: string[] = this.transports.map(i => i.Type)
      const types: string[] = []

      if(useds.filter(i => i == 'I').length < 15) types.push('I')
      if(useds.filter(i => i == 'II').length < 10) types.push('II')
      if(useds.filter(i => i == 'III').length < 3) types.push('III')
      if(useds.filter(i => i == 'IV').length < 2) types.push('IV')
      return types
    }
  },

  mounted() {
    Firebase.firestore().collection('transports').where('EndAt', '==', null).onSnapshot(snap => {
      this.transports = snap.empty ? [] : snap.docs.map(i => {
        const data: any = i.data()
        data.id = i.id
        this.formatTimestamp(data)
        return data
      })
    })
  },

  components: { DateTimePicker },
  
  methods: {
    async save(item: any) {
      const max: number = this.maxWeightFor(item.Type)
      if(item.amount > max) {
        this.$emit('toast', 'A quantidade deve ser no máximo ' + max)
        return
      }

      if(item.amount < 1) {
        this.$emit('toast', 'A quantidade deve ser maior que 1')
        return
      }

      this.loading2 = true
      const { apiurl } = this.$store.state
      const response = Axios.post(apiurl + 'freighter/checkin', {
        id: item.id,
        amount: item.amount,
        mineral: item.mineral,
        endAt: item.endDate,
        startAt: item.startAtISO,
        requestId: Math.round(Math.random() * 1000).toString(),
      })
      
      const result = await response
        .then(r => r.data).catch(err => {
          console.log(err); 
          return {
            success: false,
            message: 'Tente novamente mais tarde'
          }
      })

      this.loading2 = false
      this.$emit('toast', result.success 
        ? 'Retorno registrado com sucesso' 
        : result.message)
    },

    mineralsFor(type: string) {
      if(type == 'I') return ['D']
      if(type == 'II') return ['A']
      if(type == 'III') return ['C']
      if(type == 'IV') return ['B', 'C']
    },

    priorityFor(item: any) {
      if(item.RecommendedMineral) return item.RecommendedMineral
      else return this.mineralsFor(item.Type)![0]
    },

    maxWeightFor(type: string): number {
      if(type == 'I') return 5000
      if(type == 'II') return 3000
      if(type == 'III') return 2000
      if(type == 'IV') return 500
      return 0
    },

    formatTimestamp(item: any): void {
      const date = item.StartAt.toDate()
      item.startAt =  date.toLocaleDateString() + ' às ' + date.toLocaleTimeString()
      item.startAtISO = date.toLocaleDateString('ru').split('.').reverse().join('-')
      item.startAtISO += 'T' + date.toTimeString().split(' ')[0]
    },

    reset() {
      this.type = ''
      const outDT: any = this.$refs.outDT
      const frm: any = this.$refs.frm
      outDT.reset()
      frm.reset()
    },

    async send() {
      const frm: any = this.$refs.frm
      if(frm.validate()) {
        const { apiurl } = this.$store.state

        this.loading = true
        const response = Axios.post(apiurl + 'freighter/checkout', {
          type: this.type,
          startAt: this.outDateTime,
          requestId: Math.round(Math.random() * 1000).toString(),
        })
        
        const result = await response
          .then(r => r.data)
          .catch(err => {
            console.log(err); 
            return {
              success: false
            }
        })

        this.loading = false
        this.$emit('toast', result.success 
          ? 'Saída registrada com sucesso' 
          : 'Tente novamente mais tarde')

        const frm: any = this.$refs.frm
        if(frm) frm.reset()
      }
    }
  }
})
</script>