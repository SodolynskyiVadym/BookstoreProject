<template>
    <div class="update-publisher-section" v-if="loaded">
        <label key="name">Name</label>
        <input type="text" v-model="publisher.name" id="name" placeholder="Name" @change="checkIsActive">
        <button @click="updatePublisher" :class="{ 'button-update': isActive, 'button-update-disabled': !isActive }" :disabled="!isActive">Update</button>
    </div>
  </template>
  
  <script>
  import * as listURL from "@/js/listUrl";
  
  
  export default {
    data(){
        return{
            publisher: null,
            isActive: true,
            loaded: false
        }
    },
  
    methods: {
        async checkIsActive(){
            if(this.publisher.name){
                this.isActive = true;
            }else{
                this.isActive = false;
            }
        },
  
  
        async updatePublisher(){
            const data = {
              id: this.$route.params.id,
              name: this.publisher.name,
            }
            await listURL.requestPatchUpdatePublisher(data);
        }
    },
  
    async mounted(){
      this.publisher = await listURL.requestGetPublisher(this.$route.params.id);

      console.log(this.publisher)
  
      if(this.publisher) this.loaded = true;
    }
  }
  </script>
  
  <style>
  
  .update-publisher-section {
    padding-top: 120px;
    justify-content: center;
    display: grid;
  }
  
  
  .update-publisher-section label {
    font-size: x-large;
  }
  
  .update-publisher-section input{
    font-size: x-large;
    width: 650px;
    height: 50px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
  }
  
  .button-update {
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
  
  .button-update:hover {
    background-color: rgb(48, 45, 45);
  }
  
  
  .button-update-disabled {
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