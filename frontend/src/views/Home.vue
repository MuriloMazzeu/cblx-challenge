<template>
  <v-container fluid>
    <v-row justify="center">
      <v-col cols=5>
        <h1>Cargas Recebidas</h1>
      </v-col>

      <v-col cols=5 align="right">
        <v-menu max-width="290" v-model="menu" left bottom>
          <template v-slot:activator="{ on, attrs }">
            <v-text-field outlined dense readonly hide-details v-model="date" label="Data de Referência" prepend-inner-icon="mdi-calendar" class="txt-date-picker" v-bind="attrs" v-on="on" />
          </template>

          <v-date-picker scrollable v-model="date" type="month"></v-date-picker>
        </v-menu>
      </v-col>
      
      <v-col cols=10>
        <v-data-table :headers="headers" :items="items" :loading="loading" class="elevation-1" item-key="id">
          <template #item.endAt="{ value }">
            <span>{{new Date(value).toLocaleString()}}</span>
          </template>

          <template #item.cost="{ value }">
            <v-tooltip bottom>
              <template #activator="{ on, attrs }">
                <span v-bind="attrs" v-on="on">
                  {{format(value, 1)}}
                </span>
              </template>
              <span>$ {{value.toLocaleString()}}</span>
            </v-tooltip>
          </template>
        </v-data-table>
      </v-col>

      <v-col cols=5>
        <v-simple-table class="elevation-1">
          <template #default>
            <thead>
              <tr>
                <th>Minério</th>
                <th>Valor Total</th>
              </tr>
            </thead>

            <tbody>
              <tr v-if="Object.keys(costs).length == 0">
                <td colspan="2" class="text-center grey--text text-caption">Não há dados disponíveis</td>
              </tr>

              <tr v-else v-for="(item, key, n) in costs" :key="key + n">
                <td>{{key}}</td>
                <td>
                  <v-tooltip bottom>
                    <template #activator="{ on, attrs }">
                      <span v-bind="attrs" v-on="on">{{format(item, 1)}}</span>
                    </template>
                    <span>$ {{item.toLocaleString()}}</span>
                  </v-tooltip>
                </td>
              </tr>
            </tbody>
          </template>
        </v-simple-table>
      </v-col>

      <v-col cols=5>
        <v-simple-table class="elevation-1">
          <template #default>
            <thead>
              <tr>
                <th>Classe</th>
                <th>Índice de Ociosidade</th>
              </tr>
            </thead>

            <tbody>
              <tr v-if="Object.keys(idleness).length == 0">
                <td colspan="2" class="text-center grey--text text-caption">Não há dados disponíveis</td>
              </tr>

              <tr v-else v-for="(item, key, n) in idleness" :key="key + n">
                <td>{{key}}</td>
                <td>{{item.toLocaleString()}}%</td>
              </tr>
            </tbody>
          </template>
        </v-simple-table>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue'
  import Axios from 'axios'

  export default Vue.extend({
    name: 'Home',
    data: () => ({
      date: new Date().toISOString().substr(0, 7),
      menu: false, loading: false,
      items: [], costs: [], idleness: [], headers: [
        { text: 'Recebimento', align: 'start', sortable: true, value: 'endAt'},
        { text: 'Minério', align: 'start', sortable: true, value: 'mineral'},
        { text: 'Valor Total', align: 'start', sortable: true, value: 'cost'},
      ]
    }),

    mounted() { 
      this.load() 
    },

    watch: {
      date() { 
        this.load() 
      },
    },

    methods: {
      format(num: number, digits: number): string {
        const lookup = [
          { value: 1, symbol: "" },
          { value: 1e3, symbol: "k" },
          { value: 1e6, symbol: "M" },
          { value: 1e9, symbol: "B" },
          { value: 1e12, symbol: "T" },
          { value: 1e15, symbol: "P" },
          { value: 1e18, symbol: "E" }
        ];

        const rx = /\.0+$|(\.[0-9]*[1-9])0+$/;
        var item = lookup.slice().reverse().find(item => num >= item.value);
        return item ? (num / item.value).toFixed(digits).replace(rx, "$1") + item.symbol : "0";
      },

      async load(): Promise<void> {
        const date = this.date + '-01'
        const { apiurl } = this.$store.state

        this.loading = true
        var response: any = await Axios.get(apiurl + 'received-minerals', {
          params: {
            requestId: Math.round(Math.random() * 1000).toString(),
            period: date
          }
        })
        .then(res => res.data)
        .catch(err => {
          console.log(err)
          return { success: false }
        })

        this.loading = false
        if(response.success) {
          this.items = response.data.items
          this.costs = response.data.totalCost
          this.idleness = response.data.idlenessIndex
        }
        else {
          this.$emit('toast', 'Tente novamente mais')
          this.items = []
          this.costs = []
          this.idleness = []
        }
      }
    }
  })
</script>

<style lang="scss">
.txt-date-picker {
  max-width: 160px !important;
  input { text-align: center; }
}
</style>