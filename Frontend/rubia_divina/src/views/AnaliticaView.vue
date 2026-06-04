<script setup>
import { ref, onMounted } from 'vue'
import { obtenerAnalitica } from '../services/analiticaService'

const datos = ref(null)

onMounted(async () => {
  datos.value =
    await obtenerAnalitica()
})
</script>

<template>
<div class="container">

  <h1>Analítica del Negocio</h1>

  <div v-if="datos">

    <div class="card">
      <h3>Total Pedidos</h3>
      <p>{{ datos.totalPedidos }}</p>
    </div>

    <div class="card">
      <h3>Ventas Totales</h3>
      <p>$ {{ datos.ventasTotales }}</p>
    </div>

    <div class="card">
      <h3>Día Más Vendido</h3>
      <p>{{ datos.diaMasVendido }}</p>
    </div>

    <div class="card">
      <h3>Hora Pico</h3>
      <p>{{ datos.horaMasVendida }}:00</p>
    </div>

    <div class="card recomendacion">
      <h3>Recomendación</h3>
      <p>{{ datos.recomendacion }}</p>
    </div>

  </div>

</div>
</template>

<style scoped>

.container{
padding:30px;
}

.card{
background:white;
padding:20px;
margin-bottom:15px;
border-radius:12px;
}

.recomendacion{
background:#fff3cd;
}

</style>