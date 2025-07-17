<script setup>
import { ref, onMounted, watch } from 'vue'
import axios from 'axios'
import { Bar } from 'vue-chartjs'

const mjesec = ref(new Date().getMonth() + 1)
const godina = ref(new Date().getFullYear())

const mjeseci = [
  'Siječanj', 'Veljača', 'Ožujak', 'Travanj', 'Svibanj', 'Lipanj',
  'Srpanj', 'Kolovoz', 'Rujan', 'Listopad', 'Studeni', 'Prosinac'
]

// Zadnjih 5 godina
const trenutna = new Date().getFullYear()
const godine = []
for (let i = 0; i < 5; i++) {
  godine.push(trenutna - i)
}

// Defaultni prikaz u biti
const chartData = ref({
  labels: ['Mjesec'],
  datasets: [{
    label: 'Broj narudžbi',
    data: [0],
    backgroundColor: ['rgba(0, 196, 204)'],
    barThickness: 80
  }]
})


const options = {
  responsive: true,
  plugins: {
    legend: {
      display: false
    },
    title: {
      display: true,
      text: 'Broj narudžbi po mjesecu i godini',
      color: '#f1f1f1',
      font: {
        size: 16,
        weight: 'bold'
      }
    }
  },
  scales: {
    y: {
      beginAtZero: true,
      title: {
        display: true,
        text: 'Broj narudžbi',
        color: '#f1f1f1'
      },
      ticks: {
        color: '#f1f1f1'
      },
      grid: {
        color: 'rgba(255,255,255,0.2)'
      }
    },
    x: {
      title: {
        display: true,
        text: 'Vremenski period',
        color: '#f1f1f1'
      },
      ticks: {
        color: '#f1f1f1'
      },
      grid: {
        color: 'rgba(255,255,255,0.2)'
      }
    }
  }
}


// Šaljem parametre, dobivam podatke
async function ucitajPodatke() {
  try {
    const response = await axios.get('/api/Statistika/BrojNarudzbi', {
      params: { mjesec: mjesec.value, godina: godina.value }
    })
    const data = response.data
    chartData.value = {
      labels: ['Mjesec'],
      datasets: [{
        label: 'Broj narudžbi',
        data: [data.mjesec],
        backgroundColor: ['rgba(0, 196, 204)'],
        barThickness: 80
      }]
    }
  } catch (e) {
    chartData.value = {
      labels: ['Mjesec'],
      datasets: [{
        label: 'Broj narudžbi',
        data: [0],
        backgroundColor: ['rgba(0, 196, 204)'],
        barThickness: 80
      }]
    }
  }
}
onMounted(ucitajPodatke)
// Kad se godina ili mjsec promjenje, zove funkciju
watch([mjesec, godina], ucitajPodatke)
</script>

<template>
  <div>
    <div class="d-flex gap-3 mb-3">
      <div>
        <label for="mjesec" class="form-label">Mjesec:</label>
        <select id="mjesec" class="form-control" v-model="mjesec">
          <option v-for="(label, index) in mjeseci" :key="index" :value="index + 1">{{ label }}</option>
        </select>
      </div>
      <div>
        <label for="godina" class="form-label">Godina:</label>
        <select id="godina" class="form-control" v-model="godina">
          <option v-for="g in godine" :key="g" :value="g">{{ g }}</option>
        </select>
      </div>
    </div>

    
      <Bar :data="chartData" :options="options" />
   
  </div>
</template>