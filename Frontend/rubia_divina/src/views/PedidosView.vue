<template>
  <div class="page-container">

    <div class="page-header">

      <div>
        <h1>📋 Pedidos</h1>
        <p>Gestión de pedidos realizados</p>
      </div>

      <div class="actions-header">

        <button
          class="add-btn"
          @click="nuevoPedido">

          + Nuevo Pedido

        </button>

        <RouterLink
          to="/dashboard"
          class="back-btn">

          Volver

        </RouterLink>

      </div>

    </div>

    <div class="table-card">

      <table>

        <thead>

          <tr>
            <th>ID</th>
            <th>Fecha</th>
            <th>Total</th>
            <th>Promoción</th>
            <th>Productos</th>
            <th>Acciones</th>
          </tr>

        </thead>

        <tbody>

          <tr
            v-for="pedido in pedidos"
            :key="pedido.id">

            <td>#{{ pedido.id }}</td>

            <td>
              {{ formatDate(pedido.fechaPedido) }}
            </td>

            <td>

              <span class="total">

                ${{ pedido.total.toFixed(2) }}

              </span>

            </td>

            <td>

              <span
                v-if="pedido.promocion"
                class="promo-badge">

                🎁 {{ pedido.promocion.nombre }}

              </span>

              <span v-else>
                Sin promoción
              </span>

            </td>

            <td>

              <div class="product-list">

                <span
                  v-for="detalle in pedido.detalles"
                  :key="detalle.id"
                  class="product-tag">

                  {{ detalle.producto.nombre }}
                  x{{ detalle.cantidad }}

                </span>

              </div>

            </td>

            <td>

              <button
                class="edit-btn"
                @click="editarPedido(pedido)">

                Editar

              </button>

              <button
                class="delete-btn"
                @click="eliminarPedidoHandler(pedido.id)">

                Eliminar

              </button>

            </td>

          </tr>

        </tbody>

      </table>

    </div>

    <PedidoModal
      v-if="showModal"
      :modelValue="pedidoSeleccionado"
      :productos="productos"
      :promociones="promociones"
      @close="cerrarModal"
      @save="guardarPedido"
    />

  </div>
</template>

<script setup>

import { ref,onMounted } from 'vue'

import {
  getPedidos,
  createPedido,
  updatePedido,
  deletePedido
}
from '../services/pedidoService'

import { getProductos }
from '../services/productoService'

import { getPromociones }
from '../services/promocionService'

import PedidoModal
from '../components/PedidoModal.vue'

const pedidos = ref([])
const productos = ref([])
const promociones = ref([])

const showModal = ref(false)
const pedidoSeleccionado = ref(null)

async function cargarPedidos(){

  const { data } = await getPedidos()

  pedidos.value = data

}

async function cargarProductos(){

  const { data } = await getProductos()

  productos.value = data

}

async function cargarPromociones(){

  const { data } = await getPromociones()

  promociones.value = data

}

function nuevoPedido(){

  pedidoSeleccionado.value = null

  showModal.value = true

}

function editarPedido(pedido){

  pedidoSeleccionado.value = {
    ...pedido
  }

  showModal.value = true

}

function cerrarModal(){

  showModal.value = false

  pedidoSeleccionado.value = null

}

async function guardarPedido(payload){

  try{

    if(pedidoSeleccionado.value){

      await updatePedido(
        pedidoSeleccionado.value.id,
        payload
      )

    }
    else{

      await createPedido(payload)

    }

    cerrarModal()

    await cargarPedidos()

  }
  catch(error){

    console.error(error)

  }

}

async function eliminarPedidoHandler(id){

  if(!confirm('¿Eliminar pedido?'))
    return

  await deletePedido(id)

  await cargarPedidos()

}

function formatDate(fecha){

  return new Date(fecha)
    .toLocaleString()

}

onMounted(async()=>{

  await cargarPedidos()
  await cargarProductos()
  await cargarPromociones()

})

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
  margin:0;
}

.page-header p{
  color:#8d6e63;
}

.actions-header{
  display:flex;
  gap:10px;
}

.add-btn{
  background:#6d4c41;
  color:white;
  border:none;
  padding:10px 20px;
  border-radius:10px;
  cursor:pointer;
}

.back-btn{
  background:#8d6e63;
  color:white;
  text-decoration:none;
  padding:10px 20px;
  border-radius:10px;
}

.table-card{
  background:white;
  border-radius:20px;
  overflow:hidden;
  box-shadow:0 4px 12px rgba(0,0,0,.12);
}

table{
  width:100%;
  border-collapse:collapse;
}

thead{
  background:#6d4c41;
  color:white;
}

th,td{
  padding:15px;
}

tbody tr{
  border-bottom:1px solid #eee;
}

tbody tr:hover{
  background:#faf7f4;
}

.total{
  color:#2e7d32;
  font-weight:bold;
}

.product-list{
  display:flex;
  flex-wrap:wrap;
  gap:6px;
}

.product-tag{
  background:#efebe9;
  color:#5d4037;
  padding:4px 10px;
  border-radius:15px;
  font-size:.85rem;
}

.promo-badge{
  background:#fff3e0;
  color:#ef6c00;
  padding:5px 10px;
  border-radius:15px;
  font-weight:bold;
}

.edit-btn{
  background:#ffb300;
  border:none;
  color:white;
  padding:8px 15px;
  border-radius:8px;
  margin-right:5px;
}

.delete-btn{
  background:#e53935;
  border:none;
  color:white;
  padding:8px 15px;
  border-radius:8px;
}

</style>