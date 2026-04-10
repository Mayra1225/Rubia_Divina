<template>
  <div class="auth-page">
    <div class="auth-card">
      <span class="badge">Nuevo usuario</span>
      <h1>Crear cuenta</h1>
      <p class="subtitle">Registre un usuario para ingresar al CRUD protegido.</p>

      <form @submit.prevent="handleRegister">
        <label>
          Nombre
          <input v-model.trim="form.nombre" type="text" placeholder="Javier Arias" required />
        </label>

        <label>
          Correo electrónico
          <input v-model.trim="form.email" type="email" placeholder="correo@ejemplo.com" required />
        </label>

        <label>
          Contraseña
          <input v-model.trim="form.password" type="password" placeholder="Mínimo 6 caracteres" required />
        </label>

        <label>
          Confirmar contraseña
          <input v-model.trim="confirmPassword" type="password" required />
        </label>

        <button class="btn btn-primary full" type="submit" :disabled="loading">
          {{ loading ? 'Registrando...' : 'Registrarse' }}
        </button>
      </form>

      <p v-if="error" class="feedback error">{{ error }}</p>
      <p class="auth-link">¿Ya tiene cuenta? <RouterLink to="/login">Volver al login</RouterLink></p>
    </div>
  </div>
</template>

<script setup>
import { reactive, ref } from 'vue'
import { RouterLink, useRouter } from 'vue-router'
import { registerRequest } from '../services/authService'
import { saveSession } from '../services/storageService'

const router = useRouter()
const loading = ref(false)
const error = ref('')
const confirmPassword = ref('')
const form = reactive({
  nombre: '',
  email: '',
  password: '',
})

async function handleRegister() {
  error.value = ''

  if (form.password.length < 6) {
    error.value = 'La contraseña debe tener al menos 6 caracteres.'
    return
  }

  if (form.password !== confirmPassword.value) {
    error.value = 'Las contraseñas no coinciden.'
    return
  }

  loading.value = true
  try {
    const { data } = await registerRequest(form)
    saveSession(data.token, { nombre: data.nombre, email: data.email })
    router.push('/dashboard')
  } catch (err) {
    error.value = err.response?.data?.message || 'No fue posible registrar el usuario.'
  } finally {
    loading.value = false
  }
}
</script>
