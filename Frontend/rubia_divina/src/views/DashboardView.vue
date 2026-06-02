<template>
  <div class="dashboard-page">
    <header class="topbar">
      <div>
        <span class="badge"> ☕ Rubia Divina Café </span>

        <h1>Panel de Productos</h1>

        <p>Bienvenido {{ user?.nombre }}</p>
      </div>

      <div class="topbar-actions">
        <button class="btn-add" @click="openCreateModal">+ Agregar Producto</button>

        <button class="btn-logout" @click="logout">Cerrar sesión</button>
      </div>
    </header>

    <p v-if="globalError" class="feedback error">
      {{ globalError }}
    </p>

    <p v-if="successMessage" class="feedback success">
      {{ successMessage }}
    </p>

    <div v-if="loading" class="loading">Cargando productos...</div>

    <div v-else class="products-grid">
      <div v-for="producto in productos" :key="producto.id" class="product-card">
        <img
          :src="
            producto.imagenUrl || 'https://images.unsplash.com/photo-1509042239860-f550ce710b93'
          "
          class="product-image"
        />

        <div class="product-body">
          <div class="product-category">
            {{ producto.categoria?.nombre }}
          </div>

          <h3>
            {{ producto.nombre }}
          </h3>

          <p class="description">
            {{ producto.descripcion || 'Sin descripción' }}
          </p>

          <div class="product-footer">
            <span class="price"> ${{ Number(producto.precio).toFixed(2) }} </span>

            <span class="stock">
              Stock:
              {{ producto.stock }}
            </span>
          </div>

          <div class="actions">
            <button class="edit-btn" @click="openEditModal(producto)">Editar</button>

            <button class="delete-btn" @click="handleDelete(producto.id)">Eliminar</button>
          </div>
        </div>
      </div>
    </div>

    <!-- MODAL -->

    <div v-if="showModal" class="modal-overlay">
      <div class="modal-content">
        <ProductoForm :model-value="editingProduct" @save="handleSave" @cancel="closeModal" />
      </div>
    </div>
  </div>
</template>

<script setup>
import { onMounted, ref } from 'vue'

import { useRouter } from 'vue-router'

import ProductoForm from '../components/ProductoForm.vue'

import {
  createProducto,
  deleteProducto,
  getProductos,
  updateProducto,
} from '../services/productoService'

import { clearSession, getUser } from '../services/storageService'

const router = useRouter()

const user = ref(getUser())

const productos = ref([])

const loading = ref(false)

const globalError = ref('')

const successMessage = ref('')

const editingProduct = ref(null)

const showModal = ref(false)

async function loadProductos() {
  loading.value = true

  try {
    const { data } = await getProductos()

    productos.value = data
  } catch (err) {
    globalError.value = err.response?.data?.message || 'No fue posible cargar productos.'
  } finally {
    loading.value = false
  }
}

function openCreateModal() {
  editingProduct.value = null
  showModal.value = true
}

function openEditModal(producto) {
  editingProduct.value = {
    ...producto,
    categoriaId: producto.categoriaId,
  }

  showModal.value = true
}

function closeModal() {
  showModal.value = false
  editingProduct.value = null
}

async function handleSave(payload) {
  try {
    if (editingProduct.value?.id) {
      await updateProducto(editingProduct.value.id, payload)

      successMessage.value = 'Producto actualizado.'
    } else {
      await createProducto(payload)

      successMessage.value = 'Producto creado.'
    }

    closeModal()

    await loadProductos()
  } catch (err) {
    globalError.value = err.response?.data?.message || 'Error al guardar.'
  }
}

async function handleDelete(id) {
  const confirmado = window.confirm('¿Eliminar producto?')

  if (!confirmado) return

  try {
    await deleteProducto(id)

    successMessage.value = 'Producto eliminado.'

    await loadProductos()
  } catch (err) {
    globalError.value = 'No se pudo eliminar.'
  }
}

function logout() {
  clearSession()

  router.push('/login')
}

onMounted(loadProductos)
</script>

<style scoped>
.dashboard-page {
  min-height: 100vh;
  background:
    linear-gradient(rgba(20, 20, 20, 0.85), rgba(20, 20, 20, 0.92)),
    url('https://images.unsplash.com/photo-1495474472287-4d71bcdd2085');
  background-size: cover;
  background-position: center;
  padding: 40px;
  color: white;
}

.topbar {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 40px;
}

.badge {
  background: #c59d5f;
  color: black;
  padding: 8px 16px;
  border-radius: 999px;
  font-weight: bold;
}

.topbar h1 {
  font-size: 42px;
  margin-top: 15px;
}

.topbar p {
  color: #ddd;
}

.topbar-actions {
  display: flex;
  gap: 15px;
}

.btn-add,
.btn-logout {
  border: none;
  padding: 14px 22px;
  border-radius: 14px;
  cursor: pointer;
  font-weight: bold;
  transition: 0.3s;
}

.btn-add {
  background: #c59d5f;
  color: black;
}

.btn-add:hover {
  transform: scale(1.05);
}

.btn-logout {
  background: #8b1e1e;
  color: white;
}

.products-grid {
  display: grid;
  grid-template-columns: repeat(auto-fill, minmax(300px, 1fr));
  gap: 30px;
}

.product-card {
  background: rgba(255, 255, 255, 0.08);
  backdrop-filter: blur(10px);
  border-radius: 24px;
  overflow: hidden;
  border: 1px solid rgba(255, 255, 255, 0.1);
  transition: 0.3s;
}

.product-card:hover {
  transform: translateY(-6px);
}

.product-image {
  width: 100%;
  height: 230px;
  object-fit: cover;
}

.product-body {
  padding: 24px;
}

.product-category {
  display: inline-block;
  background: #c59d5f;
  color: black;
  padding: 6px 12px;
  border-radius: 999px;
  font-size: 13px;
  margin-bottom: 15px;
  font-weight: bold;
}

.product-body h3 {
  font-size: 26px;
  margin-bottom: 12px;
}

.description {
  color: #ddd;
  line-height: 1.5;
  margin-bottom: 20px;
}

.product-footer {
  display: flex;
  justify-content: space-between;
  margin-bottom: 20px;
}

.price {
  color: #ffd27d;
  font-size: 24px;
  font-weight: bold;
}

.stock {
  color: #eee;
}

.actions {
  display: flex;
  gap: 12px;
}

.edit-btn,
.delete-btn {
  flex: 1;
  border: none;
  padding: 12px;
  border-radius: 12px;
  cursor: pointer;
  font-weight: bold;
}

.edit-btn {
  background: #c59d5f;
  color: black;
}

.delete-btn {
  background: #8b1e1e;
  color: white;
}

.feedback {
  margin-bottom: 25px;
  padding: 15px;
  border-radius: 14px;
}

.error {
  background: rgba(255, 0, 0, 0.15);
  color: #ffbdbd;
}

.success {
  background: rgba(0, 255, 100, 0.15);
  color: #c9ffd8;
}

.loading {
  text-align: center;
  font-size: 24px;
}

.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.7);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 999;
}

.modal-content {
  width: 100%;
  max-width: 600px;
  background: #1f1f1f;
  border-radius: 24px;
  padding: 30px;
}
</style>
