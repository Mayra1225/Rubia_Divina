<template>

<div class="page-container">

  <div class="page-header">

    <h1>🎁 Promociones</h1>

    <div>

      <button
        class="add-btn"
        @click="nuevaPromocion">

        + Nueva Promoción

      </button>

      <RouterLink
        to="/dashboard"
        class="back-btn">

        Volver

      </RouterLink>

    </div>

  </div>

  <div class="cards-grid">

    <div
      v-for="promo in promociones"
      :key="promo.id"
      class="promo-card">

      <h3>
        {{ promo.nombre }}
      </h3>

      <p>
        {{ promo.descripcion }}
      </p>

      <div class="discount">

        {{ promo.descuento }}%

      </div>

      <div class="dates">

        <p>
          Inicio:
          {{ formatDate(promo.fechaInicio) }}
        </p>

        <p>
          Fin:
          {{ formatDate(promo.fechaFin) }}
        </p>

      </div>

      <span
        class="status"
        :class="
          promo.activa
            ? 'active'
            : 'inactive'
        ">

        {{
          promo.activa
            ? 'Activa'
            : 'Inactiva'
        }}

      </span>

      <div class="actions">

        <button
          class="edit-btn"
          @click="editarPromocion(promo)">

          Editar

        </button>

        <button
          class="delete-btn"
          @click="eliminarPromocion(promo.id)">

          Eliminar

        </button>

      </div>

    </div>

  </div>

  <PromocionModal
    v-if="showModal"
    :modelValue="promocionSeleccionada"
    @close="cerrarModal"
    @save="guardarPromocion"
  />

</div>

</template>

<script setup>

import {
  ref,
  onMounted
}
from 'vue'

import { RouterLink }
from 'vue-router'

import {
  getPromociones,
  createPromocion,
  updatePromocion,
  deletePromocion
}
from '../services/promocionService'

import PromocionModal
from '../components/PromocionModal.vue'

const promociones = ref([])

const showModal = ref(false)

const promocionSeleccionada = ref(null)

async function cargarPromociones(){

  const { data } =
    await getPromociones()

  promociones.value = data

}

function nuevaPromocion(){

  promocionSeleccionada.value = null

  showModal.value = true

}

function editarPromocion(promo){

  promocionSeleccionada.value = {
    ...promo
  }

  showModal.value = true

}

function cerrarModal(){

  promocionSeleccionada.value = null

  showModal.value = false

}

async function guardarPromocion(payload){

  try{

    if(promocionSeleccionada.value){

      await updatePromocion(
        promocionSeleccionada.value.id,
        payload
      )

    }
    else{

      await createPromocion(payload)

    }

    cerrarModal()

    await cargarPromociones()

  }
  catch(error){

    console.error(error)

  }

}

async function eliminarPromocion(id){

  if(!confirm(
    '¿Eliminar promoción?'
  )) return

  await deletePromocion(id)

  await cargarPromociones()

}

function formatDate(fecha){

  return new Date(fecha)
    .toLocaleDateString()

}

onMounted(
  cargarPromociones
)

</script>

<style scoped>

.page-container{
  padding:30px;
  background:#f6f1ea;
  min-height:100vh;
}

.page-header{
  display:flex;
  justify-content:space-between;
  align-items:center;
  margin-bottom:30px;
}

.page-header h1{
  color:#5d4037;
}

.cards-grid{
  display:grid;
  grid-template-columns:
    repeat(auto-fill,minmax(320px,1fr));
  gap:25px;
}

.promo-card{
  background:white;
  border-radius:20px;
  padding:20px;
  box-shadow:0 5px 15px rgba(0,0,0,.1);
}

.promo-card h3{
  color:#4e342e;
}

.discount{
  font-size:28px;
  font-weight:bold;
  color:#2e7d32;
  margin:15px 0;
}

.status{
  display:inline-block;
  padding:8px 15px;
  border-radius:30px;
  color:white;
  margin-bottom:15px;
}

.active{
  background:#43a047;
}

.inactive{
  background:#e53935;
}

.actions{
  display:flex;
  gap:10px;
  margin-top:15px;
}

.edit-btn{
  background:#ffb300;
  color:white;
  border:none;
  padding:10px;
  border-radius:10px;
  flex:1;
}

.delete-btn{
  background:#e53935;
  color:white;
  border:none;
  padding:10px;
  border-radius:10px;
  flex:1;
}

.add-btn{
  background:#6d4c41;
  color:white;
  border:none;
  padding:10px 20px;
  border-radius:10px;
  margin-right:10px;
}

.back-btn{
  background:#8d6e63;
  color:white;
  text-decoration:none;
  padding:10px 20px;
  border-radius:10px;
}

</style>