<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
// Samo zaposlenici
const zaposlenici = ref([])
const odabraniZaposlenik = ref(null)
const mod = ref('dodaj')

const username = ref('')
const email = ref('')
const password = ref('')
const firstName = ref('')
const lastName = ref('')
const msg = ref('')
const brisanjeMsg = ref('')


const showBrisanjeModal = ref(false)
const showZaposlenikModal = ref(false)


const sviKorisnici = ref([])

// Zaposlenici, admini, korisnici
async function dohvatiSveKorisnike() {
  const res = await axios.get('/api/auth/svi-korisnici')
  sviKorisnici.value = res.data
}


// Zaposlenici i admini
async function dohvatiZaposlenike() {
  const res = await axios.get('/api/auth/zaposlenici')
  zaposlenici.value = res.data
}


function pripremiDodavanje() {
  mod.value = 'dodaj'
  username.value = ''
  email.value = ''
  password.value = ''
  firstName.value = ''
  lastName.value = ''
  msg.value = ''
  showZaposlenikModal.value = true
}


function pripremiUredi(z) {
  mod.value = 'uredi'
  odabraniZaposlenik.value = z
  username.value = z.userName
  email.value = z.email
  password.value = ''
  firstName.value = z.firstName
  lastName.value = z.lastName
  msg.value = ''
  showZaposlenikModal.value = true
}

// Provjera inputa, kod uređivanja se lozinka ne mjenja
function validacija() {
  if (!username.value || !email.value || !firstName.value || !lastName.value || (mod.value === 'dodaj' && !password.value)) {
    msg.value = 'Sva polja su obavezna.'
    return false
  }

  if (mod.value === 'dodaj') {
    const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$/
    if (!passwordRegex.test(password.value)) {
      msg.value = 'Lozinka mora sadržavati veliko slovo, broj i poseban znak'
      return false
    }
  }
  return true
}



function postojiKorisnik() {
  const uname = username.value.toLowerCase()
  const mail = email.value.toLowerCase()
  const aktivniId = odabraniZaposlenik.value?.id

 
  const emailZauzet = sviKorisnici.value.some(x =>
// Ako dodajem onda je prvi uvjet true, inače, uređujem, isključujem trenutnog korisnika da ga ne detektira kao duplikatJ
    (mod.value === 'dodaj' || x.id !== aktivniId) && 
    x.email?.toLowerCase() === mail
  )

  const usernameZauzet = sviKorisnici.value.some(x =>
    (mod.value === 'dodaj' || x.id !== aktivniId) &&
    x.userName.toLowerCase() === uname
  )

  if (emailZauzet) {
    msg.value = 'Email je već u upotrebi.'
    return true
  }

  if (usernameZauzet) {
    msg.value = 'Korisničko ime je već zauzeto.'
    return true
  }

  return false
}


async function spremiZaposlenika() {
  if (!validacija()) return
  if (postojiKorisnik()) {
    return
  }
  // Registracija korisnika ako se dodaje

  if (mod.value === 'dodaj') {
    await axios.post('/api/auth/register', {
      username: username.value,
      email: email.value,
      password: password.value,
      firstName: firstName.value,
      lastName: lastName.value,
      rola: '33333333-cccc-cccc-cccc-333333333333'
    })
  } else if (mod.value === 'uredi') {
    await axios.put(`/api/auth/uredi/${odabraniZaposlenik.value.id}`, {
      username: username.value,
      email: email.value,
      firstName: firstName.value,
      lastName: lastName.value
    })
  }

  await dohvatiZaposlenike()
  showZaposlenikModal.value = false
}

// Brisanje zaposlenika
async function obrisiZaposlenika() {
  if (!odabraniZaposlenik.value) return

  if (odabraniZaposlenik.value.role === 'Admin') {
    brisanjeMsg.value = 'Nije moguće obrisati admina.'
    return
  }

  await axios.delete(`/api/auth/obrisi/${odabraniZaposlenik.value.id}`)
  odabraniZaposlenik.value = null
  await dohvatiZaposlenike()
  showBrisanjeModal.value = false
}

onMounted(async () => {
  await dohvatiZaposlenike()
  await dohvatiSveKorisnike()
})

</script>

<template>
  <div>
    <div class="d-flex justify-content-between mb-3">
      <h3>Zaposlenici</h3>
      <!--Priprema modal-->
      <Button icon="pi pi-plus" severity="primary" @click="pripremiDodavanje" />
    </div>

      <table class="table table-striped table-dark table-rounded table-bordered">
      <thead>
        <tr>
          <th>Rbr.</th>
          <th>Korisničko ime</th>
          <th>Email</th>
          <th>Ime</th>
          <th>Prezime</th>
          <th>Rola</th>
          <th>Uredi</th>
          <th>Obriši</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(z, index) in zaposlenici.filter(z => z.role === 'Zaposlenik' || z.role === 'Admin')" :key="z.id">
          <td>{{ index + 1 }}.</td>
          <td>{{ z.userName }}</td>
          <td>{{ z.email }}</td>
          <td>{{ z.firstName }}</td>
          <td>{{ z.lastName }}</td>
          <td>{{ z.role }}</td>

          
          <td>
            <Button icon="pi pi-pencil" severity="success" size="small" @click="() => pripremiUredi(z)" />
          </td>
          <td>
            <Button icon="pi pi-trash" severity="danger" size="small" @click="() => { odabraniZaposlenik = z;  brisanjeMsg = ''; showBrisanjeModal = true }" />
          </td>
        </tr>
      </tbody>
    </table>
    <Dialog v-model:visible="showZaposlenikModal" modal :header="mod === 'dodaj' ? 'Dodaj zaposlenika' : 'Uredi zaposlenika'" :closable="false">
      <div class="p-fluid">

        <InputText v-model="username" placeholder="Korisničko ime" class="mb-2" />
        <InputText v-model="email" placeholder="Email" class="mb-2" />
        <Password v-if="mod === 'dodaj'" v-model="password" placeholder="Lozinka" toggleMask class="mb-2" />
        <InputText v-model="firstName" placeholder="Ime" class="mb-2" />
        <InputText v-model="lastName" placeholder="Prezime" class="mb-2" />
        <small v-if="msg" class="p-error">{{ msg }}</small>
      </div>
      <template #footer>
        <Button label="Zatvori" severity="warn" @click="showZaposlenikModal = false" />
        <Button label="Spremi" severity="primary" @click="spremiZaposlenika" />
      </template>
    </Dialog>



    <Dialog v-model:visible="showBrisanjeModal" modal header="Brisanje zaposlenika" :closable="false">
      <p>Želite li obrisati zaposlenika <strong>{{ odabraniZaposlenik?.firstName }} {{ odabraniZaposlenik?.lastName }}</strong>?</p>
      <small v-if="brisanjeMsg" class="p-error">{{ brisanjeMsg }}</small>
      <template #footer>
        <Button label="Odustani" severity="warn" @click="showBrisanjeModal = false" />
        <Button label="Obriši" severity="danger" @click="obrisiZaposlenika" />
      </template>
    </Dialog>
  </div>
</template>

<style>
.p-dialog .p-inputtext,
.p-dialog .p-password {
  width: 100%;
  display: block;
}
.table-rounded {
  border-collapse: separate !important;
  border-spacing: 0;
  border-radius: 0.5rem;
  overflow: hidden;
}

</style>