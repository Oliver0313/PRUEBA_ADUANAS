<template>
  <div class="container mt-4">
    <h2 class="mb-4">Productos</h2>

    <form @submit.prevent="guardarProducto" class="mb-4">
      <div class="mb-3">
        <label class="form-label">Nombre</label>
        <input
          v-model="productoForm.nombre"
          type="text"
          class="form-control"
          placeholder="Nombre"
          required
        />
      </div>

      <div class="mb-3">
        <label class="form-label">Descripción</label>
        <input
          v-model="productoForm.descripcion"
          type="text"
          class="form-control"
          placeholder="Descripción"
          required
        />
      </div>

      <div class="mb-3">
        <label class="form-label">Precio</label>
        <input
          v-model="productoForm.precio"
          type="number"
          class="form-control"
          placeholder="Precio"
          required
        />
      </div>

      <div class="mb-3">
        <label class="form-label">Stock</label>
        <input
          v-model="productoForm.stock"
          type="number"
          class="form-control"
          placeholder="Stock"
          required
        />
      </div>

      <button type="submit" class="btn btn-primary">
        {{ editando ? 'Actualizar' : 'Agregar' }}
      </button>
    </form>

    <table class="table table-bordered">
      <thead>
        <tr>
          <th>ID</th>
          <th>Nombre</th>
          <th>Descripción</th>
          <th>Precio</th>
          <th>Stock</th>
          <th>Acciones</th>
        </tr>
      </thead>

      <tbody>
        <tr v-for="producto in productos" :key="producto.id">
          <td>{{ producto.id }}</td>
          <td>{{ producto.nombre }}</td>
          <td>{{ producto.descripcion }}</td>
          <td>{{ producto.precio }}</td>
          <td>{{ producto.stock }}</td>
          <td>
            <button
              class="btn btn-warning btn-sm me-2"
              @click="editarProducto(producto)"
            >
              Editar
            </button>

            <button
              class="btn btn-danger btn-sm"
              @click="eliminarProducto(producto.id)"
            >
              Eliminar
            </button>
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'

const API_URL = 'https://localhost:7226/api'

const productos = ref([])
const editando = ref(false)

const productoForm = ref({
  id: 0,
  nombre: '',
  descripcion: '',
  precio: 0,
  stock: 0
})

function getToken() {
  return localStorage.getItem('token')
}

async function cargarProductos() {
  const response = await fetch(`${API_URL}/productos`, {
    headers: {
      Authorization: 'Bearer ' + getToken()
    }
  })

  if (!response.ok) {
    alert('Error cargando productos')
    return
  }

  productos.value = await response.json()
}

async function guardarProducto() {
  if (editando.value) {
    await actualizarProducto()
  } else {
    await crearProducto()
  }
}

async function crearProducto() {
  const response = await fetch(`${API_URL}/productos`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + getToken()
    },
    body: JSON.stringify(productoForm.value)
  })

  if (response.ok) {
    limpiarFormulario()
    cargarProductos()
  } else {
    alert('Error creando producto')
  }
}

async function actualizarProducto() {
  const response = await fetch(
    `${API_URL}/productos/${productoForm.value.id}`,
    {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: 'Bearer ' + getToken()
      },
      body: JSON.stringify(productoForm.value)
    }
  )

  if (response.ok) {
    limpiarFormulario()
    editando.value = false
    cargarProductos()
  } else {
    alert('Error actualizando producto')
  }
}

function editarProducto(producto) {
  productoForm.value = { ...producto }
  editando.value = true
}

async function eliminarProducto(id) {
  if (!confirm('¿Eliminar producto?')) return

  const response = await fetch(`${API_URL}/productos/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: 'Bearer ' + getToken()
    }
  })

  if (response.ok) {
    cargarProductos()
  } else {
    alert('Error eliminando producto')
  }
}

function limpiarFormulario() {
  productoForm.value = {
    id: 0,
    nombre: '',
    descripcion: '',
    precio: 0,
    stock: 0
  }
}

onMounted(() => {
  cargarProductos()
})
</script>