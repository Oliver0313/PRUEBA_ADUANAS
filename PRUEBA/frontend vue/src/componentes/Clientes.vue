<script setup>
import { ref, onMounted } from 'vue'

const API_URL = 'https://localhost:7226/api'

const clientes = ref([])
const editando = ref(false)

const clienteForm = ref({
  id: 0,
  nombre: '',
  correo: '',
  telefono: '',
  password: ''
})

function getToken() {
  return localStorage.getItem('token')
}

async function cargarClientes() {
  const response = await fetch(`${API_URL}/clientes`, {
    headers: {
      Authorization: 'Bearer ' + getToken()
    }
  })

  if (!response.ok) {
    alert('Error cargando clientes')
    return
  }

  clientes.value = await response.json()
}

async function guardarCliente() {
  if (editando.value) {
    await actualizarCliente()
  } else {
    await crearCliente()
  }
}

async function crearCliente() {
  const response = await fetch(`${API_URL}/clientes`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + getToken()
    },
    body: JSON.stringify({
      nombre: clienteForm.value.nombre,
      correo: clienteForm.value.correo,
      telefono: clienteForm.value.telefono,
      password: clienteForm.value.password
    })
  })

  if (!response.ok) {
    alert('Error creando cliente')
    return
  }

  limpiarFormulario()
  await cargarClientes()
}

function editarCliente(cliente) {
  editando.value = true

  clienteForm.value = {
    id: cliente.id,
    nombre: cliente.nombre,
    correo: cliente.correo,
    telefono: cliente.telefono,
    password: cliente.password
  }
}

async function actualizarCliente() {
  const response = await fetch(`${API_URL}/clientes/${clienteForm.value.id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + getToken()
    },
    body: JSON.stringify({
      id: clienteForm.value.id,
      nombre: clienteForm.value.nombre,
      correo: clienteForm.value.correo,
      telefono: clienteForm.value.telefono,
      password: clienteForm.value.password
    })
  })

  if (!response.ok) {
    alert('Error actualizando cliente')
    return
  }

  limpiarFormulario()
  await cargarClientes()
}

async function eliminarCliente(id) {
  if (!confirm('¿Seguro que quieres eliminar este cliente?')) return

  const response = await fetch(`${API_URL}/clientes/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: 'Bearer ' + getToken()
    }
  })

  if (!response.ok) {
    alert('Error eliminando cliente')
    return
  }

  await cargarClientes()
}

function limpiarFormulario() {
  editando.value = false

  clienteForm.value = {
    id: 0,
    nombre: '',
    correo: '',
    telefono: '',
    password: ''
  }
}

onMounted(() => {
  cargarClientes()
})
</script>

<template>
  <div class="section-card">
    <div class="section-header">
      <h2>Clientes</h2>
      <p>Administración de clientes registrados</p>
    </div>

    <div class="form-card">
      <h2>{{ editando ? 'Editar Cliente' : 'Nuevo Cliente' }}</h2>

      <input
        v-model="clienteForm.nombre"
        placeholder="Nombre"
      >

      <input
        v-model="clienteForm.correo"
        placeholder="Correo"
      >

      <input
        v-model="clienteForm.telefono"
        placeholder="Teléfono"
      >

      <input
        v-model="clienteForm.password"
        type="password"
        placeholder="Password"
      >

      <div class="buttons">
        <button @click="guardarCliente">
          {{ editando ? 'Actualizar' : 'Guardar' }}
        </button>

        <button
          v-if="editando"
          class="cancel-btn"
          @click="limpiarFormulario"
        >
          Cancelar
        </button>
      </div>
    </div>

    <div class="clientes-grid">
      <div
        class="cliente-card"
        v-for="cliente in clientes"
        :key="cliente.id"
      >
        <h3>{{ cliente.nombre }}</h3>

        <p>
          <strong>Correo:</strong>
          {{ cliente.correo }}
        </p>

        <p>
          <strong>Teléfono:</strong>
          {{ cliente.telefono }}
        </p>

        <div class="buttons">
          <button @click="editarCliente(cliente)">
            Editar
          </button>

          <button
            class="delete-btn"
            @click="eliminarCliente(cliente.id)"
          >
            Eliminar
          </button>
        </div>
      </div>
    </div>
  </div>
</template>