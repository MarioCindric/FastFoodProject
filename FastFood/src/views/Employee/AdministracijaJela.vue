<script setup>
import { ref, onMounted, computed } from 'vue'
import { useRouter } from 'vue-router'
import axios from 'axios'
import 'bootstrap/dist/js/bootstrap.bundle'

const jela = ref([])
const router = useRouter()
const filterNaziv = ref('')
const kategorije = ref([])

const dialogVidljiv = ref(false)
const odabranoJelo = ref(null)
const prosjecnaOcjena = ref(null)
const statistikaOcjena = ref({})


const prikaziNedostupne = ref(true)


onMounted(async () => {
  jela.value = (await axios.get('/jelo')).data
  await dohvatiKategorije()
})


async function dohvatiKategorije() {
  try {
    const res = await axios.get('/KategorijaJelo/kategorije/sve')
    kategorije.value = res.data.map(k => k.naziv) 
  } catch (err) {
    console.error('Greška pri dohvaćanju kategorija:', err)
  }
}


function grupirajPoKategoriji(jela) {
  const grupe = {}
  jela.forEach(j => {
    if (!grupe[j.nazivKategorije]) grupe[j.nazivKategorije] = []
    grupe[j.nazivKategorije].push(j)
  })
  return grupe


}




// uzima filtirana jela i grupira ih po kategoriji
const filtriranaJela = computed(() => {
  const naziv = filterNaziv.value.trim().toLowerCase()
  return jela.value.filter(j =>
    j.naziv.toLowerCase().includes(naziv) &&
    (prikaziNedostupne.value || j.dostupno === true)
  )
})

function togglePrikaz() {
  prikaziNedostupne.value = !prikaziNedostupne.value
}

const grupiranaJela = computed(() => grupirajPoKategoriji(filtriranaJela.value))

function otvoriJelo(jeloId) {
  router.push(`/jelo/${jeloId}`)
}



function resizeUrl(url) {
  return url.replace('/upload/', '/upload/w_180,h_120,c_fill/')
  //url.replace('/upload/', '/upload/w_180,h_120,c_pad,b_white/')

}



async function prikaziOcjenu(jelo) {
  odabranoJelo.value = jelo
  dialogVidljiv.value = true
  prosjecnaOcjena.value = null
  statistikaOcjena.value = []

  try {
    const prosjekRes = await axios.get(`/api/OcjenaJela/prosjecna/${jelo.id}`)
    prosjecnaOcjena.value = parseFloat(prosjekRes.data)

    const statistikaRes = await axios.get(`/api/OcjenaJela/ocjeneStatistika/${jelo.id}`)
    statistikaOcjena.value = statistikaRes.data
  } catch {
    prosjecnaOcjena.value = null
    statistikaOcjena.value = []
  }
}




function getSpeedDialItems(jelo) {
  return [
    {
      label: 'Uredi',
      icon: 'pi pi-pencil',
      command: () => otvoriJelo(jelo.id)
    },
    {
      label: 'Ocjena',
      icon: 'pi pi-star',
      command: () => prikaziOcjenu(jelo)
    }
  ]
}

</script>
  
<template>
  
  <!--Ispis kategorija-->
  <div class="my-3 px-4">
  <div class="d-flex flex-wrap justify-content-center gap-2">
    <div v-for="kategorija in kategorije" :key="kategorija">
      <a :href="`#${kategorija}`" class="kategorija-btn">
        {{ kategorija }}
      </a>
    </div>
  </div>
</div>


<!--Filtriranje jela-->
  <div class="input-group my-4" style="max-width: 300px;">
    <input
      v-model="filterNaziv"
      type="text"
      class="form-control"
      placeholder="Unesi naziv jela..."
    />
  </div>

<!-- Sakrivanje / prikazivanje nedostupnog jela-->
  <div class="my-3">
  <button @click="togglePrikaz" class="btn btn-secondary">
    {{ prikaziNedostupne ? 'Sakrij nedostupna jela' : 'Prikaži nedostupne jela' }}
  </button>
</div>

  <!-- Ispis jela u kartice-->
  <div v-for="(grupa, naziv) in grupiranaJela" :key="naziv" class="mb-5">
    <h3 :id="naziv">{{ naziv }}</h3>
    <div class="row">
     <div
        v-for="jelo in grupa"
        :key="jelo.id"
        class="col-md-4 mb-4"
        style="cursor: pointer;"
      >

          <div class="card h-100 d-flex flex-row align-items-center position-relative" style="min-height: 200px;">
          <div class="p-3 flex-grow-1 d-flex flex-column justify-content-between h-100">
            <div>
              <h5 class="card-title">{{ jelo.naziv }}</h5>
              
              <p class="card-text opis-jela">{{ jelo.opis }}</p>
              
            </div>
            <div class="mt-auto">
              <p class="card-text fw-bold mb-0">{{ jelo.cijena.toFixed(2) }} €</p>
            </div>
          </div>
          <img
            v-if="jelo.slikaUrl"
            :src="resizeUrl(jelo.slikaUrl)"
            style="width: 180px; height: 120px; object-fit: contain; border-radius: 8px;"
            alt="slika jela"
            class="m-4"
          />
          <span v-if="!jelo.dostupno" class="badge bg-secondary position-absolute mt-5 m-2">Nedostupno</span>
          <SpeedDial
            :model="getSpeedDialItems(jelo)"
            direction="down"
            :transitionDelay="80"
            showIcon="pi pi-plus"
            hideIcon="pi pi-times"
            class="position-absolute"
            mask
            :style="{ top: '0px', right: '0px' }"
            :buttonProps="{ style: { backgroundColor: '#00c4cc', color: '#121212', border: 'none' },severity: 'info', rounded: false }"
          />

        </div>

      </div>
    </div>
  </div>
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
      <div class="mt-4 w-100">
      <div
        v-for="o in statistikaOcjena"
        :key="o.zvjezdice"
        class="d-flex justify-content-between border-bottom py-1 px-3"
      >
        <span>{{ o.zvjezdice }}  <i class="pi pi-star text-warning ms-1"></i></span>
        <span>{{ o.broj }}</span>
      </div>
    </div>


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

</style>