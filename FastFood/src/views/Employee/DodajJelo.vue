<script setup>
import { ref, onMounted } from 'vue'
import axios from 'axios'
import { useRouter } from 'vue-router'
import { useToast } from 'primevue/usetoast'



const router = useRouter()


const jelo = ref({
  naziv: '',
  opis: '',
  cijena: 0,
  dostupno: true,
  slika: null,
  popust: 0,
  kategorijaId: null
})

const kategorije = ref([])
const previewUrl = ref(null)
const slikaFile = ref(null)
const toast = useToast()


onMounted(async () => {
  // dohvati kategorije
  const res = await axios.get('/KategorijaJelo/kategorije/sve')

  kategorije.value = res.data
})

function handleFile(e) {
  slikaFile.value = e.target.files[0]
  previewUrl.value = URL.createObjectURL(slikaFile.value)
}




async function uploadSlikaNaCloudinary() {
  const formData = new FormData()
  formData.append('file', slikaFile.value)
  formData.append('upload_preset', 'FastFood')

  const response = await fetch('https://api.cloudinary.com/v1_1/dfmvfks2w/image/upload', {
    method: 'POST',
    body: formData
  })

  if (!response.ok) {
    throw new Error('Greška pri slanju slike na Cloudinary')
  }

  const data = await response.json()
  return data.secure_url
}



// Validacija unosa
function validacija() {
  if (!jelo.value.naziv.trim() || !jelo.value.opis.trim() || jelo.value.cijena === null || typeof jelo.value.cijena !== 'number' || isNaN(jelo.value.cijena) ||
   jelo.value.cijena <= 0 || jelo.value.kategorijaId === null || !slikaFile.value
  ) {
    toast.add({severity: 'secondary', summary: 'Greška', detail: 'Sva polja su obavezna.', life: 3000 })
    return false
  }
  return true
}


// Dodavanje jela
async function submitForm() {
  if (!validacija()) return
  const slikaUrl = await uploadSlikaNaCloudinary()
  
  const podaci = {
    naziv: jelo.value.naziv,
    opis: jelo.value.opis,
    cijena: jelo.value.cijena,
    dostupno: jelo.value.dostupno,
    slikaUrl: slikaUrl,
    popust: jelo.value.popust,
    kategorijaId: jelo.value.kategorijaId
  }

  await axios.post('jelo/add', podaci)
  router.push('/AdministracijaJela') 
}
</script>

<template>
  <div class="container mt-4" style="max-width: 70dvb; margin: 0 auto;">
    <Toast position="bottom-center" />
    <h2 class="mb-4 fw-bold">Dodaj novo jelo</h2>
    <form @submit.prevent="submitForm" class="mt-3">
      <div class="mb-3">
        <label class="form-label">Naziv</label>
        <input v-model="jelo.naziv" type="text" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">Opis</label>
        <input v-model="jelo.opis" type="text" class="form-control" maxlength="80" />
      </div>

      <div class="mb-3">
        <label class="form-label">Cijena (€)</label>
        <input v-model="jelo.cijena" type="number" step="0.01" class="form-control" />
      </div>

      <div class="mb-3">
        <label class="form-label">Dostupno</label>
        <input v-model="jelo.dostupno" type="checkbox" class="form-check-input ms-2" />
      </div>

      <div class="mb-3">
        <label class="form-label">Kategorija</label>
        <select v-model="jelo.kategorijaId" class="form-control">
          <option v-for="(k, index) in kategorije" :key="k.id" :value="k.id">{{ index + 1 }}. {{ k.naziv }}</option>
        </select>
      </div>

      <div class="mb-3">
        <label class="form-label">Slika</label>
        <input @change="handleFile" type="file" class="form-control" accept="image/*" />
      </div>

      <div class="mb-3" v-if="previewUrl">
        <label class="form-label">Pregled slike</label>
        <img :src="previewUrl" class="img-thumbnail d-block mx-auto" style="max-height: 200px;" />
      </div>

      <button type="submit" class="btn btn-info">Spremi</button>
    </form>
  </div>
</template>


<style scoped>
.container {
  background-color: #1e1e1e;
  color: #f1f1f1;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 16px rgba(0, 0, 0, 0.3);
  max-width: 800px;
  margin: 2rem auto;
}

.form-control,
.form-check-input,
select {
  background-color: #2a2a2a;
  color: #f1f1f1;
  border: 1px solid #444;
  border-radius: 6px;
}

.form-label {
  font-weight: 500;
  margin-bottom: 0.4rem;
}

.img-thumbnail {
  background-color: #2a2a2a;
  border: 1px solid white;
}
</style>
