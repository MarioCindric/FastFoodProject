<script setup>
import Logout from '../Logout.vue'
</script>

<template>
    <div>
    <nav class="navbar navbar-expand-lg px-4">
        <ul class="navbar-nav me-auto">
             <li class="nav-item">
                <router-link to="/AdministracijaJela" class="nav-link"exact-active-class="active-nav"><i class="pi pi-pen-to-square"></i> Administracija jela</router-link>
            </li>
             <li class="nav-item">
                <router-link to="/DodajJelo" class="nav-link"exact-active-class="active-nav"><i class="pi pi-plus"></i> Dodaj jelo</router-link>
            </li>
             <li class="nav-item">
                <router-link to="/Zaposlenici" class="nav-link"exact-active-class="active-nav"><i class="pi pi-user"></i> Zaposlenici</router-link>
            </li>
            <li class="nav-item">
                <router-link to="/Korisnici" class="nav-link"exact-active-class="active-nav"> <i class="pi pi-user"></i> Korisnici </router-link>
            </li>
             <li class="nav-item">
                <router-link to="/Statistika" class="nav-link"exact-active-class="active-nav"> <i class="pi pi-chart-bar"></i> Statistika</router-link>
            </li>
            <li class="nav-item">
                <router-link to="/ZavrseneNarudzbe" class="nav-link"exact-active-class="active-nav"> <i class="pi pi-check"></i> Završene narudžbe</router-link>
            </li>
           
            <Logout />
        </ul>
    </nav>
    
        <div class="container mt-4">
            <router-view />
        </div>
    </div>
</template>
