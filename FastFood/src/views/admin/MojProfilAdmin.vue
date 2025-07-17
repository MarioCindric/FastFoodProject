<script setup>
import { useAuthStore } from '../../stores/auth'
const auth = useAuthStore()
</script>

<template>
  <div class="container mt-4">
    <table class="table table-striped">
      <thead class="table-dark">
        <tr>
          <th>Ime</th>
          <th>Prezime</th>
          <th>Email</th>
        </tr>
      </thead>
      <tbody>
        <tr>
          <td>{{ auth.user.firstName }}</td>
          <td>{{ auth.user.lastName }}</td>
          <td>{{ auth.user.email }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>
