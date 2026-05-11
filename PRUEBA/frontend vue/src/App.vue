<script setup>
import { ref, onMounted } from 'vue'

import './styles/app.css'

import Clientes from './componentes/Clientes.vue'
import Productos from './componentes/Productos.vue'
import Ventas from './componentes/Ventas.vue'
import RegistroClientes from './componentes/RegistroClientes.vue'
import ProductosClientes from './componentes/ProductosClientes.vue'

const API_URL = 'https://localhost:7226/api'

const username = ref('')
const password = ref('')

const logueado = ref(false)
const role = ref('')
const modo = ref('login')
const seccionActual = ref('productos')

function cambiarSeccion(seccion) {
  seccionActual.value = seccion
  window.location.hash = seccion
}

async function loginAdmin() {

  const response = await fetch(`${API_URL}/auth/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      username: username.value.trim(),
      password: password.value.trim()
    })
  })

  if (!response.ok) {
    alert('Credenciales de admin incorrectas')
    return
  }

  const data = await response.json()

  localStorage.setItem('token', data.token)
  localStorage.setItem('role', 'Admin')

  role.value = 'Admin'
  logueado.value = true

  cambiarSeccion('clientes')
}

async function loginCliente() {

  const response = await fetch(`${API_URL}/clientes/login`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify({
      correo: username.value.trim(),
      password: password.value.trim()
    })
  })

  if (!response.ok) {
    alert('Credenciales de cliente incorrectas')
    return
  }

  const data = await response.json()

  localStorage.setItem('token', data.token)
  localStorage.setItem('role', 'Cliente')
  localStorage.setItem('clienteNombre', data.nombre)
  localStorage.setItem('clienteCorreo', data.correo)

  role.value = 'Cliente'
  logueado.value = true

  cambiarSeccion('productos')
}

function logout() {

  localStorage.removeItem('token')
  localStorage.removeItem('role')
  localStorage.removeItem('clienteNombre')
  localStorage.removeItem('clienteCorreo')

  username.value = ''
  password.value = ''

  logueado.value = false
  role.value = ''
  modo.value = 'login'
  seccionActual.value = 'productos'

  window.location.hash = ''
}

onMounted(() => {

  const token = localStorage.getItem('token')
  const savedRole = localStorage.getItem('role')

  if (token && savedRole) {

    logueado.value = true
    role.value = savedRole

    const hash = window.location.hash.replace('#', '')

    if (hash) {
      seccionActual.value = hash
    }
    else {
      seccionActual.value =
        savedRole === 'Admin'
          ? 'clientes'
          : 'productos'
    }
  }

})
</script>

<template>

  <div class="container">

    <div v-if="!logueado">

      <div
        v-if="modo === 'login'"
        class="login-card"
      >

        <h1>Acceder</h1>

        <input
          v-model="username"
          placeholder="Correo o Username"
        >

        <input
          v-model="password"
          type="password"
          placeholder="Password"
        >

        <button @click="loginCliente">
          Entrar como Cliente
        </button>

        <button
          class="secondary-btn"
          @click="loginAdmin"
        >
          Entrar como Admin
        </button>

        <p class="switch-text">
          ¿No tienes cuenta?
        </p>

        <button
          class="secondary-btn"
          @click="modo = 'registro'"
        >
          Registrarme
        </button>

      </div>

      <div v-if="modo === 'registro'">

        <RegistroClientes />

        <br><br>

        <button
          class="secondary-btn center-btn"
          @click="modo = 'login'"
        >
          Volver
        </button>

      </div>

    </div>

    <div
      v-else
      class="page-card"
    >

      <div class="top-bar">

        <div>

          <h1>
            {{
              role === 'Admin'
                ? 'Admin'
                : 'Tienda'
            }}
          </h1>

          <p class="subtitle">
            {{
              role === 'Admin'
                ? 'Panel de administración'
                : 'Bienvenido a tu tienda'
            }}
          </p>

        </div>

        <button
          class="logout-btn"
          @click="logout"
        >
          Logout
        </button>

      </div>

      <div
        v-if="role === 'Admin'"
        class="menu"
      >

        <button
          :class="{ active: seccionActual === 'clientes' }"
          @click="cambiarSeccion('clientes')"
        >
          Clientes
        </button>

        <button
          :class="{ active: seccionActual === 'productos' }"
          @click="cambiarSeccion('productos')"
        >
          Productos
        </button>

        <button
          :class="{ active: seccionActual === 'ventas' }"
          @click="cambiarSeccion('ventas')"
        >
          Historial de Compras
        </button>

      </div>

      <Clientes
        v-if="
          role === 'Admin' &&
          seccionActual === 'clientes'
        "
      />

      <Productos
        v-if="
          role === 'Admin' &&
          seccionActual === 'productos'
        "
      />

      <Ventas
        v-if="
          role === 'Admin' &&
          seccionActual === 'ventas'
        "
      />

      <ProductosClientes
        v-if="
          role === 'Cliente' &&
          seccionActual === 'productos'
        "
      />

    </div>

  </div>

</template>