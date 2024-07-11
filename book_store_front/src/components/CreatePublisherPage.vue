<template>
  <div class="main-section">
    <label key="name">Name</label><br>
    <input type="text" id="name" v-model="name" @change="checkButtonActive" placeholder="Name"><br>
    <label key="file">Photo</label><br>
    <input type="text" v-model="imageUrl" @change="checkButtonActive"><br>
    <button @click="createPublisher" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Create</button><br>
  </div>
</template>

<script>
import * as publisherAPI from "@/js/API/publisherAPI";
export default {
  data(){
    return{
      name: "",
      imageUrl: "",
      isActive: false
    }
  },


  methods: {
    async checkButtonActive(){
      this.isActive = this.name && this.imageUrl;
    },

    async createPublisher() {
      this.isActive = false;
      const token = localStorage.getItem("token")
      if (token){
        const data = {
          name: this.name,
          imageUrl: this.imageUrl
        };

        await publisherAPI.postCreatePublisher(data, token);
        this.name = "";
        this.imageUrl = "";
      }
    }
  }
}
</script>

<style>
@import "@/assets/css/styles.css";
</style>
