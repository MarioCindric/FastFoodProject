<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../stores/auth'
import axios from 'axios'
import { onMounted } from 'vue'


const korisnici = ref([])
const username = ref('')
const email= ref('')
const password = ref('')
const firstName= ref('')
const lastName= ref('')
const msg= ref('')
const showPassword = ref(false)


const auth   = useAuthStore()
const router = useRouter()


// Funkcija za dohvacanje svih korisnika, zaposlenika i admina
async function dohvatiKorisnike() {
  const res = await axios.get('/api/auth/svi-korisnici')
  korisnici.value = res.data
}


// Provjera postoji li korisnik
function postojiKorisnik() {
  const uname = username.value.toLowerCase()
  const mail = email.value.toLowerCase()

    // Prolazi kroz sve korisnike, provjera uvjeta, ako je true postoji, ako je false ne postoji
  const postojiEmail = korisnici.value.some(x => x.email.toLowerCase() === mail)
  const postojiUsername = korisnici.value.some(x => x.userName.toLowerCase() === uname)

  if (postojiEmail) {
    msg.value = 'Email adresa je već zauzeta'
    return true
  }

  if (postojiUsername) {
    msg.value = 'Korisničko ime je već zauzeto'
    return true
  }

  return false
}

onMounted(dohvatiKorisnike)


async function submit() {
  if (!firstName.value.trim() || !lastName.value.trim() || !username.value.trim() || !email.value.trim() || !password.value.trim()) {
  msg.value = 'Sva polja su obavezna';
  return;
  }
  const passwordRegex = /^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{6,}$/;
  if (!passwordRegex.test(password.value)) {
    msg.value = 'Lozinka mora sadržavati veliko slovo, broj i poseban znak';
    return;
  }

  if (postojiKorisnik()) {
  return;
  }

  try {
    await auth.register(username.value, email.value, password.value, firstName.value, lastName.value)
    msg.value = 'Registracija uspješna. Preusmjeravanje na login...'
    await new Promise(resolve => setTimeout(resolve, 3000)) // Korisnik vidi poruku 3 sekunde
    router.push('/login')
  } catch {
    msg.value = 'Greška'
  }
}
</script>

<template>
  <section class="vh-100 gradient-custom">
    <div class="container py-5 h-100">
      <div class="row d-flex justify-content-center align-items-center h-100">
        <div class="col-12 col-md-8 col-lg-6 col-xl-5">
          <div class="card bg-dark text-white" style="border-radius:1rem;">
            <div class="card-body p-5 text-center">
              <h2 class="fw-bold mb-4 text-uppercase">Registracija</h2>

              <form @submit.prevent="submit">
                <div class="form-outline form-white mb-3">
                  <input v-model="firstName" class="form-control form-control-lg" />
                  <label class="form-label">Ime</label>
                </div>

                <div class="form-outline form-white mb-3">
                  <input v-model="lastName" class="form-control form-control-lg" />
                  <label class="form-label">Prezime</label>
                </div>

                <div class="form-outline form-white mb-3">
                  <input v-model="username" class="form-control form-control-lg" />
                  <label class="form-label">Username</label>
                </div>

                <div class="form-outline form-white mb-3">
                  <input v-model="email" type="email" class="form-control form-control-lg" />
                  <label class="form-label">Email</label>
                </div>

                <div class="form-outline form-white mb-4">
                  <div class="input-group">
                    <input v-model="password" :type="showPassword ? 'text' : 'password'" class="form-control form-control-lg" />
                    <div class="input-group-text bg-secondary border-0">
                      <input class="form-check-input mt-0" type="checkbox" v-model="showPassword" />
                    </div>
                  </div>
                  <label class="form-label">Lozinka</label>
                </div>


                <button class="btn btn-outline-light btn-lg px-5" type="submit">Registriraj</button>
                <p v-if="msg" class="mt-3">{{ msg }}</p>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>
