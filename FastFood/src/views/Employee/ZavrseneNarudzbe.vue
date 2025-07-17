<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';

const sveNarudzbe = ref([]);

const statusFilter = ref("Sve")
const filterDatumTekst = ref('')

// Computed svojstvo koje filtrira narudžbe ovisno o statusu i datumu
const filtriraneNarudzbe = computed(() => {
  return sveNarudzbe.value.filter(n => {
    const jeStatus = statusFilter.value === "Sve"
      ? ["Završeno", "Otkazana"].includes(n.statusNarudzbe)
      : n.statusNarudzbe === statusFilter.value

    const datumString = new Date(n.datumKreiranja).toLocaleDateString('hr-HR')
    const jeDatum = filterDatumTekst.value === '' || datumString.includes(filterDatumTekst.value)

    return jeStatus && jeDatum
  })
})



// Reaktivne varijable za modal i trenutno odabranu narudžbu
const showDetaljiModal = ref(false);
const aktivnaNarudzba = ref(null);
const detaljiJela = ref([]);

// Otvaranje detalja narudžbe
async function otvoriDetalje(narudzba) {
  aktivnaNarudzba.value = narudzba;
  try {
    const res = await axios.get(`/NarudzbaJelo/narudzba/${narudzba.id}`);
    detaljiJela.value = res.data;
  } catch (err) {
    console.error("Greška pri dohvaćanju jela:", err);
    detaljiJela.value = [];
  }
  showDetaljiModal.value = true;
}

// Zatvaranje detalja
function zatvoriDetalje() {
  showDetaljiModal.value = false;
  aktivnaNarudzba.value = null;
  detaljiJela.value = [];
}

// Dohvaćanje svih narudžbi kod mountanja komponente
onMounted(async () => {
  try {
    const res = await axios.get('/Narudzba/sve');
    sveNarudzbe.value = res.data;
  } catch (err) {
    console.error("Greška pri dohvaćanju narudžbi:", err);
  }
});
</script>

<template>
  <div class="container mt-4">
    <h3>Završene narudžbe</h3>

    <!-- Filter za prikaz statusa narudžbi -->
    <div class="mb-3">
      <label for="statusFilter" class="form-label">Filtriraj po statusu</label>
      <select id="statusFilter" class="form-control w-auto" v-model="statusFilter">
        <option value="Sve">Sve</option>
        <option value="Završeno">Završeno</option>
        <option value="Otkazana">Otkazana</option>
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

    <table class="table table-striped table-dark table-rounded table-bordered">
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
            day: '2-digit', month: '2-digit',  year: 'numeric', hour: '2-digit', minute: '2-digit'
          })}}</td>
          <td>
            <Button icon="pi pi-eye" :style="{ backgroundColor: '#00c4cc', color: '#121212', border: 'none' }" size="small" @click="() => otvoriDetalje(n)" />
          </td>
        </tr>
      </tbody>
    </table>

    <!-- Modal za prikaz detalja narudžbe -->
    <Dialog v-model:visible="showDetaljiModal" modal :closable="false" header-class="text-center">
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
    <Button label="Zatvori" severity="warn" @click="zatvoriDetalje" />
  </template>
</Dialog>

  </div>
</template>

<style>
.table-rounded {
  border-collapse: separate !important;
  border-spacing: 0;
  border-radius: 0.5rem;
  overflow: hidden;
}
</style>
