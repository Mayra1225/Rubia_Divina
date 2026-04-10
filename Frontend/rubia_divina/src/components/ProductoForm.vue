<template>
  <form class="form-card" @submit.prevent="submitForm">
    <h3>{{ isEditing ? 'Editar producto' : 'Nuevo producto' }}</h3>

    <div class="grid-form">
      <label>
        Nombre
        <input v-model="form.nombre" type="text" maxlength="80" required />
      </label>

      <label>
        Categoría
        <select v-model="form.categoria" required>
          <option disabled value="">Seleccione</option>
          <option>Bebida</option>
          <option>Postre</option>
          <option>Snack</option>
          <option>Promoción</option>
        </select>
      </label>

      <label class="full-width">
        Descripción
        <textarea v-model="form.descripcion" rows="3" maxlength="250"></textarea>
      </label>

      <label>
        Precio
        <input v-model.number="form.precio" type="number" min="0.01" step="0.01" required />
      </label>

      <label>
        Stock
        <input v-model.number="form.stock" type="number" min="0" step="1" required />
      </label>
    </div>

    <p v-if="error" class="feedback error">{{ error }}</p>

    <div class="actions">
      <button class="btn btn-primary" type="submit">
        {{ isEditing ? 'Guardar cambios' : 'Crear producto' }}
      </button>
      <button v-if="isEditing" class="btn btn-secondary" type="button" @click="cancelEdit">
        Cancelar
      </button>
    </div>
  </form>
</template>

<script setup>
import { computed, reactive, watch } from 'vue'

const props = defineProps({
  modelValue: {
    type: Object,
    default: null,
  },
})

const emit = defineEmits(['save', 'cancel'])

const form = reactive({
  nombre: '',
  categoria: '',
  descripcion: '',
  precio: 0,
  stock: 0,
})

const isEditing = computed(() => Boolean(props.modelValue?.id))
const error = computed(() => '')

watch(
  () => props.modelValue,
  (value) => {
    form.nombre = value?.nombre ?? ''
    form.categoria = value?.categoria ?? ''
    form.descripcion = value?.descripcion ?? ''
    form.precio = value?.precio ?? 0
    form.stock = value?.stock ?? 0
  },
  { immediate: true },
)

function submitForm() {
  if (!form.nombre.trim() || !form.categoria.trim()) {
    return
  }

  emit('save', {
    nombre: form.nombre.trim(),
    categoria: form.categoria,
    descripcion: form.descripcion.trim(),
    precio: Number(form.precio),
    stock: Number(form.stock),
  })
}

function cancelEdit() {
  emit('cancel')
}
</script>
