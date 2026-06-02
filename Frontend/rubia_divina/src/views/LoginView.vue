<template>
  <div class="auth-page">
    <div class="overlay"></div>

    <div class="auth-card">
      <div class="brand">
        <h1>☕ Rubia Divina</h1>
        <p>Cafetería & Postres Artesanales</p>
      </div>

      <h2>Iniciar Sesión</h2>

      <form @submit.prevent="handleLogin">
        <label>
          Correo electrónico

          <input
            v-model.trim="form.email"
            type="email"
            placeholder="admin@rubiadivina.com"
            required
          />
        </label>

        <label>
          Contraseña

          <input v-model.trim="form.password" type="password" placeholder="••••••••" required />
        </label>

        <button class="btn-primary" type="submit" :disabled="loading">
          {{ loading ? 'Ingresando...' : 'Ingresar' }}
        </button>
      </form>

      <div class="demo-box">
        <strong>Usuario Demo</strong>

        <p>Email: admin@rubiadivina.com</p>

        <p>Clave: Admin123*</p>
      </div>

      <p v-if="error" class="error">
        {{ error }}
      </p>

      <p class="link">
        ¿No tienes cuenta?

        <RouterLink to="/register"> Crear cuenta </RouterLink>
      </p>
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

    saveSession(data.token, {
      nombre: data.nombre,
      email: data.email,
    })

    router.push('/dashboard')
  } catch (err) {
    error.value = err.response?.data?.message || 'No fue posible iniciar sesión.'
  } finally {
    loading.value = false
  }
}
</script>

<style scoped>
.auth-page {
  min-height: 100vh;

  background-image: url('https://images.unsplash.com/photo-1495474472287-4d71bcdd2085');

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

  background: rgba(0, 0, 0, 0.55);
}

.auth-card {
  width: 420px;

  background: rgba(30, 20, 15, 0.92);

  backdrop-filter: blur(8px);

  border-radius: 28px;

  padding: 40px;

  position: relative;

  z-index: 10;

  color: white;

  box-shadow: 0 20px 60px rgba(0, 0, 0, 0.45);
}

.brand {
  text-align: center;

  margin-bottom: 25px;
}

.brand h1 {
  font-size: 38px;

  color: #facc15;
}

.brand p {
  color: #fde68a;
}

h2 {
  margin-bottom: 20px;

  text-align: center;
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

  border: none;

  border-radius: 14px;

  background: #fff7ed;

  color: #3f2a1d;

  font-size: 15px;
}

.btn-primary {
  background: linear-gradient(135deg, #b45309, #f59e0b);

  border: none;

  padding: 15px;

  border-radius: 14px;

  color: white;

  font-size: 16px;

  font-weight: bold;

  cursor: pointer;

  margin-top: 10px;
}

.demo-box {
  margin-top: 25px;

  background: rgba(255, 255, 255, 0.08);

  padding: 16px;

  border-radius: 14px;

  color: #fde68a;
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
