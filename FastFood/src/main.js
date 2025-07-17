import { createApp } from 'vue'
import App from './App.vue'
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.js'
import 'bootstrap-icons/font/bootstrap-icons.css'
import router from './router'
import VueDatePicker from '@vuepic/vue-datepicker'
import '@vuepic/vue-datepicker/dist/main.css'
import { createVfm, VueFinalModal } from 'vue-final-modal'
import 'vue-final-modal/style.css'
import { createPinia } from 'pinia'
import axios from 'axios'
import {
  Chart as ChartJS,
  Title,
  Tooltip,
  Legend,
  BarElement,
  CategoryScale,
  LinearScale
} from 'chart.js'
import { MatrixController, MatrixElement } from 'chartjs-chart-matrix'

import PrimeVue from 'primevue/config'
import Dialog from 'primevue/dialog'
import Aura from '@primeuix/themes/aura'
import Button from 'primevue/button'
import InputText from 'primevue/inputtext'
import Password from 'primevue/password'
import Toast from 'primevue/toast'
import ToastService from 'primevue/toastservice'
import ScrollTop from 'primevue/scrolltop';
import Menubar from 'primevue/menubar'
import Tabs from 'primevue/tabs'
import TabList from 'primevue/tablist'
import Tab from 'primevue/tab'
import TabPanels from 'primevue/tabpanels'
import TabPanel from 'primevue/tabpanel'
import DatePicker from 'primevue/datepicker'
import Rating from 'primevue/rating'
import SpeedDial from 'primevue/speeddial';
import StarRating from 'vue3-star-ratings'
import Carousel from 'primevue/carousel'
import InputNumber from 'primevue/inputnumber'
import Checkbox from 'primevue/checkbox'
import Dropdown from 'primevue/dropdown'
import 'primeicons/primeicons.css'
import './style.css'

 
ChartJS.register(MatrixController, MatrixElement, CategoryScale, LinearScale, BarElement, Title, Tooltip, Legend)

axios.defaults.baseURL = 'https://localhost:5001'
const token = localStorage.getItem('token')
if (token) axios.defaults.headers.common.Authorization = `Bearer ${token}`

const vfm = createVfm()

createApp(App)
  .component('VueDatePicker', VueDatePicker)
  .component('VueFinalModal', VueFinalModal)
  .use(router)
  .component('Button', Button)
  .component('Dialog', Dialog)
  .component('Password', Password)
  .component('InputText', InputText)
  .component('Toast', Toast)
  .component('ScrollTop', ScrollTop)
  .component('MenuBar', Menubar)
  .component('Tabs', Tabs)
  .component('TabPanel', TabPanel)
  .component('TabList', TabList)
  .component('Tab', Tab)
  .component('TabPanels', TabPanels)
  .component('DatePicker', DatePicker)
  .component('Rating', Rating)
  .component('SpeedDial', SpeedDial)
  .component('StarRating', StarRating)
  .component('Carousel', Carousel)
  .component('InputNumber', InputNumber)
  .component('Checkbox', Checkbox)
  .component('Dropdown', Dropdown)
  .use(ToastService)
  .use(PrimeVue, {
  theme: { preset: Aura, dark: true}
  })
  .use(createPinia())
  .use(vfm)
  .mount('#app')
