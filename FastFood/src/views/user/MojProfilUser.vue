<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useAuthStore } from '../../stores/auth'
import AdresaPrikaz from '../../components/user/AdresaPrikaz.vue'
import KarticaPrikaz from '../../components/user/KarticaPrikaz.vue'
import KorisnikPodaci from '../../components/user/KorisnikPodaci.vue'

// Funkcija za ažuriranje adrese iz child komponente
function azurirajAdresu(nova) {
  if (nova === null) {
    adresa.value = []
  } else {
    adresa.value = [nova]
  }
}

// Funkcija za ažuriranje kreditne kartice iz child komponente
function azurirajKarticu(nova) {
  if (nova === null) {
    kartica.value = []
  } else {
    kartica.value = [nova]
  }
}

// Dohvaćanje podataka o korisniku iz auth storea
const auth = useAuthStore()
const adresa = ref([])
const kartica = ref([])
const aktivanTab = ref('podaci')

// Dohvaća adresu i karticu korisnika
onMounted(async () => {
  const id = auth.user.id
  try {
    const resAdresa = await axios.get(`/Adresa/adresa/korisnik/${id}`)
    adresa.value = resAdresa.data

    const resKartica = await axios.get(`/KreditnaKartica/kartica/korisnik/${id}`)
    kartica.value = resKartica.data
  } catch (err) {
    console.error("Greška pri dohvaćanju podataka:", err)
  }
})
</script>


<template>
  <div class="container mt-4">
    <ul class="nav nav-tabs">
      <li class="nav-item">
        <a
          class="nav-link"
          :class="{ active: aktivanTab === 'podaci' }"
          @click="aktivanTab = 'podaci'"
        >
          Osobni podaci
        </a>
      </li>
      <li class="nav-item">
        <a
          class="nav-link"
          :class="{ active: aktivanTab === 'adresa' }"
          @click="aktivanTab = 'adresa'"
        >
          Adresa
        </a>
      </li>
      <li class="nav-item">
        <a
          class="nav-link "
          :class="{ active: aktivanTab === 'kartica' }"
          @click="aktivanTab = 'kartica'"
        >
          Način plaćanja
        </a>
      </li>
    </ul>
<!-- Na osnovu aktivnog taba prikazuje potrebne podatke-->
    <div class="tab-content mt-4">
      <div v-if="aktivanTab === 'podaci'">
        <KorisnikPodaci />
      </div>
      <div v-if="aktivanTab === 'adresa'">
        <AdresaPrikaz :adresa="adresa" @adresaAzurirana="azurirajAdresu" />
      </div>
      <div v-if="aktivanTab === 'kartica'">
        <KarticaPrikaz :kartica="kartica" @karticaAzurirana="azurirajKarticu" />
      </div>
    </div>
  </div>
</template>

<style scoped>
.nav-tabs .nav-link.active {
  background-color: rgba(255, 255, 255, 0.1);
  color: #fff;
  border-color: transparent;
  border-radius: 0.3rem;
}

.container {
  background-color: #1e1e1e;
  min-height: 85vh;
  max-width: 100%;
  padding: 2rem;
  border-radius: 0.5rem;
}

.nav-link.active
{
  color: #00c4cc!important;
  
}
</style>

