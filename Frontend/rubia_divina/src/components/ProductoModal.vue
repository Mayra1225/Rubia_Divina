<template>
  <div class="modal-overlay">
    <div class="modal">
      <h2>
        {{ modelValue ? 'Editar Producto' : 'Nuevo Producto' }}
      </h2>

      <form @submit.prevent="guardar">
        <label>
          Nombre
          <input v-model="form.nombre" type="text" required />
        </label>

        <label>
          Categoría
          <select v-model="form.categoriaId" required>
            <option value="">Seleccione</option>

            <option value="1">Bebidas</option>

            <option value="2">Postres</option>

            <option value="3">Snacks</option>
          </select>
        </label>

        <label>
          Descripción
          <textarea v-model="form.descripcion" />
        </label>

        <label>
          Precio
          <input v-model.number="form.precio" type="number" min="0.01" step="0.01" required />
        </label>

        <label>
          Stock
          <input v-model.number="form.stock" type="number" min="0" required />
        </label>

        <label>
          Imagen URL
          <input v-model="form.imagenUrl" type="text" />
        </label>

        <div class="buttons">
          <button type="submit" class="save">Guardar</button>

          <button type="button" class="cancel" @click="$emit('close')">Cancelar</button>
        </div>
      </form>
    </div>
  </div>
</template>

<script setup>
import { reactive, watch } from 'vue'

const props = defineProps({
  modelValue: Object,
})

const emit = defineEmits(['save', 'close'])

const form = reactive({
  nombre: '',
  descripcion: '',
  precio: 0,
  stock: 0,
  categoriaId: '',
  imagenUrl: '',
})

watch(
  () => props.modelValue,
  (value) => {
    if (value) {
      form.nombre = value.nombre
      form.descripcion = value.descripcion
      form.precio = value.precio
      form.stock = value.stock
      form.categoriaId = value.categoriaId
      form.imagenUrl = value.imagenUrl
    } else {
      form.nombre = ''
      form.descripcion = ''
      form.precio = 0
      form.stock = 0
      form.categoriaId = ''
      form.imagenUrl = ''
    }
  },
  { immediate: true },
)

function guardar() {
  emit('save', {
    nombre: form.nombre,
    descripcion: form.descripcion,
    precio: form.precio,
    stock: form.stock,
    categoriaId: Number(form.categoriaId),
    imagenUrl: form.imagenUrl,
  })
}
</script>

<style scoped>
.modal-overlay {
  position: fixed;
  inset: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  justify-content: center;
  align-items: center;
  z-index: 999;
}

.modal {
  width: 500px;
  max-width: 95%;
  background: white;
  padding: 25px;
  border-radius: 20px;
}

form {
  display: flex;
  flex-direction: column;
  gap: 15px;
}

input,
textarea,
select {
  width: 100%;
  padding: 10px;
  box-sizing: border-box;
}

.buttons {
  display: flex;
  justify-content: flex-end;
  gap: 10px;
}

.save {
  background: #6d4c41;
  color: white;
  border: none;
  padding: 10px 15px;
  cursor: pointer;
}

.cancel {
  background: #9e9e9e;
  color: white;
  border: none;
  padding: 10px 15px;
  cursor: pointer;
}
</style>
