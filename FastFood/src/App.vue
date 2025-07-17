<script setup>
import Navigacija from "./components/guest/NavigacijaGost.vue"
import NavigacijaUser from "./components/user/NavigacijaUser.vue"
import NavigacijaAdmin from "./components/admin/NavigacijaAdmin.vue"
import { useAuthStore } from './stores/auth.js'
import NavigacijaZaposlenik from "./components/employee/NavigacijaZaposlenik.vue"

const auth = useAuthStore()
</script>

<template>
  <NavigacijaUser v-if="auth.roles.includes('User')" />
  <NavigacijaAdmin v-else-if="auth.roles.includes('Admin')" />
  <NavigacijaZaposlenik v-else-if="auth.roles.includes('Zaposlenik')" />
  <Navigacija v-else/>
</template>

<style>

</style>
