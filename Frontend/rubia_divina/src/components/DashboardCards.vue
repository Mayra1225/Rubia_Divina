<template>

<div class="stats-grid">

<div class="card">
<h3>Total Ventas</h3>
<h2>${{ data.totalVentas }}</h2>
</div>

<div class="card">
<h3>Total Pedidos</h3>
<h2>{{ data.totalPedidos }}</h2>
</div>

<div class="card">
<h3>Producto Top</h3>
<h2>{{ data.productoMasVendido?.producto }}</h2>
</div>

<div class="card">
<h3>Promoción Top</h3>
<h2>{{ data.promoMasUsada?.promocion }}</h2>
</div>

</div>

</template>

<script setup>

defineProps({
data:Object
})

</script>