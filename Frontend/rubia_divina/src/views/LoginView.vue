<template>
  <div class="auth-page">
    <div class="auth-card">
      <span class="badge">CRUD + Login</span>
      <h1>Rubia Divina</h1>
      <p class="subtitle">Iniciar sesión para acceder al panel protegido.</p>

      <form @submit.prevent="handleLogin">
        <label>
          Correo electrónico
          <input v-model.trim="form.email" type="email" placeholder="admin@rubiadivina.com" required />
        </label>

        <label>
          Contraseña
          <input v-model.trim="form.password" type="password" placeholder="••••••••" required />
        </label>

        <button class="btn btn-primary full" type="submit" :disabled="loading">
          {{ loading ? 'Ingresando...' : 'Ingresar' }}
        </button>
      </form>

      <div class="demo-box">
        <strong>Usuario de prueba</strong>
        <p>Correo: admin@rubiadivina.com</p>
        <p>Clave: Admin123*</p>
      </div>

      <p v-if="error" class="feedback error">{{ error }}</p>
      <p class="auth-link">¿No tiene cuenta? <RouterLink to="/register">Crear usuario</RouterLink></p>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { loginRequest } from '../services/authService'
import { saveSession } from '../services/storageService'

const router = useRouter()
const loading = ref(false)
const error = ref('')
const form = reactive({
  email: 'admin@rubiadivina.com',
  password: 'Admin123*',
})

async function handleLogin() {
  error.value = ''
  loading.value = true
  try {
    const { data } = await loginRequest(form)
    saveSession(data.token, { nombre: data.nombre, email: data.email })
    router.push('/dashboard')
  } catch (err) {
    error.value = err.response?.data?.message || 'No fue posible iniciar sesión.'
  } finally {
    loading.value = false
  }
}
</script>
