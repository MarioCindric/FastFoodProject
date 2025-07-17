<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'

const sveNarudzbe = ref([])
const modalOtvoren = ref(false)
const aktivnaNarudzba = ref(null)
const detaljiJela = ref([])
const user = JSON.parse(localStorage.getItem('user'))

const statusFilter = ref("Sve")
const filterDatumTekst = ref('')

// Computed svojstvo koje filtrira narudžbe ovisno o statusu i dodatno po datumu
const filtriraneNarudzbe = computed(() => {
  return sveNarudzbe.value.filter(n => {
    const jeStatus = statusFilter.value === "Sve"
      ? ["Kreirana", "U pripremi", 'Na dostavi'].includes(n.statusNarudzbe)
      : n.statusNarudzbe === statusFilter.value

    const datumString = new Date(n.datumKreiranja).toLocaleDateString('hr-HR')
    const jeDatum = filterDatumTekst.value === '' || datumString.includes(filterDatumTekst.value)

    return jeStatus && jeDatum
  })
})

async function dohvatiNarudzbe() {
  try {
    const res = await axios.get('/Narudzba/sve')
    sveNarudzbe.value = res.data
  } catch (err) {
    console.error("Greška pri dohvaćanju narudžbi:", err)
  }
}

async function otvoriModal(narudzba) {
  aktivnaNarudzba.value = narudzba
  try {
    const res = await axios.get(`/NarudzbaJelo/narudzba/${narudzba.id}`)
    detaljiJela.value = res.data
  } catch (err) {
    console.error("Greška pri dohvaćanju jela:", err)
    detaljiJela.value = []
  }
  modalOtvoren.value = true
}

function zatvoriModal() {
  modalOtvoren.value = false
  aktivnaNarudzba.value = null
  detaljiJela.value = []
}

// async function promijeniStatus(noviStatus) {
//   const podaci = {
//     ...aktivnaNarudzba.value,
//     statusNarudzbe: noviStatus
//   }
//   try {
//     await axios.post("https://localhost:5001/Narudzba/edit", podaci)
//     zatvoriModal()
//     await dohvatiNarudzbe()
//   } catch (err) {
//     console.error("Greška pri ažuriranju statusa:", err)
//   }
// }

async function promijeniStatus(noviStatus) {
  try {
    // 1. Ažuriranje statusa
    const podaci = {
      ...aktivnaNarudzba.value,
      statusNarudzbe: noviStatus
    }

    await axios.post("https://localhost:5001/Narudzba/edit", podaci)

    // 2. Ako prelazi iz "Kreirana" u "U pripremi", dodaj zaposlenika
    if (
      aktivnaNarudzba.value.statusNarudzbe === "Kreirana" &&
      (noviStatus === "U pripremi"  || noviStatus === "Otkazana")
    ) {
      const zaposlenikPodaci = {
        id: aktivnaNarudzba.value.id,
        zaposlenikImePrezime: `${user.firstName} ${user.lastName}`
      }

      await axios.post("https://localhost:5001/Narudzba/edit-zaposlenik", zaposlenikPodaci)
    }

    zatvoriModal()
    await dohvatiNarudzbe()
  } catch (err) {
    console.error("Greška pri ažuriranju narudžbe:", err)
  }
}



onMounted(() => {
  dohvatiNarudzbe()
  setInterval(dohvatiNarudzbe, 10000)
})
</script>

