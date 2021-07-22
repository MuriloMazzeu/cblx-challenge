<template>
  <v-container fluid class="fill-height wallpaper">
    <v-row align="center" justify="center" class="fill-height">
      <v-col align="center" cols="12">
        <v-card :loading="!ready" min-width=320 max-width=420>
          <v-card-title>Autenticação OAuth</v-card-title>
          <v-divider></v-divider>

          <v-card-text class="text-center">
            <v-img width="96" class="mx-auto rounded-circle" src="../assets/logo.jpg"></v-img>
            <p class="text-subtitle-2 my-5">Para continuar, por favor faça login no sistema</p>
            <v-btn outlined :disabled="loading || !ready" :loading="loading" @click="login" color="green darken-1">
              <v-icon left>mdi-google</v-icon>
              <span class="btn-google-text">continuar com google</span>
            </v-btn>
          </v-card-text>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script lang="ts">
  import Vue from 'vue'
  import firebase from 'firebase/app'
  import 'firebase/auth'

  export default Vue.extend({
    name: 'LoginPanel',
    props: ['ready'],
    methods: {
      async login() {
        this.loading = true
        try {
          const provider = new firebase.auth.GoogleAuthProvider()
          await firebase.auth().signInWithPopup(provider)
          this.$emit('logged')
        } catch (error) {
          console.log(error);
        }
        this.loading = false
      }
    },

    data: () => ({
     loading: false
    }),
  })
</script>

<style lang="scss">
.wallpaper {
  background-image: url('../assets/bg.png'), linear-gradient(to right, #02446e, #044a7e);
  background-position: center;
  background-size: cover;
}
.btn-google-text{
  border-left: 1px solid;
  padding-left: 6px;
}
</style>