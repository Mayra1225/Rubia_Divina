<template>
  <div class="dashboard-page">
    <header class="topbar">
      <div>
        <span class="badge">Sección protegida</span>
        <h1>Panel de productos</h1>
        <p>Bienvenido, {{ user?.nombre }}. Aquí se ejecutan las operaciones CRUD.</p>
      </div>
      <button class="btn btn-danger" @click="logout">Cerrar sesión</button>
    </header>

    <p v-if="globalError" class="feedback error">{{ globalError }}</p>
    <p v-if="successMessage" class="feedback success">{{ successMessage }}</p>

    <div class="dashboard-grid">
      <ProductoForm :model-value="editingProduct" @save="handleSave" @cancel="resetForm" />

      <section class="table-card">
        <div class="table-header">
          <h3>Listado de productos</h3>
          <button class="btn btn-secondary" @click="loadProductos">Actualizar</button>
        </div>

        <div class="table-wrapper">
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
              <tr v-if="loading">
                <td colspan="5">Cargando productos...</td>
              </tr>
              <tr v-else-if="!productos.length">
                <td colspan="5">No hay productos registrados.</td>
              </tr>
              <tr v-for="producto in productos" :key="producto.id">
                <td>
                  <strong>{{ producto.nombre }}</strong>
                  <p class="cell-desc">{{ producto.descripcion || 'Sin descripción' }}</p>
                </td>
                <td>{{ producto.categoria }}</td>
                <td>${{ Number(producto.precio).toFixed(2) }}</td>
                <td>{{ producto.stock }}</td>
                <td>
                  <div class="row-actions">
                    <button class="btn btn-secondary" @click="selectProduct(producto)">Editar</button>
                    <button class="btn btn-danger" @click="handleDelete(producto.id)">Eliminar</button>
                  </div>
                </td>
              </tr>
            </tbody>
          </table>
        </div>
      </section>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import ProductoForm from '../components/ProductoForm.vue'
import { createProducto, deleteProducto, getProductos, updateProducto } from '../services/productoService'
import { clearSession, getUser } from '../services/storageService'

const router = useRouter()
const user = ref(getUser())
const productos = ref([])
const loading = ref(false)
const globalError = ref('')
const successMessage = ref('')
const editingProduct = ref(null)

async function loadProductos() {
  loading.value = true
  globalError.value = ''
  try {
    const { data } = await getProductos()
    productos.value = data
  } catch (err) {
    globalError.value = err.response?.data?.message || 'No fue posible cargar los productos.'
  } finally {
    loading.value = false
  }
}

function selectProduct(producto) {
  editingProduct.value = { ...producto }
  successMessage.value = ''
  globalError.value = ''
}

function resetForm() {
  editingProduct.value = null
}

async function handleSave(payload) {
  globalError.value = ''
  successMessage.value = ''
  try {
    if (editingProduct.value?.id) {
      await updateProducto(editingProduct.value.id, payload)
      successMessage.value = 'Producto actualizado correctamente.'
    } else {
      await createProducto(payload)
      successMessage.value = 'Producto creado correctamente.'
    }
    resetForm()
    await loadProductos()
  } catch (err) {
    globalError.value = err.response?.data?.message || 'No fue posible guardar el producto.'
  }
}

async function handleDelete(id) {
  const confirmado = window.confirm('¿Desea eliminar este producto?')
  if (!confirmado) return

  globalError.value = ''
  successMessage.value = ''
  try {
    await deleteProducto(id)
    successMessage.value = 'Producto eliminado correctamente.'
    if (editingProduct.value?.id === id) {
      resetForm()
    }
    await loadProductos()
  } catch (err) {
    globalError.value = err.response?.data?.message || 'No fue posible eliminar el producto.'
  }
}

function logout() {
  clearSession()
  router.push('/login')
}

onMounted(loadProductos)
</script>