<template>
  <div class="container mt-4">
    <h2>Moje aktivne narudžbe</h2>

    <div class="mb-3">
      <label for="statusFilter" class="form-label">Filtriraj po statusu</label>
      <select id="statusFilter" class="form-control w-auto" v-model="statusFilter">
        <option value="Sve">Sve</option>
        <option value="Kreirana">Kreirane</option>
        <option value="U pripremi">U pripremi</option>
        <option value="Na dostavi">Na dostavi</option>
      </select>
    </div>

    <!-- Filter za datum-->
    <div class="input-group my-4" style="max-width: 300px;">
      <input
        v-model="filterDatumTekst"
        type="text"
        class="form-control"
        placeholder="Pretraži po datumu..."
      />
    </div>

    <table class="table table-striped table-dark table-rounded">
      <thead>
        <tr>
          <th>Rbr.</th>
          <th>Način dostave</th>
          <th>Način plaćanja</th>
          <th>Status</th>
          <th>Datum</th>
          <th>Detalji</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(n, index) in filtriraneNarudzbe" :key="n.id">
          <td>{{ index + 1 }}.</td>
          <td>{{ n.nacinDostave }}</td>
          <td>{{ n.nacinPlacanja }}</td>
          <td>{{ n.statusNarudzbe }}</td>
          <td>{{ new Date(n.datumKreiranja).toLocaleString('hr-HR', {
            day: '2-digit', month: '2-digit', hour: '2-digit', minute: '2-digit'
          }) }}</td>
          <td>
            <Button icon="pi pi-eye" severity="info" size="small" @click="() => otvoriModal(n)" />
          </td>
        </tr>
      </tbody>
    </table>

    <Dialog v-model:visible="modalOtvoren" modal :closable="false" header-class="text-center">
      <template #header>
        <div class="w-100 text-center fw-bold fs-5">Detalji narudžbe</div>
      </template>
      <div v-if="aktivnaNarudzba">
        <h5 class="mb-3 text-center">Narudžba #{{ aktivnaNarudzba.id }}</h5>
        <ul>
          <li v-for="(j, index) in detaljiJela" :key="j.id" class="d-flex justify-content-between">
            <span>{{ index + 1 }}. {{ j.nazivJela }}</span>
            <span class="ms-auto">{{ j.kolicina }} × {{ j.cijena.toFixed(2) }} €</span>
          </li>
        </ul>

        <hr />
        <div class="d-flex justify-content-end">
        <span class="fw-bold">Ukupno: {{
          detaljiJela.reduce((s, j) => s + (j.cijena * j.kolicina), 0).toFixed(2)
        }} €</span>
      </div>
       <hr />
        <p><strong>Vrijeme dostave:</strong>
          {{ new Date(aktivnaNarudzba.vrijemeDostave).toLocaleTimeString('hr-HR', {
            hour: '2-digit', minute: '2-digit'
          }) }}
        </p>

        <div v-if="aktivnaNarudzba.nacinDostave === 'Dostava'">
          <p><strong>Adresa:</strong>
            {{ aktivnaNarudzba.grad }}, {{ aktivnaNarudzba.ulica }} {{ aktivnaNarudzba.kucniBroj }}
          </p>
        </div>

        <div v-if="aktivnaNarudzba.napomena">
          <p><strong>Napomena:</strong>
            {{ aktivnaNarudzba.napomena }}
          </p>
        </div>

        <div v-if="aktivnaNarudzba.zaposlenikImePrezime">
          <p><strong>Odobrio: </strong>
          {{ aktivnaNarudzba.zaposlenikImePrezime }}
         </p>
        </div>
      </div>

      <template #footer>
        <Button label="Zatvori" severity="warn" @click="zatvoriModal" />
        <Button
          v-if="aktivnaNarudzba.statusNarudzbe === 'Kreirana'"
          label="Otkaži" severity="danger"
          @click="promijeniStatus('Otkazana')" />
        <Button
          v-if="aktivnaNarudzba.statusNarudzbe === 'Kreirana'"
          label="Odobri" severity="success"
          @click="promijeniStatus('U pripremi')" />
        <Button
          v-if="aktivnaNarudzba.statusNarudzbe === 'U pripremi'"
          label="Završi" severity="success"
          @click="promijeniStatus(aktivnaNarudzba.nacinDostave === 'Dostava' ? 'Na dostavi' : 'Završeno')" />
        <Button
          v-if="aktivnaNarudzba.statusNarudzbe === 'Na dostavi'"
          label="Završi" severity="success"
          @click="promijeniStatus('Završeno')" />
      </template>
    </Dialog>
  </div>
</template>
