<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useAuthStore } from '../../stores/auth'
import { useRouter } from 'vue-router'

const router = useRouter()
const auth = useAuthStore()

const ime = ref(auth.user.firstName)
const prezime = ref(auth.user.lastName)
const email = ref(auth.user.email)

const poruka = ref('')
const greska = ref('')
const sviKorisnici = ref([])

const showUrediModal = ref(false)
const showBrisanjeModal = ref(false)


async function dohvatiSveKorisnike() {
  const res = await axios.get('/api/auth/svi-korisnici')
  sviKorisnici.value = res.data
}


function emailZauzet() {
  const mail = email.value.toLowerCase()
  return sviKorisnici.value.some(x =>
    x.id !== auth.user.id && x.email?.toLowerCase() === mail
  )
}


async function azurirajProfil() {
  greska.value = ''
  if (!ime.value || !prezime.value || !email.value) {
    greska.value = 'Sva polja su obavezna.'
    return
  }
  if (emailZauzet()) {
    greska.value = 'Email je već u upotrebi.'
    return
  }

  try {
    await axios.put(`/api/auth/uredi/korisnik/${auth.user.id}`, {
      firstName: ime.value,
      lastName: prezime.value,
      email: email.value
    })
    auth.user.firstName = ime.value
    auth.user.lastName = prezime.value
    auth.user.email = email.value
    showUrediModal.value = false
  } catch {
    greska.value = 'Greška pri ažuriranju.'
  }
}

// Brisanje profila
async function obrisiProfil() {
  // Brisanje vezane adrese ako ima
  try {
    const adresaRes = await axios.get(`/Adresa/adresa/korisnik/${auth.user.id}`)
    const adresa = adresaRes.data[0]
    if (adresa) await axios.delete(`/Adresa/delete/${adresa.id}`)
    const karticaRes = await axios.get(`/KreditnaKartica/kartica/korisnik/${auth.user.id}`)
    const kartica = karticaRes.data[0]
    if (kartica) await axios.delete(`/KreditnaKartica/delete/${kartica.id}`)
        // Mjenjam id u null 
    await axios.put(`/Narudzba/promjeniId`,
     { 
        korisnikId: auth.user.id
     })

    // Brisem korisnika
    await axios.delete(`/api/auth/obrisi/${auth.user.id}`)

    auth.logout()
    await router.push('/')
  } catch {
    poruka.value = 'Greška pri brisanju profila.'
  }
}
</script>

<template>
  <div class="korisnik-list">
    <div class="korisnik-row">
      <span class="label">Ime</span>
      <span class="value">{{ auth.user.firstName }}</span>
    </div>
    <div class="korisnik-row">
      <span class="label">Prezime</span>
      <span class="value">{{ auth.user.lastName }}</span>
    </div>
    <div class="korisnik-row">
      <span class="label">Email</span>
      <span class="value">{{ auth.user.email }}</span>
    </div>
    <div class="korisnik-row actions">
      <Button label="Uredi" severity="success"
              @click="showUrediModal=true; dohvatiSveKorisnike()" />
      <Button label="Obriši" severity="danger" class="ms-2"
              @click="showBrisanjeModal=true" />
    </div>

    <Dialog v-model:visible="showUrediModal" modal header="Uredi profil" :closable="false">
      <div class="field">
        <label>Ime</label>
        <InputText v-model="ime" class="w-100" />
      </div>
      <div class="field mt-2">
        <label>Prezime</label>
        <InputText v-model="prezime" class="w-100" />
      </div>
      <div class="field mt-2">
        <label>Email</label>
        <InputText v-model="email" class="w-100" />
      </div>
      <p class="text-danger mt-2" v-if="greska">{{ greska }}</p>
      <template #footer>
        <Button label="Odustani" severity="secondary" @click="showUrediModal=false" />
        <Button label="Spremi" severity="success" @click="azurirajProfil" />
      </template>
    </Dialog>

    <Dialog v-model:visible="showBrisanjeModal" modal header="Brisanje profila" :closable="false">
      <p>Želite li sigurno obrisati profil?</p>
      <p class="text-danger" v-if="poruka">{{ poruka }}</p>
      <template #footer>
        <Button label="Odustani" severity="secondary"  @click="showBrisanjeModal=false" />
        <Button label="Obriši" severity="danger" @click="obrisiProfil" />
      </template>
    </Dialog>
  </div>
</template>

<style scoped>
.korisnik-list {
  max-width: 900px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.korisnik-row {
  display: flex;
  justify-content: space-between;
  align-items: center;
  border-bottom: 1px solid #333;
  padding: 0.75rem 0;
}


.actions {
  border: none;
  padding-top: 0.5rem;
}

</style>
