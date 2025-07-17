<script setup>
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
      <ul class="navbar-nav">
        <li class="nav-item">
          <router-link to="/" class="nav-link"exact-active-class="active-nav"><i class="pi pi-star"></i> Početna</router-link>
        </li>
      </ul>
      <ul class="navbar-nav">
        <li class="nav-item">
          <router-link to="/Login" class="nav-link"exact-active-class="active-nav"><i class="pi pi-sign-in"></i> Login</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/Registracija" class="nav-link"exact-active-class="active-nav"><i class="pi pi-user-plus"></i> Registracija</router-link>
        </li>
      </ul>
    </nav>

       <div v-if="route.path === '/'" class="banner-wrapper">
        <Carousel :value="slike" :numVisible="1" :numScroll="1" :circular="true" :autoplayInterval="15000" :showIndicators="false" :showNavigators="false">
          <template #item="slotProps">
            <img :src="slotProps.data.url" class="banner-img" />
          </template>
        </Carousel>
    </div>

    <main class="container mt-4 flex-fill">
      <router-view />
    </main>

    <Kraj v-if="route.path === '/'" />
  </div>
</template>

<style>
.banner-wrapper {
  width: 100%;
  overflow: hidden;
}


.banner-img {
  width: 100%;
  height: 400px;
  object-fit: cover;
  display: block;
}

.navbar {
  background-color: #121212 !important;
  border-bottom: 1px solid #333;
}

.nav-link {
  color: #f1f1f1 !important;
  font-weight: 500;
}

.nav-link:hover {
  color: #00c4cc !important;
}

.nav-link {
  position: relative;
}

.nav-link::after {
  content: '';
  position: absolute;
  left: 0;
  bottom: -3px;
  width: 0;
  height: 2px;
  background-color: #00c4cc;
  transition: width 0.3s ease;
}

.nav-link:hover::after {
  width: 100%;
}


</style>
