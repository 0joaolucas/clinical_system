import { createApp } from 'vue'
import { createPinia } from 'pinia'

import App from './App.vue'
import router from './router'
import './style.css'

import { setupInterceptors } from './services/setupInterceptors'

const app = createApp(App)

const pinia = createPinia()
app.use(pinia)

setupInterceptors(pinia)

app.use(router)

app.mount('#app')
