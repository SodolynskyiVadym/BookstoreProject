<template>
    <div class="main-section" v-if="loaded">
        <label key="email">Email</label><br>
        <input type="text" id="email" v-model="user.email" readonly><br>
        <label key="name">Name</label><br>
        <input type="text" id="name" v-model="user.name" @change="checkIsActive"><br>
        <label key="password">Password</label><br>
        <input type="password" id="password" v-model="user.password" @change="checkIsActive"><br>
        <button @click="sendChanges" :class="{ 'main-button-changes': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Change</button>
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
        await listURL.patchUpdateUser(data, token);
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
        this.user = await listURL.getUserByToken(token);
        this.user.passsword = "";
        this.currentName = this.user.name;
        this.loaded = true;
    }else this.$router.push("/login");
    
  }
}
</script>
  
<style>
@import "@/assets/css/styles.css";
</style>