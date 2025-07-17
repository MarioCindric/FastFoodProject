<script setup>
import { ref, onMounted, watch, computed } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import { useToast } from 'primevue/usetoast'

const router = useRouter()
const toast = useToast()


const cart = ref([])


const korisnikId = ref(null)

const nacinDostave = ref('Preuzimanje u restoranu')
const nacinPlacanja = ref('Plaćanje pri preuzimanju')

const adresaGrad = ref('')
const adresaUlica = ref('')
const adresaZgrada = ref('')
const adresaKat = ref('')


const now = new Date(Date.now() + 30 * 60 * 1000)


const vrijemeDostave = ref({
  hours: now.getHours(),
  minutes: now.getMinutes(),
  seconds: 0
})

const karticaBrojKartice = ref('')
const karticaExp = ref('')
const karticaCvc = ref('')

const adresaZaKorisnika = ref(null)
const karticaZaKorisnika = ref(null)
const napomena = ref('')

// Izračun cijene
const ukupno = computed(() =>
  cart.value.reduce((s, i) => s + i.cijena * i.count, 0).toFixed(2)
)



const narudzbaTime = () => {
  const now = new Date()
  const time = vrijemeDostave.value
  const dateTime = new Date(
    now.getFullYear(),
    now.getMonth(),
    now.getDate(),
    time.hours + 2, 
    time.minutes,
    time.seconds
  )
  return isNaN(dateTime.valueOf()) ? now : dateTime
}

watch(nacinDostave, (novi) => {
  if (novi === 'Dostava' && adresaZaKorisnika.value) {
    adresaGrad.value = adresaZaKorisnika.value.grad
    adresaUlica.value = adresaZaKorisnika.value.ulica
    adresaZgrada.value = adresaZaKorisnika.value.brojStana || ''
    adresaKat.value = adresaZaKorisnika.value.kat || ''
  }
})


watch(nacinPlacanja, (novi) => {
  if (novi === 'Kartično plaćanje' && karticaZaKorisnika.value) {
    karticaBrojKartice.value = karticaZaKorisnika.value.brojKartice
    karticaExp.value = karticaZaKorisnika.value.datumIsteka
    karticaCvc.value = karticaZaKorisnika.value.sigurnosniKod
  }
})

// Validacija kartice preko regexa
function validirajKarticu() {
  const brojKarticeRegex = /^\d{4} \d{4} \d{4} \d{4}$/
  const datumIstekaRegex = /^(0[1-9]|1[0-2])\/\d{2}$/
  const sigurnosniKodRegex = /^\d{3}$/

  if (!karticaBrojKartice.value.trim() || !karticaExp.value.trim() || !karticaCvc.value.trim()) {
    toast.add({ severity: 'secondary', summary: 'Kartica', detail: 'Sva polja su obavezna.', life: 3000 })
    return false
  }
  if (!brojKarticeRegex.test(karticaBrojKartice.value)) {
    toast.add({ severity: 'secondary', summary: 'Kartica', detail: 'Format: 1234 1234 1234 1234', life: 3000 })
    return false
  }
  if (!datumIstekaRegex.test(karticaExp.value)) {
    toast.add({ severity: 'secondary', summary: 'Kartica', detail: 'Format: MM/YY (npr. 02/25)', life: 3000 })
    return false
  }
  if (!sigurnosniKodRegex.test(karticaCvc.value)) {
    toast.add({ severity: 'secondary', summary: 'Kartica', detail: 'Sigurnosni kod mora imati 3 znamenke.', life: 3000 })
    return false
  }

  return true
}


