<template>
  <form class="product-form" @submit.prevent="submitForm">
    <h2>
      {{ isEditing ? 'Editar Producto' : 'Nuevo Producto' }}
    </h2>

    <label>
      Nombre

      <input v-model="form.nombre" type="text" required />
    </label>

    <label>
      Categoría

      <select v-model="form.categoriaId" required>
        <option disabled value="">Seleccione</option>

        <option v-for="categoria in categorias" :key="categoria.id" :value="categoria.id">
          {{ categoria.nombre }}
        </option>
      </select>
    </label>

    <label>
      Descripción

      <textarea v-model="form.descripcion" />
    </label>

    <label>
      Precio

      <input v-model.number="form.precio" type="number" step="0.01" required />
    </label>

    <label>
      Stock

      <input v-model.number="form.stock" type="number" required />
    </label>

    <label>
      URL Imagen

      <input v-model="form.imagenUrl" type="text" />
    </label>

    <div class="actions">
      <button type="submit">
        {{ isEditing ? 'Guardar' : 'Crear' }}
      </button>

      <button type="button" class="cancel" @click="emit('cancel')">Cancelar</button>
    </div>
  </form>
</template>

<script setup>
import { computed, reactive, ref, watch, onMounted } from 'vue'

import { getCategorias } from '../services/categoriaService'

const props = defineProps({
  modelValue: Object,
})

const emit = defineEmits(['save', 'cancel'])

const categorias = ref([])

const form = reactive({
  nombre: '',
  descripcion: '',
  precio: 0,
  stock: 0,
  categoriaId: '',
  imagenUrl: '',
})

const isEditing = computed(() => Boolean(props.modelValue?.id))

watch(
  () => props.modelValue,
  (value) => {
    form.nombre = value?.nombre ?? ''

    form.descripcion = value?.descripcion ?? ''

    form.precio = value?.precio ?? 0

    form.stock = value?.stock ?? 0

    form.categoriaId = value?.categoriaId ?? ''

    form.imagenUrl = value?.imagenUrl ?? ''
  },
  { immediate: true },
)

async function loadCategorias() {
  const { data } = await getCategorias()

  categorias.value = data
}

function submitForm() {
  emit('save', {
    nombre: form.nombre,
    descripcion: form.descripcion,
    precio: form.precio,
    stock: form.stock,
    categoriaId: form.categoriaId,
    imagenUrl: form.imagenUrl,
  })
}

onMounted(loadCategorias)
</script>
