<script setup>
import Logout from '../Logout.vue'
import Kraj from '../../components/Kraj.vue'
import { useRoute } from 'vue-router'
import slika1 from '../../assets/banne4.png'
import slika2 from '../../assets/banner2.jpg'
import slika3 from '../../assets/banner7.jpg'
import slika4 from '../../assets/banner8.jpg'
import slika5 from '../../assets/banner9.jpg'
import slika6 from '../../assets/banner10.avif'
import slika7 from '../../assets/banner11.jpg'



const route = useRoute()

const slike = [
  { url: slika1 },
  { url: slika2 },
  { url: slika3 },
  { url: slika4 },
  { url: slika5 },
  { url: slika6 },
  { url: slika7 }
]

</script>

<template>
  <div id="layout">
    <nav class="navbar navbar-expand-lg px-4">
      <ul class="navbar-nav me-auto">
        <li class="nav-item">
          <router-link to="/Pocetna" class="nav-link" exact-active-class="active-nav"><i class="pi pi-star"></i> Početna</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/KorisnickiProfil" class="nav-link" exact-active-class="active-nav"><i class="pi pi-user"></i> Moj profil</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/AktivneNarudzbe" class="nav-link" exact-active-class="active-nav"><i class="pi pi-spinner-dotted"></i> Aktivne narudžbe</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/Povijest" class="nav-link" exact-active-class="active-nav"><i class="pi pi-check"></i> Završene narudžbe</router-link>
        </li>
        <Logout />
      </ul>
    </nav>


    <div v-if="route.path === '/Pocetna'" class="banner-wrapper">
        <Carousel :value="slike" :numVisible="1" :numScroll="1" :circular="true" :autoplayInterval="15000" :showIndicators="false" :showNavigators="false">
          <template #item="slotProps">
            <img :src="slotProps.data.url" class="banner-img" />
          </template>
        </Carousel>
    </div>

    <main class="container mt-4 flex-fill">
      <router-view />
    </main>

    <Kraj v-if="route.path === '/Pocetna'" />
  </div>
</template>


<style>

.banner-wrapper {
  width: 100%;
  overflow: hidden;
}

.banner-img {
  width: 100%;
  height: 500px;
  object-fit: cover;
  display: block;
}

.active-nav {
  color: #00c4cc !important;
}

html, body {
  height: 100%;
  margin: 0;
}

#layout {
  display: flex;
  flex-direction: column;
  min-height: 100vh;
}

main {
  flex: 1;
}
</style>
