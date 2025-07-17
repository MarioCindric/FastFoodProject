  <script setup>
import { ref, onMounted, computed } from 'vue'
import axios from 'axios'
import 'bootstrap/dist/js/bootstrap.bundle'
import Kosarica from '../components/Kosarica.vue'
import { watch } from 'vue'
import { useToast } from 'primevue/usetoast'
const toast = useToast()

const jela = ref([])
const cart = ref([])

const filterNaziv = ref('')
const kategorije = ref([])

const dialogVidljiv = ref(false)
const odabranoJelo = ref(null)
const prosjecnaOcjena = ref(null)

async function prikaziOcjenuDialog(jelo) {
  odabranoJelo.value = jelo
  dialogVidljiv.value = true
  prosjecnaOcjena.value = null

  try {
    const res = await axios.get(`/api/OcjenaJela/prosjecna/${jelo.id}`)
    prosjecnaOcjena.value = res.data
  } catch {
    prosjecnaOcjena.value = null
  }
}


const saved = localStorage.getItem('cart')
if (saved) cart.value = JSON.parse(saved)

watch(cart, v =>
  localStorage.setItem('cart', JSON.stringify(v)),
  { deep: true }
)


async function dohvatiKategorije() {
  try {
    const res = await axios.get('/KategorijaJelo/kategorije/sve')
    kategorije.value = res.data.map(k => k.naziv)
  } catch (err) {
    console.error('Greška pri dohvaćanju kategorija:', err)
  }
}


onMounted(async () => {
  jela.value = (await axios.get('https://localhost:5001/jelo')).data
  await dohvatiKategorije()
})


function grupirajPoKategoriji(jela) {
  const grupe = {}
  jela.forEach(j => {
    if (!grupe[j.nazivKategorije]) grupe[j.nazivKategorije] = []
    grupe[j.nazivKategorije].push(j)
  })
  return grupe

  
}


const filtriranaJela = computed(() => {
  const naziv = filterNaziv.value.trim().toLowerCase()
  let rezultat = jela.value.filter(j => j.dostupno)
  if (!naziv) return rezultat
  return rezultat.filter(j => j.naziv.toLowerCase().includes(naziv))
})

const grupiranaJela = computed(() => grupirajPoKategoriji(filtriranaJela.value))

function dodajUKosaricu(proizvod) {
  const stavka = cart.value.find(x => x.id === proizvod.id)
  if (stavka) stavka.count++
  else
    cart.value.push({
      id: proizvod.id,
      naziv: proizvod.naziv,
      cijena: proizvod.cijena,
      count: 1
    })
  toast.add({
    severity: 'secondary',
    summary: 'Uspjeh',
    detail: "Proizvod uspješno dodan u košaricu",
    life: 2000
  })
}


function ukloniUKosaricu(proizvod) {

  
  const stavka = cart.value.find(x => x.id === proizvod.id)
  if (!stavka) return
  stavka.count--
  if (stavka.count <= 0) {
    cart.value = cart.value.filter(x => x.id !== proizvod.id)
  }
}

function resizeUrl(url) {
  if (!url) return ''
  return url.replace('/upload/', '/upload/w_200,h_120,c_fill/')
}
</script>

<template>
  <Toast position="bottom-right" />
  <Kosarica
    :items="cart"
    @decrement-item="ukloniUKosaricu"
  />

  <div class="my-3 px-4">
  <div class="d-flex flex-wrap justify-content-center gap-2">
    <div v-for="kategorija in kategorije" :key="kategorija">
      <a :href="`#${kategorija}`" class="kategorija-btn">
        {{ kategorija }}
      </a>
    </div>
  </div>
</div>


  <!--Filter jela-->
  <div class="input-group my-4" style="max-width: 300px;">
  <input
    v-model="filterNaziv"
    type="text"
    class="form-control"
    placeholder="Unesi naziv jela..."
  />
</div>

  <div v-for="(grupa, naziv) in grupiranaJela" :key="naziv" class="mb-5">
    <h3 :id="naziv">{{ naziv }}</h3>
    <div class="row">
      <div v-for="jelo in grupa" :key="jelo.id" class="col-md-4 mb-4">
        <div class="card h-100 d-flex flex-row align-items-center position-relative" style="min-height: 200px;" @click="prikaziOcjenuDialog(jelo)">
          <div class="p-3 flex-grow-1 d-flex flex-column justify-content-between h-100">
            <h5 class="card-title">{{ jelo.naziv }}</h5>
            <p class="card-text">{{ jelo.opis }}</p>
            <p class="card-text">{{ jelo.cijena.toFixed(2) }} €</p>
          </div>

          <div v-if="jelo.slikaUrl" class="image-container">
            <img
              :src="resizeUrl(jelo.slikaUrl)"
              alt="slika jela"
            />
            <button
                @click.stop="dodajUKosaricu(jelo)"
                class="btn-overlay btn btn-success"
                :disabled="!jelo.dostupno"
              >
                +
          </button>

          </div>
        </div>
      </div>
    </div>
  </div>
  <!-- Gumb za vraćanje na vrh -->
<ScrollTop target="window" :threshold="200" icon="pi pi-arrow-up" :buttonProps="{ severity: 'contrast', raised: true, rounded: true }"/>
<Dialog
  v-model:visible="dialogVidljiv"
  modal
  :style="{ width: '400px' }"
  :closable="false"
>
  <div
    class="d-flex flex-column align-items-center text-center"
    style="gap: 2rem; padding: 2rem 2rem 1rem;"
  >
    <h4 class="mb-0">Ocjena jela</h4>
    <h3 class="mb-0">{{ odabranoJelo?.naziv }}</h3>

    <div class="d-flex flex-column align-items-center">
      <span class="text">Prosječna ocjena jela</span>
      <StarRating
      v-if="prosjecnaOcjena !== null"
      v-model="prosjecnaOcjena"
      :starSize="32"
      starColor="gold"
      inactiveColor="#333333"
      :numberOfStars="5"
      :disableClick="true" 
      />
    </div>
    
  </div>
  <template #footer>
  <div class="d-flex justify-content-end px-3 pb-3">
    <Button label="Zatvori" severity="danger" @click="dialogVidljiv = false" />
  </div>
</template>

</Dialog>

 
</template>


<style scoped>
.card {
  position: relative;
}

.image-container {
  position: relative;
  width: 180px;
  height: 120px;
  margin: 1rem;
  flex-shrink: 0;
}
.image-container img {
  width: 100%;
  height: 100%;
  object-fit: cover;
  border-radius: 8px;
}

</style>
