<script setup>
import {ref, onMounted, computed} from 'vue'
import axios from 'axios'

const korisnici = ref([])
const korisnikFilter = ref('')


async function dohvatiSveKorisnike()
{
    const res = await axios.get('/api/auth/samoKorisnici')
    korisnici.value = res.data
    
}

onMounted(async () => {
    await dohvatiSveKorisnike()
})

const filtriraniKorisnici = computed (() =>{
    const ime = korisnikFilter.value.trim().toLowerCase()
    let rezultat = korisnici.value.filter(k => k.firstName || k.lastName || k.email)
    if (!ime) return rezultat

    return rezultat.filter(i => i.firstName.toLowerCase().includes(ime) ||i.lastName.toLowerCase().includes(ime)
|| i.email.toLowerCase().includes(ime))

})



</script>

<template>
<div>
     <div class="d-flex justify-content-between mb-3">
        <h3>Korisnici</h3>
    </div>
</div>

 <div class="input-group my-4" style="max-width: 300px;">
  <input
    v-model="korisnikFilter"
    type="text"
    class="form-control"
    placeholder="Korisnik"
  />
</div>

  <table class="table table-striped table-dark table-rounded table-bordered">
    <thead>
        <tr>
            <th>Rbr.</th>
            <th>Email</th>
            <th>Ime</th>
            <th>Prezime</th>
        </tr>
    </thead>
    <tbody>
        <tr v-for="(z, index) in filtriraniKorisnici" :key="z.id">
            <td>{{ index + 1 }}.</td>
            <td>{{ z.email }}</td>
            <td>{{ z.firstName }}</td>
            <td>{{ z.lastName }}</td>
        </tr>
    </tbody>
</table>
</template>