<template>
  <div style="padding-top: 100px; text-align: center">
    <input type="file" ref="fileInput" @change="uploadFile">
    <button @click="uploadFile">Send</button>
  </div>
</template>

<script>
import * as listUrl from "@/js/listUrl";
export default {
  methods: {
    async uploadFile() {
      const file = this.$refs.fileInput.files[0];
      const formData = new FormData();
      formData.append('file', file);

      // Відправка файлу на сервер
      await listUrl.testImage(formData);
    }
  }
}
</script>
