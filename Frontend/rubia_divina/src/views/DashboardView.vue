<template>
  <div class="dashboard-page">
    <header class="dashboard-header">
      <div>
        <h1>☕ Rubia Divina Dashboard</h1>
        <p>Bienvenido {{ user?.nombre }}</p>
      </div>

      <button class="logout-btn" @click="logout">Cerrar Sesión</button>
    </header>

    <div class="dashboard-links">
      <RouterLink to="/pedidos" class="nav-card"> 📋 Pedidos </RouterLink>

      <RouterLink to="/promociones" class="nav-card"> 🎁 Promociones </RouterLink>
    </div>

    <div v-if="dashboard" class="stats-grid">
      <div class="stat-card">
        <h3>Total Ventas</h3>

        <h2>${{ dashboard.totalVentas?.toFixed(2) || 0 }}</h2>
      </div>

      <div class="stat-card">
        <h3>Total Pedidos</h3>

        <h2>
          {{ dashboard.totalPedidos || 0 }}
        </h2>
      </div>

      <div class="stat-card">
        <h3>Producto Más Vendido</h3>

        <h2>
          {{ dashboard.productoMasVendido?.producto || 'Sin datos' }}
        </h2>
      </div>

      <div class="stat-card">
        <h3>Promoción Más Utilizada</h3>

        <h2>
          {{ dashboard.promoMasUsada?.promocion || 'Sin datos' }}
        </h2>
      </div>
    </div>

    <div v-if="horarioPico" class="peak-card">
      <h2>🔥 Horario Pico</h2>

      <p>
        Día:
        <strong>
          {{ traducirDia(horarioPico.diaSemana) }}
        </strong>
      </p>

      <p>
        Hora:
        <strong> {{ horarioPico.hora }}:00 </strong>
      </p>

      <p>
        Pedidos:
        <strong>
          {{ horarioPico.cantidadPedidos }}
        </strong>
      </p>
    </div>

    <div v-if="productosMasVendidosDia.length" class="analytics-card">
      <h2>📊 Producto Más Vendido por Día</h2>

      <table class="analytics-table">
        <thead>
          <tr>
            <th>Día</th>
            <th>Producto Más Vendido</th>
            <th>Cantidad Vendida</th>
            <th>Recomendación</th>
          </tr>
        </thead>

        <tbody>
          <tr v-for="item in productosMasVendidosDia" :key="item.diaSemana">
            <td>{{ item.diaSemana }}</td>

            <td>{{ item.producto }}</td>

            <td>{{ item.cantidadVendida }}</td>

            <td>{{ item.recomendacion }}</td>
          </tr>
        </tbody>
      </table>
    </div>

    <div v-if="dashboard?.recomendacionPromocion" class="recomendation-card">
      <h2>Recomendacion de Promoción</h2>

      <p>
        {{ dashboard.recomendacionPromocion }}
      </p>
    </div>

    <div class="section-header">
      <h2>Productos</h2>

      <div class="header-actions">
        <select v-model="categoriaSeleccionada" @change="filtrarCategoria" class="categoria-select">
          <option value="">Todas las categorías</option>

          <option v-for="categoria in categorias" :key="categoria.id" :value="categoria.id">
            {{ categoria.nombre }}
          </option>
        </select>

        <button class="add-btn" @click="showProductoModal = true">+ Nuevo Producto</button>
      </div>
    </div>

    <div class="products-grid">
      <div v-for="producto in productos" :key="producto.id" class="product-card">
        <img :src="producto.imagenUrl || defaultImage" alt="producto" />

        <h3>
          {{ producto.nombre }}
        </h3>

        <p>
          {{ producto.descripcion }}
        </p>

        <span class="price"> ${{ producto.precio }} </span>

        <div class="actions">
          <button class="edit-btn" @click="editarProducto(producto)">Editar</button>

          <button class="delete-btn" @click="eliminarProducto(producto.id)">Eliminar</button>
        </div>
      </div>
    </div>

    <ProductoModal
      v-if="showProductoModal"
      :modelValue="productoSeleccionado"
      @close="cerrarModal"
      @save="guardarProducto"
    />
  </div>
</template>

<script setup>
import { ref, onMounted } from 'vue'
import { RouterLink, useRouter } from 'vue-router'

import {
  getProductos,
  getProductosPorCategoria,
  createProducto,
  updateProducto,
  deleteProducto,
} from '../services/productoService'

import { getCategorias } from '../services/categoriaService'

import { getDashboard } from '../services/dashboardService'

import { getProductosMasVendidosDia } from '../services/dashboardService'

import { getHorariosPico } from '../services/horarioPicoService'

import { getUser, clearSession } from '../services/storageService'

import ProductoModal from '../components/ProductoModal.vue'

const router = useRouter()

const user = ref(getUser())

const dashboard = ref(null)

const productosMasVendidosDia = ref([])

const horarioPico = ref(null)

const productos = ref([])

const categorias = ref([])

const categoriaSeleccionada = ref('')

const showProductoModal = ref(false)

const productoSeleccionado = ref(null)

const defaultImage = 'https://images.pexels.com/photos/302899/pexels-photo-302899.jpeg'

async function cargarDashboard() {
  try {
    const { data } = await getDashboard()

    dashboard.value = data
  } catch (error) {
    console.log(error)
  }
}

async function cargarProductosMasVendidosDia() {
  const { data } = await getProductosMasVendidosDia()

  productosMasVendidosDia.value = data
}

async function cargarHorarioPico() {
  try {
    const { data } = await getHorariosPico()

    horarioPico.value = data.find((x) => x.esHorarioPico)
  } catch (error) {
    console.log(error)
  }
}

