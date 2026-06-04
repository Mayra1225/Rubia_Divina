<template>
  <div class="modal-overlay">

    <div class="modal">

      <h2>
        {{
          modelValue
            ? 'Editar Promoción'
            : 'Nueva Promoción'
        }}
      </h2>

      <form @submit.prevent="guardar">

        <label>
          Nombre
          <input
            v-model="form.nombre"
            required
          />
        </label>

        <label>
          Descripción
          <textarea
            v-model="form.descripcion"
          />
        </label>

        <label>
          Descuento (%)
          <input
            v-model.number="form.descuento"
            type="number"
            min="0"
            max="100"
            required
          />
        </label>

        <label>
          Fecha Inicio
          <input
            v-model="form.fechaInicio"
            type="datetime-local"
            required
          />
        </label>

        <label>
          Fecha Fin
          <input
            v-model="form.fechaFin"
            type="datetime-local"
            required
          />
        </label>

        <label class="checkbox">

          <input
            v-model="form.activa"
            type="checkbox"
          />

          Activa

        </label>

        <div class="buttons">

          <button
            type="submit"
            class="save">

            Guardar

          </button>

          <button
            type="button"
            class="cancel"
            @click="$emit('close')">

            Cancelar

          </button>

        </div>

      </form>

    </div>

  </div>
</template>

<script setup>
import {
  reactive,
  watch
}
from 'vue'

const props = defineProps({
  modelValue: Object
})

const emit = defineEmits([
  'save',
  'close'
])

const form = reactive({
  nombre: '',
  descripcion: '',
  descuento: 0,
  fechaInicio: '',
  fechaFin: '',
  activa: true
})

watch(
  () => props.modelValue,
  (value) => {

    if (value) {

      form.nombre = value.nombre
      form.descripcion = value.descripcion
      form.descuento = value.descuento

      form.fechaInicio =
        value.fechaInicio?.slice(0,16)

      form.fechaFin =
        value.fechaFin?.slice(0,16)

      form.activa = value.activa

    }
    else {

      form.nombre = ''
      form.descripcion = ''
      form.descuento = 0
      form.fechaInicio = ''
      form.fechaFin = ''
      form.activa = true

    }

  },
  { immediate:true }
)

function guardar(){

  emit('save',{
    nombre: form.nombre,
    descripcion: form.descripcion,
    descuento: form.descuento,
    fechaInicio: form.fechaInicio,
    fechaFin: form.fechaFin,
    activa: form.activa
  })

}
</script>

<style scoped>

.modal-overlay{
  position:fixed;
  inset:0;
  background:rgba(0,0,0,.55);
  display:flex;
  justify-content:center;
  align-items:center;
  z-index:999;
}

.modal{
  width:600px;
  max-width:95%;
  background:white;
  padding:25px;
  border-radius:20px;
}

.modal h2{
  color:#5d4037;
  margin-bottom:20px;
}

form{
  display:flex;
  flex-direction:column;
  gap:15px;
}

input,
textarea{
  width:100%;
  padding:10px;
}

.buttons{
  display:flex;
  justify-content:flex-end;
  gap:10px;
}

.save{
  background:#6d4c41;
  color:white;
  border:none;
  padding:10px 20px;
  border-radius:10px;
}

.cancel{
  background:#9e9e9e;
  color:white;
  border:none;
  padding:10px 20px;
  border-radius:10px;
}

.checkbox{
  display:flex;
  align-items:center;
  gap:10px;
}

</style>