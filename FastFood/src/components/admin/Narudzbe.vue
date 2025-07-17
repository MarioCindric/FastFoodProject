<script setup>

// invisible filter koristim  za skrivanje filtera da se izjednači visina grafova
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { Bar } from 'vue-chartjs'


const chartData = ref({
  labels: ['Dan', 'Tjedan', 'Mjesec', 'Godina', 'Sveukupno'],
  datasets: [{
    label: 'Broj narudžbi',
    data: [],
    backgroundColor: [
      'rgba(0, 196, 204)',
      'rgba(0, 196, 204)',
      'rgba(0, 196, 204)',
      'rgba(0, 196, 204)',
      'rgba(0, 196, 204)'
    ]
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
      text: 'Broj narudžbi za tekuću godinu i ukupnu količinu', 
      color: '#f1f1f1',
      font: {
        size: 16,
        weight: 'bold'
      }
    }
  },
  scales: {
    // osi
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
  },
     backgroundColor: 'transparent'
}


// Ima samo jedan dataset, tu radim sve jer ne trebam optionse mjenjati
onMounted(async () => {
  const response = await axios.get('/api/Statistika/BrojNarudzbi')
  const data = response.data
  chartData.value.datasets[0].data = [
    data.dan,
    data.tjedan,
    data.mjesec,
    data.godina,
    data.sveukupno
  ]
})
</script>

<template>
  <div class="graf-wrapper">
    <div class="d-flex gap-3 mb-3 invisible-filter">
      <div>
        <label for="mjesec" class="form-label">Nista</label>
        <select id="mjesec" class="form-select">
          <option>--</option>
        </select>
      </div>
    </div>
    <!-- Data su podaci options je ponašanje grafa-->
    <Bar v-if="chartData.datasets[0].data.length" :data="chartData" :options="options" />
  </div>
</template>


<style>
.invisible-filter {
  visibility: hidden;
}

</style>



