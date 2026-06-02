<template>
  <div class="auth-page">
    <div class="overlay"></div>

    <div class="auth-card">
      <div class="brand">
        <h1>☕ Rubia Divina</h1>
        <p>Únete a nuestra cafetería</p>
      </div>

      <h2>Crear Cuenta</h2>

      <form @submit.prevent="handleRegister">
        <label>
          Nombre

          <input v-model.trim="form.nombre" type="text" required />
        </label>

        <label>
          Correo

          <input v-model.trim="form.email" type="email" required />
        </label>

        <label>
          Contraseña

          <input v-model.trim="form.password" type="password" required />
        </label>

        <label>
          Confirmar contraseña

          <input v-model.trim="confirmPassword" type="password" required />
        </label>

        <button class="btn-primary" type="submit" :disabled="loading">
          {{ loading ? 'Registrando...' : 'Registrarse' }}
        </button>
      </form>

      <p v-if="error" class="error">
        {{ error }}
      </p>

      <p class="link">
        ¿Ya tienes cuenta?

        <RouterLink to="/login"> Iniciar sesión </RouterLink>
      </p>
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

  if (form.password !== confirmPassword.value) {
    error.value = 'Las contraseñas no coinciden.'

    return
  }

  loading.value = true

  try {
    const { data } = await registerRequest(form)

    saveSession(data.token, {
      nombre: data.nombre,
      email: data.email,
    })

    router.push('/dashboard')
  } catch (err) {
    error.value = err.response?.data?.message || 'No fue posible registrar.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-page {
  min-height: 100vh;

  background-image: url('https://images.unsplash.com/photo-1509042239860-f550ce710b93');

  background-size: cover;

  background-position: center;

  display: flex;

  justify-content: center;

  align-items: center;

  position: relative;
}

.overlay {
  position: absolute;
  inset: 0;

  background: rgba(0, 0, 0, 0.6);
}

.auth-card {
  width: 420px;

  background: rgba(40, 25, 15, 0.92);

  padding: 40px;

  border-radius: 28px;

  color: white;

  position: relative;

  z-index: 10;
}

.brand {
  text-align: center;

  margin-bottom: 25px;
}

.brand h1 {
  color: #facc15;

  font-size: 36px;
}

.brand p {
  color: #fde68a;
}

form {
  display: flex;

  flex-direction: column;

  gap: 18px;
}

label {
  display: flex;

  flex-direction: column;

  gap: 8px;

  color: #fef3c7;
}

input {
  padding: 14px;

  border-radius: 14px;

  border: none;

  background: #fff7ed;

  color: #3f2a1d;
}

.btn-primary {
  background: linear-gradient(135deg, #b45309, #f59e0b);

  border: none;

  padding: 15px;

  border-radius: 14px;

  color: white;

  font-weight: bold;

  cursor: pointer;
}

.error {
  color: #fca5a5;

  margin-top: 15px;

  text-align: center;
}

.link {
  text-align: center;

  margin-top: 20px;
}

a {
  color: #facc15;

  text-decoration: none;

  font-weight: bold;
}
</style>
