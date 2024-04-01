<template>
  <div class="create-publisher-section">
    <label key="name">Name</label>
    <input type="text" id="name" v-model="name" @change="checkButtonActive">
    <label key="file">Photo file</label>
    <input id="file" type="file" ref="fileInput" accept=".jpg, .jpeg" @change="checkButtonActive">
    <button @click="createPublisher" :class="{ 'button-create': isActive, 'button-create-disabled': !isActive }" :disabled="!isActive">Create</button>
  </div>
</template>

<script>
import * as listUrl from "@/js/listUrl";
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

        await listUrl.requestPostCreatePublisher(data, token);

        const file = this.$refs.fileInput.files[0];
        const formData = new FormData();
        formData.append('file', file);
        formData.append('name', this.name);

        await listUrl.requestCreatePublisherImage(formData, token);

        this.$refs.fileInput.value = ''
        this.name = "";
      }


    }
  }
}
</script>

<style>
.create-publisher-section {
  padding-top: 120px;
  justify-content: center;
  display: grid;
}


.create-publisher-section label {
  font-size: x-large;
}

.create-publisher-section input{
  font-size: x-large;
  width: 650px;
  height: 50px;
  display: block;
  margin-bottom: 20px;
  border-radius: 15px;
}

.button-create {
  color: white;
  position: center;
  width: 100px;
  height: 50px;
  text-align: center;
  background-color: rgb(0, 0, 0);
  border-radius: 15px;
  cursor: pointer;
  font-weight: blod;
  font-size: 1.2em;
  border: none;
}

.button-create:hover {
  background-color: rgb(48, 45, 45);
}


.button-create-disabled {
  color: white;
  position: center;
  width: 100px;
  height: 50px;
  text-align: center;
  background-color: rgb(119, 116, 116);
  border-radius: 15px;
  cursor: pointer;
  font-weight: blod;
  font-size: 1.2em;
  border: none;
}
</style>
