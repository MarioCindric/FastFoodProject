<script setup>
import { ref, computed } from 'vue'
import axios from 'axios'
import { useAuthStore } from '../../stores/auth'

const props = defineProps({
  adresa: Array
})
const emit = defineEmits(['adresaAzurirana'])


const auth = useAuthStore()
const korisnikId = auth.user.id

// Backend daje listu ali pošto imam samo jednu uzimam tu ili null ako nema
const aktivnaAdresa = computed(() => props.adresa?.[0] || null)

// Dodaj ili uredi
const mod = ref('dodaj') 
const showAdresaModal = ref(false)
const showBrisanjeModal = ref(false)


const forma = ref({
  id: null,
  grad: '',
  ulica: '',
  brojStana: '',
  kat: ''
})

const msg = ref('')


function pripremiDodavanje() {
  mod.value = 'dodaj'
  forma.value = {
    id: null,
    grad: '',
    ulica: '',
    brojStana: '',
    kat: ''
  }
  msg.value = ''
  showAdresaModal.value = true
}


function pripremiUredi() {
  mod.value = 'uredi'
  const a = aktivnaAdresa.value
  forma.value = {
    id: a.id,
    grad: a.grad,
    ulica: a.ulica,
    brojStana: a.brojStana || '',
    kat: a.kat || ''
  }
  msg.value = ''
  showAdresaModal.value = true
}

// Samo su grad i ulica obavezni jer korisnik ne mora zivjeti u stanu
function validiraj() {
  if (!forma.value.grad.trim() || !forma.value.ulica.trim()) {
    msg.value = 'Grad i ulica su obavezni.'
    return false
  }
  msg.value = ''
  return true
}

// Dodavanje / spremanje adrese
async function spremiAdresu() {
  if (!validiraj()) return

  try {

    if (mod.value === 'dodaj') {
      await axios.post('/Adresa/add', {
        korisnikId,
        grad: forma.value.grad,
        ulica: forma.value.ulica,
        brojStana: forma.value.brojStana,
        kat: forma.value.kat
      })
    } else {
      await axios.post('/Adresa/edit', { ...forma.value })
    }

    
    const get = await axios.get(`/Adresa/adresa/korisnik/${korisnikId}`)
    const nova = get.data[0] || null
    emit('adresaAzurirana', nova)

    showAdresaModal.value = false
  } catch {
    msg.value = 'Greška pri spremanju adrese.'
  }
}


// Brisanje adrese
async function obrisiAdresu() {
  try {
    const id = aktivnaAdresa.value.id
    await axios.delete(`/Adresa/delete/${id}`)
    emit('adresaAzurirana', null)
    showBrisanjeModal.value = false
  } catch (err) {
    msg.value = 'Greška pri brisanju adrese.'
  }
}
</script>

<template>
  <div class="adresa-list">
    <template v-if="aktivnaAdresa">
      <div class="adresa-row">
        <span class="label">Grad</span>
        <span class="value">{{ aktivnaAdresa.grad }}</span>
      </div>
      <div class="adresa-row">
        <span class="label">Ulica</span>
        <span class="value">{{ aktivnaAdresa.ulica }}</span>
      </div>
      <div v-if="aktivnaAdresa.brojStana" class="adresa-row">
        <span class="label">Broj stana</span>
        <span class="value">{{ aktivnaAdresa.brojStana }}</span>
      </div>
      <div v-if="aktivnaAdresa.kat" class="adresa-row">
        <span class="label">Kat</span>
        <span class="value">{{ aktivnaAdresa.kat }}</span>
      </div>
      <div class="adresa-row actions">
        <Button label="Uredi" severity="success" @click="pripremiUredi" />
        <Button label="Obriši" severity="danger" class="ms-2" @click="showBrisanjeModal = true" />
      </div>
    </template>
    <template v-else>
      <div class="adresa-row actions">
        <Button label="Dodaj" severity="primary" @click="pripremiDodavanje" />
      </div>
    </template>
  </div>

  <Dialog v-model:visible="showAdresaModal" modal :header="mod==='dodaj'?'Dodaj adresu':'Uredi adresu'" :closable="false">
    <div class="p-fluid">
      <InputText v-model="forma.grad" placeholder="Grad" class="mb-2" />
      <InputText v-model="forma.ulica" placeholder="Ulica" class="mb-2" />
      <InputText v-model="forma.brojStana" placeholder="Broj stana (opcionalno)" class="mb-2" />
      <InputText v-model="forma.kat" placeholder="Kat (opcionalno)" class="mb-2" />
      <small v-if="msg" class="p-error">{{ msg }}</small>
    </div>
    <template #footer>
      <Button label="Zatvori" severity="warn" @click="showAdresaModal=false" />
      <Button label="Spremi" severity="success" @click="spremiAdresu" />
    </template>
  </Dialog>

  <Dialog v-model:visible="showBrisanjeModal" modal header="Brisanje adrese" :closable="false">
    <p>Želite li sigurno obrisati adresu?</p>
    <template #footer>
      <Button label="Odustani" severity="warn" @click="showBrisanjeModal=false" />
      <Button label="Obriši" severity="danger" @click="obrisiAdresu" />
    </template>
  </Dialog>

</template>

<style scoped>
.adresa-list {
  max-width: 900px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.adresa-row {
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

