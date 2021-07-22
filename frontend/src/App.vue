<template>
  <v-app>

    <v-app-bar app dark color="black">
      <v-img contain class="shrink mr-2" src="./assets/logo-full-white.png" />
      <v-spacer></v-spacer>

      <v-btn text href="https://github.com/cblx/code-challenge" target="_blank">
        <span class="mr-2">link do desafio</span>
        <v-icon>mdi-open-in-new</v-icon>
      </v-btn>
    </v-app-bar>

    <login-panel v-if="shouldLogin" :ready="ready" @logged="shouldLogin = false" />
    <v-main v-else> <router-view @toast="toast" /> </v-main>

    <v-navigation-drawer app permanent v-if="!shouldLogin && ready">
      <v-list-item two-line>
        <v-list-item-avatar>
            <img :src="userPhoto">
          </v-list-item-avatar>
          <v-list-item-content>
            <v-list-item-title>{{userName}}</v-list-item-title>
            <v-list-item-subtitle>{{userEmail}}</v-list-item-subtitle>
          </v-list-item-content>
      </v-list-item>
      <v-divider></v-divider>

      <v-list dense>
        <v-list-item to="/">
          <v-list-item-icon>
            <v-icon>mdi-notebook</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>Relatório de Cargueiros</v-list-item-title>
          </v-list-item-content>
        </v-list-item>

        <v-list-item to="/add">
          <v-list-item-icon>
            <v-icon>mdi-bus-school</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>Registro de Cargueiros</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
        
        <v-list-item link @click="logout">
          <v-list-item-icon>
            <v-icon>mdi-exit-to-app</v-icon>
          </v-list-item-icon>

          <v-list-item-content>
            <v-list-item-title>Sair da Conta</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-snackbar v-model="showToast">{{toastText}}</v-snackbar>
  </v-app>
</template>

<script lang="ts">
import Vue from 'vue';
import LoginPanel from './components/LoginPanel.vue'
import firebase from 'firebase/app'
import 'firebase/auth'

export default Vue.extend({
  name: 'App',
  components: { LoginPanel },
  methods: {
    toast(text: string) {
      this.showToast = true
      this.toastText = text
    },

    async logout() {
      await firebase.auth().signOut()
      this.toast('Você saiu da sua conta!')
    }
  },

  created() {
    firebase.auth().onAuthStateChanged(user => {
      if(user) {
        this.shouldLogin = false
        this.userEmail = user.email ?? ''
        this.userPhoto = user.photoURL ?? ''
        this.userName = user.displayName ?? ''
      }
      else {
        this.shouldLogin = true
      }
      this.ready = true
    })
  },

  data: () => ({
    shouldLogin: true,
    ready: false,
    showToast: false,
    toastText: '',
    userPhoto: '',
    userEmail: '',
    userName: '',
  }),
});
</script>
