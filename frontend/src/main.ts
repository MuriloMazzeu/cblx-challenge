import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'

import firebase from 'firebase/app'
firebase.initializeApp({
  apiKey: "AIzaSyDD9KcCqizo-EoRul8WlD2X_ve5WifJzqw",
  authDomain: "cblx-challenge.firebaseapp.com",
  projectId: "cblx-challenge",
  storageBucket: "cblx-challenge.appspot.com",
  messagingSenderId: "569988305097",
  appId: "1:569988305097:web:f83e3e247ac4badfbbfd18"
})

Vue.config.productionTip = false
new Vue({ router, store, vuetify, render: h => h(App) }).$mount('#app')
