<script setup>
import { ref, watch, onMounted } from 'vue'
import axios from 'axios'
import { Bar } from 'vue-chartjs'

const godina = ref('')
const mjesec = ref('')

const mjeseci = [
  'Siječanj', 'Veljača', 'Ožujak', 'Travanj', 'Svibanj', 'Lipanj',
  'Srpanj', 'Kolovoz', 'Rujan', 'Listopad', 'Studeni', 'Prosinac'
]

const trenutna = new Date().getFullYear()
const godine = []
for (let i = 0; i < 5; i++) {
  godine.push(trenutna - i)
}


const prometData = ref({
  labels: [],
  datasets: [{
    label: 'Ukupan promet (EUR)',
    data: [],
    backgroundColor: 'rgba(0, 196, 204)'
  }]
})

const options = {
  // prilagodba veličini containera
  responsive: true,
  plugins: {
    legend: { display: false },
    title: {
      display: true,
      text: 'Promet', // naslov
      color: '#f1f1f1',
      font: {
        size: 16,
        weight: 'bold'
      }
    }
  },
  // u scales automatski rasporedi Y os po vrijednosti koja se ispisuje
  scales: {
    y: {
      beginAtZero: true,
      title: {
        display: true,
        text: 'EUR',
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
        text: 'Period',
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


async function ucitajPromet() {
  // Može a i ne mora imati parametre
  const params = {}
  if (mjesec.value) params.mjesec = mjesec.value
  if (godina.value) params.godina = godina.value

  try {
    const response = await axios.get('/api/Statistika/promet', { params })
    const data = response.data
// Ovo dobivam od backenda, ako je oboje 0 onda sveukupno, ako je godina bez mjeseca onda samo godinam
// Ako je oboje onda dobivam npr 6.2025, u biti za label
    prometData.value = {
      labels: data.map(item => // Vraća niz pa mapiram
        item.godina === 0 ? 'Sveukupno' :
        item.mjesec === 0 ? `${item.godina}` :
        `${item.mjesec}.${item.godina}`
      ),
      // Npr, ukupan promet (eur) 200
      datasets: [{
        label: 'Ukupan promet (EUR)',
        data: data.map(item => item.ukupno),
        backgroundColor: 'rgba(0, 196, 204)'
      }]
    }
  } catch {
    prometData.value = {
      labels: [],
      datasets: [{
        label: 'Ukupan promet (EUR)',
        data: [],
        backgroundColor: 'rgba(0, 196, 204)'
      }]
    }
  }
}

onMounted(ucitajPromet)
watch([mjesec, godina], ucitajPromet)
</script>

<template>
  <div>
    <div class="mb-3 d-flex gap-3 align-items-end">
      <div>
        <label for="mjesec" class="form-label">Mjesec:</label>
        <select id="mjesec" class="form-control" v-model="mjesec">
          <option value="">Sve</option>
          <option v-for="(label, index) in mjeseci" :key="index" :value="index + 1">{{ label }}</option>
        </select>
      </div>
      <div>
        <label for="godina" class="form-label">Godina:</label>
        <select id="godina" class="form-control" v-model="godina">
          <option value="">Sve</option>
          <option v-for="g in godine" :key="g" :value="g">{{ g }}</option>
        </select>
      </div>
    </div>

    <Bar :data="prometData" :options="options" />
  </div>
</template>
