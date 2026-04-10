<template>
  <div class="container">
    <div class="login-card">
      <h1>☕ Rubia Divina</h1>
      <p class="subtitle">Café & Bar</p>

      <input v-model="email" placeholder="Correo electrónico" />
      <input v-model="password" type="password" placeholder="Contraseña" />

      <button @click="handleLogin">Ingresar</button>

      <p class="link">
        ¿No tienes cuenta?
        <router-link to="/register">Regístrate</router-link>
      </p>

      <p v-if="error" class="error">{{ error }}</p>
    </div>
  </div>
</template>

<script>
import { login } from '../services/authService'

export default {
  data() {
    return {
      email: '',
      password: '',
      error: '',
    }
  },
  methods: {
    async handleLogin() {
      try {
        const res = await login({
          email: this.email.trim(),
          password: this.password.trim(),
        })

        localStorage.setItem('token', res.data.token)
        this.$router.push('/dashboard')
      } catch {
        this.error = 'Credenciales incorrectas'
      }
    },
  },
}
</script>

<style scoped>
.container {
  height: 100vh;
  display: flex;
  justify-content: center;
  align-items: center;
  background:
    linear-gradient(rgba(0, 0, 0, 0.7), rgba(0, 0, 0, 0.9)),
    url('https://images.unsplash.com/photo-1509042239860-f550ce710b93');
  background-size: cover;
  background-position: center;
}

.login-card {
  background: #1e1e1e;
  padding: 40px;
  border-radius: 15px;
  width: 320px;
  text-align: center;
  color: white;
  box-shadow: 0 0 20px rgba(0, 0, 0, 0.5);
}

h1 {
  color: #d4af37;
  margin-bottom: 5px;
}

.subtitle {
  color: #c8a27c;
  margin-bottom: 20px;
}

input {
  width: 100%;
  padding: 10px;
  margin: 10px 0;
  border: none;
  border-radius: 8px;
  outline: none;
}

button {
  width: 100%;
  padding: 10px;
  background: #6f4e37;
  color: white;
  border: none;
  border-radius: 8px;
  cursor: pointer;
  transition: 0.3s;
}

button:hover {
  background: #c8a27c;
}

.link {
  margin-top: 15px;
}

a {
  color: #d4af37;
}

.error {
  color: red;
  margin-top: 10px;
}
</style>
