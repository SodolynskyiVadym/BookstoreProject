<template>
    <div class="user-section" v-if="loaded">
        <label key="email">Email</label>
        <input type="text" id="email" v-model="user.email" readonly>
        <label key="name">Name</label>
        <input type="text" id="name" v-model="user.name" @change="checkIsActive">
        <label key="password">Password</label>
        <input type="password" id="password" v-model="user.password" @change="checkIsActive">
        <button @click="sendChanges" :class="{ 'button-send-changes': isActive, 'button-send-changes-disabled': !isActive }" :disabled="!isActive">Change</button>
    </div>
</template>

<script>
import * as listURL from "@/js/listUrl";

export default {
  data(){
      return{
          user: null,
          currentName: "",
          isActive: false,
          loaded: false
      }
  },

  methods: {
    async sendChanges(){
      const data = {
        name: this.user.name,
        password: this.user.password
      }
       const token = localStorage.getItem("token");
       if(token){
        this.isActive = false;
        await listURL.requestPatchUpdateUser(data, token);
        this.currentName = this.user.name;
       }
    },

    async checkIsActive(){
        if(this.user.name && ((this.user.password && this.user.password.length >= 8) 
        || (!this.user.password && this.currentName != this.user.name))) this.isActive = true;
        else this.isActive = false;
    }
  },

  async mounted(){
    const token = localStorage.getItem("token");
    if(token){
        this.user = await listURL.requestGetUserByToken(token);
        this.user.passsword = "";
        this.currentName = this.user.name;
        this.loaded = true;
    }else this.$router.push("/login");
    
  }
}
</script>
  
<style>
.user-section {
    padding-top: 120px;
    justify-content: center;
    display: grid;
}


.user-section label {
    font-size: x-large;
}

.user-section input {
    font-size: x-large;
    width: 650px;
    height: 50px;
    display: block;
    margin-bottom: 20px;
    border-radius: 15px;
}


.button-send-changes{
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

.button-send-changes:hover {
    background-color: rgb(48, 45, 45);
}



.button-send-changes-disabled {
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