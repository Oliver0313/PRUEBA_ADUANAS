<script setup>
import { ref, onMounted } from 'vue'

const API_URL = 'https://localhost:7226/api'

const productos = ref([])

function getToken() {
  return localStorage.getItem('token')
}

function getClienteNombre() {
  return localStorage.getItem('clienteNombre')
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

async function comprarProducto(producto) {
  if (producto.stock <= 0) {
    alert('Producto sin stock')
    return
  }

  const venta = {
    fecha: new Date().toISOString(),
    cliente: getClienteNombre(),
    listaProductos: producto.nombre,
    total: Number(producto.precio)
  }

  const responseVenta = await fetch(`${API_URL}/ventas`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + getToken()
    },
    body: JSON.stringify(venta)
  })

  if (!responseVenta.ok) {
    alert('Error realizando compra')
    return
  }

  const productoActualizado = {
    id: producto.id,
    nombre: producto.nombre,
    descripcion: producto.descripcion,
    precio: Number(producto.precio),
    stock: producto.stock - 1
  }

  const responseProducto = await fetch(`${API_URL}/productos/${producto.id}`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + getToken()
    },
    body: JSON.stringify(productoActualizado)
  })

  if (!responseProducto.ok) {
    alert('Compra realizada, pero no se pudo actualizar el stock')
    return
  }

  alert(`Compra realizada: ${producto.nombre}`)

  await cargarProductos()
}

onMounted(() => {
  cargarProductos()
})
</script>

<template>
  <div class="section-card">
    <div class="section-header">
      <h2>Tienda</h2>
      <p>Productos agregados por el administrador</p>
    </div>

    <div class="clientes-grid">
      <div
        class="cliente-card"
        v-for="producto in productos"
        :key="producto.id"
      >
        <h3>{{ producto.nombre }}</h3>

        <p><strong>Descripción:</strong> {{ producto.descripcion }}</p>

        <p><strong>Precio:</strong> RD$ {{ producto.precio }}</p>

        <p><strong>Stock:</strong> {{ producto.stock }}</p>

        <button
          :disabled="producto.stock <= 0"
          @click="comprarProducto(producto)"
        >
          {{ producto.stock <= 0 ? 'Sin stock' : 'Comprar' }}
        </button>
      </div>
    </div>
  </div>
</template>