function validirajVrijemeDostave() {
  const sada = new Date()
  const uneseno = new Date(
    sada.getFullYear(),
    sada.getMonth(),
    sada.getDate(),
    vrijemeDostave.value.hours,
    vrijemeDostave.value.minutes,
    vrijemeDostave.value.seconds || 0
  )

  const razlikaMin = (uneseno - sada) / (1000 * 60) // milisekunde dijelim da dobijem minute
  if (razlikaMin < 29) {
    toast.add({ severity: 'secondary', summary: 'Vrijeme dostave', detail: 'Dostava mora biti barem 30 minuta od trenutnog vremena.', life: 3000 })
    return false
  }

  if (
    uneseno.getHours() > 20 ||
    (uneseno.getHours() === 20 && uneseno.getMinutes() > 30)
  ) {
    toast.add({ severity: 'secondary', summary: 'Vrijeme dostave', detail: 'Narudžbe se primaju do 20:30.', life: 3000 })
    return false
  }

  return true
}


async function Dodaj() {
  if (!validirajVrijemeDostave()) return

  if (nacinDostave.value === 'Dostava') {
    if (!adresaGrad.value.trim() || !adresaUlica.value.trim()) {
      toast.add({ severity: 'secondary', summary: 'Dostava', detail: 'Za dostavu morate unijeti grad i ulicu.', life: 3000 })
      return
    }
  }

  if (nacinPlacanja.value === 'Kartično plaćanje') {
    if (!validirajKarticu()) return
  }

  const user = JSON.parse(localStorage.getItem('user'))
  const korisnikIdFinal = user?.id || null
  if (korisnikIdFinal) korisnikId.value = korisnikIdFinal

  // Dodavanje narudžbe
  const podaci = {
    KorisnikId: korisnikIdFinal,
    UkupnaCijena: parseFloat(ukupno.value),
    NacinDostave: nacinDostave.value,
    NacinPlacanja: nacinPlacanja.value,
    StatusNarudzbe: 'Kreirana',
    VrijemeDostave: narudzbaTime().toISOString(),
    Grad: adresaGrad.value,
    Ulica: adresaUlica.value,
    Stan: adresaZgrada.value,
    Kat: adresaKat.value,
    BrojKartica: karticaBrojKartice.value,
    DatumIsteka: karticaExp.value,
    SigurnosniKod: karticaCvc.value,
    Napomena: napomena.value
  }

  try {
    await axios.post("https://localhost:5001/Narudzba/add", podaci)
    const res = await axios.get('https://localhost:5001/Narudzba/sve')
    const sveNarudzbe = res.data
    
    const zadnja = sveNarudzbe.reduce((max, cur) => cur.id > max.id ? cur : max, sveNarudzbe[0])
    const narudzbaId = zadnja.id

    // Preko for of, uzmi vrijednost svakog jela iz košarice i spremi ga u NarudzbaJelo
    for (let item of cart.value) {
      await axios.post('https://localhost:5001/NarudzbaJelo/add', {
        narudzbaId,
        jeloId: item.id,
        kolicina: item.count,
        cijena: item.cijena
      })
    }

    localStorage.removeItem('cart')
    cart.value = []

    if (!user) {
      localStorage.setItem('zadnjaNarudzbaId', narudzbaId)
      router.push('/PotvrdaNarudzbe')
    } else {
      toast.add({
        severity: 'secondary',
        summary: 'Narudžba uspješna',
        detail: 'Status možete pratiti pod "Aktivne narudžbe".',
        life: 3000
      })
      setTimeout(() => router.push('/AktivneNarudzbe'), 3000)
    }

  } catch (e) {
    const err = e.response?.data || e.message
    console.error(err)
    toast.add({ severity: 'secondary', summary: 'Greška', detail: JSON.stringify(err), life: 3000 })
  }
}

onMounted(async () => {
  const saved = localStorage.getItem('cart')
  if (saved) cart.value = JSON.parse(saved)

  const user = JSON.parse(localStorage.getItem('user'))
  if (!user?.id) return

  korisnikId.value = user.id

  try {
  const res = await axios.get(`/Adresa/adresa/korisnik/${user.id}`)
  const adresa = res.data
  adresaZaKorisnika.value = adresa?.[0] || null
} catch {
  adresaZaKorisnika.value = null
}

try {
  const res = await axios.get(`/KreditnaKartica/kartica/korisnik/${user.id}`)
  const kartice = res.data
  karticaZaKorisnika.value = kartice?.[0] || null
} catch {
  karticaZaKorisnika.value = null
}

})
</script>


