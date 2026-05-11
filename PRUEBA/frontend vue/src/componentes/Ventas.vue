<script setup>
import { ref, onMounted } from 'vue'

const API_URL = 'https://localhost:7226/api'

const ventas = ref([])
const editando = ref(false)

const ventaForm = ref({
  id: 0,
  fecha: '',
  cliente: '',
  listaProductos: '',
  total: 0
})

function getToken() {
  return localStorage.getItem('token')
}

async function cargarVentas() {
  const response = await fetch(`${API_URL}/ventas`, {
    headers: {
      Authorization: 'Bearer ' + getToken()
    }
  })

  if (!response.ok) {
    alert('Error cargando historial')
    return
  }

  const data = await response.json()

  ventas.value = data.sort(
    (a, b) => new Date(b.fecha) - new Date(a.fecha)
  )
}

async function guardarVenta() {
  if (editando.value) {
    await actualizarVenta()
  } else {
    await crearVenta()
  }
}

async function crearVenta() {
  const response = await fetch(`${API_URL}/ventas`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + getToken()
    },
    body: JSON.stringify({
      fecha: ventaForm.value.fecha,
      cliente: ventaForm.value.cliente,
      listaProductos: ventaForm.value.listaProductos,
      total: Number(ventaForm.value.total)
    })
  })

  if (!response.ok) {
    alert('Error creando venta')
    return
  }

  limpiarFormulario()
  await cargarVentas()
}

function editarVenta(venta) {
  editando.value = true

  ventaForm.value = {
    id: venta.id,
    fecha: venta.fecha?.substring(0, 10),
    cliente: venta.cliente,
    listaProductos: venta.listaProductos,
    total: venta.total
  }
}

async function actualizarVenta() {
  const response = await fetch(`${API_URL}/ventas/${ventaForm.value.id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + getToken()
    },
    body: JSON.stringify({
      id: ventaForm.value.id,
      fecha: ventaForm.value.fecha,
      cliente: ventaForm.value.cliente,
      listaProductos: ventaForm.value.listaProductos,
      total: Number(ventaForm.value.total)
    })
  })

  if (!response.ok) {
    alert('Error actualizando venta')
    return
  }

  limpiarFormulario()
  await cargarVentas()
}

async function eliminarVenta(id) {
  if (!confirm('¿Seguro que quieres eliminar esta venta?')) return

  const response = await fetch(`${API_URL}/ventas/${id}`, {
    method: 'DELETE',
    headers: {
      Authorization: 'Bearer ' + getToken()
    }
  })

  if (!response.ok) {
    alert('Error eliminando venta')
    return
  }

  await cargarVentas()
}

function limpiarFormulario() {
  editando.value = false

  ventaForm.value = {
    id: 0,
    fecha: '',
    cliente: '',
    listaProductos: '',
    total: 0
  }
}

function formatearFecha(fecha) {
  return new Date(fecha).toLocaleDateString()
}

onMounted(() => {
  cargarVentas()
})
</script>

<template>
  <div class="section-card">
    <div class="section-header">
      <h2>Historial de Compras</h2>
    </div>

    <div class="form-card">
      <h2>{{ editando ? 'Editar Venta' : 'Registrar Venta Manual' }}</h2>

      <input
        v-model="ventaForm.fecha"
        type="date"
      >

      <input
        v-model="ventaForm.cliente"
        placeholder="Cliente"
      >

      <input
        v-model="ventaForm.listaProductos"
        placeholder="Lista de productos"
      >

      <input
        v-model="ventaForm.total"
        type="number"
        placeholder="Total"
      >

      <div class="buttons">
        <button @click="guardarVenta">
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

    <div class="historial-list">
      <div
        class="historial-item"
        v-for="venta in ventas"
        :key="venta.id"
      >
        <div>
          <h3>Compra #{{ venta.id }}</h3>
          <p><strong>Fecha:</strong> {{ formatearFecha(venta.fecha) }}</p>
          <p><strong>Cliente:</strong> {{ venta.cliente }}</p>
          <p><strong>Productos:</strong> {{ venta.listaProductos }}</p>
        </div>

        <div class="historial-actions">
          <strong class="total">RD$ {{ venta.total }}</strong>

          <div class="buttons">
            <button @click="editarVenta(venta)">
              Editar
            </button>

            <button
              class="delete-btn"
              @click="eliminarVenta(venta.id)"
            >
              Eliminar
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>