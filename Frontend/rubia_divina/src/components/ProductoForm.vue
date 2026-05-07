<template>
  <form class="form-card" @submit.prevent="submitForm">
    <h3>{{ isEditing ? 'Editar producto' : 'Nuevo producto' }}</h3>

    <div class="grid-form">
      <label>
        Nombre
        <input
          v-model="form.nombre"
          type="text"
          maxlength="80"
          required
        />
      </label>

      <label>
        Categoría
        <select v-model="form.categoriaId" required>
          <option disabled value="">Seleccione una categoría</option>

          <option
            v-for="categoria in categorias"
            :key="categoria.id"
            :value="categoria.id"
          >
            {{ categoria.nombre }}
          </option>
        </select>
      </label>

      <label class="full-width">
        Descripción
        <textarea
          v-model="form.descripcion"
          rows="3"
          maxlength="250"
        ></textarea>
      </label>

      <label>
        Precio
        <input
          v-model.number="form.precio"
          type="number"
          min="0.01"
          step="0.01"
          required
        />
      </label>

      <label>
        Stock
        <input
          v-model.number="form.stock"
          type="number"
          min="0"
          step="1"
          required
        />
      </label>
    </div>

    <p v-if="error" class="feedback error">
      {{ error }}
    </p>

    <div class="actions">
      <button class="btn btn-primary" type="submit">
        {{ isEditing ? 'Guardar cambios' : 'Crear producto' }}
      </button>

      <button
        v-if="isEditing"
        class="btn btn-secondary"
        type="button"
        @click="cancelEdit"
      >
        Cancelar
      </button>
    </div>
  </form>
</template>

<script setup>
import { computed, reactive, watch, ref, onMounted } from 'vue'
import { getCategorias } from '../services/categoriaService'

const props = defineProps({
  modelValue: {
    type: Object,
    default: null,
  },
})

const emit = defineEmits(['save', 'cancel'])

const categorias = ref([])

const form = reactive({
  nombre: '',
  categoriaId: '',
  descripcion: '',
  precio: 0,
  stock: 0,
})

const isEditing = computed(() => Boolean(props.modelValue?.id))

const error = ref('')

watch(
  () => props.modelValue,
  (value) => {
    form.nombre = value?.nombre ?? ''
    form.categoriaId = value?.categoriaId ?? ''
    form.descripcion = value?.descripcion ?? ''
    form.precio = value?.precio ?? 0
    form.stock = value?.stock ?? 0
  },
  { immediate: true },
)

async function loadCategorias() {
  try {
    const { data } = await getCategorias()
    categorias.value = data
  } catch (err) {
    error.value = 'No fue posible cargar las categorías.'
  }
}

function submitForm() {
  error.value = ''

  if (!form.nombre.trim()) {
    error.value = 'Ingrese el nombre del producto.'
    return
  }

  if (!form.categoriaId) {
    error.value = 'Seleccione una categoría.'
    return
  }

  if (form.precio <= 0) {
    error.value = 'El precio debe ser mayor a 0.'
    return
  }

  if (form.stock < 0) {
    error.value = 'El stock no puede ser negativo.'
    return
  }

  emit('save', {
    nombre: form.nombre.trim(),
    categoriaId: Number(form.categoriaId),
    descripcion: form.descripcion.trim(),
    precio: Number(form.precio),
    stock: Number(form.stock),
  })
}

function cancelEdit() {
  emit('cancel')
}

onMounted(() => {
  loadCategorias()
})
</script>