async function cargarCategorias() {
  try {
    const { data } = await getCategorias()

    categorias.value = data
  } catch (error) {
    console.log(error)
  }
}

async function cargarProductos() {
  try {
    const { data } = await getProductos()

    productos.value = data
  } catch (error) {
    console.log(error)
  }
}

async function filtrarCategoria() {
  try {
    if (!categoriaSeleccionada.value) {
      await cargarProductos()
      return
    }

    const { data } = await getProductosPorCategoria(categoriaSeleccionada.value)

    productos.value = data
  } catch (error) {
    console.log(error)
  }
}

function traducirDia(dia) {
  const dias = {
    Monday: 'Lunes',
    Tuesday: 'Martes',
    Wednesday: 'Miércoles',
    Thursday: 'Jueves',
    Friday: 'Viernes',
    Saturday: 'Sábado',
    Sunday: 'Domingo',
  }

  return dias[dia] || dia
}

function editarProducto(producto) {
  productoSeleccionado.value = {
    ...producto,
  }

  showProductoModal.value = true
}

function cerrarModal() {
  productoSeleccionado.value = null

  showProductoModal.value = false
}

async function guardarProducto(payload) {
  try {
    if (productoSeleccionado.value) {
      await updateProducto(productoSeleccionado.value.id, payload)
    } else {
      await createProducto(payload)
    }

    cerrarModal()

    // Si hay un filtro aplicado, se mantiene
    if (categoriaSeleccionada.value) {
      await filtrarCategoria()
    } else {
      await cargarProductos()
    }
  } catch (error) {
    console.log(error)
  }
}

async function eliminarProducto(id) {
  if (!confirm('¿Eliminar producto?')) return

  try {
    await deleteProducto(id)

    // Si hay un filtro aplicado, se mantiene
    if (categoriaSeleccionada.value) {
      await filtrarCategoria()
    } else {
      await cargarProductos()
    }
  } catch (error) {
    console.log(error)
  }
}

function logout() {
  clearSession()

  router.push('/login')
}

onMounted(async () => {
  await cargarDashboard()

  await cargarHorarioPico()

  await cargarCategorias()

  await cargarProductos()

  await cargarProductosMasVendidosDia()
})
</script>

<style scoped>
.dashboard-page {
  padding: 30px;
  background: #f6f1ea;
  min-height: 100vh;
}

.dashboard-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 30px;
}

.dashboard-header h1 {
  color: #5d4037;
}

.logout-btn {
  background: #8d6e63;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 10px;
  cursor: pointer;
}

.dashboard-links {
  display: flex;
  gap: 20px;
  margin-bottom: 30px;
}

.nav-card {
  background: white;
  padding: 20px;
  border-radius: 15px;
  text-decoration: none;
  color: #5d4037;
  font-weight: bold;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 20px;
  margin-bottom: 30px;
}

.stat-card {
  background: white;
  padding: 20px;
  border-radius: 15px;
  text-align: center;
}

.peak-card {
  background: #fff3e0;
  border-left: 6px solid #ff9800;
  border-radius: 15px;
  padding: 20px;
  margin-bottom: 30px;
  box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
}

.section-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 20px;
}

.add-btn {
  background: #6d4c41;
  color: white;
  border: none;
  padding: 10px 20px;
  border-radius: 10px;
  cursor: pointer;
}

.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(250px, 1fr));
  gap: 20px;
}

.product-card {
  background: white;
  border-radius: 20px;
  overflow: hidden;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.product-card img {
  width: 100%;
  height: 180px;
  object-fit: cover;
}

.product-card h3 {
  padding: 10px;
  color: #4e342e;
}

.product-card p {
  padding: 0 10px;
  color: #555;
}

.price {
  display: block;
  padding: 10px;
  font-weight: bold;
  color: #2e7d32;
}

.actions {
  display: flex;
  justify-content: space-around;
  padding: 15px;
}

.edit-btn {
  background: #ffb300;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 8px;
}

.delete-btn {
  background: #e53935;
  color: white;
  border: none;
  padding: 8px 16px;
  border-radius: 8px;
}

.analytics-card {
  background: white;
  margin-bottom: 30px;
  padding: 25px;
  border-radius: 20px;
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.12);
}

.analytics-card h2 {
  color: #5d4037;
  margin-bottom: 20px;
}

.analytics-table {
  width: 100%;
  border-collapse: collapse;
}

.analytics-table th {
  background: #6d4c41;
  color: white;
  padding: 12px;
}

.analytics-table td {
  padding: 12px;
  border-bottom: 1ox solid #eee;
}

.analytics-table tr:hover {
  background: #f8f4ef;
}

.recomendation-card {
  background: #e8f5e9;
  border-left: 6px solid #43a047;
  border-radius: 20px;
  padding: 25px;
  margin-bottom: 30px;
  box-shadow: 0 4px 12px rgbd(0, 0, 0, 0.12);
}

.recomendation-card h2 {
  color: #2e7d32;
  margin-bold: 10px;
}

.recomendation-card p {
  color: #1b5e20;
  font-size: 16px;
  line-height: 1.6;
}

.header-actions {
  display: flex;
  align-items: center;
  gap: 15px;
}

.categoria-select {
  min-width: 220px;
  padding: 10px 15px;
  border: 1px solid #d7ccc8;
  border-radius: 10px;
  background: white;
  color: #5d4037;
  font-size: 15px;
  cursor: pointer;
  transition: 0.3s;
}

.categoria-select:hover {
  border-color: #8d6e63;
}

.categoria-select:focus {
  outline: none;
  border-color: #6d4c41;
  box-shadow: 0 0 0 3px rgba(109, 76, 65, 0.15);
}
</style>