<template>
  <Toast position="bottom-center" />

  <div class="narudzba-container">
  <div class="row">
    <div class="col-8">
      <form @submit.prevent="Dodaj">
        <div class="form-group">
          <label>Način dostave</label>
          <select class="form-control" v-model="nacinDostave">
            <option>Preuzimanje u restoranu</option>
            <option>Dostava</option>
          </select>
        </div>

        <template v-if="nacinDostave === 'Dostava'">
          <div class="form-group">
            <label>Grad</label>
            <input v-model="adresaGrad" type="text" class="form-control" />
          </div>
          <div class="form-group">
            <label>Ulica</label>
            <input v-model="adresaUlica" type="text" class="form-control" />
          </div>
          <div class="form-group">
            <label>Broj stana</label>
            <input v-model="adresaZgrada" type="text" class="form-control" />
          </div>
          <div class="form-group">
            <label>Kat</label>
            <input v-model="adresaKat" type="text" class="form-control" />
          </div>
        </template>

        <div class="form-group">
          <label>Vrijeme dostave</label>
          <VueDatePicker dark v-model="vrijemeDostave" time-picker/>
        </div>

        <div class="form-group">
          <label>Način plaćanja</label>
          <select class="form-control" v-model="nacinPlacanja">
            <option>Plaćanje pri preuzimanju</option>
            <option>Kartično plaćanje</option>
          </select>
        </div>

        <template v-if="nacinPlacanja === 'Kartično plaćanje'">
          <div class="form-group">
            <label>Broj kartice</label>
            <input v-model="karticaBrojKartice" type="text" class="form-control" />
          </div>
          <div class="row">
            <div class="col">
              <label>Ističe</label>
              <input v-model="karticaExp" type="text" class="form-control" />
            </div>
            <div class="col">
              <label>Sigurnosni kod</label>
              <input v-model="karticaCvc" type="text" class="form-control" />
            </div>
          </div>
        </template>
        <div class="form-group mt-4">
            <label>Napomena</label>
            <textarea v-model="napomena" class="form-control" rows="3"></textarea>
        </div>


        <button type="submit" class="btn btn-success mt-5">Naruči</button>
      </form>
    </div>

    <div class="col-4">
      <h5 class="mt-4">Pregled košarice</h5>
      <ul class="list-group mb-3">
        <li v-for="i in cart" :key="i.id" class="list-group-item d-flex justify-content-between align-items-center">
          <span>{{ i.naziv }} ×{{ i.count }}</span>
          <span>{{ (i.cijena * i.count).toFixed(2) }} €</span> 
        </li>
      </ul>
      <p class="fw-bold">Ukupno: {{ ukupno }} €</p>
    </div>
  </div>
  </div>
</template>

<style scoped>
.list-group-item {
  background-color: #1e1e1e ;
  color: #f1f1f1 ;
  border: 1px solid #333 ;
}

.list-group {
  border-radius: 8px;
  overflow: hidden;
}

.narudzba-container {
  max-width: 1000px;
  margin: 2rem auto;
  padding: 2rem;
  background-color: #1c1c1c;
  border-radius: 12px;
  box-shadow: 0 4px 16px rgba(0,0,0,0.3);
  color: #f1f1f1;
}

.narudzba-container .form-group,
.narudzba-container .row > .col {
  margin-bottom: 1.25rem;
}

.narudzba-container input,
.narudzba-container select,
.narudzba-container textarea {
  background-color: #2a2a2a;
  color: #f1f1f1;
  border: 1px solid #444;
  border-radius: 6px;
  padding: 0.5rem;
}

.narudzba-container label {
  margin-bottom: 0.5rem;
  display: block;
  font-weight: 500;
}

.narudzba-container .btn-success {
  width: 100%;
  padding: 0.75rem;
  font-weight: bold;
}

</style>

