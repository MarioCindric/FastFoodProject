<script setup>
import Top5 from "../../components/admin/Top5.vue"
import Narudzbe from "../../components/admin/Narudzbe.vue"
import Mapa from "../../components/admin/Mapa.vue"
import NarudzbaMjesecGodina from "../../components/admin/NarudzbaMjesecGodina.vue";
import Promet from "../../components/admin/Promet.vue"
</script>


<template>
  <div class="statistika-layout">
    <div class="graf"><Narudzbe /></div>
    <div class="graf"><NarudzbaMjesecGodina /></div>
    <div class="graf"><Top5 /></div>
    <div class="graf"><Promet /></div>
    <div class="heatmap"><Mapa /></div>
  </div>
</template>

 <style scoped>
 body
 {
  background-color:white !important;
 }
.statistika-layout {
  display: flex;
  flex-wrap: wrap;
  justify-content: center;
  gap: 32px;
  padding: 32px;
   background-color: #121212;
  color: #f1f1f1;
}

.graf {
  flex: 1 1 45%;
  max-width: 750px;
  min-width: 350px;
  padding: 1rem;
  background-color: #1e1e1e;
}

.heatmap{
  flex: 1 1 100%;
  max-width: 100%;
  background-color: #1e1e1e;
  padding: 1rem;
  border-radius: 8px;
}

label,
.form-label,
small {
  color: #f1f1f1;
}
</style>
