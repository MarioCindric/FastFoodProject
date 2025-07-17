<script setup>
import { ref, watch, onMounted } from 'vue'
import axios from 'axios'
import { Chart } from 'vue-chartjs'


// Odabrani vremenski period (default: tjedan)
const period = ref('dan')

// Odvojeni filteri za od i do datuma
const datumOd = ref(null)
const datumDo = ref(null)

// Za broj narudzbi
// const brojNarudzbi = ref(0)

// Podaci za heatmap
const heatmapData = ref({
  datasets: [{
    label: 'Broj narudžbi',
    data: [],
    backgroundColor: ctx => {
      const value = ctx.raw?.v ?? 0
      if (value < 3) return 'rgba(25, 135, 84, 0.1)'
      if (value <= 4) return 'rgba(139, 213, 131)'
      return 'rgba(25, 135, 84, 1)'
    },
    borderWidth: 1,
    borderColor: 'rgba(255,255,255,0.3)',
    width: () => 20,
    height: () => 20
  }]
})

// Opcije za chart prikaz
const options = {
  responsive: true,
  plugins: {
    title: {
      display: true,
      text: () => {
        if (datumOd.value && datumDo.value) {
          const od = datumOd.value.toLocaleDateString('hr-HR')
          const doD = datumDo.value.toLocaleDateString('hr-HR')
          return `Narudžbe za datum: ${od} – ${doD}`
        }

        const p = period.value
        if (p === 'dan') return 'Narudžbe za današnji dan'
        if (p === 'tjedan') return 'Narudžbe za tekući tjedan'
        if (p === 'mjesec') return 'Narudžbe za tekući mjesec'
        if (p === 'godina') return 'Narudžbe za tekuću godinu'
        return 'Sveukupno'
      },
      color: '#f1f1f1',
      font: {
        size: 16,
        weight: 'bold'
      }
    },
    legend: { display: false },
    tooltip: {
      backgroundColor: '#f1f1f1',
      titleColor: '#121212',
      bodyColor: '#121212',
      callbacks: {
        title: ctx => {
          const d = ctx[0]?.raw?.y ?? '-'
          const i = ctx[0]?.raw?.x ?? '-'
          const h = Math.floor(i)
          const min = (i % 1 === 0) ? '00' : '30'
          return `Dan: ${['Ned', 'Pon', 'Uto', 'Sri', 'Čet', 'Pet', 'Sub'][d]}, Vrijeme: ${h}:${min}`
        },
        label: ctx => `Broj narudžbi: ${ctx.raw?.v ?? 0}`
      }
    }
  },
  scales: {
    x: {
      type: 'linear',
      min: 8,
      max: 20.5,
      title: {
        display: true,
        text: 'Vrijeme',
        color: '#f1f1f1'
      },
      ticks: {
        color: '#f1f1f1',
        stepSize: 0.5,
        callback: value => {
          const h = Math.floor(value)
          const min = (value % 1 === 0) ? '00' : '30'
          return `${h}:${min}`
        }
      },
      grid: {
        color: 'rgba(255,255,255,0.2)'
      }
    },
    y: {
      type: 'linear',
      min: 1,
      max: 6,
      title: {
        display: true,
        text: 'Dani u tjednu',
        color: '#f1f1f1'
      },
      ticks: {
        color: '#f1f1f1',
        callback: value => ['Ned', 'Pon', 'Uto', 'Sri', 'Čet', 'Pet', 'Sub'][value]
      },
      grid: {
        color: 'rgba(255,255,255,0.2)'
      }
    }
  }
}

// Postavljanje podataka za chart
function postaviPodatke(data) {
  heatmapData.value = {
    datasets: [{
      label: 'Broj narudžbi',
      data: data.map(item => ({
        x: item.interval / 2,
        y: item.dan,
        v: item.broj
      })),
      backgroundColor: ctx => {
        const value = ctx.raw?.v ?? 0
        if (value < 3) return 'rgba(69, 104, 103)'
        if (value <= 4) return 'rgba(144, 255, 254)'
        return 'rgba(0, 223, 254)'
      },
      borderWidth: 1,
      borderColor: 'rgba(255,255,255,0.3)',
      width: () => 20,
      height: () => 20
    }]
  }
}

// Učitavanje podataka iz backend-a
async function ucitajPoFilterima() {
  const params = { period: period.value }

  // Ako korisnik ručno definira datume, dodaj ih u query, sv-SE koristim zbog korektnog formata za slanje u query, hrvatski ne valja
if (datumOd.value) params.od = datumOd.value.toLocaleDateString('sv-SE')
if (datumDo.value) params.doDatuma = datumDo.value.toLocaleDateString('sv-SE')


  const response = await axios.get('/api/Statistika/heatmap', { params })
  postaviPodatke(response.data)

  //Za broj narudzbi
  //brojNarudzbi.value = response.data.reduce((suma, item) => suma + item.broj, 0)

}

// Automatsko učitavanje prilikom mounta
onMounted(ucitajPoFilterima)

// Reakcija na promjenu filtera
watch([period, datumOd, datumDo], ucitajPoFilterima)
watch(period, () => {
  datumOd.value = null
  datumDo.value = null
  ucitajPoFilterima()
})
</script>

<template>
  <div>
    <!-- Red 1: Vremenski period -->
    <label class="form-label">Vremenski period:</label>
    <div class="d-flex gap-3 mb-3">
      <select id="period" class="form-control" v-model="period" style="max-width: 200px;">
        <option value="dan">Dan</option>
        <option value="tjedan">Tjedan</option>
        <option value="mjesec">Mjesec</option>
        <option value="godina">Godina</option>
        <option value="sve">Sve</option>
      </select>
    </div>

    <!-- Red 2: Od i Do -->
    <div class="d-flex gap-3 flex-wrap">
      <!-- Od -->
      <div style="max-width: 200px;">
        <label class="form-label">Od:</label>
          <VueDatePicker 
          dark
          v-model="datumOd" 
          locale="hr" 
          :format="'dd.MM.yyyy'" 
          :enable-time-picker="false" 
          auto-apply
          :max-date="new Date()"
        />
      </div>

      <!-- Do -->
      <div style="max-width: 200px;">
        <label class="form-label">Do:</label>
        <VueDatePicker 
          dark
          v-model="datumDo" 
          locale="hr" 
          :format="'dd.MM.yyyy'" 
          :enable-time-picker="false" 
          auto-apply 
          :max-date="new Date()"
        />
      </div>
    </div>
   
 <!-- Za broj narudzbi 
    <p class="mt-3 text-white">Broj narudžbi: {{ brojNarudzbi }}</p>
--> 
    

   <label class="mt-4"><b>Legenda</b></label>
    <div class="d-flex justify-content-start align-items-center gap-2 mt-2 mb-1">
      
      <div class="d-flex align-items-center gap-2">
        <div style="width: 20px; height: 20px; background-color: rgba(69, 104, 103); border: 1px solid #ccc;"></div>
        <small>0 - 2</small>
        <div style="width: 20px; height: 20px; background-color: rgba(144, 255, 254); border: 1px solid #ccc;"></div>
        <small>3 – 4</small>
        <div style="width: 20px; height: 20px; background-color: rgba(0, 223, 254); border: 1px solid #ccc;"></div>
        <small> > 4</small>
      </div>
  </div>
    <!-- Prikaz grafa -->
    <Chart :type="'matrix'" :data="heatmapData" :options="options" />  
  </div>
</template>
