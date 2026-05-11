<script setup>
import { ref } from 'vue'

const API_URL = 'https://localhost:7226/api'

const cliente = ref({
  nombre: '',
  correo: '',
  telefono: '',
  password: ''
})

async function registrarCliente() {
  const response = await fetch(`${API_URL}/clientes`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(cliente.value)
  })

  if (!response.ok) {
    alert('Error registrando cliente')
    return
  }

  alert('Cliente registrado correctamente')

  cliente.value = {
    nombre: '',
    correo: '',
    telefono: '',
    password: ''
  }
}
</script>

<template>
  <div class="login-card">
    <h1>Registro Cliente</h1>

    <input v-model="cliente.nombre" placeholder="Nombre">
    <input v-model="cliente.correo" placeholder="Correo">
    <input v-model="cliente.telefono" placeholder="Teléfono">
    <input v-model="cliente.password" type="password" placeholder="Password">

    <button @click="registrarCliente">
      Registrarme
    </button>
  </div>
</template>