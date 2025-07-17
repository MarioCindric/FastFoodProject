<script setup>
import { useAuthStore } from '../stores/auth.js'
import { useRouter } from 'vue-router'

const auth   = useAuthStore()
const router = useRouter()

function Odjava() {
  auth.logout()      
  router.push('/login')
}
</script>

<template>
  <button type = "button" @click = "Odjava" class="btn btn-info">Odjava</button>
</template>

<style scoped>
button
{
  margin-left:20px;
  background-color: #00c4cc;
}
</style>
