<template>
  <div class="main-section">
    <label key="name">Name</label><br>
    <input type="text" id="name" v-model="name" @change="checkButtonActive" placeholder="Name"><br>
    <label key="file">Photo file</label><br>
    <input id="file" type="file" ref="fileInput" accept=".jpg, .jpeg" @change="checkButtonActive"><br>
    <button @click="createPublisher" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Create</button><br>
  </div>
</template>

<script>
import * as publisherAPI from "@/js/API/publisherAPI";
export default {
  data(){
    return{
      name: "",
      isActive: false
    }
  },


  methods: {
    async checkButtonActive(){
      this.isActive = this.name && this.$refs.fileInput.value;
    },

    async createPublisher() {
      this.isActive = false;
      const token = localStorage.getItem("token")
      if (token){
        const data = {
          name: this.name
        };

        await publisherAPI.postCreatePublisher(data, token);

        const file = this.$refs.fileInput.files[0];
        const formData = new FormData();
        formData.append('file', file);
        formData.append('name', this.name);

        await publisherAPI.postCreatePublisherImage(formData, token);

        this.$refs.fileInput.value = ''
        this.name = "";
      }


    }
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>
