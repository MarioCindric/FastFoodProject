<script setup>
import { ref, onMounted, watch } from 'vue'
import axios from 'axios'
import { Bar } from 'vue-chartjs'

const mjesec = ref(new Date().getMonth() + 1)
const godina = ref(new Date().getFullYear())
const kategorija = ref('sve')
const kategorije = ref([])

const mjeseci = [
  'Siječanj', 'Veljača', 'Ožujak', 'Travanj', 'Svibanj', 'Lipanj',
  'Srpanj', 'Kolovoz', 'Rujan', 'Listopad', 'Studeni', 'Prosinac'
]
const trenutna = new Date().getFullYear()
const godine = []
for (let i = 0; i < 5; i++) {
  godine.push(trenutna - i)
}


const top5ChartData = ref({
  labels: [],
  datasets: [{
    label: 'Ukupna količina',
    data: [],
    backgroundColor: []
  }]
})

const top5Options = {
  // prilagodba veličini containera
  responsive: true,
  plugins: {
    legend: { display: false },
    title: {
      display: true,
      text: 'Top 5 najtraženijih jela',
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
        text: 'Količina',
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
        text: 'Naziv jela',
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


async function dohvatiKategorije() {
  try {
    const res = await axios.get('/KategorijaJelo/kategorije/sve')
    kategorije.value = res.data.map(k => k.naziv)
  } catch (err) {
    console.error('Greška pri dohvaćanju kategorija:', err)
  }
}

// Šaljem parametre backend funkciji
async function ucitajTop5() {
  try {
    const response = await axios.get('/api/Statistika/top5', {
      params: {
        mjesec: mjesec.value,
        godina: godina.value,
        kategorija: kategorija.value
      }
    })

    const data = response.data
    // Dobivam objekt, mapiram vrijednosti, npr pizza = 40 burger = 30
    top5ChartData.value = {
      labels: data.map(i => i.nazivJela),
      datasets: [{
        label: 'Ukupna količina',
        data: data.map(i => i.ukupno),
        backgroundColor: data.map(() => 'rgba(0, 196, 204)')
      }]
    }
  } catch (e) {
    top5ChartData.value = {
      labels: [],
      datasets: [{
        label: 'Ukupna količina',
        data: [],
        backgroundColor: []
      }]
    }
  }
}


onMounted(() => {
  dohvatiKategorije()
  ucitajTop5()
})
watch([mjesec, godina, kategorija], ucitajTop5)
</script>

<template>
  <div>
    <div class="mb-3 d-flex gap-3 align-items-end">
      <div>
        <label for="mjesec" class="form-label">Mjesec:</label>
        <select id="mjesec" class="form-control" v-model="mjesec">
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
      <div>
        <label for="kategorija" class="form-label">Kategorija:</label>
        <select id="kategorija" class="form-control" v-model="kategorija">
          <option value="sve">Sve</option>
          <option v-for="naziv in kategorije" :key="naziv" :value="naziv">
            {{ naziv }}
          </option>
      </select>
      </div>
    </div>

       <!--Komponenta se rekreira svaki put kad se ključ promjeni jer nije radilo drugačije-->
      <Bar
        :key="mjesec + '-' + godina + '-' + kategorija"
        :data="top5ChartData"
        :options="top5Options"
      />
  </div>
</template>

