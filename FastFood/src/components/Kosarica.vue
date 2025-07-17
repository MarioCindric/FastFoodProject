<script setup>
import { computed } from 'vue'
import { defineProps, defineEmits } from 'vue'

const props = defineProps({
  items: { type: Array, default: () => [] }
})

const emit = defineEmits(['decrement-item'])

const prazno = computed(() => props.items.length === 0)


// Šalje događaj s artiklom koji se treba umanjiti, kad se pozove funkcija, pokrece event decrement-item 
// i salje roditelju idem kao argument
function decrement(item) {
  emit('decrement-item', item)
}

// Radno vrijeme je do 21 ali uvjet je da nema narudzbi sat vremena prije zatvaranja
const unutarRadnogVremena = computed(() => {
  const sada = new Date()
  const sat = sada.getHours()
  const dan = sada.getDay()
  return dan !== 0 && sat >= 8 && sat < 20
})

</script>

<style scoped>
.custom-offcanvas {
  --bs-offcanvas-width: 25  %;
}
</style>


<template>
  <!-- z- index da se prikazuje nad tijelom kosarice -->
  <button
  class="btn-kosarica position-fixed"
  style="top: 7px; right: 10px; z-index: 1050;" type="button" data-bs-toggle="offcanvas" data-bs-target="#offcanvasScrolling" aria-controls="offcanvasScrolling">
  <i class="bi bi-cart-check"></i>
</button>


  <div class="offcanvas offcanvas-end custom-offcanvas" data-bs-scroll="true" data-bs-backdrop="false" tabindex="-1" id="offcanvasScrolling" aria-labelledby="offcanvasScrollingLabel">
    <div class="offcanvas-header">
      <h5 class="offcanvas-title" id="offcanvasScrollingLabel">Košarica</h5>
      
    </div>
    <div class="offcanvas-body">
      <ul class="list-unstyled mb-0">
        <li
          v-for="item in items"
          :key="item.id"
          class="d-flex justify-content-between align-items-center mb-2"
        >
          <span>
            {{ item.naziv }} – {{ (item.cijena * item.count).toFixed(2) }} €
            <small>({{ item.count }}×)</small>
          </span>
          <button
            @click="decrement(item)"
            class="btn btn-sm btn-danger"
          >−</button>
        </li>
      </ul>
      <!-- Računa koliko ima proizvoda, count * cijena, 0 je početna vrijednost sume -->
      <p class="mt-3 fw-bold">  
        Ukupno:
        {{items.reduce((sum, x) => sum + x.cijena * x.count, 0).toFixed(2)}}€
      </p>

   <router-link v-if="!prazno && unutarRadnogVremena" to="/narudzba" class="btn btn-success w-100"><i class="pi pi-shopping-cart me-2"></i> Naruči</router-link>
    <button v-else class="btn btn-success w-100 disabled" disabled><i class="pi pi-shopping-cart me-2"></i>{{
    !unutarRadnogVremena? 'Naručivanje izvan radnog vremena' : 'Naruči'}}</button>

    </div>
  </div>
</template>


<style scoped>
.custom-offcanvas {
  background-color: #121212;
  color: #f1f1f1;
  border-left: 1px solid #333;
}

.custom-offcanvas .offcanvas-header,
.custom-offcanvas .offcanvas-body {
  background-color: #121212;
  color: #f1f1f1;
}

.custom-offcanvas .offcanvas-header {
  border-bottom: 1px solid #333;
  margin-bottom: 1rem;
  padding-bottom: 0.5rem;
}

.custom-offcanvas .offcanvas-body {
  display: flex;
  flex-direction: column;
  height: 100%;
}



</style>