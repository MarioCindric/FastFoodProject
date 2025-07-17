<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'

const router = useRouter()

// Dohvacam zadnju narudžbu koju sam spremio kod naručivanja
const narudzbaId = localStorage.getItem('zadnjaNarudzbaId')
const status = ref('Kreirana')
const vrijeme = ref(null)
const vrijemeCountdown = ref(null)
const countdown = ref('')
const error = ref(false)
let intervalId = null
let countdownInterval = null

// Provjera statusa narudzbe
async function provjeriStatus() {
  try {
    // Dohvaćam status narudžbe
    const res = await axios.get(`/Narudzba/status/${narudzbaId}`)
    status.value = res.data.status


    // Nakon 5 minuta briše id local storagea i vraća na početnu stranicu
    if (status.value === 'Završeno' || status.value === 'Otkazana') {
      // Kada završi, nema potrebe za zvanjem funkcije više
      clearInterval(intervalId)
      setTimeout(() => {
        localStorage.removeItem('zadnjaNarudzbaId')
        router.push('/')
      }, 5 * 60 * 1000)
    }

  } catch {
    error.value = true
    clearInterval(intervalId)
    router.push('/404')
  }
}


// Countdown
function azurirajCountdown()
{
  if(!vrijemeCountdown.value) return

  // Razlika između odabranog vremena i sadašnjosti
  const sada = new Date();
  const razlika = vrijemeCountdown.value - sada

  // Ako je vrijeme prošlo prikazuje 0 i miče interval da se funkcija više ne poziva
  if (razlika <= 0) 
  {
    countdown.value = '0h : 0m : 0s'
    if (countdownInterval !== null) clearInterval(countdownInterval)
    return
  }


  const sati = Math.floor(razlika / ( 1000 * 60 * 60)) // 1000 milisekundi  * 60 sekundi * 60 minuta
  const minute = Math.floor((razlika % (1000 * 60 * 60)) / (1000 * 60))
  const sekunde = Math.floor((razlika % (1000 * 60)) / 1000) 

  countdown.value = `${sati}h : ${minute}m : ${sekunde}s`
}
onMounted(async () => {
  if (!narudzbaId) {
    router.push('/404')
    return
  }

  try {
    const res = await axios.get(`/Narudzba/vrijeme/${narudzbaId}`)
    vrijemeCountdown.value = new Date(res.data.vrijemeDostave)
    vrijeme.value = new Date(res.data.vrijemeDostave).toLocaleTimeString('hr-HR', {
      hour: '2-digit',
      minute: '2-digit'
    })
  } catch {
    vrijeme.value = null
  }

  provjeriStatus()
  countdownInterval = setInterval(azurirajCountdown, 1000)
  intervalId = setInterval(provjeriStatus, 10000)
})


</script>

<template>
  <div class="container mt-5 text-center">
    <h3>Vaša narudžba je zaprimljena. Na ovoj stranici možete pratiti narudžbu</h3>

    <p v-if="!error">
      Trenutni status: <strong>{{ status }}</strong>
    </p>

    <p v-if="status === 'U pripremi'" class="alert alert-info mt-4">
      Vaša narudžba se priprema, biti će gotova oko {{ vrijeme }}<br><br>
      Preostalo: <b>{{ countdown }}</b>
    </p>
    
    <p v-if="status === 'Na dostavi'" class="alert alert-info mt-4">
      Vaša narudžba se dostavlja
    </p>

    <p v-if="status === 'Završeno'" class="alert alert-success mt-4">
      Vaša narudžba je završena!
    </p>

    <div v-if="status === 'Otkazana'" class="alert alert-danger mt-4">
      Vaša narudžba je otkazana.
    </div>
  </div>
</template>
