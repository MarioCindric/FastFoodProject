<script setup>
import { ref, computed } from 'vue'
import axios from 'axios'
import { useAuthStore } from '../../stores/auth'


const props = defineProps({
  kartica: Array
})
const emit = defineEmits(['karticaAzurirana'])


const auth = useAuthStore()
const korisnikId = auth.user.id

// Computed za aktivnu karticu (prva iz niza ili null)
const aktivnaKartica = computed(() => props.kartica?.[0] || null)


const mod = ref('dodaj')
const showKarticaModal = ref(false)
const showBrisanjeModal = ref(false)

const forma = ref({
  id: null,
  brojKartice: '',
  datumIsteka: '',
  sigurnosniKod: ''
})

const msg = ref('')


function pripremiDodavanje() {
  mod.value = 'dodaj'
  forma.value = { id: null, brojKartice: '', datumIsteka: '', sigurnosniKod: '' }
  msg.value = ''
  showKarticaModal.value = true
}


function pripremiUredi() {
  if (!aktivnaKartica.value) return
  mod.value = 'uredi'
  forma.value = {
    id: aktivnaKartica.value.id,
    brojKartice: aktivnaKartica.value.brojKartice,
    datumIsteka: aktivnaKartica.value.datumIsteka,
    sigurnosniKod: aktivnaKartica.value.sigurnosniKod
  }
  msg.value = ''
  showKarticaModal.value = true
}

function validiraj() {
  const brojKarticeRegex = /^\d{4} \d{4} \d{4} \d{4}$/
  const datumIstekaRegex = /^(0[1-9]|1[0-2])\/\d{2}$/
  const sigurnosniKodRegex = /^\d{3}$/

  if (!forma.value.brojKartice.trim() || !forma.value.datumIsteka.trim() || !forma.value.sigurnosniKod.trim()) {
    msg.value = 'Sva polja su obavezna.'
    return false
  }
  if (!brojKarticeRegex.test(forma.value.brojKartice)) {
    msg.value = 'Traženi format: 1234 1234 1234 1234'
    return false
  }
  if (!datumIstekaRegex.test(forma.value.datumIsteka)) {
    msg.value = 'Traženi format: MM/YY (npr. 02/25)'
    return false
  }
  if (!sigurnosniKodRegex.test(forma.value.sigurnosniKod)) {
    msg.value = 'Sigurnosni kod mora imati 3 znamenke'
    return false
  }
  msg.value = ''
  return true
}

// Spremanje nove ili uređene kartice 
async function spremiKarticu() {
  if (!validiraj()) return;

  try {
    if (mod.value === 'dodaj') {
      await axios.post('/KreditnaKartica/add', {
        korisnikId,
        brojKartice: forma.value.brojKartice,
        datumIsteka: forma.value.datumIsteka,
        sigurnosniKod: forma.value.sigurnosniKod
      });
    } else {
      await axios.post('/KreditnaKartica/edit', { ...forma.value });
    }

    // dohvat kartice i emit
    const get = await axios.get(`/KreditnaKartica/kartica/korisnik/${korisnikId}`);
    const novaKartica = get.data[0] || null;
    emit('karticaAzurirana', novaKartica);

    showKarticaModal.value = false;
  } catch {
    msg.value = mod.value === 'dodaj'
      ? 'Greška pri dodavanju kartice.'
      : 'Greška pri ažuriranju kartice.';
  }
}

// Brisanje postojeće kartice: emit je u biti - kartica azurirana, evo ti nova vrijednost
async function obrisiKarticu() {
  try {
    const id = aktivnaKartica.value?.id
    if (!id) return
    await axios.delete(`/KreditnaKartica/delete/${id}`)
    emit('karticaAzurirana', null)
    showBrisanjeModal.value = false
  } catch {
    msg.value = 'Greška pri brisanju kartice.'
  }
}
</script>

<template>
  <div class="kartica-list">
    <template v-if="aktivnaKartica">
      <div class="kartica-row">
        <span class="label">Broj kartice</span>
        <span class="value">{{ aktivnaKartica.brojKartice }}</span>
      </div>
      <div class="kartica-row">
        <span class="label">Datum isteka</span>
        <span class="value">{{ aktivnaKartica.datumIsteka }}</span>
      </div>
      <div class="kartica-row">
        <span class="label">Sigurnosni kod</span>
        <span class="value">{{ aktivnaKartica.sigurnosniKod }}</span>
      </div>
      <div class="kartica-row actions">
        <Button label="Uredi" severity="success" @click="pripremiUredi" />
        <Button label="Obriši" severity="danger" class="ms-2" @click="showBrisanjeModal = true" />
      </div>
    </template>
    <template v-else>
      <div class="kartica-row actions">
        <Button label="Dodaj" severity="primary" @click="pripremiDodavanje" />
      </div>
    </template>
  </div>
  <Dialog v-model:visible="showKarticaModal" modal :header="mod === 'dodaj' ? 'Dodaj karticu' : 'Uredi karticu'" :closable="false">
    <div class="p-fluid">
      <InputText v-model="forma.brojKartice" placeholder="Broj kartice" class="mb-2" />
      <InputText v-model="forma.datumIsteka" placeholder="Datum isteka (MM/YY)" class="mb-2" />
      <InputText v-model="forma.sigurnosniKod" placeholder="Sigurnosni kod" class="mb-2" />
      <small v-if="msg" class="p-error">{{ msg }}</small>
    </div>
    <template #footer>
      <Button label="Zatvori" severity="warn" @click="showKarticaModal = false" />
      <Button label="Spremi" severity="success" @click="spremiKarticu" />
    </template>
  </Dialog>

  <Dialog v-model:visible="showBrisanjeModal" modal header="Brisanje kartice" :closable="false">
    <p>Želite li sigurno obrisati karticu?</p>
    <template #footer>
      <Button label="Odustani" severity="warn" @click="showBrisanjeModal = false" />
      <Button label="Obriši" severity="danger" @click="obrisiKarticu" />
    </template>
  </Dialog>

</template>


<style scoped>
.kartica-list {
  max-width: 900px;
  margin: 0 auto;
  display: flex;
  flex-direction: column;
  gap: 1.25rem;
}

.kartica-row {
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

/* Modal širina */
.p-dialog {
  width: 450px !important;
}
</style>
