<template>
    <div class="main-section" v-if="loaded">
        <label key="email">Email</label><br>
        <input type="text" id="email" v-model="user.email" readonly><br>
        <label key="password">Password</label><br>
        <input type="password" id="password" v-model="user.password" @input="checkIsActive"><br>
        <button @click="sendChanges" style="width: auto;" :class="{ 'main-button-changes': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Update password</button>
    </div>
</template>

<script>
import * as authAPI from "@/js/API/authAPI";

export default {
  data(){
      return{
          user: null,
          isActive: false,
          loaded: false
      }
  },

  methods: {
    async sendChanges(){
      const data = {
        password: this.user.password
      }
       const token = localStorage.getItem("token");
       if(token){
        this.isActive = false;
        await authAPI.updatePassword(data, token);
        this.currentName = this.user.name;
       }
    },

    async checkIsActive(){
        this.isActive = this.user.password.length > 7;
    }
  },

  async mounted(){
    const token = localStorage.getItem("token");
    if(token){
        this.user = await authAPI.getUserByToken(token);
        this.user.passsword = "";
        this.loaded = true;
    }else this.$router.push("/login");
    
  }
}
</script>
  
<style>
@import "@/assets/css/styles.css";
</style>