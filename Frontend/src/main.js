import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import 'vue-toast-notification/dist/theme-sugar.css';
import VueSweetalert2 from 'vue-sweetalert2';
import 'sweetalert2/dist/sweetalert2.min.css';
import 'vuetify/styles'
import { createVuetify } from 'vuetify'
import * as components from 'vuetify/components'
import * as directives from 'vuetify/directives'

const vuetify = createVuetify({
    components,
    directives,
  })
  


const app = createApp(App)
app.use(vuetify)
app.use(router);
app.use(VueSweetalert2);
app.mount("#app");


