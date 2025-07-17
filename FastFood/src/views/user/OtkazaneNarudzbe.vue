

<!-- VIŠAK -->


<script setup>
import { ref, computed, onMounted } from 'vue'
import axios from 'axios'


const sveNarudzbe = ref([])
const modalOtvoren = ref(false)
const aktivnaNarudzba = ref(null)
const detaljiJela = ref([])
const user = JSON.parse(localStorage.getItem('user'))


const filtriraneNarudzbe = computed(() =>
  sveNarudzbe.value.filter(n =>
    n.korisnikId === user.id &&
    (n.statusNarudzbe === "Otkazana")
  )
)

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


onMounted(() => {
  dohvatiNarudzbe()
  setInterval(dohvatiNarudzbe, 10000)
})
</script>

<template>
  <div class="container mt-4">
    <h3>Otkazane narudžbe</h3>
    <table class="table table-striped">
      <thead>
        <tr>
          <th>Rbr.</th>
          <th>Ukupna cijena</th>
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
          <td>{{ n.ukupnaCijena.toFixed(2) }}€</td>
          <td>{{ n.nacinDostave }}</td>
          <td>{{ n.nacinPlacanja }}</td>
          <td>{{ n.statusNarudzbe }}</td>
          <td>{{ new Date(n.datumKreiranja).toLocaleString('hr-HR', {
            day: '2-digit',
            month: '2-digit',
            hour: '2-digit',
            minute: '2-digit'
          })}}</td>
          <td>
            <Button icon="pi pi-eye" severity="info" size="small" @click="() => otvoriModal(n)" />
          </td>
        </tr>
      </tbody>
    </table>

    <Dialog v-model:visible="modalOtvoren" modal header="Detalji narudžbe" :closable="false">
      <div v-if="aktivnaNarudzba">
        <h5 class="mb-3">Narudžba #{{ aktivnaNarudzba.id }}</h5>
        <ul>
          <li v-for="j in detaljiJela" :key="j.id">{{ j.nazivJela }} x{{ j.kolicina }}</li>
        </ul>

        <hr />
        <p><strong>Vrijeme dostave:</strong>
          {{ new Date(aktivnaNarudzba.vrijemeDostave).toLocaleTimeString('hr-HR', {
            hour: '2-digit',
            minute: '2-digit'
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
      </div>

      <template #footer>
        <Button label="Zatvori" severity="warn" @click="zatvoriModal" />
      </template>
    </Dialog>
  </div>
</template>


