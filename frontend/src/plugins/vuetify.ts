import Vue from 'vue';
import pt from 'vuetify/src/locale/pt';
import Vuetify from 'vuetify/lib/framework';

Vue.use(Vuetify);
export default new Vuetify({
    lang: {
        locales: { pt },
        current: 'pt',
    },
});
