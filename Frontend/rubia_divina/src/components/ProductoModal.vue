<template>
  <div class="modal-overlay">
    <div class="modal">

      <h2>
        {{ modelValue ? 'Editar Pedido' : 'Nuevo Pedido' }}
      </h2>

      <form @submit.prevent="guardar">

        <label>
          Fecha
          <input
            v-model="form.fechaPedido"
            type="datetime-local"
            required
          />
        </label>

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
          class="detalle-card">

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
          />

          <button
            type="button"
            class="delete-btn"
            @click="eliminarDetalle(index)">

            X

          </button>

        </div>

        <button
          type="button"
          class="add-btn"
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
import {
  reactive,
  watch
}
from 'vue'

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
  fechaPedido: '',
  promocionId: null,
  detalles: []
})

watch(
  () => props.modelValue,
  (value) => {

    if(value){

      form.fechaPedido =
        value.fechaPedido?.substring(0,16)

      form.promocionId =
        value.promocionId

      form.detalles =
        value.detalles.map(d=>({

          productoId:d.productoId,
          cantidad:d.cantidad

        }))

    }
    else{

      form.fechaPedido =
        new Date()
        .toISOString()
        .substring(0,16)

      form.promocionId = null

      form.detalles = []

    }

  },
  {
    immediate:true
  }
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

  emit('save',{

    fechaPedido:
      form.fechaPedido,

    promocionId:
      form.promocionId,

    detalles:
      form.detalles

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
}

.modal{
  width:700px;
  max-height:90vh;
  overflow:auto;
  background:white;
  border-radius:20px;
  padding:25px;
}

.detalle-card{
  display:flex;
  gap:10px;
  margin-bottom:10px;
}

.buttons{
  display:flex;
  justify-content:flex-end;
  gap:10px;
  margin-top:20px;
}

.save{
  background:#6d4c41;
  color:white;
  border:none;
  padding:10px 20px;
}

.cancel{
  background:#999;
  color:white;
  border:none;
  padding:10px 20px;
}

.add-btn{
  background:#2e7d32;
  color:white;
  border:none;
  padding:10px;
  margin-top:10px;
}

.delete-btn{
  background:red;
  color:white;
  border:none;
  padding:0 10px;
}

</style>