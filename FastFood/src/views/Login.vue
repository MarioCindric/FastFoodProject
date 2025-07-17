<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import { useAuthStore } from '../stores/auth';


const username = ref('');
const password = ref('');
const error = ref('');
const router = useRouter();
const auth = useAuthStore();
const showPassword = ref(false)

// login, ako uspije idem na pocetnu
async function submit() {
  try {
    await auth.login(username.value, password.value);
    //
    //console.log(auth.roles) 
    
    if(auth.roles.includes("Zaposlenik"))
    {
      router.push("/AdministracijaJela")
    }
    else if(auth.roles.includes("Admin"))
    {
      router.push("/AdministracijaJela")
    }
    else
    {
       router.push('/Pocetna');
    }
   
  } catch {
    error.value = 'Neispravan username ili lozinka';
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
              <h2 class="fw-bold mb-4 text-uppercase">Login</h2>

              <form @submit.prevent="submit">
                <div class="form-outline form-white mb-4">
                  <input v-model="username" class="form-control form-control-lg" />
                  <label class="form-label">Username</label>
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

                <button class="btn btn-outline-light btn-lg px-5" type="submit">Login</button>
                <p v-if="error" class="mt-3">{{ error }}</p>
              </form>
            </div>
          </div>
        </div>
      </div>
    </div>
  </section>
</template>


