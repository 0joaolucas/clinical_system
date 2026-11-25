
<template>
    <div class="placeholder">
        <h1>{{ route.meta.title }}</h1>
        <p>Esta tela será desenvolvida em breve.</p>
    </div>
</template>

<script setup>
import { useRoute } from 'vue-router'

const route = useRoute()
</script>

<style scoped>
.placeholder {
    background-color: white;
    padding: 30px;
    border-radius: 10px;
    min-height: 250px;
}

.placeholder h1 {
    color: #243b53;
    margin-bottom: 12px;
}

.placeholder p {
    color: #718096;
}
</style>
