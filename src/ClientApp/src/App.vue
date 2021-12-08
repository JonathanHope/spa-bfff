<template>
  <p>{{ message }}</p>
</template>

<script lang="ts">
import { defineComponent, ref } from 'vue'
import ky from 'ky'

export default defineComponent({
  name: 'App',
  setup () {
    const message = ref<string>('Loading...')
    ky.get('api/get?hello=world').text().then(result => {
      message.value = result
    })

    return {
      message
    }
  }
})
</script>