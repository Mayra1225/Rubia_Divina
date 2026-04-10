<script setup>
import { ref, onMounted } from 'vue'
import productoService from '../services/productoService'

const productos = ref([])
const error = ref('')
const cargando = ref(false)

const form = ref({
  nombre: '',
  categoria: '',
  descripcion: '',
  precio: 0,
  stock: 0
})

const categorias = ['Bebida', 'Postre', 'Snack', 'Otro']

const cargarProductos = async () => {
  try {
    cargando.value = true
    error.value = ''
    const response = await productoService.getAll()
    productos.value = response.data
  } catch (err) {
    console.error('Error al cargar productos:', err)
    error.value = 'No fue posible cargar los productos.'
  } finally {
    cargando.value = false
  }
}

const crearProducto = async () => {
  try {
    error.value = ''

    await productoService.create({
      nombre: form.value.nombre,
      categoria: form.value.categoria,
      descripcion: form.value.descripcion,
      precio: Number(form.value.precio),
      stock: Number(form.value.stock)
    })

    form.value = {
      nombre: '',
      categoria: '',
      descripcion: '',
      precio: 0,
      stock: 0
    }

    await cargarProductos()
  } catch (err) {
    console.error('Error al crear producto:', err)
    error.value = 'No fue posible crear el producto.'
  }
}

const eliminarProducto = async (id) => {
  try {
    error.value = ''
    await productoService.delete(id)
    await cargarProductos()
  } catch (err) {
    console.error('Error al eliminar producto:', err)
    error.value = 'No fue posible eliminar el producto.'
  }
}

const cerrarSesion = () => {
  localStorage.removeItem('token')
  window.location.href = '/login'
}

onMounted(() => {
  cargarProductos()
})
</script>

<template>
  <div class="dashboard-page">
    <div class="header">
      <span class="badge">Sección protegida</span>
      <button class="logout-btn" @click="cerrarSesion">Cerrar sesión</button>
    </div>

    <h1>Panel de productos</h1>
    <p class="subtitle">Bienvenido, Administrador. Aquí se ejecutan las operaciones CRUD.</p>

    <div v-if="error" class="alert-error">
      {{ error }}
    </div>

    <div class="content">
      <div class="card form-card">
        <h2>Nuevo producto</h2>

        <label>Nombre</label>
        <input v-model="form.nombre" type="text" />

        <label>Categoría</label>
        <select v-model="form.categoria">
          <option value="">Seleccione</option>
          <option v-for="cat in categorias" :key="cat" :value="cat">
            {{ cat }}
          </option>
        </select>

        <label>Descripción</label>
        <textarea v-model="form.descripcion"></textarea>

        <div class="row">
          <div>
            <label>Precio</label>
            <input v-model="form.precio" type="number" min="0" step="0.01" />
          </div>
          <div>
            <label>Stock</label>
            <input v-model="form.stock" type="number" min="0" step="1" />
          </div>
        </div>

        <button class="primary-btn" @click="crearProducto">Crear producto</button>
      </div>

      <div class="card table-card">
        <div class="table-header">
          <h2>Listado de productos</h2>
          <button class="secondary-btn" @click="cargarProductos">Actualizar</button>
        </div>

        <table>
          <thead>
            <tr>
              <th>Nombre</th>
              <th>Categoría</th>
              <th>Precio</th>
              <th>Stock</th>
              <th>Acciones</th>
            </tr>
          </thead>
          <tbody>
            <tr v-if="cargando">
              <td colspan="5">Cargando productos...</td>
            </tr>

            <tr v-else-if="productos.length === 0">
              <td colspan="5">No hay productos registrados.</td>
            </tr>

            <tr v-else v-for="producto in productos" :key="producto.id">
              <td>{{ producto.nombre }}</td>
              <td>{{ producto.categoria }}</td>
              <td>${{ Number(producto.precio).toFixed(2) }}</td>
              <td>{{ producto.stock }}</td>
              <td>
                <button class="delete-btn" @click="eliminarProducto(producto.id)">
                  Eliminar
                </button>
              </td>
            </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>

<style scoped>
.dashboard-page {
  min-height: 100vh;
  padding: 24px;
  background: linear-gradient(90deg, #091b3a 0%, #2c2f70 100%);
  color: white;
  font-family: Arial, sans-serif;
}

.header {
  display: flex;
  justify-content: space-between;
  align-items: center;
}

.badge {
  display: inline-block;
  background: #f2ebff;
  color: #5b2bd9;
  padding: 10px 14px;
  border-radius: 999px;
  font-weight: 600;
}

.logout-btn {
  background: #ef2b2b;
  color: white;
  border: none;
  padding: 14px 22px;
  border-radius: 14px;
  font-weight: 700;
  cursor: pointer;
}

h1 {
  margin-top: 26px;
  font-size: 54px;
}

.subtitle {
  color: #c9d0df;
  margin-bottom: 24px;
}

.alert-error {
  background: #ffe5e5;
  color: #b12a2a;
  padding: 18px;
  border-radius: 16px;
  margin-bottom: 18px;
}

.content {
  display: grid;
  grid-template-columns: 380px 1fr;
  gap: 24px;
}

.card {
  background: #f5f5f7;
  color: #111827;
  border-radius: 24px;
  padding: 24px;
}

.form-card label {
  display: block;
  margin-top: 16px;
  margin-bottom: 8px;
  font-weight: 600;
}

.form-card input,
.form-card select,
.form-card textarea {
  width: 100%;
  padding: 14px;
  border-radius: 14px;
  border: 1px solid #ddd;
  box-sizing: border-box;
}

.form-card textarea {
  min-height: 100px;
  resize: vertical;
}

.row {
  display: grid;
  grid-template-columns: 1fr 1fr;
  gap: 16px;
  margin-top: 8px;
}

.primary-btn,
.secondary-btn,
.delete-btn {
  border: none;
  border-radius: 14px;
  padding: 14px 18px;
  font-weight: 700;
  cursor: pointer;
}

.primary-btn {
  margin-top: 22px;
  background: #7c3aed;
  color: white;
}

.secondary-btn {
  background: #f0f0f0;
}

.delete-btn {
  background: #ef4444;
  color: white;
}

.table-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 18px;
}

table {
  width: 100%;
  border-collapse: collapse;
  background: white;
}

th,
td {
  text-align: left;
  padding: 16px;
  border-bottom: 1px solid #e5e7eb;
}

@media (max-width: 900px) {
  .content {
    grid-template-columns: 1fr;
  }

  h1 {
    font-size: 36px;
  }
}
</style>