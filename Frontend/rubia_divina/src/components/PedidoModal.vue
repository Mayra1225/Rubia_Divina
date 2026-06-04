<template>
  <div class="modal-overlay">

    <div class="modal">

      <h2>
        {{ modelValue ? 'Editar Pedido' : 'Nuevo Pedido' }}
      </h2>

      <form @submit.prevent="guardar">

        <label>
          Promoción

          <select v-model="form.promocionId">

            <option :value="null">
              Sin promoción
            </option>

            <option
              v-for="promo in promociones"
              :key="promo.id"
              :value="promo.id">

              {{ promo.nombre }}

            </option>

          </select>

        </label>

        <h3>Productos</h3>

        <div
          v-for="(detalle,index) in form.detalles"
          :key="index"
          class="detalle">

          <select
            v-model="detalle.productoId"
            required>

            <option value="">
              Seleccione producto
            </option>

            <option
              v-for="producto in productos"
              :key="producto.id"
              :value="producto.id">

              {{ producto.nombre }}

            </option>

          </select>

          <input
            v-model.number="detalle.cantidad"
            type="number"
            min="1"
            placeholder="Cantidad"
            required>

          <button
            type="button"
            class="remove-btn"
            @click="eliminarDetalle(index)">

            X

          </button>

        </div>

        <button
          type="button"
          class="add-product-btn"
          @click="agregarDetalle">

          + Agregar Producto

        </button>

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

import { reactive, watch } from 'vue'

const props = defineProps({
  modelValue: Object,
  productos: Array,
  promociones: Array
})

const emit = defineEmits([
  'save',
  'close'
])

const form = reactive({
  promocionId: null,
  detalles: []
})

watch(
  () => props.modelValue,
  (value) => {

    if(value){

      form.promocionId =
        value.promocionId

      form.detalles =
        value.detalles.map(d => ({
          productoId: d.productoId,
          cantidad: d.cantidad
        }))

    }
    else{

      form.promocionId = null

      form.detalles = [
        {
          productoId: '',
          cantidad: 1
        }
      ]

    }

  },
  { immediate:true }
)

function agregarDetalle(){

  form.detalles.push({
    productoId:'',
    cantidad:1
  })

}

function eliminarDetalle(index){

  form.detalles.splice(index,1)

}

function guardar(){

  emit('save', {
    promocionId: form.promocionId,
    detalles: form.detalles
  })

}

</script>

<style scoped>

.modal-overlay{
  position:fixed;
  inset:0;
  background:rgba(0,0,0,.5);
  display:flex;
  justify-content:center;
  align-items:center;
  z-index:999;
}

.modal{
  width:700px;
  max-width:95%;
  background:white;
  padding:25px;
  border-radius:20px;
}

form{
  display:flex;
  flex-direction:column;
  gap:15px;
}

.detalle{
  display:flex;
  gap:10px;
}

select,
input{
  flex:1;
  padding:10px;
}

.add-product-btn{
  background:#4caf50;
  color:white;
  border:none;
  padding:10px;
}

.remove-btn{
  background:#e53935;
  color:white;
  border:none;
  width:40px;
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
}

.cancel{
  background:#9e9e9e;
  color:white;
  border:none;
  padding:10px 20px;
}

</style>