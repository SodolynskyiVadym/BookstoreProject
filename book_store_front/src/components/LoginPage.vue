<template>
    <div class="main-section">
        <label key="email">Email</label><br>
        <input type="text" v-model="email" id="email" placeholder="Email" @change="checkIsActive"><br>
        <label key="password">Password</label><br>
        <input type="text" v-model="password" id="password" placeholder="Password" @change="checkIsActive"><br>

        <button @click="login" :class="{ 'main-button': isActive, 'main-button-disabled': !isActive }" :disabled="!isActive">Login</button>
    </div>
</template>

<script>
import * as listURL from "@/js/listUrl";


export default {
    data(){
        return{
            email: "",
            password: "",
            isActive: false
        }
    },

    methods: {
        async checkIsActive(){
            if(this.email && this.password){
                this.isActive = true;
            }else{
                this.isActive = false;
            }
        },


        async login(){
            const dataLogin = {
                email: this.email,
                password: this.password
            }
            const data = await listURL.requestPostLogin(dataLogin);
            const token = data.token
            localStorage.setItem("token", token)

            this.$router.push('/');
            setTimeout(() => {
                window.location.reload();
            }, 10);
        }
    }
}
</script>

<style>

@import "@/assets/css/styles.css";

</style>