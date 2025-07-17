<script setup>
import { ref, computed, onMounted } from 'vue';
import axios from 'axios';

// Filtriranje narudzbi koje su u pripremi
const sveNarudzbe = ref([]);
const filtriraneNarudzbe = computed(() =>
  sveNarudzbe.value.filter(n => n.statusNarudzbe === "U pripremi")
);

// Reaktivne varijable
const showDetaljiModal = ref(false);
const aktivnaNarudzba = ref(null);
const detaljiJela = ref([]);

// Funkcija za dohvacanje narudzbi
async function dohvatiNarudzbe() {
  try {
    const res = await axios.get('/Narudzba/sve');
    sveNarudzbe.value = res.data;
  } catch (err) {
    console.error("Greška pri dohvaćanju narudžbi:", err);
  }
}

// Funkcija za detalje, postavlja aktivnu narudzbu i dohvaća jela za nju, otvara modal
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

// Zatvara modal, prazni listu
function zatvoriDetalje() {
  showDetaljiModal.value = false;
  aktivnaNarudzba.value = null;
  detaljiJela.value = [];
}
// ažuriranje statusa, ... spread operator, napravim kopiju te zamjenim status
async function promijeniStatus(noviStatus) {
  const podaci = {
    ...aktivnaNarudzba.value,
    statusNarudzbe: noviStatus
  };

  try {
    await axios.post("https://localhost:5001/Narudzba/edit", podaci);
    zatvoriDetalje();
    await dohvatiNarudzbe();
  } catch (err) {
    console.error("Greška pri ažuriranju statusa:", err);
  }
}

onMounted(() => {
  dohvatiNarudzbe();
});
</script>


<template>
  <div class="container mt-4">
    <h3>Obrada narudžbe</h3>
    <table class="table table-striped table-rounded">
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
            <Button icon="pi pi-eye" severity="info" size="small" @click="() => otvoriDetalje(n)" />
          </td>
        </tr>
      </tbody>
    </table>

    <Dialog v-model:visible="showDetaljiModal" modal header="Detalji narudžbe" :closable="false">
        <!-- Mora biti jer baca null nekad -->
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
        <Button label="Zatvori" severity="secondary" @click="zatvoriDetalje" />
        <Button
          label="Završi"
          severity="success"
          @click="promijeniStatus(aktivnaNarudzba?.nacinDostave === 'Dostava' ? 'Na dostavi' : 'Završeno')"
        />
      </template>
    </Dialog>
  </div>
</template>
